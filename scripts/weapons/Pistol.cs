using AO;
using ReusableWeapons;

public class Pistol : Weapon
{
    public Pistol(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

    public override WeaponType WeaponType => WeaponType.Blunderbuss;
    public override string WeaponSkin => "weapons/pistol";
    public override string ProjectilePrefab => "ProjectilePistol.prefab";
    // public override Vector2 BulletSpawnOffset => new Vector2(1.0f, 0.5f);
    public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.LightAmmo;
    public override float BaseTimeBetweenShots => PISTOL_TIME_BETWEEN_SHOTS;
    public override long AmmoWithFirstPickup => 12;
    public override long AmmoWithExtraPickup => 6;
    public override string FireSound => "sfx/weapons/pistol-shoot.wav";

    public static float PISTOL_TIME_BETWEEN_SHOTS = 0.75f;


    public override Ability GetPrimaryAbility(MyPlayer player)
    {
        return player.GetAbility<PistolAbility>();
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
            inputPlayer.ActivateAbility<PistolAbility>(0);
        }
    }
}

public class PistolAbility : BaseSingleShotWeaponAbility
{
    public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
    public override bool DrawAimingIndicators => false;
    public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(BasicWeaponAimingEffect) : null; // Having a targetting effect on this breaks the aiming on PC since the aiming effect is already on
    public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.Pistol.ItemDefinition.Icon);
    public override float MaxDistance => 10f;
    public override float Cooldown => CalculateCooldown(EquippedWeapon != null ? GetWeaponCooldownWithRarity() : Pistol.PISTOL_TIME_BETWEEN_SHOTS);
}

public class PistolProjectile : BaseProjectile
{
    public override string TravelAnimation => "016ARP/Bullet_Loop";
    public override string HitAnimation => "016ARP/Bullet_Loop";
    public override string ProjectileSkin => "bullet_1";
    public override float BaseDamage => 9.0f;
}