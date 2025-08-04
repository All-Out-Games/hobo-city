using AO;

namespace ReusableWeapons
{
    public class PoisonGrenadeConfigs
    {
        public static float THROW_COOLDOWN = 10f;

        public static float GRENADE_TRAVEL_SPEED = 10.0f;
        public static float GRENADE_AOE_RADIUS = 5.0f;
        public static float GRENADE_DAMAGE = 45.0f;

        public static float POISON_DAMAGE_PER_TICK = 3.0f;
        public static float POISON_TICK_INTERVAL = 1.0f;
        public static float POISON_DURATION = 10.0f;
    }

    public class PoisonGrenade : Weapon
    {
        public PoisonGrenade(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.PoisonGrenade;
        public override string WeaponSkin => "weapons/grenade";
        public override string ProjectilePrefab => "Projectile_PoisonGrenade.prefab";
        public override Vector2 BulletSpawnOffset => new Vector2(0.0f, 0.0f);
        public override CustomItemDefinition AmmoType => null;
        public override long AmmoWithFirstPickup => -1;
        public override long AmmoWithExtraPickup => -1;
        public override float BaseTimeBetweenShots => PoisonGrenadeConfigs.THROW_COOLDOWN;
        public override string FireAnimation => "throw";
        public override string FireSound => "sounds/reusable-weapons/player_throw_grenade.wav";
        public override string UseIkBool => USE_1_HAND_IK;

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return player.GetAbility<PoisonGrenadeAbility>();
        }

        public override void OnEquip(MyPlayer player)
        {
            base.OnEquip(player);

            if (!player.IsPlayingOnMobile)
            {
                player.RemoveEffect<PoisonGrenadeAimingEffect>(false);
                player.AddEffect<PoisonGrenadeAimingEffect>();
            }
        }

        public override void OnUnequip(MyPlayer player)
        {
            base.OnUnequip(player);

            player.RemoveEffect<PoisonGrenadeAimingEffect>(false);
        }

        public override void TryHandleMouseDown(MyPlayer inputPlayer)
        {
            base.TryHandleMouseDown(inputPlayer);

            if (inputPlayer.IsLocal)
            {
                inputPlayer.ActivateAbility<PoisonGrenadeAbility>(0);
            }
        }

        public override bool CheckHasEnoughAmmo(MyPlayer player, int? amountToCheck = null) => true;

        public override BaseProjectile SpawnProjectile(MyPlayer player, string projectileID = null, Vector2? spawnPosition = null, Vector2? shootDirection = null)
        {
            // Don't call base.SpawnProjectile since this one shoots with a lobbed projectile
            //return base.SpawnProjectile(player, projectileID, spawnPosition, shootDirection);

            if (Network.IsServer)
            {
                player.RequestRemoveItemCountFromSlot(player.CurrentHoveredSlot, 1);
                return LobbedProjectile.Spawn(Assets.GetAsset<Prefab>(ProjectilePrefab), player, player.Position, GetClickedPosition(player), PoisonGrenadeConfigs.GRENADE_TRAVEL_SPEED, ApplyRarityToDamage(PoisonGrenadeConfigs.GRENADE_DAMAGE), ServerOnGrenadeLanded);
            }

            return null;
        }

        public void ServerOnGrenadeLanded(MyPlayer owner, Vector2 position)
        {
            if (!Network.IsServer) return;

            // Damage in a radius
            foreach (var mob in Scene.Components<ThingWithHealth>())
            {
                if (!mob.Alive()) continue;

                if ((mob.Entity.Position - position).LengthSquared <= (PoisonGrenadeConfigs.GRENADE_AOE_RADIUS * PoisonGrenadeConfigs.GRENADE_AOE_RADIUS))
                {
                    float damageToApply = ApplyRarityToDamage(PoisonGrenadeConfigs.GRENADE_DAMAGE);

                    if (mob.GetComponent<Destructable>() != null)
                    {
                        damageToApply *= 5f;
                    }

                    mob.Damage((int)damageToApply, owner.Entity);
                }
            }

            // Spawn a poison cloud
            var poisonCloud = Network.InstantiateAndSpawn(WeaponReferences.Instance.Weapon_PoisonGrenadeCloud);
            poisonCloud.Position = position;

            poisonCloud.GetComponent<PoisonGrenadeAOE>().CallClient_SetOwner(owner);
        }
    }

    public class PoisonGrenadeAimingEffect : CircleAOEWeaponAimingEffect
    {
        public override void OnEffectStart(bool isDropIn)
        {
            base.OnEffectStart(isDropIn);

            Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("throw_aim_start");
            Player.SpineAnimator.SpineInstance.StateMachine.SetBool("aiming_grenade", true);
        }

