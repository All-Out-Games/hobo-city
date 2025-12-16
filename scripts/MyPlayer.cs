using AO;
using System;
using System.Collections.Generic;
using Assembly.scripts;
using ReusableWeapons;
using System.Runtime.CompilerServices;

public partial class MyPlayer : Player, INetworkedComponent
{
  // Counts to 10 and increments a currency to track play time for metrics correlation
  public float PlayTimeTimer = 0f;

  public static Texture inventoryBubbleAsset = Assets.GetAsset<Texture>("$AO/new/in_game/inventory/inventory_v2/inventory_bubble.png");
  public static Texture portalPointerAsset = Assets.GetAsset<Texture>("PortalPointer.png");
  // Timestamp of the last missile explosion used to drive a global camera flash on the local client
  public static float ExplosionFlashStartTime = -100f;
  public const float EXPLOSION_FLASH_DURATION = 1.2f; // seconds

  public int TempHoveredSlot = -1;

  public static MyPlayer localPlayer;
  public const float RESPAWN_TIME = 6.5f;

  public ulong rngSeed;
  public SyncVar<string> TeleportingToGameId = new SyncVar<string>("none");
  public SyncVar<float> StartedTeleportingAt = new SyncVar<float>(-1f);

  public SyncVar<int> KillsThisLife = new(0);
  public SyncVar<int> XP = new(0);

  // Set by HiddenArea collider
  public bool IsBehindSomething = false;
  public SyncVar<bool> IsHidden = new(false); // If they're behind something and a helicoptor isn't over them

  // Track remaining teleport cooldown time
  public SyncVar<float> TeleportCooldownRemaining = new(0f);

  // Helper property to check if teleport is on cooldown
  public bool IsTeleportOnCooldown => TeleportCooldownRemaining.Value > 0f;

  public float ClientDiedAt = -1f;

  public bool CanDealDamage => HealthManager.Alive() && HealthManager.Health > 0;

  // Item equipping
  public int CurrentHoveredSlot = 0;
  public CustomItemInstance CurrentEquippedItem;
  public List<string> BlockScrollReasons = new List<string>();

  // Tracking fields to detect when the item in the hovered slot changes (e.g. from drag-move)
  public int _LastObservedHoveredSlotIndex = -1;
  public Item_Instance _LastObservedHoveredSlotItem = null;

  // Cached list to avoid allocations when checking if the player is behind a destructible.
  public static List<ThingWithHealth> BehindCheckCache = new();
  public static List<ThingWithHealth> OnScreenCheckCache = new();

  // Used to throttle how often we run the expensive "behind something" quadtree check.
  public int behindCheckFrameCounter = 0;

  // Used to throttle how often we run the expensive client-side destructible visibility check.
  public int destructibleVisibilityFrameCounter = 0;

  public int HoveredSlotLastFrame = -1;

  public bool IsPlayingOnMobile = false;
  public MyPlayer LastKilledPlayer;

  // Level system
  public int Level => CalculateLevelFromXP(XP.Value);
  public int XPToNextLevel => CalculateXPForLevel(Level + 1) - XP.Value;

  // Dictionary to track damage dealt to other players for assist calculation
  public Dictionary<Entity, float> DamageDealtToPlayers = new Dictionary<Entity, float>();

  // Class to track assist eligibility 
  public class AssistInfo
  {
    public Entity DamagedBy;
    public float DamageDealtTime;
    public int TotalDamage;
  }

  // List to track who damaged this player for assist calculation
  public List<AssistInfo> RecentDamageDealt = new List<AssistInfo>();

  public float LastShootTime = 0;

  public bool InventoryOpen = false;

  public SyncVar<int> currentRoom = new((int)Room.ISLAND);
  public Room CurrentRoom
  {
    get => (Room)currentRoom.Value;
    set
    {
      if (Network.IsServer)
      {
        currentRoom.Set((int)value);
      }
    }
  }

