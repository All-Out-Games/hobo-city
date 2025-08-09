using System.Security.Cryptography.X509Certificates;
using AO;
using System;
using System.Collections.Generic;

// Event system enum
public enum GameEventType
{
  None,
  PlayerEnterRoom,
  PlayerExitRoom,
  BakePizza,
  PickupPizza,
  DeliverPizza,
  // Taxi events
  PickupPassenger,
  DropoffPassenger,
  VisitLandmark,
  WantedLevelCleared,
  // Tutorial events
  DestroyDestructible,
  CareerObtained,
  JobCompleted,
  HouseBought,
  // Hitman events
  EliminateTarget,
  // Pizza manager events
  DestroySussyMarketing,
  // Add other event types here
  PickupGarbage,
  EnterGarbageTruck,
  EnterPlane,
  CleanGarbageArea,
  // Detective events
  FindCorpse,
  PickupCorpse,
  ReturnCorpse,
  // Lifeguard events
  TalkToTed,
  HavePicnic,
  CheckBoat1,
  CheckBoat2,
  CheckBoat3,
  // Fired when a police officer eliminates the special RobbingTed NPC during the robbery job
  EliminateRobber,
  ReachRobberyLocation,
  HandcuffRobber,
  // President events
  StartNuclearWar,
  NuclearWarComplete,
  // Pilot cargo events
  PickupCargo,
  DropoffCargo,
  // Pilot helicopter hunt events
  DestroyEnemyHelicopter,
  // Donut delivery events
  GetDonut,
  DeliverDonut,
  // Pilot "Save the Day!" job events
  CollectWater,
  ExtinguishFire,
  // Police "Recover Stolen Cash" job events
  QuestionWitness,
  InvestigateArea,
  FindStolenCash,
}

public partial class GameManager : Component
{
  // So players joining have these right away
  public static Texture DestructibleItemTexture = Assets.KeepLoaded<Texture>("rigs/destructible-item-2/016ARP_Destructible_Items_2.png", synchronous: false);

  [Serialized] public Navmesh RootNavmesh;
  public static string SAVE_PREFIX = "1";
  // Not prefixed so they look correct on the UI
  public static string CASH_CURRENCY = "cash";
  public static string XP_CURRENCY = "xp";
  public static bool ForcePrivateJetWish = false;
  // Admin toggle flag: if false, admin commands (except the toggle command itself) are disabled
  public static bool AdminCommandsEnabled = true;

  // Leaderboard constants
  public const float LEADERBOARD_ENTRY_HEIGHT = 35f;

  public static List<DamageNumbers> ActiveDamageNumbers = new();
  public static List<Entity> ActiveDestructables = new();
  public static List<Destructable> ActiveDestructableComponents = new();
  public static List<Destructable> DestructablesShowingHealthBars = new();
  public static List<DestructibleRespawnInfo> DestructibleRespawnQueue = new();

  public const float TAX_INTERVAL = 600f; // 10 minutes
}

// Event system implementation
public class GameEvent
{
  public Player Player;
  public GameEventType EventType;
  public string Metadata;

  public GameEvent(GameEventType eventType, Player player = null, string metadata = "")
  {
    EventType = eventType;
    Player = player;
    Metadata = metadata;
  }
}

public static class EventSystem
{
  public delegate void EventCallback(GameEvent gameEvent);

  static Dictionary<GameEventType, List<EventCallback>> eventCallbacks = new Dictionary<GameEventType, List<EventCallback>>();

  public static void RegisterEvent(GameEventType eventType, EventCallback callback)
  {
    if (!eventCallbacks.ContainsKey(eventType))
    {
      eventCallbacks[eventType] = new List<EventCallback>();
    }

    eventCallbacks[eventType].Add(callback);
  }

  public static void UnregisterEvent(GameEventType eventType, EventCallback callback)
  {
    if (eventCallbacks.ContainsKey(eventType))
    {
      eventCallbacks[eventType].Remove(callback);
    }
  }

  public static void FireEvent(GameEventType eventType, Player player = null, string metadata = "")
  {
    var gameEvent = new GameEvent(eventType, player, metadata);
    FireEvent(gameEvent);
  }

