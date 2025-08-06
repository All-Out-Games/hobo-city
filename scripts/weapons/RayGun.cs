using AO;

namespace ReusableWeapons
{
    public class RayGunConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 2.0f;

        public static long AMMO_WITH_FIRST_PICKUP = 30;
        public static long AMMO_WITH_EXTRA_PICKUP = 15;

        public static float BASE_DAMAGE = 50.0f;
    }

    public class RayGun : Weapon
    {
        public RayGun(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.RayGun;
        public override string WeaponSkin => "weapons/lightning_gun";
        public override string ProjectilePrefab => "Projectile_RayGun.prefab";
        public override float BaseTimeBetweenShots => RayGunConfigs.TIME_BETWEEN_SHOTS;
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.MediumAmmo;
        public override long AmmoWithFirstPickup => RayGunConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => RayGunConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override string UseIkBool => USE_1_HAND_IK;
        public override string FireAnimation => SHOOT_1_HAND_TRIGGER;
        public override string FireSound => "sounds/reusable-weapons/ray_gun_shoot.wav";

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<RayGunAbility>();
        }

        public override void OnEquip(MyPlayer player)
        {
            base.OnEquip(player);

            if (!player.IsPlayingOnMobile)
            {
                player.RemoveEffect<BasicWeaponAimingEffect>(false);
                player.AddEffect<BasicWeaponAimingEffect>();
            }
        }

        public override void OnUnequip(MyPlayer player)
        {
            base.OnUnequip(player);

            player.RemoveEffect<BasicWeaponAimingEffect>(false);
        }

        public override void TryHandleMouseDown(MyPlayer inputPlayer)
        {
            base.TryHandleMouseDown(inputPlayer);

            if (inputPlayer.IsLocal)
            {
                inputPlayer.ActivateAbility<RayGunAbility>(0);
            }
        }
    }

    public class RayGunAbility : BaseSingleShotWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override bool DrawAimingIndicators => false;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(BasicWeaponAimingEffect) : null; // Having a targetting effect on this breaks the aiming on PC since the aiming effect is already on
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.BoomWheel.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
        public override float Cooldown => CalculateCooldown(EquippedWeapon != null ? GetWeaponCooldownWithRarity() : RayGunConfigs.TIME_BETWEEN_SHOTS);
    }

    public class RayGunProjectile : BaseProjectile
    {
        public override string TravelAnimation => BaseProjectile.TravelNormal;
        public override string HitAnimation => BaseProjectile.HitNormal;
        public override string ProjectileSkin => "ray_gun";
        public override float BaseDamage => RayGunConfigs.BASE_DAMAGE;
    }
}