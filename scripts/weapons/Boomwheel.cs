using AO;

namespace ReusableWeapons
{
    public static class BoomwheelConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 45f;

        public static float LARGE_BOMB_DAMAGE = 50.0f;
        public static float LARGE_BOMB_AOE_RADIUS = 5.0f;
        public static float LARGE_BOMB_TRAVEL_SPEED = 10.0f;

        public static float SMALL_BOMB_DAMAGE = 30.0f;
        public static float SMALL_BOMB_AOE_RADIUS = 2.0f;
        public static float SMALL_BOMB_TRAVEL_SPEED = 6.0f;
        public static int SMALL_BOMBS_PER_LARGE_BOMB = 10;
        public static float SMALL_BOMB_TARGET_RADIUS = 5.0f; // Radius of the circle that the small bombs will land on

        public static Vector2 CANNON_LOCAL_OFFSET = new Vector2(1.0f, 0.15f); // Visual offset of the cannon relative to the player's rig (shouldn't need to tweak this)
    }

    public class Boomwheel : Weapon
    {
        public Boomwheel(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.Boomwheel;
        public override string WeaponSkin => null; // This weapon spawns a cannon next to the player instead of being a skin on the player directly
        public override string ProjectilePrefab => "Projectile_BoomwheelLargeBomb.prefab";
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.HeavyAmmo;
        public override long AmmoWithFirstPickup => 18;
        public override long AmmoWithExtraPickup => 9;
        public override float BaseTimeBetweenShots => BoomwheelConfigs.TIME_BETWEEN_SHOTS;
        public override string FireAnimation => null;
        public override string FireSound => "sounds/reusable-weapons/grenade_explode.wav";


        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<BoomwheelAbility>();
        }

        public override void OnEquip(MyPlayer player)
        {
            base.OnEquip(player);

            if (!player.IsPlayingOnMobile)
            {
                player.RemoveEffect<CircleAOEWeaponAimingEffect>(false);
                player.AddEffect<CircleAOEWeaponAimingEffect>();
            }

            player.AddEffect<BoomwheelCannonEffect>();
        }

        public override void OnUnequip(MyPlayer player)
        {
            base.OnUnequip(player);

            player.RemoveEffect<CircleAOEWeaponAimingEffect>(false);
            player.RemoveEffect<BoomwheelCannonEffect>(false);
        }

        public override void TryHandleMouseDown(MyPlayer inputPlayer)
        {
            base.TryHandleMouseDown(inputPlayer);

            if (inputPlayer.IsLocal)
            {
                inputPlayer.ActivateAbility<BoomwheelAbility>(0);
            }
        }

        public override Vector2 GetBulletSpawnPosition(MyPlayer player)
        {
            // Don't call base - uses the weapon rig's bone position but that isn't in this
            //return base.GetBulletSpawnPosition(player);

            // Instead, since the cannon is a child of the player, we can just use the player's position with an offset
            return player.Entity.CalculateWorldPosition(BoomwheelConfigs.CANNON_LOCAL_OFFSET);
        }

        public override void Shoot(MyPlayer player, int? ammoToConsume = null)
        {
            base.Shoot(player, 5);

            if (player.TryGetEffect<BoomwheelCannonEffect>(out var boomwheelCannonEffect))
            {
                boomwheelCannonEffect.Cannon.Shoot();
            }
        }

        public override BaseProjectile SpawnProjectile(MyPlayer player, string projectileID = null, Vector2? spawnPosition = null, Vector2? shootDirection = null)
        {
            // Don't call base.SpawnProjectile since this one shoots with a custom projectile
            //return base.SpawnProjectile(player, projectileID, spawnPosition, shootDirection);

            if (Network.IsServer)
            {
                return LobbedProjectile.Spawn(Assets.GetAsset<Prefab>(ProjectilePrefab), player, GetBulletSpawnPosition(player), GetClickedPosition(player), BoomwheelConfigs.LARGE_BOMB_TRAVEL_SPEED, ApplyRarityToDamage(BoomwheelConfigs.LARGE_BOMB_DAMAGE, player), ServerOnLargeBombImpact);
            }

            return null;
        }

        public void ServerOnLargeBombImpact(MyPlayer owner, Vector2 position)
        {
            if (!Network.IsServer) return;

            foreach (var mob in Scene.Components<ThingWithHealth>())
            {
                if (!mob.Alive()) continue;
                if (mob.Entity == owner.Entity) continue;

                if ((mob.Entity.Position - position).LengthSquared <= (BoomwheelConfigs.LARGE_BOMB_AOE_RADIUS * BoomwheelConfigs.LARGE_BOMB_AOE_RADIUS))
                {
                    var damage = (int)BoomwheelConfigs.LARGE_BOMB_DAMAGE;

                    if (mob.Entity.GetComponent<Destructable>() != null)
                    {
                        damage *= 3;
                    }

                    mob.Damage(damage, owner.Entity);
                }
            }

            // Spawn all of the small bombs
            for (int i = 0; i < BoomwheelConfigs.SMALL_BOMBS_PER_LARGE_BOMB; i++)
            {
                float radians = i * (float)Math.PI * 2.0f / BoomwheelConfigs.SMALL_BOMBS_PER_LARGE_BOMB;

                Vector2 targetOffsetNormalized = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
                Vector2 targetOffset = targetOffsetNormalized.Normalized * BoomwheelConfigs.SMALL_BOMB_TARGET_RADIUS;

                var smallBombTarget = position + targetOffset;
                LobbedProjectile.Spawn(Assets.GetAsset<Prefab>("Projectile_BoomwheelSmallBomb.prefab"), owner, position, smallBombTarget, BoomwheelConfigs.SMALL_BOMB_TRAVEL_SPEED, BoomwheelConfigs.SMALL_BOMB_DAMAGE, ServerOnSmallBombImpact);
            }
        }

        public void ServerOnSmallBombImpact(MyPlayer owner, Vector2 position)
        {
            if (!Network.IsServer) return;

            foreach (var mob in Scene.Components<ThingWithHealth>())
            {
                if (!mob.Alive()) continue;
                if (mob.Entity == owner.Entity) continue;

                if ((mob.Entity.Position - position).LengthSquared <= (BoomwheelConfigs.SMALL_BOMB_AOE_RADIUS * BoomwheelConfigs.SMALL_BOMB_AOE_RADIUS))
                {
                    var damage = (int)BoomwheelConfigs.SMALL_BOMB_DAMAGE;

                    if (mob.Entity.GetComponent<Destructable>() != null)
                    {
                        damage *= 10;
                    }

                    mob.Damage(damage, owner.Entity);
                }
            }
        }
    }

    public class BoomwheelCannon : Component
    {
        public const string CANNON_AIM_BONE = "AIM";

        [Serialized] public Spine_Animator Skeleton;

        private ulong MoveSoundId;

        public override void Awake()
        {
            base.Awake();

            SetupSkeleton();

            MoveSoundId = SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/cannon_roll_loop.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Entity, Loop = true, Volume = 0.5f });
        }

        public override void OnDestroy()
        {
            SFX.Stop(MoveSoundId);
        }

        public void SetupSkeleton()
        {
            Skeleton.Awaken();
            var sm = StateMachine.Make();
            Skeleton.SpineInstance.SetStateMachine(sm, Entity);

            var baseLayer = sm.CreateLayer("main");

            var idleState = baseLayer.CreateState("idle", 0, true);
            var moveState = baseLayer.CreateState("roll_loop", 0, true);
            baseLayer.InitialState = idleState;

            var movingVar = sm.CreateVariable("moving", StateMachineVariableKind.BOOLEAN);
            baseLayer.CreateTransition(idleState, moveState, false).CreateBoolCondition(movingVar, true);
            baseLayer.CreateTransition(moveState, idleState, false).CreateBoolCondition(movingVar, false);

            baseLayer.AddSimpleTriggeredState("shoot", "shoot", true);
        }

        public void Shoot()
        {
            Skeleton.SpineInstance.StateMachine.SetTrigger("shoot");
        }

        public void UpdateCannonAnims(MyPlayer player)
        {
            // Update the aiming
            var worldAimPosition = player.Position + (player.CurrentTargettingDirection * player.CurrentTargettingMagnitude);

            // On mobile, the aiming is handled differently internally and so we need to update the aiming target here
            if (player.IsPlayingOnMobile)
            {
                var equippedWeapon = player.CurrentEquippedItem.CustomDefinition as Weapon;
                if (equippedWeapon != null)
                {
                    var aimingPosition = equippedWeapon.GetClickedPosition(player);
                    player.SetAimTarget(aimingPosition);
                }
            }

            Skeleton.SpineInstance.SetBonePosition(CANNON_AIM_BONE, Skeleton.GetBonePositionFromWorld(worldAimPosition, player.GetFacingDirection()));

            // Update the movement
            Skeleton.SpineInstance.StateMachine.SetBool("moving", player.Agent.Velocity.LengthSquared > 0.01f);

            SFX.UpdateSoundDesc(MoveSoundId, new SFX.PlaySoundDesc() { EntityToFollow = Entity, Loop = true, Volume = player.Agent.Velocity.LengthSquared > 0.01f ? 0.5f : float.Epsilon });
        }
    }

    public class BoomwheelCannonEffect : MyEffect
    {
        public override bool IsActiveEffect => false;

        public BoomwheelCannon Cannon;

        public override void OnEffectStart(bool isDropIn)
        {
            base.OnEffectStart(isDropIn);

            CreateCannon();

            Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("hands_out_start");
        }

        public void CreateCannon()
        {
            var cannonInstance = Entity.Instantiate(WeaponReferences.Instance.Weapon_BoomwheelCannon);
            Cannon = cannonInstance.GetComponent<BoomwheelCannon>();
            Cannon.Entity.SetParent(Player.SpineAnimator.Entity, false);

            Cannon.Entity.LocalPosition = BoomwheelConfigs.CANNON_LOCAL_OFFSET;
            Cannon.Entity.LocalScale = Vector2.One;
        }

        public override void OnEffectUpdate()
        {
            base.OnEffectUpdate();

            if (Cannon != null)
            {
                Cannon.UpdateCannonAnims(Player);
            }
        }

        public override void OnEffectEnd(bool interrupt)
        {
            base.OnEffectEnd(interrupt);

            CleanupCannon();

            Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("hands_out_end");
        }

        public void CleanupCannon()
        {
            if (Cannon != null)
            {
                Cannon.Entity.Destroy();
                Cannon = null;
            }
        }
    }

    public class BoomwheelAbility : BaseSingleShotWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(CircleAOEWeaponAimingEffect) : null;

        public override float Cooldown => CalculateCooldown(EquippedWeapon != null ? GetWeaponCooldownWithRarity() : BoomwheelConfigs.TIME_BETWEEN_SHOTS);
        public override float MaxDistance => 10f;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.BoomWheel.ItemDefinition.Icon);
    }

    public partial class BoomwheelLargeBombProjectile : LobbedProjectile
    {
        public override string ProjectileSkin => "bomb";
        public override string TravelAnimation => TravelSpin;
        public override string HitAnimation => HitSpin;

        [ClientRpc]
        public override void ClientOnLanded(Vector2 position)
        {
            VFXManager.Instance.TrySpawnVFX(WeaponReferences.Instance.VFX_BoomwheelNuke, position, 1.5f);
            VFXManager.Instance.TrySpawnVFX(WeaponReferences.Instance.VFX_BoomwheelCrater, position, 2.0f);

            SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/cannon_explode.wav"), new SFX.PlaySoundDesc() { Position = Entity.Position, Positional = true, RangeMultiplier = 3 });
        }
    }

    public partial class BoomwheelSmallBombProjectile : LobbedProjectile
    {
        public override string ProjectileSkin => "bomb";
        public override string TravelAnimation => TravelSpin;
        public override string HitAnimation => HitSpin;

        [ClientRpc]
        public override void ClientOnLanded(Vector2 position)
        {
            VFXManager.Instance.TrySpawnVFX(WeaponReferences.Instance.VFX_ShotgunExplosion, position, 2.0f);
        }
    }
}
