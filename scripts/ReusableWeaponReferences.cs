using AO;

namespace ReusableWeapons
{
    /// <summary>
    /// In Red Sun, there is a References class that just links to a bunch of useful prefabs
    /// This is the same idea but split into a separate class in case the game also has a References class
    /// This is also prefabbed so should be relatively easy to drop into any project
    /// If for some reason the links are broken, the prefabs are intentionally named the exact same as the fields here so should be easy to replace them
    /// </summary>
    public class WeaponReferences : Singleton<_WeaponReferences> { }
    public class _WeaponReferences : Component
    {
        // Loot
        [Serialized] public Prefab Loot_Chest;
        [Serialized] public Prefab Loot_Pickup;

        // Weapon VFX
        [Serialized] public Prefab VFX_WaterBalloonSplash;
        [Serialized] public Prefab VFX_ShotgunExplosion;
        [Serialized] public Prefab VFX_PlayerHeal;
        [Serialized] public Prefab VFX_ProjectileSmallBurst;
        [Serialized] public Prefab VFX_MobPoisonedAura;
        [Serialized] public Prefab VFX_MobSlowedAura;
        [Serialized] public Prefab VFX_MobParalyzedAura;
        [Serialized] public Prefab VFX_MobBurningAura;
        [Serialized] public Prefab VFX_BoomwheelNuke;
        [Serialized] public Prefab VFX_BoomwheelCrater;
        [Serialized] public Prefab VFX_GoldNuke;
        [Serialized] public Prefab VFX_MissileHit;
        [Serialized] public Prefab VFX_DynamiteExplosion;
        [Serialized] public Prefab VFX_PlasmaProjectileBurst;
        [Serialized] public Prefab VFX_IceBurst;

        // Weapon Supports (ex: spawned in when using a weapon)
        [Serialized] public Prefab Weapon_PoisonGrenadeCloud;
        [Serialized] public Prefab Weapon_BoomwheelCannon;
        [Serialized] public Prefab Weapon_OctoSiphonerBeam;
        [Serialized] public Prefab Weapon_OctoSiphonerAOEHeal;
        [Serialized] public Prefab Weapon_SSSArmedDynamite;
        [Serialized] public Prefab Weapon_VoidSplitterBeam;
        [Serialized] public Prefab Weapon_HydroCannonGeyser;
        [Serialized] public Prefab Weapon_GlaciatorIceBlock;
        [Serialized] public Prefab Weapon_StormsEyeTornado;
    }
}