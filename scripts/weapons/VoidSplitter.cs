using AO;

namespace ReusableWeapons
{
    public class VoidSplitterConfigs
    {
        public static float TIME_BETWEEN_SHOTS = 0.275f;

        public static long AMMO_WITH_FIRST_PICKUP = 60;
        public static long AMMO_WITH_EXTRA_PICKUP = 30;

        public static float MOB_DAMAGE_PER_TICK = 10f;

        public static float COLLISION_CAST_RADIUS = 0.4f;
        public static int COLLISION_CAST_MAX_HITS = 15;
    }

    public class VoidSplitter : Weapon
    {
        public VoidSplitter(Item_Definition itemDef, ItemRarity rarity, ItemCategory category) : base(itemDef, rarity, category) { }

        public override WeaponType WeaponType => WeaponType.VoidSplitter;
        public override string WeaponSkin => "weapons/void_spitter";
        public override string ProjectilePrefab => null; // No projectile for this weapon, just the beam in the anim
        public override CustomItemDefinition AmmoType => GameManager.Instance.GameItems.LightAmmo;
        public override long AmmoWithFirstPickup => VoidSplitterConfigs.AMMO_WITH_FIRST_PICKUP;
        public override long AmmoWithExtraPickup => VoidSplitterConfigs.AMMO_WITH_EXTRA_PICKUP;
        public override float BaseTimeBetweenShots => VoidSplitterConfigs.TIME_BETWEEN_SHOTS;
        public override string FireAnimation => null;
        public override float CamShakeIntensity => 0.08f;
        public override string UseIkBool => USE_1_HAND_IK;
        public override int DefaultAmmoConsumedPerShot => 2;

        public const string VOID_SPLITTER_BEAM_START_BONE = "beam_base";
        public const string VOID_SPLITTER_BEAM_END_BONE = "AIM";

        public override Ability GetPrimaryAbility(MyPlayer player)
        {
            return (player.IsPlayingOnMobile)
                ? player.GetAbility<VoidSplitterMobileAbility>()
                : player.GetAbility<VoidSplitterPCAbility>();
        }

        // No projectile for this weapon, just the cone of frost in the anim
        public override BaseProjectile SpawnProjectile(MyPlayer player, string projectileID = null, Vector2? spawnPosition = null, Vector2? shootDirection = null)
        {
            return null;
        }

        public override void OnEquip(MyPlayer player)
        {
            base.OnEquip(player);

            if (!player.IsPlayingOnMobile)
            {
                player.RemoveEffect<VoidSplitterAimingEffect>(false);
                player.AddEffect<VoidSplitterAimingEffect>();
            }
        }

        public override void OnUnequip(MyPlayer player)
        {
            base.OnUnequip(player);

            player.RemoveEffect<VoidSplitterAimingEffect>(false);
            player.RemoveEffect<VoidSplitterShootingEffect>(false);
        }

        public override void TryHandleMouseDown(MyPlayer inputPlayer)
        {
            base.TryHandleMouseDown(inputPlayer);

            if (CheckHasEnoughAmmo(inputPlayer))
            {
                inputPlayer.AddEffect<VoidSplitterShootingEffect>();
            }
        }

        public override void TryHandleMouseUp(MyPlayer inputPlayer)
        {
            base.TryHandleMouseUp(inputPlayer);

            inputPlayer.RemoveEffect<VoidSplitterShootingEffect>(false);
        }
    }

