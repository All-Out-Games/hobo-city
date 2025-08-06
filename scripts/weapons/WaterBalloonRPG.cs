using AO;

namespace ReusableWeapons
{
    public class WaterBalloonRPGConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 5.0f;
        public static float BASE_DAMAGE = 65.0f;

        public static long AMMO_WITH_FIRST_PICKUP = 18;
        public static long AMMO_WITH_EXTRA_PICKUP = 9;

        public static float AOE_RADIUS = 4.0f;

        public static float PROJECTILE_TRAVEL_SPEED = 10.0f;
    }

    public class WaterBalloonRPG : Weapon
    {
        public WaterBalloonRPG(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override string FireSound => "sounds/reusable-weapons/waterballoon_rpg_shoot.wav";

        public override WeaponType WeaponType => WeaponType.WaterBalloonRPG;
        public override string WeaponSkin => "weapons/waterballoon_RPG";
        public override string ProjectilePrefab => "Projectile_WaterBalloonRPG.prefab";
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.LightAmmo;
        public override long AmmoWithFirstPickup => WaterBalloonRPGConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => WaterBalloonRPGConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override float BaseTimeBetweenShots => WaterBalloonRPGConfigs.TIME_BETWEEN_SHOTS;

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<WaterBalloonRPGAbility>();
        }

        public override void OnEquip(MyPlayer player)
        {
            base.OnEquip(player);

            if (!player.IsPlayingOnMobile)
            {
                player.RemoveEffect<CircleAOEWeaponAimingEffect>(false);
                player.AddEffect<CircleAOEWeaponAimingEffect>();
            }
        }

        public override void OnUnequip(MyPlayer player)
        {
            base.OnUnequip(player);

            player.RemoveEffect<CircleAOEWeaponAimingEffect>(false);
        }

        public override void TryHandleMouseDown(MyPlayer inputPlayer)
        {
            base.TryHandleMouseDown(inputPlayer);

            if (inputPlayer.IsLocal)
            {
                inputPlayer.ActivateAbility<WaterBalloonRPGAbility>(0);
            }
        }

        public override BaseProjectile SpawnProjectile(MyPlayer player, string projectileID = null, Vector2? spawnPosition = null, Vector2? shootDirection = null)
        {
            // Don't call base.SpawnProjectile since this one shoots with a lobbed projectile
            //return base.SpawnProjectile(player, projectileID, spawnPosition, shootDirection);

            if (Network.IsServer)
            {
                return LobbedProjectile.Spawn(Assets.GetAsset<Prefab>(ProjectilePrefab), player, GetBulletSpawnPosition(player), GetClickedPosition(player), WaterBalloonRPGConfigs.PROJECTILE_TRAVEL_SPEED, ApplyRarityToDamage(WaterBalloonRPGConfigs.BASE_DAMAGE, player), ServerOnBalloonLanded);
            }

            return null;
        }

        public void ServerOnBalloonLanded(MyPlayer owner, Vector2 position)
        {
            if (!Network.IsServer) return;

            // Damage in a radius
            foreach (var mob in Scene.Components<ThingWithHealth>())
            {
                if (!mob.Alive()) continue;
                if (mob.Entity == owner.Entity) continue;

                if ((mob.Entity.Position - position).LengthSquared <= (WaterBalloonRPGConfigs.AOE_RADIUS * WaterBalloonRPGConfigs.AOE_RADIUS))
                {
                    float damageToApply = ApplyRarityToDamage(WaterBalloonRPGConfigs.BASE_DAMAGE, owner);
                    if (mob.Entity.GetComponent<Destructable>() != null)
                    {
                        damageToApply *= 3;
                    }

                    mob.Damage((int)damageToApply, owner.Entity);
                }
            }
        }
    }

    public class WaterBalloonRPGAbility : BaseSingleShotWeaponAbility
    {
        public const float BALLOON_TRAVEL_SPEED = 10.0f;

        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(CircleAOEWeaponAimingEffect) : null;

        public override float Cooldown => CalculateCooldown(EquippedWeapon != null ? GetWeaponCooldownWithRarity() : WaterBalloonRPGConfigs.TIME_BETWEEN_SHOTS);
        public override float MaxDistance => 10f;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.WaterBalloonRPG.ItemDefinition.Icon);
    }

    public partial class WaterBalloonRPGProjectile : LobbedProjectile
    {
        public override string ProjectileSkin => "water_balloon";
        public override string TravelAnimation => TravelSpin;
        public override string HitAnimation => HitSpin;

        public static readonly string[] Sounds =
        {
            "sounds/reusable-weapons/geyser_splash_01.wav",
            "sounds/reusable-weapons/geyser_splash_02.wav",
            "sounds/reusable-weapons/geyser_splash_03.wav",
        };

        [ClientRpc]
        public override void ClientOnLanded(Vector2 position)
        {
            SFX.Play(Assets.GetAsset<AudioAsset>(Sounds.GetRandom()), new SFX.PlaySoundDesc() { Position = Entity.Position, Positional = true, RangeMultiplier = 3 });

            VFXManager.Instance.TrySpawnVFX(WeaponReferences.Instance.VFX_WaterBalloonSplash, position);
        }
    }

    public partial class WaterBalloonRPGProjectileVisuals : Component
    {
        [Serialized] public Spine_Animator Spine;

        public override void Awake()
        {
            base.Awake();

            SetColour(Random.Shared.Next(0, 10));
        }

        public void SetColour(int colourIndex)
        {
            Spine.SetCrewchsia(colourIndex);
        }
    }
}
