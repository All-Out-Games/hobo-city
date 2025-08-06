using AO;

namespace ReusableWeapons
{
    public class GoldenRocketLauncherConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 5.0f;
        public static float BASE_DAMAGE = 100.0f;
        public static float AOE_RADIUS = 5.0f;

        public static long AMMO_WITH_FIRST_PICKUP = 18;
        public static long AMMO_WITH_EXTRA_PICKUP = 9;

        public static float ROCKET_TRAVEL_SPEED = 10.0f;
    }

    public class GoldenRocketLauncher : Weapon
    {
        public GoldenRocketLauncher(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.GoldenRocketLauncher;
        public override string WeaponSkin => "weapons/golden_bazooka";
        public override string ProjectilePrefab => "Projectile_GoldenRocketLauncher.prefab";
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.HeavyAmmo;
        public override long AmmoWithFirstPickup => GoldenRocketLauncherConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => GoldenRocketLauncherConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override float BaseTimeBetweenShots => GoldenRocketLauncherConfigs.TIME_BETWEEN_SHOTS;
        public override string UseIkBool => USE_1_HAND_IK;
        public override string FireAnimation => SHOOT_1_HAND_TRIGGER;
        public override string FireSound => "sounds/reusable-weapons/rocket_launcher_shoot.wav";

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<GoldenRocketLauncherAbility>();
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
                inputPlayer.ActivateAbility<GoldenRocketLauncherAbility>(0);
            }
        }

        public override BaseProjectile SpawnProjectile(MyPlayer player, string projectileID = null, Vector2? spawnPosition = null, Vector2? shootDirection = null)
        {
            // Don't call base.SpawnProjectile since this one shoots with a lobbed projectile
            //return base.SpawnProjectile(player, projectileID, spawnPosition, shootDirection);

            if (Network.IsServer)
            {
                return LobbedProjectile.Spawn(Assets.GetAsset<Prefab>(ProjectilePrefab), player, GetBulletSpawnPosition(player), GetClickedPosition(player), GoldenRocketLauncherConfigs.ROCKET_TRAVEL_SPEED, ApplyRarityToDamage(GoldenRocketLauncherConfigs.BASE_DAMAGE, player), ServerOnLanded);
            }

            return null;
        }

        public void ServerOnLanded(MyPlayer owner, Vector2 position)
        {
            if (!Network.IsServer) return;

            foreach (var mob in Scene.Components<ThingWithHealth>())
            {
                if (!mob.Alive()) continue;

                if ((mob.Entity.Position - position).LengthSquared <= (GoldenRocketLauncherConfigs.AOE_RADIUS * GoldenRocketLauncherConfigs.AOE_RADIUS))
                {
                    float damageToApply = ApplyRarityToDamage(GoldenRocketLauncherConfigs.BASE_DAMAGE, owner);
                    mob.Damage((int)damageToApply, owner.Entity);
                }
            }
        }
    }

    public class GoldenRocketLauncherAbility : BaseSingleShotWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(CircleAOEWeaponAimingEffect) : null;

        public override float Cooldown => CalculateCooldown(EquippedWeapon != null ? GetWeaponCooldownWithRarity() : GoldenRocketLauncherConfigs.TIME_BETWEEN_SHOTS);
        public override float MaxDistance => 10f;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.BoomWheel.ItemDefinition.Icon);
    }

    public partial class GoldenRocketLauncherProjectile : LobbedProjectile
    {
        public override string ProjectileSkin => "gold_rocket";

        [ClientRpc]
        public override void ClientOnLanded(Vector2 position)
        {
            SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/rocket_launcher_explode.wav"), new SFX.PlaySoundDesc() { Position = position, Positional = true, RangeMultiplier = 3.0f });
            VFXManager.Instance.TrySpawnVFX(WeaponReferences.Instance.VFX_GoldNuke, position);
        }
    }
}
