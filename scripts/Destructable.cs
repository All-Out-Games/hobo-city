using AO;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class Destructable : Component
{
  public static Prefab CashRewardPrefab = Assets.GetAsset<Prefab>("ExplodeAndLerpToPlayer.prefab");
  [Serialized] public int MaxHealth;
  [Serialized] public string skin;
  [Serialized] public float RespawnTime = 10;
  [Serialized] public int CashReward = 10;

  public SyncVar<float> DestroyedAt = new(-1f);
  public float LastHitTime = -1f;

  public Spine_Animator SpineAnimator;
  public ThingWithHealth Health;
  public Vector2 BaseScale;
  public Polygon_Collider PolygonCollider;
  public Box_Collider BoxCollider;
  public HiddenArea HiddenArea;
  public Vector2 InitialPosition;
  public float InitialRotation;
  public bool HasPlayedCarAlarm = false;

  public override void Awake()
  {
    Health = Entity.Unsafe_AddComponent<ThingWithHealth>();
    Health.MaxHealth = MaxHealth;
    Health.Health = MaxHealth;

    PolygonCollider = GetComponent<Polygon_Collider>();
    BoxCollider = GetComponent<Box_Collider>();

    HiddenArea = Entity.TryGetChildByName("HiddenArea")?.GetComponent<HiddenArea>();

    SpineAnimator = GetComponent<Spine_Animator>();
    SpineAnimator.Awaken();
    SpineAnimator.SpineInstance.SetSkin(skin);
    ConstructStateMachine();
    SpineAnimator.LocalEnabled = false;

    Health.OnChanged += (int oldHealth, int newHealth) =>
    {
      if (newHealth <= 0)
      {
        HandleDestroyed();
      }

      if (newHealth < oldHealth)
      {
        if (newHealth <= 0) return;

        HandleHit();
      }

    };

    InitialPosition = Entity.Position;
    InitialRotation = Entity.Rotation;

    GameManager.ActiveDestructables.Add(Entity);
  }

  public override void OnDestroy()
  {
    GameManager.ActiveDestructables.Remove(Entity);
  }

  public void HandleHit()
  {
    LastHitTime = Time.TimeSinceStartup;
    SpineAnimator.SpineInstance.StateMachine.SetTrigger("hit");

    if (Entity.Name.Contains("Car") && !HasPlayedCarAlarm)
    {
      SFX.Play(Assets.GetAsset<AudioAsset>("sfx/car-alarm.wav"), new SFX.PlaySoundDesc() { Volume = 0.21f, Position = Entity.Position, Positional = true });
      HasPlayedCarAlarm = true;
    }
  }

  public void HandleDestroyed()
  {
    HiddenArea?.OnItemDestroyed();

    SpineAnimator.SpineInstance.StateMachine.SetTrigger("break");
    if (PolygonCollider.Alive())
    {
      PolygonCollider.LocalEnabled = false;
    }

    if (BoxCollider.Alive())
    {
      BoxCollider.LocalEnabled = false;
    }

    // GameManager.Instance.RootNavmesh.MarkedForRebuild = true;

    SFX.Play(Assets.GetAsset<AudioAsset>("sfx/break-boulder.wav"), new SFX.PlaySoundDesc() { Volume = 0.5f, Position = Entity.Position, Positional = true });

    if (Entity.Name == "DI_vault_door")
    {
      SFX.Play(Assets.GetAsset<AudioAsset>("sfx/vault-open.wav"), new SFX.PlaySoundDesc() { Volume = 0.6f, Position = Entity.Position, Positional = true });
    }

    if (Network.IsClient)
    {
      CashRewardPrefab.Instantiate(onBeforeAwake: (entity) =>
      {
        if (!Health.LastDamagedBy.Value.Alive()) return;

        var explodeAndLerp = entity.GetComponent<ExplodeAndLerpToPlayer>();
        explodeAndLerp.Player = Health.LastDamagedBy.Value.GetComponent<Player>();
        explodeAndLerp.Texture = Assets.GetAsset<Texture>("icons/cash.png");
        explodeAndLerp.Count = CashReward;
        entity.SetParent(Entity, false);
      });
    }

    // SERVER ONLY FROM HERE
    if (!Network.IsServer) return;
    DestroyedAt.Set(Time.TimeSinceStartup);

    // Add to respawn queue
    GameManager.DestructibleRespawnQueue.Add(new DestructibleRespawnInfo(Entity, RespawnTime, Time.TimeSinceStartup));

    if (Health.LastDamagedBy.Value.Alive() && Health.LastDamagedBy.Value.GetComponent<Player>().Alive())
    {
      Economy.DepositCurrency(Health.LastDamagedBy.Value.GetComponent<Player>(), GameManager.CASH_CURRENCY, CashReward);
    }
  }
  public void ConstructStateMachine()
  {
    var stateMachine = StateMachine.Make();
    var mainLayer = stateMachine.CreateLayer("main");

    var spawnState = mainLayer.CreateState("016ARP/Spawn_In", 0, false);
    var idleState = mainLayer.CreateState("016ARP/Idle", 0, true);
    var hitState = mainLayer.CreateState("016ARP/Hit", 0, false);
    var breakState = mainLayer.CreateState("016ARP/Break", 0, false);

    var spawnTrigger = stateMachine.CreateVariable("spawn", StateMachineVariableKind.TRIGGER);
    var hitTrigger = stateMachine.CreateVariable("hit", StateMachineVariableKind.TRIGGER);
    var breakTrigger = stateMachine.CreateVariable("break", StateMachineVariableKind.TRIGGER);

    mainLayer.CreateGlobalTransition(spawnState).CreateTriggerCondition(spawnTrigger);
    mainLayer.CreateTransition(spawnState, idleState, true);
    mainLayer.CreateGlobalTransition(breakState).CreateTriggerCondition(breakTrigger);
    mainLayer.CreateTransition(idleState, hitState, false).CreateTriggerCondition(hitTrigger);
    mainLayer.CreateTransition(hitState, idleState, true);

    mainLayer.InitialState = idleState;
    SpineAnimator.SpineInstance.SetStateMachine(stateMachine, Entity);
  }

  // Respawning is handled in game manager

  [ClientRpc]
  public void Respawn()
  {
    SFX.Play(Assets.GetAsset<AudioAsset>("sfx/destruct-spawn-1.wav"), new SFX.PlaySoundDesc() { Volume = 0.35f, Position = Entity.LocalPosition, Positional = true, RangeMultiplier = 2f });
    SpineAnimator.SpineInstance.StateMachine.SetTrigger("spawn");
    HasPlayedCarAlarm = false;
    if (PolygonCollider.Alive())
    {
      PolygonCollider.LocalEnabled = true;
    }

    if (BoxCollider.Alive())
    {
      BoxCollider.LocalEnabled = true;
    }

    HiddenArea?.OnItemRespawned();
  }
}
