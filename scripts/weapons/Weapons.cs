using AO;

namespace ReusableWeapons
{
    public enum WeaponType
    {
        AssaultRifle,
        BeamMachine,
        Blunderbuss,
        Buzzshot,
        ExplosiveShotgun,
        GatlingGun,
        IceLauncher,
        SolarSiphoner,
        WaterBalloonRPG,
        WindBlaster,

        PoisonGrenade, // Special type of weapon that doesn't use ammo

        None,

        // Added for loadouts
        WaterGun,
        GoldenRocketLauncher,
        RayGun,
        FireRay,
        AkimboPistols,

        // Mythics
        Boomwheel,
        SunnySideShotty,
        Glaciator,
        StormsEye,
        PlasmaBurst,
        OctoSiphoner,
        MegaBlade,
        MissileBarrage,
        VoidSplitter,
        HydroCannon,

        SubmachineGun,
    }

    // Base weapon class, defaults are for a two handed weapon since that seems to be the most common
    public abstract class Weapon : CustomItemDefinition
    {
        public Weapon(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

        // Cache for ammo type enum to avoid repeated lookups
        private global::AmmoType? _cachedAmmoTypeEnum;

        protected static readonly string[] BaseFireSounds =
        {
            "sounds/reusable-weapons/assault_rifle_01.wav",
            "sounds/reusable-weapons/assault_rifle_02.wav",
            "sounds/reusable-weapons/assault_rifle_03.wav",
            "sounds/reusable-weapons/assault_rifle_04.wav",
            "sounds/reusable-weapons/assault_rifle_05.wav",
            "sounds/reusable-weapons/assault_rifle_06.wav",
            "sounds/reusable-weapons/assault_rifle_07.wav",
        };

        public const string USE_1_HAND_IK = "use_1h_ik";
        public const string USE_2_HAND_IK = "use_2h_ik";

        public const string SHOOT_1_HAND_TRIGGER = "shoot_1h_weapon";
        public const string SHOOT_2_HAND_TRIGGER = "shoot_2h_weapon";

        // Guns should generally have the aiming overriden for PC so that the shooting can happen instantly
        // Some weapons may not want this, so override this to false in that case
        public override bool OverrideTargettingOnPC => true;

        // Weapon Data
        public abstract WeaponType WeaponType { get; }
        public abstract string ProjectilePrefab { get; }
        public abstract float BaseTimeBetweenShots { get; }
        public float GetTimeBetweenShotsAfterRarity(MyPlayer player) => ApplyRarityToCooldown(BaseTimeBetweenShots, player);
        public virtual string ProjectileID => $"{GetType().Name}_bullet";
        public abstract CustomItemDefinition AmmoType { get; }
        public abstract long AmmoWithFirstPickup { get; } // The ammo the player gets when picking this gun type up for the first time
        public abstract long AmmoWithExtraPickup { get; } // The extra ammo the player gets when picking this gun type up after the first time
        public virtual int DefaultAmmoConsumedPerShot => 1;

        // Animations
        public virtual string EquipAnimation => "pull_out_2h_weapon";
        public virtual string UnequipAnimation => "put_away_2h_weapon";
        public virtual string FireAnimation => SHOOT_2_HAND_TRIGGER;
        public virtual string UseIkBool => USE_2_HAND_IK;
        public virtual Vector2 BulletSpawnOffset => Vector2.Zero;

        // Skins
        public abstract string WeaponSkin { get; }

        // Sounds
        public virtual string FireSound => null; //BaseFireSounds.GetRandom();
        public virtual float FireVolume => 0.7f;
        public virtual float FireVolumePerturb => 0.1f;
        public virtual float FireSpeedPerturb => 0.1f;
        public virtual float FireRangeMultiplier => 2.0f;

        // Screenshake
        public virtual float CamShakeIntensity => 0.06f;
        public virtual float CamShakeDuration => 0.06f;

        public override void OnEquip(MyPlayer player)
        {
            if (WeaponSkin != null)
            {
                player.SpineAnimator.SpineInstance.EnableSkin(WeaponSkin);
                player.SpineAnimator.SpineInstance.RefreshSkins();
            }

            // if (!string.IsNullOrEmpty(EquipAnimation))
            // {
            //     player.SpineAnimator.SpineInstance.StateMachine.SetTrigger(EquipAnimation);
            // }

            if (!string.IsNullOrEmpty(UseIkBool))
            {
                player.SpineAnimator.SpineInstance.StateMachine.SetBool(UseIkBool, true);
            }
        }

        public override void OnUnequip(MyPlayer player)
        {
            // if (!string.IsNullOrEmpty(UnequipAnimation))
            // {
            //     player.SpineAnimator.SpineInstance.StateMachine.SetTrigger(UnequipAnimation);
            // }

            if (!string.IsNullOrEmpty(UseIkBool))
            {
                player.SpineAnimator.SpineInstance.StateMachine.SetBool(UseIkBool, false);
            }

            // TODO: Wait until the unequip anim is done?
            if (WeaponSkin != null)
            {
                player.SpineAnimator.SpineInstance.DisableSkin(WeaponSkin);
                player.SpineAnimator.SpineInstance.RefreshSkins();
            }
        }

        public virtual void Shoot(MyPlayer player, int? ammoToConsume = null)
        {
            if (!string.IsNullOrEmpty(FireAnimation))
            {
                player.SpineAnimator.SpineInstance.StateMachine.SetTrigger(FireAnimation);
            }

            if (!string.IsNullOrEmpty(FireSound))
            {
                SFX.Play(Assets.GetAsset<AudioAsset>(FireSound), new SFX.PlaySoundDesc() { Positional = true, Position = player.Entity.Position, Volume = FireVolume, VolumePerturb = FireVolumePerturb, SpeedPerturb = FireSpeedPerturb, RangeMultiplier = FireRangeMultiplier });
            }

            if (player.IsLocal && player.CameraControl != null)
            {
                player.CameraControl.Shake(CamShakeIntensity, CamShakeDuration);
            }

            ConsumeRPAmmo(player, ammoToConsume);
        }

        public virtual void ConsumeAmmo(MyPlayer player, int? amountToConsume = null)
        {
            var ammoToConsume = amountToConsume.HasValue ? amountToConsume.Value : DefaultAmmoConsumedPerShot;
            player.ServerConsumeItemWithCount(AmmoType.ItemDefinition, ammoToConsume);
        }

        public virtual void ConsumeRPAmmo(MyPlayer player, int? amountToConsume = null)
        {
            var ammoToConsume = amountToConsume.HasValue ? amountToConsume.Value : DefaultAmmoConsumedPerShot;
            var ammoTypeEnum = GetAmmoTypeEnum();
            if (ammoTypeEnum != global::AmmoType.None)
            {
                int newAmount = player.AmmoAmounts[ammoTypeEnum].CurrentAmount - ammoToConsume;
                player.ServerSyncAmmoAmount(ammoTypeEnum, newAmount);
            }
        }

        public virtual global::AmmoType GetAmmoTypeEnum()
        {
            // Use cached value if already computed
            if (_cachedAmmoTypeEnum.HasValue)
            {
                return _cachedAmmoTypeEnum.Value;
            }

            // Compute and cache the ammo type enum
            global::AmmoType result;
            if (AmmoType?.ItemDefinition?.Id == null)
            {
                result = global::AmmoType.None;
            }
            else
            {
                result = AmmoType.ItemDefinition.Id switch
                {
                    "__AMMO__LightAmmo" => global::AmmoType.LightAmmo,
                    "__AMMO__MediumAmmo" => global::AmmoType.MediumAmmo,
                    "__AMMO__HeavyAmmo" => global::AmmoType.HeavyAmmo,
                    "__AMMO__ShotgunShells" => global::AmmoType.ShotgunShells,
                    "__AMMO__Grenades" => global::AmmoType.Grenades,
                    "__AMMO__SolarCores" => global::AmmoType.SolarCores,
                    _ => global::AmmoType.None
                };
            }

            _cachedAmmoTypeEnum = result;
            return result;
        }

        public virtual long GetAmmoAmount(MyPlayer player)
        {
            return player.GetItemCount(AmmoType.ItemDefinition);
        }

        public virtual long GetRPAmmoAmount(MyPlayer player)
        {
            var ammoTypeEnum = GetAmmoTypeEnum();
            if (ammoTypeEnum != global::AmmoType.None)
            {
                return player.AmmoAmounts[ammoTypeEnum].CurrentAmount;
            }
            return 0;
        }

        public virtual bool CheckHasEnoughAmmo(MyPlayer player, int? amountToCheck = null)
        {
            var ammoToCheck = amountToCheck.HasValue ? amountToCheck.Value : DefaultAmmoConsumedPerShot;

            // A null ammo type means it has infinite ammo
            return (AmmoType == null) ? true : GetRPAmmoAmount(player) >= ammoToCheck;
        }

        // This can potentially return null for some guns with custom projectiles
        public virtual BaseProjectile SpawnProjectile(MyPlayer player, string projectileID = null, Vector2? spawnPosition = null, Vector2? shootDirection = null)
        {
            if (ProjectilePrefab.IsNullOrEmpty())
            {
                Log.Warn($"No projectile prefab set for weapon {GetType().Name} yet we're calling SpawnProjectile()");
                return null;
            }

            var spawnID = projectileID == null ? ProjectileID : projectileID;
            var spawnPos = spawnPosition.HasValue ? spawnPosition.Value : GetBulletSpawnPosition(player);
            var shootDir = shootDirection.HasValue ? shootDirection.Value : GetBulletShootDirection(player);

            var entity = Game.SpawnProjectile(player.Entity, ProjectilePrefab, spawnID, spawnPos, shootDir);
            var projectile = entity.GetComponent<BaseProjectile>();

            // Apply rarity scaling first
            float damageAfterRarity = ApplyRarityToDamage(projectile.BaseDamage, player);

            // Apply weapon level scaling if the weapon has level metadata
            if (player.CurrentEquippedItem != null)
            {
                var levelMetadata = player.CurrentEquippedItem.Instance.GetMetadata("level");
                if (!string.IsNullOrEmpty(levelMetadata) && int.TryParse(levelMetadata, out int weaponLevel))
                {
                    // Each level increases damage by 2%
                    float levelMultiplier = 1f + (weaponLevel - 1) * 0.02f;
                    damageAfterRarity *= levelMultiplier;
                }
            }

            projectile.DamageAfterRarity = damageAfterRarity;
            projectile.Owner = player;

            return projectile;
        }

        public virtual Vector2 GetClickedPosition(MyPlayer player)
        {
            return player.Position + (player.CurrentTargettingDirection * player.CurrentTargettingMagnitude);
        }

        public virtual Vector2 GetBulletSpawnPosition(MyPlayer player)
        {
            var bonePositionWithOffset = player.SpineAnimator.SpineInstance.GetBonePosition("PROJECTILE") + BulletSpawnOffset;
            return player.Entity.CalculateWorldPosition(bonePositionWithOffset);
        }

        public virtual Vector2 GetBulletShootDirection(MyPlayer player)
        {
            // Previously used to shoot towards the mouse
            // Now, always follows the same line as the targetting line
            //return (GetClickedPosition(player) - GetBulletSpawnPosition(player)).Normalized;
            return player.CurrentTargettingDirection;
        }

        public virtual float ApplyRarityToDamage(float baseDamage, MyPlayer player)
        {
            // Get the actual rarity from the item instance metadata
            ItemRarity actualRarity = ItemRarity;  // Default to base rarity

            if (player.CurrentEquippedItem != null)
            {
                var rarityMetadata = player.CurrentEquippedItem.Instance.GetMetadata("rarity");
                if (!string.IsNullOrEmpty(rarityMetadata) && Enum.TryParse<ItemRarity>(rarityMetadata, out var parsedRarity))
                {
                    actualRarity = parsedRarity;
                }
            }

            switch (actualRarity)
            {
                case ItemRarity.Common:
                    return baseDamage;
                case ItemRarity.Uncommon:
                    return baseDamage * 1.1f;
                case ItemRarity.Rare:
                    return baseDamage * 1.2f;
                case ItemRarity.Epic:
                    return baseDamage * 1.3f;
                case ItemRarity.Legendary:
                    return baseDamage * 1.6f;
                case ItemRarity.Mythic:
                    return baseDamage * 1.5f;
                default:
                    return baseDamage;
            }
        }

        public virtual float ApplyRarityToCooldown(float baseCooldown, MyPlayer player)
        {
            if (!player.Alive())
            {
                return baseCooldown;
            }

            // Get the actual rarity from the item instance metadata
            ItemRarity actualRarity = ItemRarity;  // Default to base rarity

            if (player != null && player.CurrentEquippedItem != null)
            {
                var rarityMetadata = player.CurrentEquippedItem.Instance.GetMetadata("rarity");
                if (!string.IsNullOrEmpty(rarityMetadata) && Enum.TryParse<ItemRarity>(rarityMetadata, out var parsedRarity))
                {
                    actualRarity = parsedRarity;
                }
            }

            switch (actualRarity)
            {
                case ItemRarity.Common:
                    return baseCooldown;
                case ItemRarity.Uncommon:
                    return baseCooldown;
                case ItemRarity.Rare:
                    return baseCooldown * 0.9f;
                case ItemRarity.Epic:
                    return baseCooldown * 0.8f;
                case ItemRarity.Legendary:
                    return baseCooldown * 0.7f;
                case ItemRarity.Mythic:
                    return baseCooldown * 0.6f;
                default:
                    return baseCooldown;
            }
        }
    }