  public UI.TextSettings LevelTextSettings = new UI.TextSettings()
  {
    Font = UI.Fonts.BarlowBold,
    Size = 27,
    Color = new Vector4(1f, 1f, 1f, 1.0f),
    DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.8f),
    DropShadowOffset = new Vector2(0f, -2f),
    HorizontalAlignment = UI.HorizontalAlignment.Center,
    VerticalAlignment = UI.VerticalAlignment.Center,
    WordWrap = false,
    Outline = true,
    OutlineThickness = 2,
  };

  public CameraControl CameraControl;

  public ThingWithHealth HealthManager;

  // Frame-synced data
  public bool IsShooting = false;
  public long FrameStartedShooting = 0;
  // Global per-player next allowed shoot frame; used to gate continuous fire regardless of effect restarts
  public long NextAllowedShootFrame = 0;
  public Vector2 FrameTargettingDirection = Vector2.Zero;
  public float FrameTargettingMagnitude = 0f;

  // The amount of each type of ammo the player has
  // AmmoType index for searching the dictionary
  // AmmoData is defined in the GameItems class and has an internal current count which represents the local player's ammo count
  public Dictionary<AmmoType, AmmoData> AmmoAmounts;

  public SyncVar<bool> PVPEnabled = new(true);
  // Server-side flag: once set to true the player will always respawn with PvP enabled
  public bool PermanentPVPEnabled = true;

  // Add a variable to track transition time for day/night cycle
  public float DayNightTransitionDuration = 3.0f;
  public float ClientTimeDayChanged = -1f;

  public SyncVar<string> InPlayerIdsHouse = new("");
  public bool IsInOwnHouse => InPlayerIdsHouse.Value == this.UserId.ToString();

  // Scavenger hunt presents collected during the current house visit
  public SyncVar<int> ScavengerHuntCount = new(0);

  public SyncVar<float> SpeedMultiplier = new(0.95f);

  // Doesn't really have to be a syncvar but easier to set around 
  public SyncVar<float> CameraZoom = new(1.55f);

  public string GenerativePrompt = "";
  public string pendingAssetId = "";

  public Entity PointToEntity;

  // Health regeneration tracking
  public float LastHealthRegenTime = -1f;

  // Player physics collider
  public Circle_Collider CircleCollider;
  public float BaseColliderRadius = 0.4f; // default was 0.333; increasing per request


  [ClientRpc]
  public void AddDash(Vector2 add, float duration)
  {
    SetFacingDirection(add.X > 0);
    Dash = add;
    DashRemainingDuration = duration;
  }

  protected void DashDecay()
  {
    if (DashRemainingDuration > 0) DashRemainingDuration -= Time.DeltaTime;
    Dash = DashRemainingDuration > 0 ? Dash : Vector2.Zero;
  }

  public Vector2 Dash = Vector2.Zero;
  protected float DashRemainingDuration;
  protected const float DashDecayThreshold = 0.1f;


  [ClientRpc]
  public void AddEnergyEffect(Player player)
  {
    if (!player.Alive()) return;

    var myPlayer = player as MyPlayer;
    if (!myPlayer.Alive()) return;
    if (myPlayer.HasEffect<SuperEnergyEffect>()) return;

    myPlayer.AddEffect<SuperEnergyEffect>();
  }

  [ClientRpc]
  public void SetInvulnerable(bool enabled, bool interruptRemove)
  {
    if (enabled)
    {
      if (!HasEffect<InvulnerabilityEffect>())
      {
        AddEffect<InvulnerabilityEffect>();
      }
    }
    else
    {
      if (HasEffect<InvulnerabilityEffect>())
      {
        RemoveEffect<InvulnerabilityEffect>(interruptRemove);
      }
    }
  }

  [ClientRpc]
  public void DisplayFTUE()
  {
    // Create FTUE dialog for new players
    Entity.Unsafe_AddComponent<FTUEDialog>();
  }


  public override void Awake()
  {
    Agent.NavmeshToLockTo = GameManager.Instance.RootNavmesh;
    rngSeed = RNG.Seed((ulong)(Time.TimeSinceStartup * 1000));

    // TODO: make this conditional on housing
    Agent.LockToNavmesh = false;

    SetupPlayerStateMachine(SpineAnimator.SpineInstance.StateMachine);

    // Stuff network spawned at runtime (like players) will run once on the server and once on the client, so we add component only on the server and then get it on the client otherwise we'd have two. 
    if (Network.IsServer)
    {
      HealthManager = Entity.Unsafe_AddComponent<ThingWithHealth>();
    }
    else
    {
      HealthManager = GetComponent<ThingWithHealth>();
    }

    // var hasSeenFTUE = Save.GetInt(this, "hasSeenFTUE", 0) == 1;
    // if (!hasSeenFTUE && Network.IsServer)
    // {
    //   Save.SetInt(this, "hasSeenFTUE", 1);
    //   CallClient_DisplayFTUE(new RPCOptions() { Target = this });
    // }

    // Ensure the player's circle collider exists and has the requested base size
    CircleCollider = GetComponent<Circle_Collider>();
    if (CircleCollider.Alive())
    {
      CircleCollider.Size = BaseColliderRadius;
      CircleCollider.Offset = new Vector2(0, 0.3f);
    }

    Agent.CustomVelocityCallback += (agent, velocity, input, dt) =>
    {
      var multiplier = SpeedMultiplier.Value;

      if (HasEffect<EnergyEffect>())
      {
        multiplier *= 1.3f;
      }

      if (HasEffect<WonkyScreenEffect>())
      {
        multiplier *= 1.75f;
      }

      if (HasEffect<SuperEnergyEffect>())
      {
        multiplier *= 1.3f;
      }

      if (HasEffect<BasicWeaponAimingEffect>())
      {
        multiplier *= 0.85f;
      }

      if (HasEffect<EatFoodEffect>())
      {
        multiplier *= 0.3f;
      }

      var velocityWithDash = velocity + (Dash * dt);
      return DefaultPlayerVelocityCalculation(velocityWithDash, input, dt, multiplier);
    };

    GameManager.Instance.IsDay.OnSync += (oldIsDay, newIsDay) =>
    {
      ClientTimeDayChanged = Time.TimeSinceStartup;
    };

    PVPEnabled.OnSync += (oldPVPEnabled, newPVPEnabled) =>
    {
      if (newPVPEnabled && IsLocal)
      {
        SFX.Play(Assets.GetAsset<AudioAsset>("sfx/pvp-enabled.wav"), new SFX.PlaySoundDesc() { Volume = 0.4f });
      }
    };

    AmmoAmounts = new()
        {
            { AmmoType.LightAmmo, new(AmmoType.LightAmmo, GameManager.Instance.GameItems.LightAmmo.ItemDefinition.Icon, 30, this) },
            { AmmoType.MediumAmmo, new(AmmoType.MediumAmmo, GameManager.Instance.GameItems.MediumAmmo.ItemDefinition.Icon, 30, this) },
            { AmmoType.HeavyAmmo, new(AmmoType.HeavyAmmo, GameManager.Instance.GameItems.HeavyAmmo.ItemDefinition.Icon, 30, this) },
            { AmmoType.Grenades, new(AmmoType.Grenades, GameManager.Instance.GameItems.Grenades.ItemDefinition.Icon, 30, this) },
            { AmmoType.ShotgunShells, new(AmmoType.ShotgunShells, GameManager.Instance.GameItems.ShotgunShells.ItemDefinition.Icon, 30, this) },

            { AmmoType.None, null },
        };

    HealthManager.OnChanged += (oldHealth, newHealth) =>
      {
        if (newHealth <= 0)
        {
          HandleDeath();
        }
        else if (newHealth < oldHealth)
        {
          SpineAnimator.SpineInstance.StateMachine.SetTrigger("flinch");
        }
      };

    if (IsLocal)
    {
      localPlayer = this;

      CallServer_ServerSyncIsPlayingOnMobile(Game.IsMobile);

      CameraControl = CameraControl.Create(0);
      CameraControl.SetPostProcessor(CustomPostProcessor);


    }

    if (Network.IsServer)
    {
      // Set XP
      XP.Set(Save.GetInt(this, "xp", 0));
      XP.OnSync += (oldXP, newXP) =>
      {
        if (newXP != 0)
        {
          Save.SetInt(this, "xp", newXP);
        }
      };

      var ammoTypesToProcess = new List<AmmoType>(AmmoAmounts.Keys);
      foreach (AmmoType ammoType in ammoTypesToProcess)
      {
        if (AmmoAmounts.TryGetValue(ammoType, out AmmoData ammoData) && ammoData != null)
        {
          string saveKey = $"ammo_{ammoType}";
          // Assuming the initial amount in AmmoData constructor (e.g., 30) is the default.
          int initialAmount = ammoData.CurrentAmount;
          ammoData.CurrentAmount = (int)Save.GetInt(this, saveKey, initialAmount);
          ammoData.RefreshFormattedAmount();

          // Sync the server's authoritative amount (loaded or default) to the client
          CallClient_SyncAmmoAmount(ammoType, ammoData.CurrentAmount);
        }
      }
    }

    Respawn("island");
  }

  [ClientRpc]
  public void SpawnCashReward(int amount, Player LastDamagedBy)
  {
    Destructable.CashRewardPrefab.Instantiate(onBeforeAwake: (entity) =>
    {
      var explodeAndLerp = entity.GetComponent<ExplodeAndLerpToPlayer>();
      explodeAndLerp.Player = LastDamagedBy;
      explodeAndLerp.Texture = Assets.GetAsset<Texture>("icons/cash.png");
      explodeAndLerp.Count = amount;
      entity.SetParent(Entity, false);
    });
  }

  public void HandleDeath()
  {
    SpineAnimator.SpineInstance.StateMachine.SetTrigger("death");

    ClearAllEffects();

    if (Network.IsServer)
    {
      // Safely resolve the player that last dealt damage
      var lastDamagedEntity = HealthManager.LastDamagedBy.Value;
      MyPlayer LastDamagedBy = null;

      // Only attempt to fetch the component if the entity is still alive – calling GetComponent on a
      // destroyed entity will throw.  This guard prevents the null-ref observed in HandleDeath().
      if (lastDamagedEntity.Alive())
      {
        LastDamagedBy = lastDamagedEntity.GetComponent<MyPlayer>();
      }

      var bounty = GetBountyReward();

      // Fire hitman elimination event so hitman jobs can listen for it
      if (LastDamagedBy != null && LastDamagedBy.Alive() && LastDamagedBy != this)
      {
        EventSystem.FireEvent(GameEventType.EliminateTarget, LastDamagedBy, this.Name);
      }

      // If it was a player that killed them
      if (LastDamagedBy != null && LastDamagedBy.Alive() && LastDamagedBy != this)
      {
        KillsThisLife.Set(0);

        // Don't increase bounty if they keep killing the same player
        if (LastDamagedBy.LastKilledPlayer != this)
        {
          LastDamagedBy.KillsThisLife.Set(LastDamagedBy.KillsThisLife.Value + 1);
          Economy.DepositCurrency(LastDamagedBy, GameManager.CASH_CURRENCY, bounty);

          if (Network.IsServer && LastDamagedBy.Alive())
          {
            CallClient_SpawnCashReward(75, LastDamagedBy);
            Economy.DepositCurrency(LastDamagedBy, GameManager.CASH_CURRENCY, 75);
          }

          // Save total kills to persistent leaderboard
          Leaderboards.IncrementPlayerScore("killsSeason1", LastDamagedBy, 1);

          // Give XP for kill
          LastDamagedBy.GainXP(100);

          // Show kill notification to the killer (include bounty + base pay)
          int totalMoneyEarned = bounty + 75;
          LastDamagedBy.CallClient_ShowKillNotification(this.Name, 100, totalMoneyEarned, false, new RPCOptions() { Target = LastDamagedBy });
        }
        else
        {
          GameManager.CallClient_SendTargetedMessage("No bounty for repeatedly killing the same player!", new RPCOptions() { Target = LastDamagedBy });
        }

        // If they had pvp on
        if (PVPEnabled.Value)
        {
          // Lose bounty when dying with pvp
          Economy.DepositCurrency(this, GameManager.CASH_CURRENCY, -bounty);
        }

        LastDamagedBy.LastKilledPlayer = this;

        // Give assist XP to players who damaged this player within 10 seconds (excluding the killer)
        foreach (var assistInfo in RecentDamageDealt)
        {
          if (assistInfo.DamagedBy != null && assistInfo.DamagedBy.Alive() &&
    assistInfo.DamagedBy != LastDamagedBy.Entity &&
    Time.TimeSinceStartup - assistInfo.DamageDealtTime <= 10f)
          {
            var assistPlayer = assistInfo.DamagedBy.GetComponent<MyPlayer>();
            if (assistPlayer.Alive())
            {
              assistPlayer.GainXP(50); // Assist XP
              GameManager.Instance.CallClient_SpawnDamageNumber(assistInfo.DamagedBy.Position + new Vector2(0, 1),
                new Vector4(0, 1, 1, 1), "ASSIST +50 XP", 1.5f, 0.0f, false);

              // Show assist notification to the assisting player (no cash for assist)
              assistPlayer.CallClient_ShowKillNotification(this.Name, 50, 0, true, new RPCOptions() { Target = assistPlayer });
            }
          }
        }

        // Clear assist tracking for this player
        RecentDamageDealt.Clear();
      }
    }
  }

  [ClientRpc]
  public void Respawn(string spawnType)
  {
    RemoveFreezeReason("dead");
    RemoveEmoteBlockReason("dead");
    SpineAnimator.SpineInstance.StateMachine.SetTrigger("spawn");

    // Position is only accurate server side so this teleport has to happen on the server
    if (Network.IsClient) return;
    // Handle PvP state depending on the permanent flag
    if (!PermanentPVPEnabled)
    {
      PVPEnabled.Set(false);
    }
    else
    {
      // Ensure PvP is enabled for players that opted in permanently
      PVPEnabled.Set(true);
    }

    // Server-side: if the player has no gun (excluding fists), grant starter Pistol and SMG
    if (Network.IsServer)
    {
      ServerEnsureStarterWeaponsIfNoGun();
    }

    if (spawnType == "general")
    {

      if (Network.IsServer)
      {
        Teleport(GetRandomSpawnPosition());
        UpdateCameraPosition(1f);
      }
    }
    else if (spawnType == "hospital")
    {
      Teleport(Entity.FindByName("HospitalSpawn").Position);
      UpdateCameraPosition(1f);
    }
    else if (spawnType == "island")
    {
      Teleport(Entity.FindByName("IslandSpawn").Position);
      UpdateCameraPosition(1f);
    }
  }

  // makes sure the camrea is in the right spot for on load assets to be actually loaded
  public void UpdateCameraPosition(float time)
  {
    if (IsLocal)
    {
      CameraControl.Position = Vector2.Lerp(CameraControl.Position, Entity.Position + new Vector2(0, 0.5f), time);
      CurrentCameraControlWorldRect = Camera.GetCurrentCameraWorldRect();
    }
  }
  public Vector2 GetRandomSpawnPosition()
  {
    var spawns = Scene.Components<Spawn_Point>();
    var spawnsList = new List<Spawn_Point>();

    foreach (var spawn in spawns)
    {
      spawnsList.Add(spawn);
    }

    var spawnIndex = RNG.RangeInt(ref rngSeed, 0, spawnsList.Count - 1);
    var spawnPoint = spawnsList[spawnIndex];
    return spawnPoint.Entity.Position;
  }

  [ClientRpc]
  public void ThrowMoney(Player player)
  {
    if (player != this) return;
    SpineAnimator.SpineInstance.StateMachine.SetTrigger("throw_money");
  }

  public static void SetupPlayerStateMachine(StateMachine sm)
  {
    var aoLayer = sm.TryGetLayerByIndex(0);

    var aoIdleState = aoLayer.TryGetStateByName("Idle");
    var aoRunState = aoLayer.TryGetStateByName("Run_Fast");
    var additiveLayer = sm.CreateLayer("additive_layer", 1);
    var movingVar = sm.TryGetVariableByName("moving");

    var use1hIk = sm.TryGetVariableByName("use_ik");
    var use2hIk = sm.CreateVariable("use_2h_ik", StateMachineVariableKind.BOOLEAN);

    var aimRun1hFast = aoLayer.CreateState("redsun/Run_Fast_mIK", 0, true);
    var aimRun2hFast = aoLayer.CreateState("redsun/Run_Fast_mIK", 0, true);
    var aimIdle1h = aoLayer.CreateState("redsun/Idle_mIK", 0, true);
    var aimIdle2h = aoLayer.CreateState("redsun/Idle_mIK", 0, true);

    //Entry
    aoLayer.CreateGlobalTransition(aoIdleState, false).CreateTriggerCondition(sm.CreateVariable("cancel_all", StateMachineVariableKind.TRIGGER));
    aoLayer.CreateTransition(aoIdleState, aimIdle1h, false).CreateBoolCondition(use1hIk, true);
    aoLayer.CreateTransition(aoIdleState, aimIdle2h, false).CreateBoolCondition(use2hIk, true);
    aoLayer.CreateTransition(aoRunState, aimRun1hFast, false).CreateBoolCondition(use1hIk, true);
    aoLayer.CreateTransition(aoRunState, aimRun2hFast, false).CreateBoolCondition(use2hIk, true);
    //Idle <-> Run
    aoLayer.CreateTransition(aimRun1hFast, aimIdle1h, false).CreateBoolCondition(movingVar, false);
    aoLayer.CreateTransition(aimIdle1h, aimRun1hFast, false).CreateBoolCondition(movingVar, true);
    aoLayer.CreateTransition(aimRun2hFast, aimIdle2h, false).CreateBoolCondition(movingVar, false);
    aoLayer.CreateTransition(aimIdle2h, aimRun2hFast, false).CreateBoolCondition(movingVar, true);
    //Exit
    aoLayer.CreateTransition(aimIdle1h, aoIdleState, false).CreateBoolCondition(use1hIk, false);
    aoLayer.CreateTransition(aimIdle2h, aoIdleState, false).CreateBoolCondition(use2hIk, false);
    aoLayer.CreateTransition(aimRun1hFast, aoRunState, false).CreateBoolCondition(use1hIk, false);
    aoLayer.CreateTransition(aimRun2hFast, aoRunState, false).CreateBoolCondition(use2hIk, false);

    var flinchState = aoLayer.CreateState("016ARP/Flinch", 0, false);
    var deathState = aoLayer.CreateState("Death_No_HP", 0, false);

    var bulletProofState = aoLayer.CreateState("016ARP/Bullet_Proof_AL", 0, false);
    var spawnInState = aoLayer.CreateState("016ARP/Spawn_In", 0, false);
    var wakeUpState = aoLayer.CreateState("016ARP/Wake_Up", 0, false);

    var flinchTrigger = sm.CreateVariable("flinch", StateMachineVariableKind.TRIGGER);
    var deathTrigger = sm.CreateVariable("death", StateMachineVariableKind.TRIGGER);
    var appleTrigger = sm.CreateVariable("apple", StateMachineVariableKind.TRIGGER);
    var bulletProofTrigger = sm.CreateVariable("bullet_proof", StateMachineVariableKind.TRIGGER);
    var energyDrinkTrigger = sm.CreateVariable("energy_drink", StateMachineVariableKind.TRIGGER);
    var spawnTrigger = sm.CreateVariable("spawn", StateMachineVariableKind.TRIGGER);
    var wakeUpTrigger = sm.CreateVariable("wake_up", StateMachineVariableKind.TRIGGER);

    // Setup transitions for existing animations
    aoLayer.CreateGlobalTransition(aoIdleState, false).CreateTriggerCondition(sm.CreateVariable("cancel_all", StateMachineVariableKind.TRIGGER));
    aoLayer.CreateGlobalTransition(flinchState).CreateTriggerCondition(flinchTrigger);
    aoLayer.CreateTransition(flinchState, aoRunState, true);

    aoLayer.CreateGlobalTransition(deathState).CreateTriggerCondition(deathTrigger);
    aoLayer.CreateTransition(deathState, aoRunState, false);

    aoLayer.CreateGlobalTransition(bulletProofState).CreateTriggerCondition(bulletProofTrigger);
    aoLayer.CreateTransition(bulletProofState, aoRunState, true);

    aoLayer.CreateGlobalTransition(spawnInState).CreateTriggerCondition(spawnTrigger);
    aoLayer.CreateTransition(spawnInState, aoRunState, true);

    aoLayer.CreateGlobalTransition(wakeUpState).CreateTriggerCondition(wakeUpTrigger);
    aoLayer.CreateTransition(wakeUpState, aoRunState, true);

    var workoutState = aoLayer.CreateState("Emote/Workout_OHP", 0, false);
    var workoutTrigger = sm.CreateVariable("workout", StateMachineVariableKind.TRIGGER);
    aoLayer.CreateGlobalTransition(workoutState).CreateTriggerCondition(workoutTrigger);
    aoLayer.CreateTransition(workoutState, aoRunState, true);

    var sitState = aoLayer.CreateState("Emote/Sit_Down", 0, false);
    var sitTrigger = sm.CreateVariable("sit", StateMachineVariableKind.TRIGGER);
    aoLayer.CreateGlobalTransition(sitState).CreateTriggerCondition(sitTrigger);
    aoLayer.CreateTransition(sitState, aoRunState, true);

    var afkState = aoLayer.CreateState("016ARP/Use_AFK_Station_Loop", 0, false);
    var afkTrigger = sm.CreateVariable("afk", StateMachineVariableKind.TRIGGER);
    aoLayer.CreateGlobalTransition(afkState).CreateTriggerCondition(afkTrigger);
    aoLayer.CreateTransition(afkState, aoRunState, true);

    var watchingYouState = aoLayer.CreateState("Emote/Im_Watching_You", 0, false);
    var watchingYouTrigger = sm.CreateVariable("watching_you", StateMachineVariableKind.TRIGGER);
    aoLayer.CreateGlobalTransition(watchingYouState).CreateTriggerCondition(watchingYouTrigger);
    aoLayer.CreateTransition(watchingYouState, aoRunState, true);

    // re-use pog
    var touchIngredientState = aoLayer.CreateState("Shoot_Gun_mIK_AL", 0, false);
    var touchIngredientTrigger = sm.CreateVariable("touch_ingredient", StateMachineVariableKind.TRIGGER);
    aoLayer.CreateGlobalTransition(touchIngredientState).CreateTriggerCondition(touchIngredientTrigger);
    aoLayer.CreateTransition(touchIngredientState, aoRunState, true);

    var throwMoneyState = aoLayer.CreateState("Emote/00. WORK FOLDER/Throw_Money", 0, false);
    var throwMoneyTrigger = sm.CreateVariable("throw_money", StateMachineVariableKind.TRIGGER);
    aoLayer.CreateGlobalTransition(throwMoneyState).CreateTriggerCondition(throwMoneyTrigger);
    aoLayer.CreateTransition(throwMoneyState, aoRunState, true);

    var wishState = aoLayer.CreateState("Collect_Item", 0, false);
    var presentState = aoLayer.CreateState("Emote/Opens_Present", 0, false);

    var wishTrigger = sm.CreateVariable("wish", StateMachineVariableKind.TRIGGER);
    var presentTrigger = sm.CreateVariable("openpresent", StateMachineVariableKind.TRIGGER);

    aoLayer.CreateGlobalTransition(wishState).CreateTriggerCondition(wishTrigger);
    aoLayer.CreateGlobalTransition(presentState).CreateTriggerCondition(presentTrigger);

    aoLayer.CreateTransition(wishState, aoRunState, true);
    aoLayer.CreateTransition(presentState, aoRunState, true);

    // Weapons

    // Additive Layer
    var idleState = additiveLayer.CreateState("__CLEAR_TRACK__", 0, true);
    additiveLayer.InitialState = idleState;

    var appleState = additiveLayer.CreateState("016ARP/Apple_AL", 0, false);
    additiveLayer.CreateGlobalTransition(appleState).CreateTriggerCondition(appleTrigger);
    additiveLayer.CreateTransition(appleState, idleState, true);

    var energyDrinkState = additiveLayer.CreateState("016ARP/Energy_Drink_AL", 0, false);
    additiveLayer.CreateGlobalTransition(energyDrinkState).CreateTriggerCondition(energyDrinkTrigger);
    additiveLayer.CreateTransition(energyDrinkState, idleState, true);

    additiveLayer.CreateGlobalTransition(idleState, false).CreateTriggerCondition(sm.TryGetVariableByName("cancel_all"));
    additiveLayer.AddSimpleTriggeredState("punch2", "016ARP/Punch_mIK_AL");
    additiveLayer.AddSimpleTriggeredState("place_block", "Place_Block_AL");
    additiveLayer.AddSimpleTriggeredState("flinch_big", "Flinch_Big");
    additiveLayer.AddSimpleTriggeredState("handless", "Handless", false, true);

    additiveLayer.AddSimpleTriggeredState("sweep", "016ARP/Clean_AL");

    additiveLayer.AddSimpleTriggeredState("handless", "Handless", false, true);
    additiveLayer.AddSimpleTriggeredState("pull_out_1h_weapon", "redsun/Pull_Out_Weapon");
    additiveLayer.AddSimpleTriggeredState("pull_out_2h_weapon", "redsun/Pull_Out_Weapon_twohands");

    additiveLayer.AddSimpleTriggeredState("put_away_1h_weapon", "redsun/Pull_Away_Weapon");
    additiveLayer.AddSimpleTriggeredState("put_away_2h_weapon", "redsun/Pull_Away_Weapon_twohands");
    additiveLayer.AddSimpleTriggeredState("shoot_1h_weapon", "redsun/Shoot_Gun_mIK_AL");
    additiveLayer.AddSimpleTriggeredState("shoot_2h_weapon", "redsun/Shoot_Gun_mIK_AL_twohands");

    // Gatling gun
    var gatlingIntro = additiveLayer.CreateState("redsun/special/Spin_Up_Gatling_Gun_mIK_AL", 0, false);
    var gatlingLoop = additiveLayer.CreateState("redsun/special/Shoot_Gatling_Gun_Loop_mIK_AL", 0, true);
    var gatlingOutro = additiveLayer.CreateState("redsun/special/Spin_Down_Gatling_Gun_mIK_AL", 0, false);
    additiveLayer.CreateTransition(idleState, gatlingLoop, false).CreateTriggerCondition(sm.CreateVariable("shoot_gatling_start", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(gatlingLoop, idleState, false).CreateTriggerCondition(sm.CreateVariable("shoot_gatling_end", StateMachineVariableKind.TRIGGER));

    // Akimbo pistols
    var akimboPistolShooting = additiveLayer.CreateState("redsun/special/Shoot_Akimbo_mIK_AL", 0, false);
    additiveLayer.CreateTransition(idleState, akimboPistolShooting, false).CreateTriggerCondition(sm.CreateVariable("shoot_akimbo_pistols", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(akimboPistolShooting, idleState, true);

    // Void splitter
    var fireVoidSplitterIntro = additiveLayer.CreateState("redsun/special/Shoot_Void_Spitter_Start", 0, false);
    var fireVoidSplitterLoop = additiveLayer.CreateState("redsun/special/Shoot_Void_Spitter_Loop", 0, true);
    var fireVoidSplitterOutro = additiveLayer.CreateState("redsun/special/Shoot_Void_Spitter_End", 0, false);
    additiveLayer.CreateTransition(idleState, fireVoidSplitterIntro, false).CreateTriggerCondition(sm.CreateVariable("void_splitter_start", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(fireVoidSplitterIntro, fireVoidSplitterLoop, true);
    additiveLayer.CreateGlobalTransition(fireVoidSplitterOutro).CreateTriggerCondition(sm.CreateVariable("void_splitter_end", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(fireVoidSplitterOutro, idleState, true);
    additiveLayer.CreateTransition(fireVoidSplitterOutro, fireVoidSplitterIntro, false).CreateTriggerCondition(sm.TryGetVariableByName("void_splitter_start"));

    // Grenade throwing
    var throwAimState = additiveLayer.CreateState("redsun/Throw_Aim_mIK_AL", 0, true);
    var throwState = additiveLayer.CreateState("redsun/Throw_mIK", 0, false);
    additiveLayer.CreateGlobalTransition(throwAimState).CreateTriggerCondition(sm.CreateVariable("throw_aim_start", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(throwAimState, idleState, false).CreateTriggerCondition(sm.CreateVariable("throw_aim_end", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(throwAimState, throwState, false).CreateTriggerCondition(sm.CreateVariable("throw", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(throwState, throwAimState, true).CreateBoolCondition(sm.CreateVariable("aiming_grenade", StateMachineVariableKind.BOOLEAN), true);
    additiveLayer.CreateTransition(throwState, idleState, true).CreateBoolCondition(sm.TryGetVariableByName("aiming_grenade"), false);

    var shootFastState = additiveLayer.CreateState("redsun/Shoot_Gun_mIK_AL_twohands_rapid", 0, true);
    additiveLayer.CreateTransition(idleState, shootFastState, false).CreateTriggerCondition(sm.CreateVariable("shoot_fast_start", StateMachineVariableKind.TRIGGER));
    additiveLayer.CreateTransition(shootFastState, idleState, false).CreateTriggerCondition(sm.CreateVariable("shoot_fast_end", StateMachineVariableKind.TRIGGER));

    additiveLayer.AddSimpleTriggeredState("throw_aim", "redsun/Throw_Aim_mIK_AL", false, true);
    additiveLayer.AddSimpleTriggeredState("throw", "redsun/Throw_mIK");

    var dustOffState = additiveLayer.CreateState("Emote/Dust_Self_Off", 0, false);
    var dustOffTrigger = sm.CreateVariable("dust_off", StateMachineVariableKind.TRIGGER);
    additiveLayer.CreateGlobalTransition(dustOffState).CreateTriggerCondition(dustOffTrigger);
    additiveLayer.CreateTransition(dustOffState, idleState, true);

    // Insert Attack_Melee_1 simple triggered state for melee attack animations
    additiveLayer.AddSimpleTriggeredState("attack_melee_1", "Attack_Melee_1");

    additiveLayer.AddSimpleTriggeredState("aim_windblaster", "redsun/Aim_Wind_Blaster_mIK_AL", false, true);
  }

  public override void Update()
  {
    HealthManager.IsInvulnerable = HasEffect<InvulnerabilityEffect>() || IsHidden.Value || CurrentRoom == Room.ISLAND;

    // Update teleport cooldown timer
    if (Network.IsServer && TeleportCooldownRemaining.Value > 0f)
    {
      TeleportCooldownRemaining.Set(Math.Max(0f, TeleportCooldownRemaining.Value - Time.DeltaTime));
    }

    PlayTimeTimer += Time.DeltaTime;
    if (Network.IsServer && PlayTimeTimer >= 10f)
    {
      PlayTimeTimer = 0f;
      Economy.DepositCurrency(this, "play_time_10s", 1);
    }

    // Scale based on damage leaderboard position
    UpdateScaleBasedOnLeaderboard();

    // Keep collider radius in sync with current visual scale
    if (CircleCollider.Alive())
    {
      float targetRadius = BaseColliderRadius * Math.Clamp(Entity.LocalScale.X, 1f, 1.2f);
      if (Math.Abs(CircleCollider.Size - targetRadius) > 0.0001f)
      {
        CircleCollider.Size = targetRadius;
      }
    }

    if (Network.IsServer)
    {
      // Ensure the player always has the "Fists" item in the first hot-bar slot
      EnsureFistsInSlotOne();

      // Only perform the behind-check every 10 frames to reduce CPU usage.
      behindCheckFrameCounter++;
      if (behindCheckFrameCounter >= 10)
      {
        behindCheckFrameCounter = 0;
        bool foundCover = false;

        // Use the quadtree to efficiently query nearby destructibles instead of iterating the full list.
        BehindCheckCache.Clear();
        GameManager.Instance.ThingWithHealthQuadtree.Query(Entity.Position, 1f, BehindCheckCache);

        foreach (var thing in BehindCheckCache)
        {
          if (!thing.Alive()) continue;
          if (!thing.Entity.Alive()) continue;

          // Skip destroyed objects
          if (thing.Health <= 0) continue;

          // If it has a child with a hidden area
          var hiddenArea = thing.Entity.TryGetChildByIndex(0)?.GetComponent<HiddenArea>();

          // Only consider hidden areas objects (they will have a Destructable component)
          if (hiddenArea == null) continue;

          foundCover = true;
          break;
        }

        IsBehindSomething = foundCover;
      }
    }

    if (Network.IsClient && IsLocal)
    {
      // Draw kill notifications
      KillNotification.DrawKillNotifications();

      // Draw XP progress bar
      XPProgressBar.DrawXPProgressBar();

      destructibleVisibilityFrameCounter++;
      if (destructibleVisibilityFrameCounter >= 10)
      {
        destructibleVisibilityFrameCounter = 0;

        OnScreenCheckCache.Clear();
        GameManager.Instance.ThingWithHealthQuadtree.Query(Entity.Position, 25f, OnScreenCheckCache);

        // Disable all first
        foreach (var desctructable in Scene.Components<Destructable>())
        {
          var sa = desctructable.SpineAnimator;
          if (sa.Alive())
          {
            sa.LocalEnabled = false;
          }
        }

        foreach (var thing in OnScreenCheckCache)
        {
          if (!thing.Alive()) continue;
          if (!thing.Entity.Alive()) continue;
          if (!thing.IsDestructable) continue;

          var sa = thing.Entity.GetComponent<Spine_Animator>();
          if (sa.Alive())
          {
            sa.LocalEnabled = true;
          }
        }
      }
    }

    if (Network.IsServer)
    {
      var shouldBeHidden = IsBehindSomething && (Time.TimeSinceStartup - LastShootTime) > 2.25f;
      IsHidden.Set(shouldBeHidden);

      // Health regeneration - tick system every second, heal completely in 1 minute (60 seconds)
      if (HealthManager.Alive() && HealthManager.Health > 0 && HealthManager.Health < HealthManager.MaxHealth)
      {
        // Initialize the timer if it hasn't been set
        if (LastHealthRegenTime < 0)
        {
          LastHealthRegenTime = Time.TimeSinceStartup;
        }

        // Check if a full second has passed
        if (Time.TimeSinceStartup - LastHealthRegenTime >= 1.0f)
        {
          // Heal MaxHealth/60 points per second to complete heal in 60 seconds
          int healAmount = Math.Max(1, HealthManager.MaxHealth / 60);
          int newHealth = Math.Min(HealthManager.MaxHealth, HealthManager.Health + healAmount);
          HealthManager.Health = newHealth;

          LastHealthRegenTime = Time.TimeSinceStartup;
        }
      }

      // Decay swole level every second independently of health
      if (LastHealthRegenTime < 0)
      {
        LastHealthRegenTime = Time.TimeSinceStartup;
      }
      if (Time.TimeSinceStartup - LastHealthRegenTime >= 1.0f)
      {
        LastHealthRegenTime = Time.TimeSinceStartup;
      }
    }

    // Track when the player died on the client
    if (Network.IsClient && HealthManager.Health <= 0 && ClientDiedAt < 0)
    {
      ClientDiedAt = Time.TimeSinceStartup;
    }
    else if (Network.IsClient && HealthManager.Health > 0)
    {
      ClientDiedAt = -1f;
    }

    if (IsHidden.Value)
    {
      if (!HasNameInvisibilityReasons())
      {
        AddNameInvisibilityReason("hidden");
      }
      SpineAnimator.SpineInstance.ColorMultiplier = new Vector4(1, 1, 1, 0.25f);
    }
    else
    {
      if (HasNameInvisibilityReasons())
      {
        RemoveNameInvisibilityReason("hidden");
      }
      SpineAnimator.SpineInstance.ColorMultiplier = new Vector4(1, 1, 1, 1);
    }

    // Dash decay runs every frame on both client and server so motion and visuals stay in sync
    DashDecay();

    if (HealthManager.DiedAt > 0f && !HasFreezeReasons())
    {
      AddEmoteBlockReason("dead");
      AddFreezeReason("dead");
    }

    if (Network.IsServer)
    {
      // Respawn at hospital after respawn cooldown
      if (HealthManager.DiedAt > 0 && Time.TimeSinceStartup - HealthManager.DiedAt > RESPAWN_TIME)
      {
        HealthManager.Reset();

        CallClient_Respawn("general");
        CallClient_SetInvulnerable(true, false);
      }
    }

    if (IsLocal)
    {
      // Update the custom aiming information in case of being on PC and this item using the special aiming system
      if (CurrentEquippedItem != null && CurrentEquippedItem.CustomDefinition.OverrideTargettingOnPC && !IsPlayingOnMobile)
      {
        var offsetToMouse = GetMousePosition() - Position;
        CurrentTargettingDirection = offsetToMouse.Normalized;
        CurrentTargettingMagnitude = offsetToMouse.Length;
      }

      // Draw the GTA 5 style "WASTED" UI when the player is dead
      if (HealthManager.DiedAt > 0 && WastedUI.startedAt == -1f)
      {
        // Calculate money lost if PVP was enabled when they died
        int moneyLost = PVPEnabled.Value ? GetBountyReward() : 0;
        WastedUI.Show(moneyLost);
      }
      else if (HealthManager.DiedAt == -1f && WastedUI.startedAt > 0)
      {
        // Reset the Wasted UI when player is alive
        WastedUI.Reset();
      }

      WastedUI.Draw();

      if (!HasEffect<SpinWheelUIEffect>())
      {
        var hotbarResult = Inventory.DrawHotbar(DefaultInventory.Id, new Inventory.DrawOptions()
        {
          ForceSelectHotbarIndex = TempHoveredSlot,
          HotbarItemCount = 7,
          AllowDragDrop = BlockScrollReasons.Count == 0,
          ScrollItemSelection = BlockScrollReasons.Count == 0,
          KeyboardItemSelection = BlockScrollReasons.Count == 0,
          EnableUseFromHotbar = false,
          EnableSelection = true,
          Rows = 5,

          OnBeforeDraw = (item, rect) =>
          {
            if (item != null)
            {
              ItemRarity? rarity = null;

              // Try to parse the rarity from the metadata first
              var rarityMetadata = item.GetMetadata("rarity");
              if (Enum.TryParse<ItemRarity>(rarityMetadata, out var parsedRarity))
              {
                rarity = parsedRarity;
              }
              else
              {
                // If that fails, try to get the default rarity for the item from the reference item
                var referenceItem = GameManager.Instance.GameItems.ItemPool.FirstOrDefault(i => i.ItemDefinition == item.Definition);
                if (referenceItem != null)
                {
                  rarity = referenceItem.ItemRarity;
                }
              }


              if (rarity.HasValue)
              {
                // Colour in the background of the item to show its rarity
                var rarityColor = GameItems.GetColorForRarity(rarity.Value);
                rarityColor.W = 0.8f;
                var highlightRect = rect.Inset(8);
                UI.Image(highlightRect, Assets.GetAsset<Texture>("$AO/new/in_game/inventory/inventory_v2/inventory_bubble.png"), rarityColor);
              }
            }
          },


          OnAfterDraw = (item, rect) =>
          {
            // Check if it's a weapon first using our quick lookup stuff
            var referenceItem = GameManager.Instance.GameItems.ItemPool.FirstOrDefault(i => i.ItemDefinition == item.Definition);

            if (item == null || referenceItem == null)
            {
              return;
            }

            // Draw the ammo count for each type of weapon
            if (item != null && referenceItem.ItemCategory == ItemCategory.Weapon)
            {
              var weaponReference = referenceItem as Weapon;
              var ammoData = weaponReference.AmmoType;

              if (ammoData != null)
              {
                // Draw the ammo icon
                var ammoRect = rect.TopLeftRect().Offset(16, -16).Grow(23, 23, 23, 23);
                UI.Image(ammoRect, Assets.GetAsset<Texture>(ammoData.ItemDefinition.Icon), Vector4.White);

                // Draw the ammo count to the left of the icon
                var textSettings = UIUtils.GetTextSettings(28, Vector4.White, UI.HorizontalAlignment.Left);
                UI.TextAsync(ammoRect.RightCenterRect().Offset(-5, 0), $"x{weaponReference.GetRPAmmoAmount(this)}", textSettings);
              }

              // Draw weapon level if it exists (where stars used to be)
              var levelMetadata = item.GetMetadata("level");
              if (!string.IsNullOrEmpty(levelMetadata) && int.TryParse(levelMetadata, out int weaponLevel))
              {
                // Check if this item is currently selected/highlighted
                bool isHighlighted = CurrentHoveredSlot < DefaultInventory.Items.Length && DefaultInventory.Items[CurrentHoveredSlot] == item;

                // Position at bottom center where stars used to be
                var levelRect = rect.BottomCenterRect().Offset(0, 8);

                UI.TextAsync(levelRect, $"Lv.{weaponLevel}", LevelTextSettings);
              }
            }
          },
        });

        TempHoveredSlot = -1;

        InventoryOpen = hotbarResult.InventoryOpen;

        // Draw the selected item name 100px above the hotbar
        if (hotbarResult.SelectedItem != null)
        {
          var itemNameRect = hotbarResult.EntireRect.TopCenterRect().Offset(0, 40);
          var textSettings = UIUtils.GetTextSettings(48, Vector4.White, UI.HorizontalAlignment.Center);
          UI.TextAsync(itemNameRect, hotbarResult.SelectedItem.Definition.Name, textSettings);
        }

        if (HoveredSlotLastFrame != hotbarResult.SelectedItemIndex)
        {
          CallServer_RequestChangeHoveredSlot(hotbarResult.SelectedItemIndex);
        }

        HoveredSlotLastFrame = hotbarResult.SelectedItemIndex;
      }

      // Refresh equipped item if the item in the current slot changed (e.g. from drag-move)
      MaybeRefreshEquippedItemFromHoveredSlot();

      // Update the custom aiming information in case of being on PC and this item using the special aiming system
      if (CurrentEquippedItem != null && CurrentEquippedItem.CustomDefinition.OverrideTargettingOnPC && !IsPlayingOnMobile)
      {
        var offsetToMouse = GetMousePosition() - Position;
        CurrentTargettingDirection = offsetToMouse.Normalized;
        CurrentTargettingMagnitude = offsetToMouse.Length;
      }

      DrawDamageNumber();

      List<Ability> abilities = new List<Ability>();
      if (CurrentEquippedItem != null && CurrentEquippedItem.CustomDefinition.GetPrimaryAbility(this) != null)
      {
        abilities.Add(CurrentEquippedItem.CustomDefinition.GetPrimaryAbility(this));

        var secondaryAbilities = CurrentEquippedItem.CustomDefinition.GetSecondaryAbilities(this);
        if (secondaryAbilities != null && secondaryAbilities.Count > 0)
        {
          abilities.AddRange(secondaryAbilities);
        }
      }
      else
      {
        abilities.Add(GetAbility<PunchAbility>());
      }

      // Add baseline dodge roll ability for all players
      abilities.Add(GetAbility<AbilityDodgeRoll>());

      // abilities.Add(GetAbility<BushDisguiseAbility>());
      DrawDefaultAbilityUI(new AbilityDrawOptions()
      {
        AbilityElementSize = 125,
        Abilities = abilities.ToArray()
      });

      CashDisplay.DrawMoney((int)Economy.GetBalance(this, GameManager.CASH_CURRENCY));
    }

    if (!IsPlayingOnMobile && CurrentEquippedItem != null)
    {
      if (IsInputDown(Input.UnifiedInput.MOUSE_LEFT))
      {
        CurrentEquippedItem.CustomDefinition.TryHandleMouseDown(this);
      }
      else if (IsInputUp(Input.UnifiedInput.MOUSE_LEFT))
      {
        CurrentEquippedItem.CustomDefinition.TryHandleMouseUp(this);
      }
    }
  }

  public override void LateUpdate()
  {
    // Client-side only zoom modifier (no SyncVar writes) is applied in CustomPostProcessor.
    if (IsLocal && !Network.IsServer)
    {

      GunButtonsUI.DrawSidebarButtons();
      UIManager.DrawUI(Position);

      if (!UI.IsChatOpen())
      {
        var leaderboardData = DamageTracker.Instance.GetClientLeaderboardData();
        // Limit to top 6 players
        var top6Players = leaderboardData.OrderByDescending(x => x.Points).Take(6).ToList();
        GameManager.DrawLeaderboard(top6Players, "Damage Dealt (120s)", Name);
      }

      if (PointToEntity.Alive())
      {
        // Little offset so it looks like it's above it
        var worldOffset = PointToEntity.Position - Entity.Position;

        var sellAreaScreenPos = Camera.WorldToScreen(PointToEntity.Position);
        var playerScreenPos = Camera.WorldToScreen(Entity.Position + new Vector2(0, 0.5f));
        var dir = (sellAreaScreenPos - playerScreenPos).Normalized;
        var pos = playerScreenPos;
        var distance = worldOffset.Length;

        float arrowSize = 50;
        var anim = (float)Math.Pow(Math.Abs(Math.Sin(Math.PI * Time.TimeSinceStartup)), 0.75);
        float distanceThreshold = 4.25f;
        if (distance >= (distanceThreshold + 0.5f))
        {
          var t = 1 - Ease.T(distance - distanceThreshold, 1);
          var arrowScreenPos = new Rect(pos, pos).Offset(dir.X * 300, dir.Y * 300).Center; // note(josh): using rects to scale by screen size
          arrowScreenPos = Vector2.Lerp(arrowScreenPos, sellAreaScreenPos, t);
          var rect = new Rect(arrowScreenPos, arrowScreenPos).Grow(arrowSize);
          var rotation = Math.Atan2(dir.Y, dir.X) * (180.0 / Math.PI);
          UI.Image(rect, portalPointerAsset, Vector4.White, default, (float)rotation);
        }
        else
        {
          var rect = new Rect(sellAreaScreenPos, sellAreaScreenPos).Grow(arrowSize);
          rect = rect.Offset(0, (anim * 50) + 75);
          UI.Image(rect, portalPointerAsset, Vector4.White, default, 270);

          if (PointToEntity.GetComponent<Destructable>() != null)
          {
            UI.Image(rect.Inset(15), Assets.GetAsset<Texture>("icons/weapons/punch-active.png"), Vector4.White);
          }
        }
      }

      // Display Swoleness level in bottom-right corner when applicable
      // if (SwoleLevel.Value > 0f)
      // {
      //   var swoleRect = UI.SafeRect.BottomRightRect().Offset(-220, 45);
      //   float swoleDisplay = (float)Math.Round(SwoleLevel.Value, 1);
      //   var swoleTextSettings = UIUtils.GetTextSettings(28, Vector4.White, UI.HorizontalAlignment.Right);
      //   UI.TextAsync(swoleRect, $"Swoleness ({swoleDisplay}/5)", swoleTextSettings);
      // }
    }

    if (Network.IsClient)
    {
      // Draw name decorations only for the local player if they're hidden
      var drawNameDecorations = (IsHidden && IsLocal) || !IsHidden;

      if (drawNameDecorations && Camera.GetCurrentCameraWorldRect().Overlaps(FinalNameRect))
      {
        HealthBar.DrawHealthBar(FinalNameRect, HealthManager.Health, HealthManager.MaxHealth, "blue", this);

        // Draw player level next to name
        DrawPlayerLevel();

        if (PVPEnabled.Value && GetBountyReward() > 0)
        {
          BountyDisplay.DrawBountyReward(Entity, GetBountyReward(), GetBountyTier());
        }
      }
    }

    // World-space weapon cooldown indicator for local player
    if (Network.IsClient && IsLocal)
    {
      WeaponCooldownIndicator.DrawForPlayerGun(this);
    }

    // This has to be at the end
    if (IsLocal)
    {
      CameraControl.Position = Vector2.Lerp(CameraControl.Position, Entity.Position + new Vector2(0, 0.5f), 0.5f);
      CurrentCameraControlWorldRect = Camera.GetCurrentCameraWorldRect();
    }
  }


  public Rect CurrentCameraControlWorldRect;

  public void CustomPostProcessor(CameraControl camera)
  {
    // ------------------------------------------------------------------
    // Dynamic camera zoom
    // ------------------------------------------------------------------
    float baseZoom = CameraZoom.Value;           // Server-authoritative baseline

    float vehicleTarget = baseZoom;

    // Smoothly lerp towards the target vehicle zoom (or baseline) over 0.3 s
    float lerpT_Zoom = Math.Min(1f, Time.DeltaTime / 0.3f);
    var zoom = AOMath.Lerp(camera.Zoom, vehicleTarget, lerpT_Zoom);

    if (HealthManager.Health <= 0)
    {
      float timeSinceDeath = Time.TimeSinceStartup - ClientDiedAt;
      float lerpValue;

      // Animation duration for initial zoom effect
      float zoomAnimDuration = 1f;
      // Longer duration for the color fade effect
      float colorFadeDuration = 3f;

      if (timeSinceDeath <= colorFadeDuration && ClientDiedAt > 0)
      {
        // Calculate lerpValue with a longer fade (3.15 to 1.0 over colorFadeDuration)
        float colorT = Math.Min(timeSinceDeath / colorFadeDuration, 1.0f);
        float colorEaseOut = 1 - (float)Math.Pow(1 - colorT, 3);
        lerpValue = 3.15f - (2.15f * colorEaseOut);

        // Zoom calculation remains with original duration
        if (timeSinceDeath <= zoomAnimDuration)
        {
          float zoomT = Math.Min(timeSinceDeath / zoomAnimDuration, 1.0f);
          float easeOutCubic = 1 - (float)Math.Pow(1 - zoomT, 3);
          zoom = 1.6f + (0.6f * easeOutCubic);
        }
        else
        {
          zoom = 2.2f;
        }
      }
      else
      {
        lerpValue = 1.0f;
        zoom = 2.2f;
      }

      PostProcessing.ColorGrade(new PostProcessing.ColorGradeConfig()
      {
        Contrast = 1f,
        Saturation = 0f,
        ColorFilter = new Vector3(lerpValue, lerpValue, lerpValue)
      });
    }
    else
    {
      // Calculate how far along we are in the day/night transition using client-side time reference
      float timeSinceChanged = Time.TimeSinceStartup - ClientTimeDayChanged;
      float transitionProgress = Math.Min(timeSinceChanged / DayNightTransitionDuration, 1.0f);

      // Default day values
      Vector3 dayColorFilter = new Vector3(1.2f, 1.1f, 1.0f);
      float daySaturation = 1.0f;

      // Night values
      Vector3 nightColorFilter = new Vector3(0.4f, 0.7f, 2.7f);
      float nightSaturation = 0.5f;

      if (GameManager.Instance.IsDay.Value)
      {
        // Transitioning from night to day
        Vector3 lerpedColorFilter = new Vector3(
          AOMath.Lerp(nightColorFilter.X, dayColorFilter.X, transitionProgress),
          AOMath.Lerp(nightColorFilter.Y, dayColorFilter.Y, transitionProgress),
          AOMath.Lerp(nightColorFilter.Z, dayColorFilter.Z, transitionProgress)
        );
        float lerpedSaturation = AOMath.Lerp(nightSaturation, daySaturation, transitionProgress);

        PostProcessing.ColorGrade(new PostProcessing.ColorGradeConfig()
        {
          Contrast = 1f,
          Saturation = lerpedSaturation,
          ColorFilter = lerpedColorFilter
        });
      }
      else
      {
        // Transitioning from day to night
        Vector3 lerpedColorFilter = new Vector3(
          AOMath.Lerp(dayColorFilter.X, nightColorFilter.X, transitionProgress),
          AOMath.Lerp(dayColorFilter.Y, nightColorFilter.Y, transitionProgress),
          AOMath.Lerp(dayColorFilter.Z, nightColorFilter.Z, transitionProgress)
        );
        float lerpedSaturation = AOMath.Lerp(daySaturation, nightSaturation, transitionProgress);

        PostProcessing.ColorGrade(new PostProcessing.ColorGradeConfig()
        {
          Contrast = 1f,
          Saturation = lerpedSaturation,
          ColorFilter = lerpedColorFilter
        });
      }
    }

    camera.Zoom = zoom;

    // --------------------------------------------------
    // Global explosion flash post-processing
    // --------------------------------------------------
    if (ExplosionFlashStartTime > 0)
    {
      float flashElapsed = Time.TimeSinceStartup - ExplosionFlashStartTime;
      if (flashElapsed <= EXPLOSION_FLASH_DURATION)
      {
        float progress = flashElapsed / EXPLOSION_FLASH_DURATION;

        // Replicate the dramatic nuke flash: bright white -> normal -> fade
        float intensity;
        if (progress < 0.3f)
        {
          float flashT = progress / 0.3f;
          intensity = 1.0f - (1.0f * flashT); // 3.0 → 1.0
        }
        else
        {
          // Hold at normal brightness – no fade to black
          intensity = 0.5f;
        }

        PostProcessing.Bloom(new PostProcessing.BloomConfig()
        {
          BloomAmount = 0.2f * MathF.Max(intensity, 0.1f)
        });

        PostProcessing.ColorGrade(new PostProcessing.ColorGradeConfig()
        {
          Contrast = 1f,
          Saturation = 1f,
          ColorFilter = new Vector3(1.5f * intensity, 1.5f * intensity, 1.5f * intensity)
        });
      }
      else
      {
        // Reset so we don't keep evaluating every frame
        ExplosionFlashStartTime = -100f;
      }
    }
  }

  protected void DrawDamageNumber()
  {
    using var _1 = UI.PUSH_CONTEXT(UI.Context.WORLD);
    using var _2 = UI.PUSH_LAYER(5);

    List<DamageNumbers> numbers = GameManager.ActiveDamageNumbers;
    for (int i = numbers.Count - 1; i >= 0; i -= 1)
    {
      var result = numbers[i];
      float speed = 1f;
      var ts = result.TextSettings;

      result.T += Time.DeltaTime * speed;
      if (result.T >= 1 && result.DoingFading)
      {
        numbers.UnorderedRemoveAt(i);
        continue;
      }
      if (result.T >= 0.6f && !result.DoingFading)
      {
        result.T = 0.0f;
        result.DoingFading = true;
      }

      if (!result.DoingFading)
      {
        var pos = result.Position;
        pos.Y += AOMath.Lerp(0, 0.5f, Ease.OutQuart(result.T));
        var color01 = Ease.FadeInAndOut(0.1f, 1f, result.T);
        ts.Color = Vector4.Lerp(ts.Color, result.Color, color01);
        result.LastPosition = pos;
      }
      else
      {
        ts.SpacingMultiplier = 1f;
        var colorAlpha = Vector4.Zero;
        ts.Color = Vector4.Lerp(ts.Color, colorAlpha, result.T);
      }

      var rect = new Rect(result.LastPosition, result.LastPosition);
      UI.TextAsync(rect, result.Text, ts);
    }
  }

  public bool ServerTryAddItem(Item_Definition itemDef, int count = 1, bool sendMessage = false, List<(string, string)> metadata = null)
  {
    if (!Network.IsServer) return false;
    if (itemDef == null) return false;

    int numStacks = (count / itemDef.StackSize) + 1;
    int numItemsLeftToAdd = count;

    // If the number of items is bigger than one stack size, we'll keep adding stacks until we have everything added
    for (int i = 0; i < numStacks && numItemsLeftToAdd > 0; i++)
    {
      int numItemsInThisStack = Math.Min(itemDef.StackSize, numItemsLeftToAdd);
      var itemInstance = Inventory.CreateItem(itemDef, numItemsInThisStack);

      numItemsLeftToAdd -= numItemsInThisStack;

      if (itemInstance != null && Inventory.CanMoveItemToInventory(itemInstance, DefaultInventory, out var _))
      {
        Inventory.MoveItemToInventory(itemInstance, DefaultInventory);

        if (metadata != null && itemDef.StackSize == 1) // Can only set metadata on non-stackable items
        {
          foreach (var (key, value) in metadata)
          {
            itemInstance.SetMetadata(key, value);
          }
        }
      }
      else
      {
        return false;
      }
    }

    if (sendMessage)
    {
      Chat.SendMessage(this, $"Added x{count} {itemDef.Name} split into {numStacks} stacks.");
    }

    CallClient_UpdateCurrentHoveredSlot();

    return true;
  }

  public bool ServerTryRemoveItem(Item_Definition itemDef)
  {
    if (!Network.IsServer) return false;

    foreach (var item in DefaultInventory.Items)
    {
      if (item != null && item.Definition == itemDef)
      {
        Inventory.RemoveItemFromInventory(item, DefaultInventory);
        CallClient_UpdateCurrentHoveredSlot();

        return true;
      }
    }

    return false;
  }

  [ServerRpc]
  public void RequestRemoveItemCountFromSlot(long slot, long count)
  {
    var instance = DefaultInventory.Items[slot];
    if (instance == null)
      return;

    if (instance.Quantity - count <= 0)
    {
      Inventory.RemoveItemFromInventory(instance, DefaultInventory);
      CallClient_UpdateCurrentHoveredSlot();
    }
    else
    {
      CallClient_SetSlotNewQuantity(slot, instance.Quantity - count);
    }
  }

  [ServerRpc]
  public void RequestChangeHoveredSlot(int slot) => CallClient_SetCurrentHoveredSlot(slot);

  [ClientRpc]
  public void SetCurrentHoveredSlot(int slot)
  {
    CurrentHoveredSlot = slot;

    UpdateCurrentHoveredSlot();
  }

  [ClientRpc]
  public void UpdateCurrentHoveredSlot()
  {
    var item = DefaultInventory.Items[CurrentHoveredSlot];
    UpdateEquippedItem(item);
  }

  // Called every frame on the client to detect when the item in the hovered slot has changed
  // (e.g. from drag-and-drop moves) and refresh the equipped item/ability UI accordingly.
  public void MaybeRefreshEquippedItemFromHoveredSlot()
  {
    if (DefaultInventory == null) return;
    if (CurrentHoveredSlot < 0 || CurrentHoveredSlot >= DefaultInventory.Items.Length) return;

    var currentItem = DefaultInventory.Items[CurrentHoveredSlot];

    // Check if slot index or item reference changed
    bool slotChanged = _LastObservedHoveredSlotIndex != CurrentHoveredSlot;
    bool itemChanged = currentItem != _LastObservedHoveredSlotItem;

    if (slotChanged || itemChanged)
    {
      _LastObservedHoveredSlotIndex = CurrentHoveredSlot;
      _LastObservedHoveredSlotItem = currentItem;
      UpdateEquippedItem(currentItem);
    }
  }

  public void UpdateEquippedItem(Item_Instance newEquippedItem)
  {
    if (CurrentEquippedItem != null && CurrentEquippedItem.Instance == newEquippedItem) return;
    if (CurrentEquippedItem == null && newEquippedItem == null) return;

    var newItemName = newEquippedItem != null ? newEquippedItem.Definition.Name : "null";
    Log.Info($"{Name}: Updating equipped item to {newItemName}");

    if (CurrentEquippedItem != null)
    {
      CurrentEquippedItem.CustomDefinition.OnUnequip(this);
      CurrentEquippedItem = null;
    }

    if (newEquippedItem != null)
    {
      if (GameItems.TryCreateCustomInstance(newEquippedItem, out var newItem)
          && newItem.CustomDefinition.IsEquippableByPlayer(this))
      {
        CurrentEquippedItem = newItem;
        CurrentEquippedItem.CustomDefinition.OnEquip(this);

        if (IsLocal && CurrentEquippedItem.CustomDefinition.ItemCategory == ItemCategory.Weapon)
        {
          SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/equip_gun.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Entity, Volume = 0.25f, VolumePerturb = 0.1f, SpeedPerturb = 0.1f });
        }
      }
    }
  }

  [ClientRpc]
  public void SetSlotNewQuantity(long slot, long newQuantity)
  {
    var instance = DefaultInventory.Items[slot];
    if (instance == null)
      return;

    instance.Quantity = newQuantity;

    UpdateCurrentHoveredSlot();
  }

  [ServerRpc]
  public void ServerSyncAmmoAmount(AmmoType ammoType, int amount)
  {
    if (!Network.IsServer) return;

    if (AmmoAmounts.TryGetValue(ammoType, out AmmoData ammoData) && ammoData != null)
    {
      string saveKey = $"ammo_{ammoType}";
      Save.SetInt(this, saveKey, amount);

      CallClient_SyncAmmoAmount(ammoType, amount);
    }
    else
    {
      Log.Error($"ServerSyncAmmoAmount: Invalid ammoType {ammoType} or null AmmoData for player {this.UserId}");
    }
  }

  [ClientRpc]
  public void SyncAmmoAmount(AmmoType ammoType, int amount)
  {
    AmmoAmounts[ammoType].CurrentAmount = amount;
    AmmoAmounts[ammoType].RefreshFormattedAmount();
  }


  [ServerRpc]
  public void ServerSyncIsPlayingOnMobile(bool isPlayingOnMobile) => CallClient_SyncIsPlayingOnMobile(isPlayingOnMobile);

  [ClientRpc]
  public void SyncIsPlayingOnMobile(bool isPlayingOnMobile)
  {
    IsPlayingOnMobile = isPlayingOnMobile;
  }

  [ClientRpc]
  public void ShowKillNotification(string killedPlayerName, int xpAmount, int moneyAmount, bool isAssist)
  {
    KillNotification.ShowKillNotification(killedPlayerName, xpAmount, moneyAmount, isAssist);
  }


  public int GetBountyReward()
  {
    float bountyPercentage = 0;
    int tier = GetBountyTier();

    // Set percentage based on tier
    switch (tier)
    {
      case 1:
        bountyPercentage = 0.10f; // 10%
        break;
      case 2:
        bountyPercentage = 0.20f; // 20%
        break;
      case 3:
        bountyPercentage = 0.30f; // 30%
        break;
      case 4:
        bountyPercentage = 0.40f; // 40%
        break;
      default:
        bountyPercentage = 0; // No bounty for tier 0
        break;
    }

    return (int)Math.Min(25000, Economy.GetBalance(this, GameManager.CASH_CURRENCY) * bountyPercentage);
  }

  public int GetBountyTier()
  {
    return Math.Clamp(KillsThisLife.Value, 0, 4);
  }

  public override void ReadFrameData(AO.StreamReader reader)
  {
    IsShooting = reader.Read<bool>();
    FrameStartedShooting = reader.Read<long>();
    FrameTargettingDirection = reader.Read<Vector2>();
    FrameTargettingMagnitude = reader.Read<float>();
    CurrentHoveredSlot = reader.Read<int>();
    UpdateCurrentHoveredSlot();

    CurrentTargettingDirection = FrameTargettingDirection;
    CurrentTargettingMagnitude = FrameTargettingMagnitude;
  }

  public override void WriteFrameData(AO.StreamWriter writer)
  {
    writer.Write(IsShooting);
    writer.Write(FrameStartedShooting);
    writer.Write(CurrentTargettingDirection);
    writer.Write(CurrentTargettingMagnitude);
    writer.Write(CurrentHoveredSlot);
  }


  // Removes X amount of an item from the inventory
  public void ServerConsumeItemWithCount(Item_Definition itemDef, long count = 1)
  {
    if (!Network.IsServer) return;

    // Find the slot with the lowest quantity of the item
    // We'll remove from this slot first, and then if there's still more to remove,
    // we'll call this function again with the leftover quantity
    int slotWithLowestQuantity = -1;
    long lowestQuantity = long.MaxValue;

    for (int i = 0; i < DefaultInventory.Items.Length; i++)
    {
      var item = DefaultInventory.Items[i];

      if (item != null && item.Definition == itemDef)
      {
        if (item.Quantity < lowestQuantity)
        {
          lowestQuantity = item.Quantity;
          slotWithLowestQuantity = i;
        }
      }
    }

    if (slotWithLowestQuantity != -1)
    {
      // If this is negative, we will have to consume more from another slot
      var leftoverQuantity = lowestQuantity - count;

      if (leftoverQuantity <= 0)
      {
        // Eliminate this stack entirely if it's now empty
        Inventory.RemoveItemFromInventory(DefaultInventory.Items[slotWithLowestQuantity], DefaultInventory);
      }
      else
      {
        // Otherwise, just reduce the quantity of this stack
        CallClient_SetSlotNewQuantity(slotWithLowestQuantity, leftoverQuantity);
      }

      // If there's still more to consume, we'll consume from the next smallest stack until we have consumed the total amount
      if (leftoverQuantity < 0)
      {
        ServerConsumeItemWithCount(itemDef, Math.Abs(leftoverQuantity));
      }
    }
    else
    {
      Log.Warn($"No slot found for {itemDef.Name} after trying to consume {count} of them");
    }
  }


  public long GetItemCount(Item_Definition def)
  {
    long count = 0;

    foreach (var item in DefaultInventory.Items)
    {
      if (item != null && item.Definition == def)
        count += item.Quantity;
    }

    return count;
  }

  // --------------------------------------------------------------------------------------
  // Utility – guarantees players always have Fists in slot 0 of their default inventory.
  // Runs server-side every frame from Update().
  // --------------------------------------------------------------------------------------
  public void EnsureFistsInSlotOne()
  {
    if (DefaultInventory == null) return;

    var fistsDef = GameManager.Instance.GameItems.Fists.ItemDefinition;

    // Safety: inventory might be initialising
    var items = DefaultInventory.Items;
    if (items == null || items.Length == 0) return;

    // If fists already occupy slot 0 we're done
    if (items[0] != null && items[0].Definition == fistsDef)
    {
      return;
    }

    long fistsSlot = -1;
    long emptySlot = -1;

    // Scan inventory once to locate fists or first empty slot
    for (long i = 0; i < items.Length; i++)
    {
      var itm = items[i];
      if (itm == null)
      {
        if (emptySlot == -1) emptySlot = i;
      }
      else if (itm.Definition == fistsDef)
      {
        fistsSlot = i;
      }
    }

    // 1) If fists are somewhere else in the inventory – swap them into slot 0
    if (fistsSlot >= 0)
    {
      Inventory.SwapItems(DefaultInventory, DefaultInventory, fistsSlot, 0);
      return;
    }

    // 2) Fists not present – create them if there is room
    if (emptySlot != -1)
    {
      var newFistsInstance = Inventory.CreateItem(fistsDef, 1);
      if (newFistsInstance != null && Inventory.CanMoveItemToInventory(newFistsInstance, DefaultInventory, out var _))
      {
        Inventory.MoveItemToInventory(newFistsInstance, DefaultInventory);
        // Next Update() will swap to slot 0 if necessary
      }
    }
    // 3) If inventory is full (no emptySlot) we'll try again in a future frame.
  }

  // --------------------------------------------------------------------
  // Grants starter weapons if the player currently has no non-fists guns
  // Runs server-side only
  // --------------------------------------------------------------------
  public void ServerEnsureStarterWeaponsIfNoGun()
  {
    if (!Network.IsServer) return;
    if (DefaultInventory == null) return;

    var items = DefaultInventory.Items;
    if (items == null || items.Length == 0) return;

    bool hasNonFistsWeapon = false;
    for (int i = 0; i < items.Length; i++)
    {
      var inst = items[i];
      if (inst == null || inst.Definition == null) continue;
      var defId = inst.Definition.Id;
      // Any weapon that isn't fists counts as having a gun
      if (!string.IsNullOrEmpty(defId) && defId.StartsWith("__WEAPON__") && defId != "__WEAPON__fists")
      {
        hasNonFistsWeapon = true;
        break;
      }
    }

    if (hasNonFistsWeapon) return;

    // Grant Common, Level 1 Pistol and Submachine Gun
    var metadata = new List<(string, string)>()
    {
      ("rarity", ItemRarity.Common.ToString()),
      ("level", "1")
    };

    var pistolDef = GameManager.Instance.GameItems.Pistol.ItemDefinition;
    var smgDef = GameManager.Instance.GameItems.SubmachineGun.ItemDefinition;

    ServerTryAddItem(pistolDef, 1, false, metadata);
    ServerTryAddItem(smgDef, 1, false, metadata);

    // Ensure at least 100 Light ammo to start using the weapons immediately
    if (AmmoAmounts != null && AmmoAmounts.TryGetValue(AmmoType.LightAmmo, out var lightAmmo) && lightAmmo != null)
    {
      int desired = 100;
      int newAmount = Math.Max(desired, lightAmmo.CurrentAmount);
      if (newAmount != lightAmmo.CurrentAmount)
      {
        // Persist and sync to client
        ServerSyncAmmoAmount(AmmoType.LightAmmo, newAmount);
      }
      else
      {
        // Still sync to ensure client is authoritative even if already >= 100
        ServerSyncAmmoAmount(AmmoType.LightAmmo, newAmount);
      }
    }
  }

  public void NetworkSerialize(AO.StreamWriter writer)
  {
    writer.Write(CurrentHoveredSlot);
  }

  public void NetworkDeserialize(AO.StreamReader reader)
  {
    CurrentHoveredSlot = reader.Read<int>();
    TempHoveredSlot = CurrentHoveredSlot;
  }

  // Super inlined cached gpt version for rarity lookup in hot hotbar loop 
  static readonly Dictionary<string, ItemRarity> _rarityCache = new(capacity: 256);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static ItemRarity GetRarity(Item_Definition def)
  {
    // Fast path: dictionary hit
    if (_rarityCache.TryGetValue(def.Id, out var rarity))
      return rarity;

    // Slow path: compute once, then cache
    rarity = ComputeRarity(def.Id);
    _rarityCache[def.Id] = rarity;
    return rarity;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static ItemRarity ComputeRarity(string id)
  {
    ReadOnlySpan<char> span = id;          // zero-alloc
    if (span.Length < 3 || span[0] != '_' || span[1] != '_')
      return ItemRarity.Common;

    return span[2] switch              // only one branch + jump table
    {
      'C' or 'B' or 'P' => ItemRarity.Rare,      // CAR / BOAT / PLANE
      'W' => ItemRarity.Epic,      // WEAPON
      'H' => ItemRarity.Mythic,    // HEALING
      _ => ItemRarity.Common
    };
  }

  // --------------------------------------------------------------------------------------
  // Utility – stable, fast, non-cryptographic hash for UserId bucketing
  // --------------------------------------------------------------------------------------
  public static uint ComputeStableHash(string str)
  {
    unchecked
    {
      const uint fnvOffset = 2166136261u;
      const uint fnvPrime = 16777619u;
      uint hash = fnvOffset;

      // FNV-1a 32-bit
      for (int i = 0; i < str.Length; i++)
      {
        hash ^= str[i];
        hash *= fnvPrime;
      }
      return hash;
    }
  }

  // Level system methods
  public static int CalculateLevelFromXP(int xp)
  {
    const int MAX_LEVEL = 50;

    if (xp <= 0) return 1;

    // Find the highest level where the cumulative XP requirement is less than or equal to the current XP
    for (int level = 1; level <= MAX_LEVEL; level++)
    {
      int xpRequired = CalculateXPForLevel(level);
      if (xp < xpRequired)
      {
        return Math.Clamp(level - 1, 1, MAX_LEVEL);
      }
    }

    return MAX_LEVEL;
  }

  public static int CalculateXPForLevel(int level)
  {
    if (level <= 1) return 0;

    // Calculate cumulative XP to reach this level
    // Formula: XP to level N = 150 * N^1.3
    int totalXP = 0;
    for (int i = 2; i <= level; i++)
    {
      totalXP += (int)(150 * Math.Pow(i - 1, 1.3));
    }
    return totalXP;
  }

  public void GainXP(int amount)
  {
    if (!Network.IsServer) return;
    if (Level >= 50) return; // Level cap

    int oldLevel = Level;
    XP.Set(XP.Value + amount);
    Economy.DepositCurrency(this, GameManager.XP_CURRENCY, amount);
    int newLevel = Level;

    if (newLevel > oldLevel)
    {
      CallClient_LevelUp(newLevel);
    }
  }

  public void RecordDamageForAssist(Entity damagedBy, int damageAmount)
  {
    if (!Network.IsServer) return;
    if (damagedBy == null || !damagedBy.Alive()) return;

    // Find existing assist info for this damager
    var existingAssist = RecentDamageDealt.Find(assist => assist.DamagedBy == damagedBy);

    if (existingAssist != null)
    {
      // Update existing record
      existingAssist.DamageDealtTime = Time.TimeSinceStartup;
      existingAssist.TotalDamage += damageAmount;
    }
    else
    {
      // Add new record
      RecentDamageDealt.Add(new AssistInfo
      {
        DamagedBy = damagedBy,
        DamageDealtTime = Time.TimeSinceStartup,
        TotalDamage = damageAmount
      });
    }

    // Clean up old damage records (older than 10 seconds)
    RecentDamageDealt.RemoveAll(assist => Time.TimeSinceStartup - assist.DamageDealtTime > 10f);
  }

  public void DrawPlayerLevel()
  {
    if (!Network.IsClient) return;

    using var _1 = UI.PUSH_CONTEXT(UI.Context.WORLD);
    using var _2 = IM.PUSH_Z(GetZOffset() - 0.001f);
    using var _3 = UI.PUSH_SCALE_FACTOR(5.0f / 540.0f);

    var levelText = $"Lv.{Level}";

    // Create text settings for the level display
    var ts = new UI.TextSettings()
    {
      Font = UI.Fonts.BarlowBold,
      Size = 25f,
      Color = new Vector4(1.0f, 1.0f, 0.0f, 1f), // Yellow color
      DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.5f),
      DropShadowOffset = new Vector2(0f, -0.01f),
      HorizontalAlignment = UI.HorizontalAlignment.Center,
      VerticalAlignment = UI.VerticalAlignment.Center,
      WordWrap = false,
      Outline = true,
      OutlineThickness = 2,
      Offset = Vector2.Zero
    };

    UI.TextAsync(FinalNameRect.RightCenterRect().Offset(27, 0), levelText, ts);
  }

  [ClientRpc]
  public static void SpawnConfetti(Player player)
  {
    // Spawn confetti behind the player
    var confettiPrefab = Assets.GetAsset<Prefab>("Confetti.prefab");
    if (confettiPrefab != null)
    {
      confettiPrefab.Instantiate(onBeforeAwake: (entity) =>
      {
        entity.Position = player.Entity.Position;
        entity.LocalScale = new Vector2(2f, 2f);
      });
    }
  }

  [ClientRpc]
  public void LevelUp(int newLevel)
  {
    SFX.Play(Assets.GetAsset<AudioAsset>("sfx/job-success.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Entity, Volume = 0.75f, VolumePerturb = 0.1f, SpeedPerturb = 0.1f });

    if (!Network.IsServer) return;

    CallClient_SpawnConfetti(this);

    // Show level up message/effect
    GameManager.Instance.CallClient_SpawnDamageNumber(Entity.Position + new Vector2(0, 1),
      new Vector4(1, 1, 0, 1), $"LEVEL {newLevel}!", 2.0f, 0.0f, false);
  }

  // Weapon selling methods
  public void RequestSellWeapon(int inventorySlot)
  {
    if (!Network.IsClient || !IsLocal) return;
    CallServer_SellWeapon(inventorySlot);
  }

  [ServerRpc]
  public void SellWeapon(int inventorySlot)
  {
    if (!Network.IsServer) return;

    var player = (MyPlayer)Network.GetRemoteCallContextPlayer();
    if (!player.Alive()) return;

    var items = player.DefaultInventory.Items;
    if (inventorySlot < 0 || inventorySlot >= items.Length) return;

    var item = items[inventorySlot];
    if (item == null || item.Definition == null) return;

    // Verify it's a weapon
    var gameItems = GameManager.Instance.GameItems;
    var customDef = gameItems.GetCustomItemDefByID(item.Definition.Id);
    if (customDef == null || customDef.ItemCategory != ItemCategory.Weapon) return;

    // Don't allow selling fists
    if (item.Definition.Id == "__WEAPON__fists") return;

    // Get weapon level and calculate value
    string levelStr = item.GetMetadata("level");
    int weaponLevel = string.IsNullOrEmpty(levelStr) ? 1 : int.Parse(levelStr);
    // Get rarity from metadata first, fallback to custom definition
    ItemRarity rarity = customDef.ItemRarity;
    string rarityStr = item.GetMetadata("rarity");
    if (!string.IsNullOrEmpty(rarityStr) && Enum.TryParse<ItemRarity>(rarityStr, out var metadataRarity))
    {
      rarity = metadataRarity;
    }

    int sellValue = Store.CalculateWeaponValue(rarity, weaponLevel);

    // Remove weapon from inventory
    Inventory.RemoveItemFromInventory(item, player.DefaultInventory);

    // Give money to player
    Economy.DepositCurrency(player, GameManager.CASH_CURRENCY, sellValue);

    // Notify player
    player.CallClient_WeaponSold(item.Definition.Name, sellValue, weaponLevel, rarity, new RPCOptions() { Target = player });
  }

  [ClientRpc]
  public void WeaponSold(string weaponName, int sellValue, int level, ItemRarity rarity)
  {
    // Show sell notification
    string rarityText = rarity.ToString().ToUpper();
    Notifications.Show($"Sold {rarityText} {weaponName} (Lv.{level}) for ${sellValue:N0}!");

    // Refresh the weapon sell shop to remove the sold item
    Store.Instance.RefreshWeaponSell();

    // Show visual feedback with damage number in rarity color
    Vector4 color = rarity switch
    {
      ItemRarity.Rare => new Vector4(0.31f, 0.76f, 0.97f, 1f),      // Light Blue
      ItemRarity.Epic => new Vector4(0.61f, 0.15f, 0.69f, 1f),      // Purple
      ItemRarity.Legendary => new Vector4(1f, 0.6f, 0f, 1f),        // Orange
      ItemRarity.Mythic => new Vector4(0.96f, 0.27f, 0.21f, 1f),    // Red
      _ => new Vector4(1f, 1f, 1f, 1f)                              // White
    };

    if (Network.IsServer)
    {
      GameManager.Instance.CallClient_SpawnDamageNumber(Entity.Position + new Vector2(0, 1), color, $"+${sellValue:N0}", 2.0f, 0.0f, false);
    }

    if (!Network.IsClient) return;



    // Play sell sound
    AudioAsset sellSound = Assets.GetAsset<AudioAsset>("sfx/purchase.wav");
    if (sellSound != null)
    {
      SFX.Play(sellSound, new SFX.PlaySoundDesc() { Volume = 0.5f });
    }
  }

  void UpdateScaleBasedOnLeaderboard()
  {
    // Get damage tracker instance
    var damageTracker = DamageTracker.Instance;
    if (!damageTracker.Alive()) return;

    // Get appropriate leaderboard data based on client/server
    var leaderboardData = damageTracker.GetClientLeaderboardData();

    // If no one is on the leaderboard, use default scale
    if (leaderboardData.Count == 0)
    {
      Entity.Scale = new Vector2(1f, 1f);
      return;
    }

    // Find this player's position in the leaderboard
    int playerPosition = -1;
    for (int i = 0; i < leaderboardData.Count; i++)
    {
      if (leaderboardData[i].Name == Name)
      {
        playerPosition = i;
        break;
      }
    }

    // If player is not on the leaderboard (no damage dealt), use minimum scale
    if (playerPosition == -1)
    {
      Entity.Scale = new Vector2(1f, 1f);
      return;
    }

    // Calculate scale based on position
    // Last place = 1.0f, First place = 2.5f
    float minScale = 1.0f;
    float maxScale = 1.6f;

    // If only one player on leaderboard, they get max scale
    if (leaderboardData.Count == 1)
    {
      Entity.Scale = new Vector2(maxScale, maxScale);
      return;
    }

    // Linear interpolation based on position
    // Position 0 (first) = maxScale, Position (count-1) (last) = minScale
    float t = 1f - ((float)playerPosition / (leaderboardData.Count - 1));
    float scale = minScale + (maxScale - minScale) * t;

    Entity.Scale = new Vector2(scale, scale);
  }
}