  public static void FireEvent(GameEvent gameEvent)
  {
    if (eventCallbacks.ContainsKey(gameEvent.EventType))
    {
      // Create a copy of the callbacks list so that modifications (registering or unregistering)
      // that happen inside the callbacks themselves do not invalidate the enumerator.
      var callbacksCopy = new List<EventCallback>(eventCallbacks[gameEvent.EventType]);
      foreach (var callback in callbacksCopy)
      {
        callback(gameEvent);
      }
    }
  }
}

public partial class GameManager : Component
{

  public Quadtree<ThingWithHealth> ThingWithHealthQuadtree;

  private static GameManager _instance;
  public static GameManager Instance
  {
    get
    {
      if (_instance.Alive()) return _instance;
      foreach (var c in Scene.Components<GameManager>(false))
      {
        _instance = c;
        _instance.Awaken();
        break;
      }
      return _instance;
    }
  }
  public GameItems GameItems;

  public SyncVar<bool> IsDay = new(true);
  public const float DAY_LENGTH_MINUTES = 1f;
  public const float NIGHT_LENGTH_MINUTES = 1f;
  public float TimeDayChanged;
  [NetSync] public float GlobalTimer;

  private ulong NightMusic;
  private ulong DayMusic;

  // Taxi passenger spawning
  float lastTaxiPassengerSpawnTime = 0f;
  const float TAXI_PASSENGER_SPAWN_INTERVAL = 15f;
  const float TAXI_PASSENGER_DESPAWN_TIME = 15f;
  ulong taxiSpawnRngSeed;

  public ulong GlobalRng;

  public override void Awake()
  {

    GlobalRng = RNG.Seed((ulong)new Random().Next());

    _instance = this;
    ThingWithHealthQuadtree = new Quadtree<ThingWithHealth>(8, new Vector2(-150, -70), new Vector2(57, 46));

    SetupLeaderboards();

    Game.SetVoiceEnabled(true);

    GameItems = new();
    GameItems.CreateItemDefinitions();

    Chat.RegisterChatCommandHandler(RunChatCommand);

    // Initialize taxi passenger spawning
    if (Network.IsServer)
    {
      taxiSpawnRngSeed = RNG.Seed((ulong)Entity.NetworkId);
      lastTaxiPassengerSpawnTime = Time.TimeSinceStartup;
    }

    // Start with day cycle
    if (Network.IsServer)
    {
      GlobalTimer = DAY_LENGTH_MINUTES * 60;
      IsDay.Set(true);
    }

    IsDay.OnSync += (old, isDay) =>
    {
      TimeDayChanged = Time.TimeSinceStartup;

      if (isDay)
      {
        if (NightMusic != 0)
        {
          SFX.FadeOutAndStop(NightMusic, 0.5f);
        }

        DayMusic = SFX.Play(Assets.GetAsset<AudioAsset>("sfx/city-ambience.wav"), new SFX.PlaySoundDesc { Volume = 0.15f, Loop = true });
      }
      else
      {
        if (DayMusic != 0)
        {
          SFX.FadeOutAndStop(DayMusic, 0.5f);
        }

        NightMusic = SFX.Play(Assets.GetAsset<AudioAsset>("sfx/city-night-ambience.wav"), new SFX.PlaySoundDesc { Volume = 0.325f, Loop = true });
      }
    };
  }

  public override void LateUpdate()
  {

    // Iterate backwards through the list so we can safely remove items
    for (int i = DestructablesShowingHealthBars.Count - 1; i >= 0; i--)
    {
      var destructable = DestructablesShowingHealthBars[i];

      // Check if the destructible is still alive and valid
      if (!destructable.Alive() || !destructable.Entity.Alive())
      {
        DestructablesShowingHealthBars.RemoveAt(i);
        continue;
      }

      // Check if the cooldown period has passed
      if (Time.TimeSinceStartup - destructable.LastHitTime > 3f || destructable.Health.Health <= 0 || destructable.LastHitTime <= 0)
      {
        DestructablesShowingHealthBars.RemoveAt(i);
        continue;
      }

      // Show health bar for this destructible
      HealthBar.DrawHealthBar(new Rect(new Vector2(destructable.Entity.Position.X, destructable.Entity.Position.Y)).TopCenterRect().Offset(0, 0), destructable.Health.Health, destructable.Health.MaxHealth, "red");
    }
  }