    [Flags]
    public enum ProjectileCollisionType
    {
        None = 0,
        Environment = 1 << 1,
        Mob = 1 << 2,
        WoodItem = 1 << 3,
    }

    public abstract class BaseProjectile : Component
    {
        public const string TravelSpin = "013RED/Travel_Loop_Spinning";
        public const string TravelNormal = "013RED/Travel_Loop";
        public const string HitSpin = "013RED/Hit_Despawn_Spinning";
        public const string HitNormal = "013RED/Hit_Despawn";

        [Serialized] public Spine_Animator Skeleton;
        [Serialized] public Circle_Collider Collider;

        public virtual float BaseDamage => 10f;
        public virtual float MaxLifetime => 0.75f;

        public abstract string ProjectileSkin { get; }
        public virtual string TravelAnimation => TravelNormal;
        public virtual string HitAnimation => HitNormal;
        public virtual string HitRPPlayerSound => "sounds/reusable-weapons/bullet_hit.wav";
        public virtual string HitEnvironmentSound => "sounds/bullet_hit_metal.wav";
        public virtual string HitWoodItemSound => "sounds/bullet_hit_wood.wav";
        public virtual Prefab HitVFX => WeaponReferences.Instance.VFX_ProjectileSmallBurst;
        public virtual ProjectileCollisionType OnlyHitTypeOnce => ProjectileCollisionType.Mob;

