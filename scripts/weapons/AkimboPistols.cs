using AO;

namespace ReusableWeapons
{
    public static class AkimboPistolsConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 0.6f;
        public static float BASE_DAMAGE = 20.0f;

        public static int AMMO_WITH_FIRST_PICKUP = 30;
        public static int AMMO_WITH_EXTRA_PICKUP = 15;
    }

    public class AkimboPistols : Weapon
    {
        public AkimboPistols(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        private static readonly string[] FireSounds =
            {
            "sounds/reusable-weapons/pistol_shoot_01.wav",
            "sounds/reusable-weapons/pistol_shoot_02.wav",
            "sounds/reusable-weapons/pistol_shoot_03.wav",
            "sounds/reusable-weapons/pistol_shoot_04.wav",
        };

        public override string FireSound => FireSounds.GetRandom();

        public override WeaponType WeaponType => WeaponType.AkimboPistols;
        public override string WeaponSkin => "weapons/akimbo_revolvers";
        public override string ProjectilePrefab => "Projectile_AkimboPistols.prefab";
        public override float BaseTimeBetweenShots => AkimboPistolsConfigs.TIME_BETWEEN_SHOTS;
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.HeavyAmmo;
        public override long AmmoWithFirstPickup => AkimboPistolsConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => AkimboPistolsConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override string FireAnimation => null; // Custom effect which fires two shots so can't use the default fire anim flow

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<AkimboPistolsAbility>();
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
                inputPlayer.ActivateAbility<AkimboPistolsAbility>(0);
            }
        }
    }

    public class AkimboPistolsAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override bool DrawAimingIndicators => false;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(BasicWeaponAimingEffect) : null; // Having a targetting effect on this breaks the aiming on PC since the aiming effect is already on
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.BoomWheel.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
        public override float Cooldown => CalculateCooldown(AkimboPistolsConfigs.TIME_BETWEEN_SHOTS);

        public override bool OnTryActivate(List<Player> targetPlayers, Vector2 positionOrDirection, float magnitude)
        {
            base.OnTryActivate(targetPlayers, positionOrDirection, magnitude);

            // Give the player the effect so it can spawn the two bullets at the right timing
            Player.AddEffect<AkimboPistolsShootEffect>(preInit: (effect) =>
            {
                effect.EquippedWeapon = EquippedWeapon;
            });

            return true;
        }
    }

    public class AkimboPistolsShootEffect : MyEffect
    {
        public override bool IsActiveEffect => false;
        public override float DefaultDuration => 0.567f;

        public Weapon EquippedWeapon { get; set; }

        private bool HasFirstShot = false;
        private bool HasSecondShot = false;

        public override void OnEffectStart(bool isDropIn)
        {
            base.OnEffectStart(isDropIn);

            Player.BlockScrollReasons.Add(nameof(AkimboPistolsShootEffect));

            Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("shoot_akimbo_pistols");
        }

        public override void OnEffectUpdate()
        {
            base.OnEffectUpdate();

            if (Util.OneTime(ElapsedTime > 0.001f, ref HasFirstShot))
            {
                TryShootGun(1);
            }
            if (Util.OneTime(ElapsedTime > 0.27f, ref HasSecondShot))
            {
                // Second shot is free since they are tied together
                TryShootGun(0);
            }
        }

        public void TryShootGun(int ammoToConsume)
        {
            if (EquippedWeapon != null)
            {
                EquippedWeapon.SpawnProjectile(Player);
                EquippedWeapon.Shoot(Player, ammoToConsume);
            }
        }

        public override void OnEffectEnd(bool interrupt)
        {
            base.OnEffectEnd(interrupt);

            Player.BlockScrollReasons.Remove(nameof(AkimboPistolsShootEffect));
        }
    }

    public class AkimboPistolsProjectile : BaseProjectile
    {
        public override string TravelAnimation => BaseProjectile.TravelNormal;
        public override string HitAnimation => BaseProjectile.HitNormal;
        public override string ProjectileSkin => "bullet";
        public override float BaseDamage => AkimboPistolsConfigs.BASE_DAMAGE;
    }
}