  [ClientRpc]
  public void SpawnDamageNumber(Vector2 worldPosition, Vector4 color, string text, float size = 0.3f, float slant = 0.0f, bool spaceText = false)
  {
    if (!Network.IsClient) return;

    float randX = Random.Shared.NextFloat(-1f, 1f);
    float randY = Random.Shared.NextFloat(1f, 1.5f);
    var searchResult = new DamageNumbers();
    searchResult.Text = text;
    searchResult.Position = worldPosition + new Vector2(randX, randY);
    searchResult.Color = color;
    searchResult.T = 0;
    searchResult.TextSettings = GetTextSettingsDamageNumbers(UI.Fonts.Barlow, Game.IsMobile ? 0.5f : size, color, slant);
    searchResult.SpaceText = spaceText;
    ActiveDamageNumbers.Add(searchResult);
  }

  public static UI.TextSettings GetTextSettings(float size, UI.HorizontalAlignment alignment = UI.HorizontalAlignment.Center)
  {
    return new UI.TextSettings()
    {
      Font = UI.Fonts.BarlowBold,
      Size = size,
      Color = new Vector4(1f, 1f, 1f, 1f),
      DropShadowColor = new Vector4(0f, 0f, 0f, 0.8f),
      DropShadowOffset = new Vector2(0f, -2f),
      HorizontalAlignment = alignment,
      VerticalAlignment = UI.VerticalAlignment.Center,
      WordWrap = false
    };
  }

  public UI.TextSettings GetTextSettingsDamageNumbers(FontAsset font, float size, Vector4 color, float slant)
  {
    var ts = new UI.TextSettings()
    {
      Font = font,
      Size = size,
      Color = color,
      DropShadowColor = new Vector4(0f, 0f, 0f, 1f),
      DropShadowOffset = new Vector2(0f, -3f),
      HorizontalAlignment = UI.HorizontalAlignment.Center,
      VerticalAlignment = UI.VerticalAlignment.Center,
      WordWrap = false,
      WordWrapOffset = 0,
      Outline = true,
      OutlineThickness = 3,
      Slant = slant,
    };
    return ts;
  }


  public override void Update()
  {
    if (Network.IsServer)
    {
      GlobalTimer -= Time.DeltaTime;
      GlobalTimer = Math.Max(GlobalTimer, 0);

      if (GlobalTimer <= 0)
      {
        // Toggle between day and night
        bool newDayState = !IsDay.Value;
        IsDay.Set(newDayState);

        // Set timer based on current state
        if (newDayState)
        {
          GlobalTimer = DAY_LENGTH_MINUTES * 60f;
        }
        else
        {
          GlobalTimer = NIGHT_LENGTH_MINUTES * 60f;
        }
      }

      // Check destructible respawns
      for (int i = DestructibleRespawnQueue.Count - 1; i >= 0; i--)
      {
        var respawnInfo = DestructibleRespawnQueue[i];
        if (Time.TimeSinceStartup - respawnInfo.DestroyedAt > respawnInfo.RespawnTime)
        {
          if (respawnInfo.Destructible.Alive())
          {
            var destructible = respawnInfo.Destructible.GetComponent<Destructable>();
            if (destructible.Alive())
            {
              destructible.CallClient_Respawn();
              destructible.Health.Reset();
              destructible.DestroyedAt.Set(-1f);
            }
          }
          DestructibleRespawnQueue.RemoveAt(i);
        }
      }
    }
  }

  // Call this with a target
  [ClientRpc]
  public static void SendTargetedMessage(string message)
  {
    Notifications.Show(message);
  }

  [ClientRpc]
  public static void PlayTargetedSFX(string sfxPath, float volume, Vector2 position)
  {
    SFX.Play(Assets.GetAsset<AudioAsset>(sfxPath), new SFX.PlaySoundDesc() { Volume = volume, Positional = true, Position = position, RangeMultiplier = 3.5f });
  }

  [ClientRpc]
  public static void PlayGlobalSFX(string sfxPath, float volume)
  {
    SFX.Play(Assets.GetAsset<AudioAsset>(sfxPath), new SFX.PlaySoundDesc() { Volume = volume });
  }