        public ProjectileCollisionType LastHitType = ProjectileCollisionType.None;
        public float LifetimeSoFar = 0.0f;
        public MyPlayer Owner;

        public float? DamageAfterRarity;

        public override void Awake()
        {
            if (Skeleton != null)
            {
                SetupSkeleton();
            }

            if (Collider != null && Collider.Alive())
            {
                Collider.OnCollisionEnter += OnCollisionEnter;
            }
        }

        public virtual void SetupSkeleton()
        {
            // Setup the skin
            Skeleton.Awaken();

            if (ProjectileSkin != null)
            {
                Skeleton.SpineInstance.EnableSkin(ProjectileSkin);
                Skeleton.SpineInstance.RefreshSkins();
            }

            // Setup the animations
            var sm = StateMachine.Make();
            Skeleton.SpineInstance.SetStateMachine(sm, Entity);
            SetupStateMachine(sm);
        }

        public virtual void SetupStateMachine(StateMachine sm)
        {
            var baseLayer = sm.CreateLayer("main");

            var loopingState = baseLayer.CreateState(TravelAnimation, 0, true);
            baseLayer.InitialState = loopingState;

            var hitState = baseLayer.CreateState(HitAnimation, 0, false);
            baseLayer.CreateTransition(loopingState, hitState, false).CreateTriggerCondition(sm.CreateVariable("hit", StateMachineVariableKind.TRIGGER));
        }

