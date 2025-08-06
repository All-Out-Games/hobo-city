using AO;
using ReusableWeapons;

public class AssaultRifle : Weapon
{
    public AssaultRifle(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

    public override WeaponType WeaponType => WeaponType.AssaultRifle;
    public override string WeaponSkin => "weapons/assault_rifle";
    public override string ProjectilePrefab => "Projectile_AssaultRifle.prefab"; /*"Projectile_AssaultRifle.prefab";*/
    public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.MediumAmmo;
    public override float BaseTimeBetweenShots => 0.26f;
    public override long AmmoWithFirstPickup => 60;
    public override long AmmoWithExtraPickup => 30;
    public override string FireAnimation => null;
    public override float CamShakeIntensity => 0.08f;
    public override string FireSound => "sfx/weapons/rifle-shoot.wav";

    public override Ability GetPrimaryAbility(MyPlayer player)
    {
        return (player.IsPlayingOnMobile)
            ? player.GetAbility<AssaultRifleMobileAbility>()
            : player.GetAbility<AssaultRiflePCAbility>();
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

// This ability is only used on mobile
public class AssaultRifleMobileAbility : BaseWeaponAbility
{
    public override TargettingMode TargettingMode => TargettingMode.Line;
    public override bool DrawAimingIndicators => true;
    public override Type TargettingEffect => typeof(WeaponContinuousShootingEffect);
    public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.AssaultRifle.ItemDefinition.Icon);
    public override float MaxDistance => 10f;
    public override Type Effect => typeof(WeaponContinuousShootingStopEffect); // The shooting for this gun is actually in the aiming effect. This is triggered when the ability is let go and so in that case, we actually want to stop shooting
}

// This ability basically doesn't do anything, it's just used to show the weapon's icon in the ability system
public class AssaultRiflePCAbility : BaseWeaponAbility
{
    public override TargettingMode TargettingMode => TargettingMode.Self;
    public override bool DrawAimingIndicators => false;
    public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.AssaultRifle.ItemDefinition.Icon);
    public override float MaxDistance => 10f;
}

public class AssaultRifleProjectile : BaseProjectile
{
    public override string TravelAnimation => "016ARP/Bullet_Loop";
    public override string HitAnimation => "016ARP/Bullet_Loop";
    public override string ProjectileSkin => "bullet";
    public override float BaseDamage => 7f;
}