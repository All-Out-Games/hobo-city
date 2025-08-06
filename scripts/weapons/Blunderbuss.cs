using AO;

namespace ReusableWeapons
{
    public static class BlunderbussConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 2.0f;
        public static float BASE_DAMAGE = 30.0f;

        public static int AMMO_WITH_FIRST_PICKUP = 30;
        public static int AMMO_WITH_EXTRA_PICKUP = 15;
    }

    public class Blunderbuss : Weapon
    {
        public Blunderbuss(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.Blunderbuss;
        public override string WeaponSkin => "weapons/blunderbuss";
        public override string ProjectilePrefab => "Projectile_Blunderbuss.prefab";
        public override float BaseTimeBetweenShots => BlunderbussConfigs.TIME_BETWEEN_SHOTS;
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.HeavyAmmo;
        public override long AmmoWithFirstPickup => BlunderbussConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => BlunderbussConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override string UseIkBool => USE_1_HAND_IK;
        public override string FireAnimation => SHOOT_1_HAND_TRIGGER;
        public override string FireSound => "sounds/reusable-weapons/blunderbuss_shoot.wav";

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<BlunderbussAbility>();
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
                inputPlayer.ActivateAbility<BlunderbussAbility>(0);
            }
        }
    }

    public class BlunderbussAbility : BaseSingleShotWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override bool DrawAimingIndicators => false;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(BasicWeaponAimingEffect) : null; // Having a targetting effect on this breaks the aiming on PC since the aiming effect is already on
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.Blunderbuss.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
        public override float Cooldown => CalculateCooldown(EquippedWeapon != null ? GetWeaponCooldownWithRarity() : BlunderbussConfigs.TIME_BETWEEN_SHOTS);
    }

    public class BlunderbussProjectile : BaseProjectile
    {
        public override string TravelAnimation => BaseProjectile.TravelSpin;
        public override string HitAnimation => BaseProjectile.HitSpin;
        public override string ProjectileSkin => "cannonball";
        public override float BaseDamage => BlunderbussConfigs.BASE_DAMAGE;
    }
}