        public override void Update()
        {
            LifetimeSoFar += Time.DeltaTime;
            if (LifetimeSoFar > MaxLifetime)
            {
                DestroyProjectile();
            }
        }

        public virtual void OnCollisionEnter(Entity other)
        {
            if (other.Alive() == false) return;
            if (OnlyHitTypeOnce != ProjectileCollisionType.None && LastHitType == OnlyHitTypeOnce) return;
            if (!Owner.Alive())
            {
                // Log.Warn($"Projectile {GetType().Name} has no owner");
                return;
            }

            if (other == Owner.Entity) return;
            if (other.GetComponent<ThingWithHealth>() == null) return;

            ProjectileCollisionType collisionType = ProjectileCollisionType.Environment;

            if (other.GetComponent<Player>().Alive())
            {
                if (other.GetComponent<MyPlayer>().HealthManager.Health > 0)
                {
                    collisionType = ProjectileCollisionType.Mob;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (other.Name.ToLower().Contains("tree") || other.Name.ToLower().Contains("bush"))
                {
                    collisionType = ProjectileCollisionType.WoodItem;
                }
            }

            OnImpact(other, collisionType);
        }

        public virtual bool TryHitMob(ThingWithHealth mob)
        {
            if (!mob.Alive()) return false;

            float damageToApply = DamageAfterRarity.HasValue ? DamageAfterRarity.Value : BaseDamage;

            var multiplier = 1f;
            if (mob.GetComponent<Destructable>() != null)
            {
                multiplier = 6f;
            }

            mob.Damage((int)(damageToApply * multiplier), Owner.Entity);
            return true;
        }

        public virtual void OnImpact(Entity other, ProjectileCollisionType collisionType)
        {
            // Alive has already been checked in OnCollisionEnter
            LastHitType = collisionType;

            TryHitMob(other.GetComponent<ThingWithHealth>());

            if (HitVFX != null)
            {
                if (collisionType.HasFlag(ProjectileCollisionType.Mob))
                {
                    SFX.Play(Assets.GetAsset<AudioAsset>(HitRPPlayerSound), new SFX.PlaySoundDesc() { Position = Entity.Position, Positional = true, RangeMultiplier = 2.0f });
                }
                else if (collisionType.HasFlag(ProjectileCollisionType.WoodItem))
                {
                    SFX.Play(Assets.GetAsset<AudioAsset>(HitWoodItemSound), new SFX.PlaySoundDesc() { Position = Entity.Position, Positional = true, RangeMultiplier = 2.0f });
                }
                else
                {
                    SFX.Play(Assets.GetAsset<AudioAsset>(HitEnvironmentSound), new SFX.PlaySoundDesc() { Volume = 0.6f, Position = Entity.Position, Positional = true, RangeMultiplier = 2.0f });
                }

                VFXManager.Instance.TrySpawnVFX(HitVFX, Entity.Position);
            }
            else
            {
                Log.Info($"No VFX to play for hit type {collisionType}");
            }

            DestroyProjectile();
        }

        public virtual void DestroyProjectile()
        {
            Entity.Destroy();
        }
    }