  [ClientRpc]
  public static void MuteBgm()
  {
    // Activate global mute flag and stop any currently playing BGM sounds.
    ProximitySound.GlobalBgmMuted = true;

    foreach (var ps in Scene.Components<ProximitySound>())
    {
      if (!ps.Alive()) continue;
      if (!ps.IsBgm) continue;

      ps.StopSound();

      // Disable the component so it will not attempt to restart later.
      ps.LocalEnabled = false;
    }

    Notifications.Show("All background music muted for this client.");
  }

  public void RunChatCommand(Player p, string command)
  {
    var parts = command.Split(' ');
    var cmd = parts[0].ToLowerInvariant();
    MyPlayer player = (MyPlayer)p;
    var allowCommands = player.IsAdmin || Game.LaunchedFromEditor;
    // Allow certain commands to be run by any player (not just admins)
    bool isPublicCommand = cmd == "pickupfurniture" || cmd == "permapvp";

    if (!allowCommands && !isPublicCommand)
    {
      return;
    }

    // Check if admin commands are disabled (except for the toggle command itself)
    if (allowCommands && !AdminCommandsEnabled && cmd != "toggleadmin")
    {
      Chat.SendMessage(p, "Admin commands are currently disabled you rat.");
      return;
    }

    switch (cmd)
    {
      case "cash":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /cash <amount>");
            return;
          }
          if (int.TryParse(parts[1], out var amount))
          {
            Economy.DepositCurrency(player, GameManager.CASH_CURRENCY, amount);
            Chat.SendMessage(p, $"Cash updated by {amount}.");
          }
          else
          {
            Chat.SendMessage(p, "Usage: /cash <amount>");
          }
          break;
        }
      case "give":
        {
          // Expected format: /give <username> <item id> [quantity]
          if (parts.Length < 3)
          {
            Chat.SendMessage(player, "Usage: /give <username> <item id> [quantity]");
            break;
          }

          string targetName = parts[1];
          string itemId = parts[2];

          // Find the target player by name (case-insensitive)
          MyPlayer targetPlayer = null;
          foreach (var pl in Scene.Components<MyPlayer>())
          {
            if (pl.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase))
            {
              targetPlayer = pl;
              break;
            }
          }

          if (targetPlayer == null)
          {
            Chat.SendMessage(player, $"Player '{targetName}' not found.");
            break;
          }

          Item_Definition item = null;
          foreach (var i in GameItems.ItemPool)
          {
            if (i.ItemDefinition.Id.Equals(itemId, StringComparison.OrdinalIgnoreCase))
            {
              item = i.ItemDefinition;
              break;
            }
          }

          if (item == null)
          {
            Chat.SendMessage(player, $"Not a valid item '{itemId}'.");
            break;
          }

          int quantity = 1;
          if (parts.Length > 3)
          {
            int.TryParse(parts[3], out quantity);
          }

          var instance = Inventory.CreateItem(item, quantity);

          // Add level metadata for weapons
          if (itemId.StartsWith("__WEAPON__"))
          {
            instance.SetMetadata("level", targetPlayer.Level.ToString());
          }

          if (Inventory.CanMoveItemToInventory(instance, targetPlayer.DefaultInventory))
          {
            Inventory.MoveItemToInventory(instance, targetPlayer.DefaultInventory);
            Chat.SendMessage(player, $"Gave x{quantity} {itemId} to {targetPlayer.Name}.");
            Chat.SendMessage(targetPlayer, $"{player.Name} gave you x{quantity} {itemId}.");

            // Refresh the target player's inventory UI
            targetPlayer.CallClient_UpdateCurrentHoveredSlot();
          }
          else
          {
            Chat.SendMessage(player, $"Cannot add item, maybe {targetPlayer.Name}'s inventory is full?");
          }

