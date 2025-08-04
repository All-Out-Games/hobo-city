using AO;

namespace ReusableWeapons
{
    public static class ExplosiveShotgunConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 2.0f;
        public static float PROJECTILE_HIT_BASE_DAMAGE = 10.0f; // Damage from the bullet directly hitting the target, does not include the AOE explosion damage

        public static int AMMO_WITH_FIRST_PICKUP = 18;
        public static int AMMO_WITH_EXTRA_PICKUP = 9;

        public static float AOE_RADIUS = 1.0f;
        public static float AOE_DAMAGE = 5.0f;

        public const int PROJECTILES_PER_SHOT = 3;
        public const float ANGLE_BETWEEN_PROJECTILES = 10.0f;
    }

    public class ExplosiveShotgun : Weapon
    {
        public ExplosiveShotgun(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.ExplosiveShotgun;
        public override string WeaponSkin => "weapons/shotgun";
        public override string ProjectilePrefab => "Projectile_ExplosiveShotgun.prefab";
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.ShotgunShells;
        public override long AmmoWithFirstPickup => ExplosiveShotgunConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => ExplosiveShotgunConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override float BaseTimeBetweenShots => ExplosiveShotgunConfigs.TIME_BETWEEN_SHOTS;
        public override string FireSound => "sounds/reusable-weapons/explosive_shotgun_shoot.wav";

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<ExplosiveShotgunAbility>();
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
                inputPlayer.ActivateAbility<ExplosiveShotgunAbility>(0);
            }
        }
    }

    public class ExplosiveShotgunAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override bool DrawAimingIndicators => false;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(BasicWeaponAimingEffect) : null;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.ExplosiveShotgun.ItemDefinition.Icon);
        public override float MaxDistance => 5.0f;
        public override float Cooldown => CalculateCooldown(EquippedWeapon?.TimeBetweenShotsAfterRarity ?? ExplosiveShotgunConfigs.TIME_BETWEEN_SHOTS);

        public override bool OnTryActivate(List<Player> targetPlayers, Vector2 direction, float magnitude)
        {
            base.OnTryActivate(targetPlayers, direction, magnitude);

            if (EquippedWeapon != null)
            {
                // First projectile is straight towards the main direction
                EquippedWeapon.SpawnProjectile(Player);

                // Other projectiles are spread out evenly in a cone shape
                int spreadInterval = 1;
                for (int i = 1; i < ExplosiveShotgunConfigs.PROJECTILES_PER_SHOT; i += 2, spreadInterval++)
                {
                    float offsetMagnitude = ExplosiveShotgunConfigs.ANGLE_BETWEEN_PROJECTILES * spreadInterval;

                    Vector2 positiveOffset = CustomUtil.GetRotatedVector(direction, offsetMagnitude);
                    Vector2 negativeOffset = CustomUtil.GetRotatedVector(direction, -offsetMagnitude);

                    EquippedWeapon.SpawnProjectile(Player, shootDirection: positiveOffset);
                    EquippedWeapon.SpawnProjectile(Player, shootDirection: negativeOffset);
                }

                // IMPORTANT: This needs to be called after all the projectiles have been spawned
                // Otherwise, the equipped weapon can become null when running out of ammo and then the above code will crash
                EquippedWeapon.Shoot(Player);
            }

            return true;
        }
    }

    public class ExplosiveShotgunProjectile : BaseProjectile
    {
        public override string ProjectileSkin => "explosive_shell";
        public override float BaseDamage => ExplosiveShotgunConfigs.PROJECTILE_HIT_BASE_DAMAGE;

        public override string TravelAnimation => BaseProjectile.TravelNormal;
        public override string HitAnimation => BaseProjectile.HitNormal;
        public override Prefab HitVFX => WeaponReferences.Instance.VFX_ShotgunExplosion;

        public override void OnImpact(Entity other, ProjectileCollisionType collisionType)
        {
            base.OnImpact(other, collisionType);

            if (Network.IsServer)
            {
                foreach (var mob in Scene.Components<ThingWithHealth>())
                {
                    if ((mob.Entity.Position - Entity.Position).LengthSquared <= (ExplosiveShotgunConfigs.AOE_RADIUS * ExplosiveShotgunConfigs.AOE_RADIUS))
                    {
                        var damage = (int)ExplosiveShotgunConfigs.AOE_DAMAGE;

                        if (mob.Entity.GetComponent<Destructable>() != null)
                        {
                            damage *= 3;
                        }

                        mob.Damage(damage, Owner.Entity);
                    }
                }
            }
        }
    }
}