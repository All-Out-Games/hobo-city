using AO;

namespace ReusableWeapons
{
    public static class GatlingGunConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 0.1f;

        public static int AMMO_WITH_FIRST_PICKUP = 360;
        public static int AMMO_WITH_EXTRA_PICKUP = 180;

        public static float BASE_DAMAGE = 4.0f;
    }

    public class GatlingGun : Weapon
    {
        public GatlingGun(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        private static readonly string[] FireSounds =
        {
            "sounds/reusable-weapons/gatling_gun_01.wav",
            "sounds/reusable-weapons/gatling_gun_02.wav",
            "sounds/reusable-weapons/gatling_gun_03.wav",
            "sounds/reusable-weapons/gatling_gun_04.wav",
            "sounds/reusable-weapons/gatling_gun_05.wav",
        };

        public override WeaponType WeaponType => WeaponType.GatlingGun;
        public override string WeaponSkin => "weapons/gatling_gun";
        public override string ProjectilePrefab => "Projectile_GatlingGun.prefab";
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.LightAmmo;
        public override long AmmoWithFirstPickup => GatlingGunConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => GatlingGunConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override float BaseTimeBetweenShots => GatlingGunConfigs.TIME_BETWEEN_SHOTS;
        public override string FireAnimation => null;
        public override string FireSound => FireSounds.GetRandom();
        public override float CamShakeIntensity => 0.07f;

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return (player.IsPlayingOnMobile)
                ? player.GetAbility<GatlingGunMobileAbility>()
                : player.GetAbility<GatlingGunPCAbility>();
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
            player.RemoveEffect<GatlingGunShootingEffect>(false);
        }

        public override void TryHandleMouseDown(MyPlayer inputPlayer)
        {
            base.TryHandleMouseDown(inputPlayer);

            if (CheckHasEnoughAmmo(inputPlayer))
            {
                inputPlayer.AddEffect<GatlingGunShootingEffect>();
            }
        }

        public override void TryHandleMouseUp(MyPlayer inputPlayer)
        {
            base.TryHandleMouseUp(inputPlayer);

            inputPlayer.RemoveEffect<GatlingGunShootingEffect>(false);
        }
    }

    public class GatlingGunMobileAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.Line;
        public override bool DrawAimingIndicators => true;
        public override Type TargettingEffect => typeof(GatlingGunShootingEffect);
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.GatlingGun.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
        public override Type Effect => typeof(WeaponContinuousShootingStopEffect);
    }

    public class GatlingGunPCAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.Self;
        public override bool DrawAimingIndicators => false;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.GatlingGun.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
    }

    public class GatlingGunShootingEffect : WeaponContinuousShootingEffect
    {
        public override string StartShootingTrigger => "shoot_gatling_start";
        public override string StopShootingTrigger => "shoot_gatling_end";

        private ulong ShootSoundId;

        public override void OnEffectStart(bool isDropIn)
        {
            base.OnEffectStart(isDropIn);

            ShootSoundId = SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/gatling_gun_spin_up_loop.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Entity, Loop = true, Volume = 0.5f });
        }

        public override void OnEffectEnd(bool interrupt)
        {
            base.OnEffectEnd(interrupt);

            SFX.Stop(ShootSoundId);
        }
    }

    public class GatlingGunProjectile : BaseProjectile
    {
        public override string ProjectileSkin => "bullet";
        public override string TravelAnimation => BaseProjectile.TravelNormal;
        public override string HitAnimation => BaseProjectile.HitNormal;
        public override float BaseDamage => GatlingGunConfigs.BASE_DAMAGE;
    }
}