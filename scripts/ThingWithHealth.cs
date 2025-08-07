using AO;
using System;

public partial class ThingWithHealth : Component
{

  private SyncVar<int> _Health = new(100);
  private SyncVar<int> _MaxHealth = new(100);
  private SyncVar<float> _DiedAt = new(-1f);

  public SyncVar<Entity> LastDamagedBy = new(null);
  public bool IsInvulnerable = false;
  // This is here so we can expose it without messing up the syncvar wrapper (at least I dunno how to have a float getter/setter with an extra callback)
  public Action<int, int> OnChanged;
  public float DiedAt => _DiedAt.Value;
  public QuadtreeEntry<ThingWithHealth> QuadtreeEntry;
  public bool IsDestructable = false;

  // Wrapper that clamps health 0 to max health
  public int Health
  {
    get => _Health.Value;
    set
    {
      if (Network.IsServer)
      {
        if (value < _Health.Value && IsInvulnerable) return;

        // Check if this is damage (health decreasing)
        if (value < _Health.Value)
        {
          var targetPlayer = Entity.GetComponent<MyPlayer>();
          if (targetPlayer.Alive())
          {
            // Set teleport cooldown to 5 seconds
            targetPlayer.TeleportCooldownRemaining.Set(5f);
          }
        }

        _Health.Set(Math.Clamp(value, 0, _MaxHealth.Value));
      }
    }
  }

  public int MaxHealth
  {
    get => _MaxHealth.Value;
    set
    {
      if (Network.IsServer)
      {
        _MaxHealth.Set(value);
      }
    }
  }

  // Wrapper that sets the damaging entity so we can reference it later
  public void Damage(int amount, Entity damagedBy)
  {
    if (!Network.IsServer) return;

    // These can of course be null if we're not hitting a player!
    var hitByPlayer = damagedBy.GetComponent<MyPlayer>();
    var targetPlayer = Entity.GetComponent<MyPlayer>();

    var shouldDealDamage = true;
    var selfHasPVPEnabled = hitByPlayer?.PVPEnabled.Value ?? false;
    var targetHasPVPEnabled = targetPlayer?.PVPEnabled.Value ?? false;

    // Flag up any player that has tried to deal damage to another player
    if (hitByPlayer.Alive() && targetPlayer.Alive())
    {
      damagedBy.GetComponent<MyPlayer>().PVPEnabled.Set(true);

      if (!targetHasPVPEnabled)
      {
        if (selfHasPVPEnabled)
        {
          GameManager.CallClient_SendTargetedMessage($"{targetPlayer.Name} has PvP disabled.", new RPCOptions(target: hitByPlayer));
        }

        shouldDealDamage = false;
      }
    }

    if (shouldDealDamage)
    {
      // Update the teleport cooldown
      if (targetPlayer.Alive())
      {
        // Set teleport cooldown to 5 seconds
        targetPlayer.TeleportCooldownRemaining.Set(5f);
      }

      // Little hack to not show damage for cops who use max health for reasons
      // Also don't show damage numbers if the player is invulnerable
      if (targetPlayer.Alive() && !IsInvulnerable)
      {
        GameManager.Instance.CallClient_SpawnDamageNumber(Entity.Position, new Vector4(1, 0f, 0, 1), amount.ToString(), 0.5f, 0.0f, false);
      }

      // Record damage for assist tracking if this is player vs player damage
      if (hitByPlayer.Alive() && targetPlayer.Alive() && hitByPlayer != targetPlayer)
      {
        targetPlayer.RecordDamageForAssist(damagedBy, amount);
      }

      // Track damage for the damage leaderboard
      if (targetPlayer.Alive() && hitByPlayer.Alive() && DamageTracker.Instance != null)
      {
        DamageTracker.Instance.RecordDamage(damagedBy, amount);
      }

      LastDamagedBy.Set(damagedBy);
      Health -= amount;
    }
  }

  public void Reset()
  {
    Health = MaxHealth;

    if (Network.IsServer)
    {
      _DiedAt.Set(-1f);
    }
  }


  public override void Awake()
  {
    if (GetComponent<Destructable>() != null)
    {
      IsDestructable = true;
    }

    if (!Entity.GetComponent<MyPlayer>().Alive())
    {
      QuadtreeEntry = GameManager.Instance.ThingWithHealthQuadtree.Insert(this, Position, Position);
    }

    if (Network.IsServer)
    {
      _Health.Set(MaxHealth);
    }

    _Health.OnSync += (oldVal, newVal) =>
    {
      OnChanged?.Invoke(oldVal, newVal);

      if (newVal <= 0)
      {
        if (Network.IsServer)
        {
          _DiedAt.Set(Time.TimeSinceStartup);
        }
      }
    };
  }
}