    public abstract class BaseWeaponAbility : MyAbility
    {
        public Weapon EquippedWeapon;

        protected float GetWeaponCooldownWithRarity()
        {
            if (EquippedWeapon != null)
            {
                return EquippedWeapon.ApplyRarityToCooldown(EquippedWeapon.BaseTimeBetweenShots, Player);
            }
            return 1.0f; // Default fallback
        }

        public override bool OnTryActivate(List<Player> targetPlayers, Vector2 direction, float magnitude)
        {
            if (!Player.Alive())
            {
                Log.Warn($"Player {Player.Name} is not alive");
                return false;
            }

            var myPlayer = (MyPlayer)Player;
            if (!myPlayer.HealthManager.Alive() || myPlayer.HealthManager.Health <= 0)
            {
                return false;
            }

            if (myPlayer.Alive() && myPlayer.HasEffect<InvulnerabilityEffect>())
            {
                myPlayer.RemoveEffect<InvulnerabilityEffect>(true);
            }

            if (Player.CurrentEquippedItem == null)
            {
                Log.Warn($"Player {Player.Name} has no equipped item");
                return false;
            }

            EquippedWeapon = Player.CurrentEquippedItem.CustomDefinition as Weapon;

            // Correct cooldown gating: block if we haven't waited long enough
            if ((Time.TimeSinceStartup - Player.LastShootTime) < Cooldown)
            {
                return false;
            }

            Player.LastShootTime = Time.TimeSinceStartup;

            return base.OnTryActivate(targetPlayers, direction, magnitude);
        }