        public override void OnEffectEnd(bool interrupt)
        {
            base.OnEffectEnd(interrupt);

            Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("throw_aim_end");
            Player.SpineAnimator.SpineInstance.StateMachine.SetBool("aiming_grenade", false);
        }
    }

    public class PoisonGrenadeAbility : BaseSingleShotWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.CircleAOE;
        public override Type TargettingEffect => Player.IsPlayingOnMobile ? typeof(CircleAOEWeaponAimingEffect) : null;

        public override float Cooldown => CalculateCooldown(EquippedWeapon?.TimeBetweenShotsAfterRarity ?? PoisonGrenadeConfigs.THROW_COOLDOWN);
        public override float MaxDistance => 10f;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.PoisonGrenade.ItemDefinition.Icon);

        public static float GRENADE_DAMAGE = 25.0f;
        public static float GRENADE_AOE_RADIUS = 5.0f;
    }

    public partial class PoisonGrenadeProjectile : LobbedProjectile
    {
        public override string ProjectileSkin => "grenade";

        [ClientRpc]
        public override void ClientOnLanded(Vector2 position)
        {
            VFXManager.Instance.TrySpawnVFX(WeaponReferences.Instance.VFX_ShotgunExplosion, position);
            SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/grenade_poison_explode.wav"), new SFX.PlaySoundDesc() { Position = position, Positional = true, RangeMultiplier = 3.0f });
        }
    }

    // TODO: Maybe apply the tier to the poison cloud?
    // Or maybe grenades shouldn't have tiers at all since they're consumable?
    public partial class PoisonGrenadeAOE : Component
    {
        [Serialized] public Circle_Collider Collider;
        [Serialized] public Spine_Animator[] Skeletons;

        public const float POISON_CLOUD_LIFETIME = 10.0f;

        public MyPlayer Owner;

        private float LifetimeSoFar = 0;
        private float OutroAnimTime = 1.0f; // The disappear animation is 1 second long
        private bool HasStartedOutro = false;

        private ulong PoisonCloudSoundId;

        public override void Awake()
        {
            base.Awake();

            Collider.OnCollisionEnter += OnCollisionEnter;
            Collider.OnCollisionExit += OnCollisionExit;

            SetupSkeletons();

            PoisonCloudSoundId = SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/poison_grenade_gas_cloud.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Entity, Loop = true });
        }

        [ClientRpc]
        public void SetOwner(MyPlayer owner)
        {
            Owner = owner;
        }

        public void SetupSkeletons()
        {
            foreach (var skeleton in Skeletons)
            {
                skeleton.Awaken();
                var sm = StateMachine.Make();
                skeleton.SpineInstance.SetStateMachine(sm, Entity);

                var baseLayer = sm.CreateLayer("main");

                var appearState = baseLayer.CreateState("Appear", 0, false);
                var loopState = baseLayer.CreateState("Idle", 0, true);
                var disappearState = baseLayer.CreateState("Disappear", 0, false);

                baseLayer.CreateTransition(appearState, loopState, true);
                baseLayer.CreateTransition(loopState, disappearState, false).CreateTriggerCondition(sm.CreateVariable("disappear", StateMachineVariableKind.TRIGGER));

                baseLayer.InitialState = appearState;

                // Make it semi-transparent
                skeleton.SpineInstance.ColorMultiplier = new Vector4(1.0f, 1.0f, 1.0f, 0.5f);
            }
        }

        public void OnCollisionEnter(Entity other)
        {
            if (other.TryGetComponent<ThingWithHealth>(out var mob))
            {
                if (!mob.Alive()) return;

            }
        }

        public void OnCollisionExit(Entity other)
        {
        }

        public override void Update()
        {
            base.Update();

            LifetimeSoFar += Time.DeltaTime;

            if (Util.OneTime((POISON_CLOUD_LIFETIME - LifetimeSoFar) <= OutroAnimTime, ref HasStartedOutro))
            {
                foreach (var skeleton in Skeletons)
                {
                    skeleton.SpineInstance.StateMachine.SetTrigger("disappear");
                }
            }

            if (Network.IsServer)
            {
                if (LifetimeSoFar >= POISON_CLOUD_LIFETIME)
                {
                    Network.Despawn(Entity);
                    Entity.Destroy();
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (PoisonCloudSoundId > 0)
            {
                SFX.Stop(PoisonCloudSoundId);
            }
        }
    }
}