    public class VoidSplitterMobileAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.Line;
        public override bool DrawAimingIndicators => true;
        public override Type TargettingEffect => typeof(VoidSplitterShootingEffect);
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.VoidSplitter.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
        public override Type Effect => typeof(WeaponContinuousShootingStopEffect);
    }

    public class VoidSplitterPCAbility : BaseWeaponAbility
    {
        public override TargettingMode TargettingMode => TargettingMode.Self;
        public override bool DrawAimingIndicators => false;
        public override Texture Icon => Assets.GetAsset<Texture>(GameManager.Instance.GameItems.VoidSplitter.ItemDefinition.Icon);
        public override float MaxDistance => 10f;
    }

    public class VoidSplitterAimingEffect : BasicWeaponAimingEffect
    {
        public override void DrawAimingIndicator()
        {
            // There isn't a need for the aiming indicator when the player is shooting since the beam is already visible
            if (!Player.HasEffect<VoidSplitterShootingEffect>())
            {
                base.DrawAimingIndicator();
            }
        }
    }

    public class VoidSplitterShootingEffect : WeaponContinuousShootingEffect
    {
        public override string StartShootingTrigger => "void_splitter_start";
        public override string StopShootingTrigger => "void_splitter_end";

        private Entity BeamEntity;
        private VoidSplitterBeam Beam;

        public ulong SoundId;

        public override void OnEffectStart(bool isDropIn)
        {
            base.OnEffectStart(isDropIn);

            BeamEntity = Entity.Instantiate(WeaponReferences.Instance.Weapon_VoidSplitterBeam);
            BeamEntity.SetParent(Player.Entity, false);
            BeamEntity.LocalPosition = new Vector2(0, 0);

            Beam = BeamEntity.GetComponent<VoidSplitterBeam>();
            UpdateBeam();

            SoundId = SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/void_splitter_shoot_loop.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Player.Entity, Loop = true, Volume = 0.7f, RangeMultiplier = 2.0f });
            SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/void_splitter_shoot_start.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Player.Entity, Volume = 0.7f, RangeMultiplier = 2.0f });
        }

        public override void OnEffectUpdate()
        {
            base.OnEffectUpdate();

            UpdateBeam();
        }

        public void UpdateBeam()
        {
            // On mobile, the aiming is handled differently internally and so we need to update the aiming target here
            if (Player.IsPlayingOnMobile)
            {
                var aimingPosition = EquippedWeapon != null ? EquippedWeapon.GetClickedPosition(Player) : Player.Position;
                Player.SetAimTarget(aimingPosition);
            }

            var beamStartPosition = EquippedWeapon != null ? EquippedWeapon.GetBulletSpawnPosition(Player) : Player.Position;
            var beamEndPosition = EquippedWeapon != null ? EquippedWeapon.GetClickedPosition(Player) : Player.Position;

            Beam.UpdateBeam(beamStartPosition, beamEndPosition, Player.GetFacingDirection());
        }

        public override void OnShootTickReached()
        {
            base.OnShootTickReached();

            var castStart = EquippedWeapon.GetBulletSpawnPosition(Player);
            var castEnd = EquippedWeapon.GetClickedPosition(Player);
            var castDirection = castEnd - castStart;
            var castDistance = castDirection.Length;

            var hits = new Physics.RaycastHit[VoidSplitterConfigs.COLLISION_CAST_MAX_HITS];
            int numHits = Physics.CircleCast(castStart, VoidSplitterConfigs.COLLISION_CAST_RADIUS, castDirection.Normalized, castDistance, ref hits);
            if (numHits > 0)
            {
                for (int i = 0; i < numHits; i++)
                {
                    var hit = hits[i];
                    if (hit.Collider == null) continue;
                    if (hit.Collider.Entity == Player.Entity) continue;
                    if (!hit.Collider.Alive()) continue;
                    if (!hit.Collider.Entity.Alive()) continue;

                    var mob = hit.Collider.GetComponent<ThingWithHealth>();
                    if (mob == null || !mob.Alive()) continue;

                    mob.Damage((int)VoidSplitterConfigs.MOB_DAMAGE_PER_TICK, Player.Entity);
                }
            }
        }

        public override void OnEffectEnd(bool interrupt)
        {
            base.OnEffectEnd(interrupt);

            BeamEntity.Destroy();

            SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/void_splitter_shoot_end.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Player.Entity, Volume = 0.7f, RangeMultiplier = 2.0f });
            SFX.Stop(SoundId);
        }
    }

    public class VoidSplitterBeam : Component
    {
        [Serialized] public Spine_Animator Skeleton;
        [Serialized] public string LoopingAnimation;

        public override void Awake()
        {
            base.Awake();

            SetupSkeleton();
        }

        public void SetupSkeleton()
        {
            Skeleton.Awaken();
            var sm = StateMachine.Make();
            Skeleton.SpineInstance.SetStateMachine(sm, Entity);

            var baseLayer = sm.CreateLayer("main");
            var idleState = baseLayer.CreateState(LoopingAnimation, 0, true);

            baseLayer.InitialState = idleState;
        }

        public void UpdateBeam(Vector2 startPosition, Vector2 endPosition, bool facingRight)
        {
            Skeleton.SetBonePositionWorld("beam_base", startPosition, facingRight);
            Skeleton.SetBonePositionWorld("AIM", endPosition, facingRight);
        }

        public void CleanUp()
        {
            // TODO: Anything extra needed here?
        }
    }
}