          break;
        }
      case "kills":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /kills <amount>");
            return;
          }
          if (int.TryParse(parts[1], out var amount) && amount >= 0)
          {
            player.KillsThisLife.Set(amount);
            Chat.SendMessage(p, $"Kills set to {amount}.");
          }
          else
          {
            Chat.SendMessage(p, "Usage: /kills <amount>");
          }
          break;
        }
      case "speed":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /speed <multiplier>");
            return;
          }
          if (float.TryParse(parts[1], out var speed))
          {
            player.SpeedMultiplier.Set(speed);
            Chat.SendMessage(p, $"Speed multiplier set to {speed}x.");
          }
          else
          {
            Chat.SendMessage(p, "Usage: /speed <multiplier>");
          }
          break;
        }
      case "zoom":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /zoom <value> (0.8-4.0)");
            return;
          }
          if (float.TryParse(parts[1], out var zoom))
          {
            zoom = Math.Clamp(zoom, 0.8f, 4.0f);
            if (Network.IsServer)
            {
              player.CameraZoom.Set(zoom);
              Chat.SendMessage(p, $"Camera zoom set to {zoom}.");
            }
          }
          else
          {
            Chat.SendMessage(p, "Usage: /zoom <value> (0.8-4.0)");
          }
          break;
        }
      case "time":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /time <day/night>");
            return;
          }

          string timeArg = parts[1].ToLower();
          if (timeArg == "day" || timeArg == "night")
          {
            bool shouldBeDay = timeArg == "day";
            if (Network.IsServer)
            {
              IsDay.Set(shouldBeDay);
              GlobalTimer = (shouldBeDay ? DAY_LENGTH_MINUTES : NIGHT_LENGTH_MINUTES) * 60f;
              Chat.SendMessage(p, $"Time set to {timeArg}.");
            }
            else
            {
              Chat.SendMessage(p, "Time can only be changed on the server.");
            }
          }
          else
          {
            Chat.SendMessage(p, "Usage: /time <day/night>");
          }
          break;
        }
      case "scale":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /scale <value> (1.0 to 2.5)");
            break;
          }

          if (float.TryParse(parts[1], out var scaleValue))
          {
            scaleValue = Math.Clamp(scaleValue, 1f, 2.5f);
            player.Entity.LocalScale = new Vector2(scaleValue, scaleValue);
            Chat.SendMessage(p, $"Player scale set to {scaleValue}. Note: This will be overridden by damage leaderboard position.");
          }
          else
          {
            Chat.SendMessage(p, "Usage: /scale <value> (1.0 to 2.5)");
          }
          break;
        }
      case "xp":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /xp <amount>");
            break;
          }

          if (int.TryParse(parts[1], out var xpAmount) && xpAmount >= 0)
          {
            if (Network.IsServer)
            {
              int oldLevel = player.Level;
              int currentXP = player.XP.Value;
              int xpDifference = xpAmount - currentXP;

              player.XP.Set(xpAmount);
              if (xpDifference != 0)
              {
                Economy.DepositCurrency(player, GameManager.XP_CURRENCY, xpDifference);
              }

              int newLevel = player.Level;
              if (newLevel > oldLevel)
              {
                player.CallClient_LevelUp(newLevel);
              }

              Chat.SendMessage(p, $"XP set to {xpAmount}. Level: {newLevel}");
            }
            else
            {
              Chat.SendMessage(p, "XP can only be set on the server.");
            }
          }
          else
          {
            Chat.SendMessage(p, "Usage: /xp <amount> (must be 0 or positive)");
          }
          break;
        }
      case "level":
        {
          if (parts.Length < 2)
          {
            Chat.SendMessage(p, "Usage: /level <level>");
            break;
          }

          if (int.TryParse(parts[1], out var targetLevel) && targetLevel >= 1 && targetLevel <= 50)
          {
            if (Network.IsServer)
            {
              int oldLevel = player.Level;
              int currentXP = player.XP.Value;
              int requiredXP = MyPlayer.CalculateXPForLevel(targetLevel);
              int xpDifference = requiredXP - currentXP;

              player.XP.Set(requiredXP);
              if (xpDifference != 0)
              {
                Economy.DepositCurrency(player, GameManager.XP_CURRENCY, xpDifference);
              }

              if (targetLevel > oldLevel)
              {
                player.CallClient_LevelUp(targetLevel);
              }

              Chat.SendMessage(p, $"Level set to {targetLevel} (XP: {requiredXP})");
            }
            else
            {
              Chat.SendMessage(p, "Level can only be set on the server.");
            }
          }
          else
          {
            Chat.SendMessage(p, "Usage: /level <level> (must be between 1 and 50)");
          }
          break;
        }
      case "ambience":
        {
          CallClient_MuteBgm(new RPCOptions(target: player));
          break;
        }
      case "reset":
        {
          // Reset cash balance
          long cashBalance = Economy.GetBalance(player, GameManager.CASH_CURRENCY);
          if (cashBalance > 0)
          {
            Economy.WithdrawCurrency(player, GameManager.CASH_CURRENCY, cashBalance);
          }

          Chat.SendMessage(p, "Your progress has been reset: all cash balance is now zero.");
          break;
        }
      case "clearinv":
        {
          // Admin-only command to clear a player's entire inventory.
          // Usage:
          //   /clearinv                 → clears your own inventory
          //   /clearinv <playername>    → clears the specified player's inventory (admin only)

          MyPlayer targetPlayer = player; // Defaults to the command issuer.

          if (parts.Length >= 2)
          {
            string targetName = parts[1];

            // Only admins (or when running in the editor) may clear another player's inventory.
            if (!player.IsAdmin && !Game.LaunchedFromEditor)
            {
              Chat.SendMessage(p, "You do not have permission to clear another player's inventory.");
              break;
            }

            MyPlayer found = null;
            foreach (var pl in Scene.Components<MyPlayer>())
            {
              if (pl.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase))
              {
                found = pl;
                break;
              }
            }

            if (found == null)
            {
              Chat.SendMessage(p, $"Player '{targetName}' not found.");
              break;
            }

            targetPlayer = found;
          }

          // Ensure we are on the server before mutating inventories.
          if (!Network.IsServer)
          {
            Chat.SendMessage(p, "Inventory can only be cleared on the server.");
            break;
          }

          if (!targetPlayer.Alive() || targetPlayer.DefaultInventory == null)
          {
            Chat.SendMessage(p, "Target player is not valid.");
            break;
          }

          var itemsArray = targetPlayer.DefaultInventory.Items;
          int clearedCount = 0;

          // Iterate through the inventory and remove every item instance.
          for (int i = 0; i < itemsArray.Length; i++)
          {
            var invItem = itemsArray[i];
            if (invItem != null)
            {
              Inventory.RemoveItemFromInventory(invItem, targetPlayer.DefaultInventory);
              clearedCount++;
            }
          }

          // Refresh UI on the affected client.
          targetPlayer.CallClient_UpdateCurrentHoveredSlot();

          Chat.SendMessage(p, $"Cleared {clearedCount} item{(clearedCount == 1 ? "" : "s")} from {targetPlayer.Name}'s inventory.");
          if (targetPlayer != player)
          {
            Chat.SendMessage(targetPlayer, $"{player.Name} cleared your inventory.");
          }

          break;
        }
      case "permapvp":
        {
          // Permanently enable PvP for the issuing player.
          if (Network.IsServer)
          {
            player.PVPEnabled.Set(true);
            player.PermanentPVPEnabled = true;
            Chat.SendMessage(p, "PvP permanently enabled. You will respawn with PvP enabled.");
          }
          else
          {
            Chat.SendMessage(p, "PvP can only be permanently enabled on the server.");
          }
          break;
        }
      case "wishjet":
        {
          // Admin-only command: force the next wishing well toss to grant a Private Jet.
          if (Network.IsServer)
          {
            GameManager.ForcePrivateJetWish = true;
            Chat.SendMessage(p, "Next wishing well toss will grant a Private Jet.");
          }
          else
          {
            Chat.SendMessage(p, "This command can only be run on the server.");
          }
          break;
        }
      case "toggleadmin":
        {
          // Toggle whether admin commands are enabled or disabled
          AdminCommandsEnabled = !AdminCommandsEnabled;
          string status = AdminCommandsEnabled ? "enabled" : "disabled";
          Chat.SendMessage(p, $"Admin commands are now {status}.");
          break;
        }
    }
  }

  private void SetupLeaderboards()
  {
    PlayerList.Register("Bounty", (Player[] players, string[] scores) =>
    {
      for (int i = 0; i < players.Length; i++)
      {
        MyPlayer op = (MyPlayer)players[i];
        scores[i] = "$" + op.GetBountyReward().ToString();
      }
    });

    PlayerList.RegisterSortCallback((Player[] players) =>
    {
      Array.Sort(players, (a, b) =>
        {
          return ((MyPlayer)b).GetBountyReward().CompareTo(((MyPlayer)a).GetBountyReward());
        });
    });
  }

  // Call this method when a destructible is hit to show its health bar
  public static void AddDestructibleToHealthBarList(Destructable destructible)
  {
    if (destructible != null && destructible.Alive() && !DestructablesShowingHealthBars.Contains(destructible))
    {
      DestructablesShowingHealthBars.Add(destructible);
    }
  }

  public static void DrawLeaderboard(List<PlayerLeaderboardData> players, string headerText, string localPlayerName = null)
  {
    var leaderboardRect = UI.SafeRect.LeftRect().Grow(0, 250, 400, 0);

    using var _ = UI.PUSH_LAYER(-10);

    // Draw header
    var headerRect = leaderboardRect.CutTop(GameManager.LEADERBOARD_ENTRY_HEIGHT);
    var headerSettings = GameManager.GetTextSettings(30, UI.HorizontalAlignment.Center);
    headerSettings.Color = new Vector4(1, 1, 1, 1);
    headerSettings.Offset = new Vector2(0, 2);
    UI.TextAsync(headerRect, headerText, headerSettings);

    var textSettings = GameManager.GetTextSettings(22, UI.HorizontalAlignment.Left);
    textSettings.Color = new Vector4(1, 1, 1, 1);
    textSettings.Offset = new Vector2(0, 2);

    // Calculate base positions for entries
    var baseRect = leaderboardRect.CutTop(GameManager.LEADERBOARD_ENTRY_HEIGHT);

    // Define alternating background colors
    var bgColor1 = new Vector4(0.2f, 0.2f, 0.2f, 0.8f); // Slightly lighter
    var bgColor2 = new Vector4(0.1f, 0.1f, 0.1f, 0.8f);    // Slightly darker

    // Draw entries with lerped positions
    for (int i = 0; i < players.Count; i++)
    {
      var player = players[i];

      // Calculate entry rectangle based on lerped position
      var entryRect = baseRect.Offset(0, -player.CurrentYOffset);

      // Draw alternating background first
      UI.Image(entryRect, null, i % 2 == 0 ? bgColor1 : bgColor2);

      // Draw medal colors for top 3
      if (i < 3)
      {
        Vector4 medalColor = i switch
        {
          0 => new Vector4(1.0f, 0.84f, 0.0f, 0.5f),    // Gold - brighter yellow
          1 => new Vector4(0.85f, 0.85f, 0.9f, 0.5f),   // Silver - slightly blue-tinted silver
          2 => new Vector4(0.87f, 0.45f, 0.23f, 0.5f),  // Bronze - more saturated bronze
          _ => default
        };
        UI.Image(entryRect, null, medalColor);
      }

      // Set text color for local player
      var nameColor = localPlayerName != null && player.Name == localPlayerName
        ? new Vector4(0.3f, 1f, 0.3f, 1f)  // Bright green for local player
        : new Vector4(1, 1, 1, 1);         // White for others

      // Draw player name with appropriate color
      textSettings.Color = nameColor;
      textSettings.DoAutofit = true;
      textSettings.AutofitMinSize = 15;
      textSettings.AutofitMaxSize = textSettings.Size;
      var nameRect = entryRect.CutLeft(240).Inset(0, 0, 0, 4);  // 4 pixels left padding
      UI.Text(nameRect, player.Name, textSettings);

      // Draw score (always white)
      var scoreSettings = GameManager.GetTextSettings(30, UI.HorizontalAlignment.Right);
      scoreSettings.Color = new Vector4(1, 1, 1, 1);
      scoreSettings.Offset = new Vector2(0, 2);
      var scoreRect = entryRect.Inset(0, 4, 0, 0);  // 4 pixels right padding
      UI.Text(scoreRect, player.Points.ToString(), scoreSettings);
    }
  }
}

public class DestructibleRespawnInfo
{
  public Entity Destructible;
  public float RespawnTime;
  public float DestroyedAt;

  public DestructibleRespawnInfo(Entity destructible, float respawnTime, float destroyedAt)
  {
    Destructible = destructible;
    RespawnTime = respawnTime;
    DestroyedAt = destroyedAt;
  }
}
