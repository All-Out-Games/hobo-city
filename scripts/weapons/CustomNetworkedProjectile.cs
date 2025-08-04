using AO;

namespace ReusableWeapons
{
    public abstract partial class CustomNetworkedProjectile : BaseProjectile
    {
        // VERY IMPORTANT: This needs to get set for the weapon when it is spawned in
        // This is done in the ServerInitialize() method of the weapon so if you override that or if you don't call SpawnCustomProjectile, make sure to cover this
        private SyncVar<Entity> _OwnerEntity = new SyncVar<Entity>(null);

        protected SyncVar<float> Speed = new SyncVar<float>(0);
        protected SyncVar<Vector2> Direction = new SyncVar<Vector2>(Vector2.Zero);
        protected Vector2 CurrentVelocity => Direction.Value.Normalized * Speed.Value;

        [Serialized] public Entity VisualRoot;
        [Serialized] public bool SyncVisualRootRotation = true;

        protected Vector2 LastPosition = Vector2.Zero;

        public static CustomNetworkedProjectile SpawnCustomProjectile(Prefab projectilePrefab, MyPlayer owner, Vector2 start, Vector2 direction, float speed, float damageAfterRarity)
        {
            var instance = Entity.Instantiate(projectilePrefab).GetComponent<CustomNetworkedProjectile>();
            instance.Entity.Position = start;

            Network.Spawn(instance.Entity);
            instance.ServerInitialize(owner, start, direction, speed, damageAfterRarity);

            return instance;
        }

        public override void Awake()
        {
            base.Awake();
            
            _OwnerEntity.OnSync += (old, newVal) =>
            {
                if (newVal != null)
                {
                    Owner = newVal.GetComponent<MyPlayer>();
                }
            };
        }

        public virtual void ServerInitialize(Player owner, Vector2 start, Vector2 direction, float speed, float damageAfterRarity)
        {
            if (!Network.IsServer) return;

            _OwnerEntity.Set(owner.Entity);
            Speed.Set(speed);
            Direction.Set(direction.Normalized);
            DamageAfterRarity = damageAfterRarity;

            CallClient_AllClientsPostInitialize(damageAfterRarity);
        }

        [ClientRpc]
        public virtual void AllClientsPostInitialize(float damageAfterRarity)
        {
            DamageAfterRarity = damageAfterRarity;

            LastPosition = Entity.Position;
        }

        public override void Update()
        {
            // The base update only destroys the projectile after its lifetime, so in this case we want to run it and the custom logic only on the server
            if (!Network.IsServer) return;

            base.Update();

            ServerUpdatePosition();
        }

        public virtual void ServerUpdatePosition()
        {
            if (!Network.IsServer) return;

            Entity.Position += Direction.Value.Normalized * Speed.Value * Time.DeltaTime;
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            TryUpdateVisualRootRotation();
        }

        // Done locally on each client so the rotation looks smooth to each player, even if it isn't 100% synced
        public virtual void TryUpdateVisualRootRotation()
        {
            if (!SyncVisualRootRotation) return;
            if (VisualRoot == null) return;

            Vector2 movement = (VisualRoot.Position - LastPosition).Normalized;

            float rotation = (float)Math.Atan2(movement.Y, movement.X);
            rotation = AOMath.ToDegrees(rotation);
            VisualRoot.Rotation = rotation;

            LastPosition = VisualRoot.Position;
        }

        public override void DestroyProjectile()
        {
            if (Network.IsServer)
            {
                Network.Despawn(Entity);
                base.DestroyProjectile();
            }
        }
    }

    // LobbedProjectile component taken from the Sigma Games project but modified heavily to work with the CustomNetworkedProjectile class
    public abstract partial class LobbedProjectile : CustomNetworkedProjectile
    {
        // High max lifetime since this projectile is going to a specific destination and then exploding
        // Should likely never hit this number, but not infinite in case it doesn't explode for some reason
        public override float MaxLifetime => 120.0f;

        [Serialized] public float ArcMaxHeight = 0.35f;

        public float DistanceTravelled => Timer * Speed;

        public Vector2 StartPosition;
        public float Range;
        public Action<MyPlayer, Vector2> ServerOnLanded; // Only fired on the server
        public bool HasImpacted = false;

        private float Timer = 0;


        // NOTE: Takes a target position, not a direction. The direction is calculated from these values instead
        public static LobbedProjectile Spawn(Prefab projectilePrefab, MyPlayer owner, Vector2 start, Vector2 destination, float speed, float damageAfterRarity, Action<MyPlayer, Vector2> serverOnLanded)
        {
            var direction = (destination - start).Normalized;
            var instance = SpawnCustomProjectile(projectilePrefab, owner, start, direction, speed, damageAfterRarity) as LobbedProjectile;

            instance.ServerOnLanded = serverOnLanded;
            instance.CallClient_LobbedProjectilePostInitialize(start, Vector2.Distance(start, destination));

            return instance;
        }

        [ClientRpc]
        public virtual void LobbedProjectilePostInitialize(Vector2 startPosition, float range)
        {
            StartPosition = startPosition;
            Range = range;
        }

        public override void Update()
        {
            base.Update();

            Timer += Time.DeltaTime;

            if (DistanceTravelled <= Range)
            {
                var height = ParabolaArcHeight(ArcMaxHeight * Range, Range, DistanceTravelled);
                VisualRoot.LocalY = height;
            }

            if (DistanceTravelled >= Range)
            {
                if (HasImpacted) return;
                HasImpacted = true;

                Entity.Rotation = 0;

                if (Network.IsServer)
                {
                    ServerOnLanded?.Invoke(Owner, Entity.Position);
                    CallClient_ClientOnLanded(Entity.Position);

                    DestroyProjectile();
                }
            }
            else if (Network.IsServer)
            {
                ServerUpdatePosition();
            }
        }

        public override void ServerUpdatePosition()
        {
            // Don't call base, since this movement is handled with the arc calculations in mind
            // TODO: May actually be able to just refactor this class more to use the base calculation
            //base.ServerUpdatePosition();

            Entity.Position = StartPosition + Direction.Value * DistanceTravelled;
        }

        public float ParabolaArcHeight(float height, float range, float x)
        {
            return -height * MathF.Pow(x / (0.5f * range) - 1, 2) + height;
        }

        // OnImpact() doesn't apply to this projectile, as it handles things with the OnLanded event instead
        [ClientRpc]
        public virtual void ClientOnLanded(Vector2 position) {}
        public override void OnImpact(Entity other, ProjectileCollisionType collisionType){}
    }
}
