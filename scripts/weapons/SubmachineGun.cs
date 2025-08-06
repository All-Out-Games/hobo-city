using AO;

namespace ReusableWeapons
{
    public static class SubmachineGunConfigs
    {
        public const float TIME_BETWEEN_SHOTS = 0.2f;
        public const float BASE_DAMAGE = 5.0f;

        public const int AMMO_WITH_FIRST_PICKUP = 300;
        public const int AMMO_WITH_EXTRA_PICKUP = 150;
    }

    public class SubmachineGun : Weapon
    {
        private static readonly string[] FireSounds =
        {
            "sounds/reusable-weapons/smg_shoot_01.wav",
            "sounds/reusable-weapons/smg_shoot_02.wav",
            "sounds/reusable-weapons/smg_shoot_03.wav",
            "sounds/reusable-weapons/smg_shoot_04.wav",
            "sounds/reusable-weapons/smg_shoot_05.wav",
            "sounds/reusable-weapons/smg_shoot_06.wav",
            "sounds/reusable-weapons/smg_shoot_07.wav",
            "sounds/reusable-weapons/smg_shoot_08.wav",
            "sounds/reusable-weapons/smg_shoot_09.wav",
            "sounds/reusable-weapons/smg_shoot_10.wav",
        };

        public SubmachineGun(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.SubmachineGun;
        public override string WeaponSkin => "weapons/smg";
        public override string ProjectilePrefab => "Projectile_SubmachineGun.prefab";

        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.LightAmmo;
        public override long AmmoWithFirstPickup => SubmachineGunConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => SubmachineGunConfigs.AMMO_WITH_EXTRA_PICKUP;

        public override float BaseTimeBetweenShots => SubmachineGunConfigs.TIME_BETWEEN_SHOTS;
        public override string FireAnimation => null;
        public override float CamShakeIntensity => 0.09f;
        public override string FireSound => FireSounds.GetRandom();

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return (player.IsPlayingOnMobile)
                ? player.GetAbility<SubmachineGunMobileAbility>()
                : player.GetAbility<SubmachineGunPCAbility>();
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
            player.RemoveEffect<WeaponContinuousShootingEffect>(false);
        }

        public override void TryHandleMouseDown(MyPlayer inputPlayer)
        {
            base.TryHandleMouseDown(inputPlayer);

            if (CheckHasEnoughAmmo(inputPlayer))
            {
                inputPlayer.AddEffect<WeaponContinuousShootingEffect>();
            }
        }

        public override void TryHandleMouseUp(MyPlayer inputPlayer)
        {
            base.TryHandleMouseUp(inputPlayer);

            inputPlayer.RemoveEffect<WeaponContinuousShootingEffect>(false);
        }
    }

    // Mobile – shows a line-aim indicator, continuous-fire while held.
    public class SubmachineGunMobileAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.Line;
        public override bool DrawAimingIndicators => true;
        public override Type TargettingEffect => typeof(WeaponContinuousShootingEffect);
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.SubmachineGun.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
        public override Type Effect => typeof(WeaponContinuousShootingStopEffect);
    }

    // PC – no aim indicator; weapon is controlled via mouse button.
    public class SubmachineGunPCAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.Self;
        public override bool DrawAimingIndicators => false;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.SubmachineGun.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
    }

    public class SubmachineGunProjectile : BaseProjectile
    {
        public override string ProjectileSkin => "bullet";
        public override string TravelAnimation => BaseProjectile.TravelNormal;
        public override string HitAnimation => BaseProjectile.HitNormal;
        public override float BaseDamage => SubmachineGunConfigs.BASE_DAMAGE;
    }
}