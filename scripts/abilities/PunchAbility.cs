using AO;
using System.Collections.Generic;
using System;

public partial class PunchAbility : MyAbility
{
  public override TargettingMode TargettingMode => TargettingMode.Self;

  public int PunchDamage = 6;
  public int DestructibleDamgeMultiplier = 14;
  public override float MaxDistance => 3.25f;
  public override int MaxTargets => 1;
  public override Texture Icon => GetCachedNearbyPunchable().Alive() ?
    Assets.GetAsset<Texture>("icons/weapons/punch-active.png") :
    Assets.GetAsset<Texture>("icons/weapons/punch-inactive.png");
  public override float Cooldown => 0.25f;

  float lastCacheTime = 0;
  float cacheRefreshRate = 0.15f; // 250ms
  ThingWithHealth cachedTarget = null;

  // For tracking highlighted destructibles with timestamps
  Dictionary<Entity, float> highlightedEntities = new();
  float highlightDuration = 0.2f; // 200ms
  Vector4 normalBrightness = new Vector4(1f, 1f, 1f, 1f);
  Vector4 highlightBrightness = new Vector4(2.5f, 2.5f, 2.5f, 1f);

  ThingWithHealth GetCachedNearbyPunchable()
  {
    float currentTime = Time.TimeSinceStartup;
    if (currentTime - lastCacheTime >= cacheRefreshRate)
    {
      cachedTarget = TryGetNearbyPunchable();
      lastCacheTime = currentTime;
    }
    return cachedTarget;
  }

  static List<ThingWithHealth> ThingsWithHealthCache = new();
  ThingWithHealth TryGetNearbyPunchable()
  {
    if (Player == null || !Player.Entity.Alive()) return null;

    float currentTime = Time.TimeSinceStartup;
    HashSet<Entity> currentNearbyEntities = new();

    ThingsWithHealthCache.Clear();
    GameManager.Instance.ThingWithHealthQuadtree.Query(Player.Entity.Position, MaxDistance, ThingsWithHealthCache);

    foreach (var player in Scene.Components<MyPlayer>())
    {
      if (player.Entity == Player.Entity) continue;
      if (player.Entity.Alive())
      {
        ThingsWithHealthCache.Add(player.Entity.GetComponent<ThingWithHealth>());
      }
    }

    // Highlight nearby destructibles and track them
    if (Network.IsClient)
    {
      foreach (var target in ThingsWithHealthCache)
      {
        if (!target.Alive()) continue;
        if (target.DiedAt >= 0) continue;
        if (target.Entity == Player.Entity) continue;
        if (target.IsInvulnerable) continue;

        float distance = Vector2.Distance(Player.Entity.Position, target.Entity.Position);
        if (distance <= MaxDistance)
        {
          currentNearbyEntities.Add(target.Entity);

          // Only highlight destructibles (non-players)
          if (!target.GetComponent<MyPlayer>().Alive())
          {
            if (!highlightedEntities.ContainsKey(target.Entity))
            {
              // Add to highlighted set with current time and brighten immediately
              highlightedEntities[target.Entity] = currentTime;
              var spineAnimator = target.Entity.GetComponent<Spine_Animator>();
              if (spineAnimator.Alive())
              {
                spineAnimator.SpineInstance.ColorMultiplier = highlightBrightness;
              }
            }
            else
            {
              // Refresh the timestamp for entities still in range
              highlightedEntities[target.Entity] = currentTime;
            }
          }
        }
      }

      // Clean up expired highlights - entities that have exceeded their time
      var entitiesToRemove = new List<Entity>();
      foreach (var kvp in highlightedEntities)
      {
        var entity = kvp.Key;
        var timestamp = kvp.Value;

        if (entity.Alive() && currentTime - timestamp > highlightDuration)
        {
          // Remove highlight and mark for removal
          var spineAnimator = entity.GetComponent<Spine_Animator>();
          if (spineAnimator.Alive())
          {
            spineAnimator.SpineInstance.ColorMultiplier = normalBrightness;
          }
          entitiesToRemove.Add(entity);
        }
      }

      foreach (var entity in entitiesToRemove)
      {
        highlightedEntities.Remove(entity);
      }
    }

    ThingWithHealth closestTarget = null;
    float minDistance = MaxDistance;
    foreach (var target in ThingsWithHealthCache)
    {
      if (target.DiedAt >= 0) continue;
      if (target.Entity == Player.Entity) continue;
      if (target.IsInvulnerable) continue;

      float distance = Vector2.Distance(Player.Entity.Position, target.Entity.Position);
      if (distance < minDistance)
      {
        minDistance = distance;
        closestTarget = target;
      }
    }

    return closestTarget;
  }

  public override bool OnTryActivate(List<Player> targetPlayers, Vector2 positionOrDirection, float magnitude)
  {
    var nearestPunchable = GetCachedNearbyPunchable();

    if (nearestPunchable.Alive())
    {
      SFX.Play(Assets.GetAsset<AudioAsset>("sfx/weapons/punch-hit.wav"), new SFX.PlaySoundDesc() { Volume = 0.5f, Positional = true, VolumePerturb = 0.1f, SpeedPerturb = 0.2f, Position = Player.Entity.Position });
      Player.SetAimTarget(nearestPunchable.Entity.Position);
    }

    Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("punch2");

    if (nearestPunchable.Alive())
    {
      if (Network.IsServer)
      {
        if (nearestPunchable.IsDestructable)
        {
          nearestPunchable.Damage(PunchDamage * DestructibleDamgeMultiplier + (int)(Player.SwoleLevel.Value * 5f), Player.Entity);
        }
        else
        {
          nearestPunchable.Damage(PunchDamage + (int)(Player.SwoleLevel.Value * 5f), Player.Entity);
        }
      }

      return true;
    }

    // no hit target
    SFX.Play(Assets.GetAsset<AudioAsset>("sfx/weapons/punch-air.wav"), new SFX.PlaySoundDesc() { Volume = 0.5f, Positional = true, VolumePerturb = 0.1f, SpeedPerturb = 0.2f, Position = Player.Entity.Position });
    return false;
  }

  public override bool CanUse()
  {
    return Player.CanDealDamage;
  }

  public override bool CanTarget(Player player)
  {
    return true;
  }
}