        public override bool CanUse()
        {
            var currentWeapon = Player.CurrentEquippedItem.CustomDefinition as Weapon;

            return base.CanUse()
                && currentWeapon != null
                && currentWeapon.CheckHasEnoughAmmo(Player);
        }
    }

    public abstract class BaseSingleShotWeaponAbility : BaseWeaponAbility
    {
        public override bool OnTryActivate(List<Player> targetPlayers, Vector2 direction, float magnitude)
        {
            base.OnTryActivate(targetPlayers, direction, magnitude);

            if (EquippedWeapon != null)
            {
                EquippedWeapon.SpawnProjectile(Player);
                EquippedWeapon.Shoot(Player);
            }

            return true;
        }
    }

    public class BasicWeaponAimingEffect : MyEffect
    {
        public override bool IsActiveEffect => false;

        public override void OnEffectStart(bool isDropIn)
        {
            base.OnEffectStart(isDropIn);

            Player.SetMouseIKEnabled(true);
        }

        public override void OnEffectLateUpdate()
        {
            base.OnEffectLateUpdate();

            if (Player.IsPlayingOnMobile)
            {
                Player.SetAimTarget(Player.Position + (Player.CurrentTargettingDirection * Player.CurrentTargettingMagnitude));
            }

            if (Player.IsLocal)
            {
                DrawAimingIndicator();
            }
        }

        public virtual void DrawAimingIndicator()
        {
            AO.Player.DrawLineAimingIndicator(Player.CurrentTargettingDirection, Player.Position);
        }

        public override void OnEffectEnd(bool interrupt)
        {
            base.OnEffectEnd(interrupt);

            Player.SetMouseIKEnabled(false);
        }
    }

    public class CircleAOEWeaponAimingEffect : BasicWeaponAimingEffect
    {
        public override void DrawAimingIndicator()
        {
            AO.Player.DrawCircleAimingIndicator(Player.Position + (Player.CurrentTargettingDirection * Player.CurrentTargettingMagnitude));
        }
    }

    public class WeaponContinuousShootingEffect : MyEffect
    {
        public override bool IsActiveEffect => true;

        public float TimeUntilNextShot = 0.0f;
        public long FramesBetweenShots => System.Math.Max(1, (long)(CalculateTimeBetweenShots(EquippedWeapon?.ApplyRarityToCooldown(EquippedWeapon?.BaseTimeBetweenShots ?? 0.0f, Player) ?? 0.0f) * 60)); // Using frames to avoid latency desync issues, minimum 1 to prevent divide by zero

        public Weapon EquippedWeapon;

        public virtual string StartShootingTrigger { get; } = "shoot_fast_start";
        public virtual string StopShootingTrigger { get; } = "shoot_fast_end";

        private float CalculateTimeBetweenShots(float baseTime)
        {
            return baseTime;
        }

        public override void OnEffectStart(bool isDropIn)
        {
            base.OnEffectStart(isDropIn);

            if (!Player.Alive())
            {
                Log.Warn($"Player {Player.Name} is not alive");
                return;
            }

            var myPlayer = (MyPlayer)Player;
            if (!myPlayer.HealthManager.Alive() || myPlayer.HealthManager.Health <= 0)
            {
                if (Player.IsLocal)
                {
                    Notifications.Show("You can't shoot while you're invulnerable or dead!");
                }
                return;
            }

            if (myPlayer.Alive() && myPlayer.HasEffect<InvulnerabilityEffect>())
            {
                myPlayer.RemoveEffect<InvulnerabilityEffect>(true);
            }

            if (Player.CurrentEquippedItem == null)
            {
                Log.Warn($"Player {Player.Name} has no equipped item");
                return;
            }

            EquippedWeapon = Player.CurrentEquippedItem.CustomDefinition as Weapon;

            if (Player.IsPlayingOnMobile)
            {
                Player.SetMouseIKEnabled(true);
            }

            if (Player.IsLocal)
            {
                Player.IsShooting = true;
                Player.FrameStartedShooting = Game.FrameNumber;
            }

            if (!string.IsNullOrEmpty(StartShootingTrigger))
            {
                Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger(StartShootingTrigger);
            }
        }

        public override void OnEffectUpdate()
        {
            base.OnEffectUpdate();

            // Remove the aiming if the gun is no longer equipped or if the gun is out of ammo
            if (EquippedWeapon == null || !EquippedWeapon.CheckHasEnoughAmmo(Player))
            {
                if (Network.IsServer)
                {
                    GameManager.CallClient_SendTargetedMessage("Out of ammo!", new RPCOptions() { Target = Player });
                }

                Player.RemoveEffect(GetType(), true);
                return;
            }
        }

        public override void OnEffectLateUpdate()
        {
            base.OnEffectLateUpdate();

            if (Player.IsShooting)
            {
                if (Player.IsPlayingOnMobile)
                {
                    Player.SetAimTarget(Player.Position + (Player.CurrentTargettingDirection * Player.CurrentTargettingMagnitude));
                }

                // Fire only when the global next-allowed frame has been reached (prevents effect restart exploits)
                if (Game.FrameNumber >= Player.NextAllowedShootFrame)
                {
                    OnShootTickReached();
                }
            }
        }

        public virtual void OnShootTickReached()
        {
            if (EquippedWeapon != null)
            {
                EquippedWeapon.Shoot(Player);
                EquippedWeapon.SpawnProjectile(Player);

                // Advance the player's next allowed shooting frame using current effective fire rate
                Player.NextAllowedShootFrame = Game.FrameNumber + FramesBetweenShots;
            }
        }

        public override void OnEffectEnd(bool interrupt)
        {
            base.OnEffectEnd(interrupt);

            if (Player.IsPlayingOnMobile)
            {
                Player.SetMouseIKEnabled(false);
            }

            if (Player.IsLocal)
            {
                Player.IsShooting = false;
            }

            if (!string.IsNullOrEmpty(StopShootingTrigger))
            {
                Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger(StopShootingTrigger);
            }
        }
    }

    public class WeaponContinuousShootingStopEffect : MyEffect
    {
        public override bool IsActiveEffect => true;
        public override float DefaultDuration => 0.1f;
    }
}