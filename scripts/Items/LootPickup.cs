using AO;

namespace ReusableWeapons
{
    /// <summary>
    /// The class that handles the items on the ground that can be picked up by players
    /// Will configure the visuals based on the item data that is supplied to it
    /// Has some animations where the item can lerp to its spawn point and will also bob up and down on the ground
    /// </summary>
    public partial class LootPickup : Component
    {
        public Action<LootPickup> OnPickedUp;

        [Serialized] public Interactable Interactable;
        [Serialized] public Sprite_Renderer ItemSprite;
        [Serialized] public Sprite_Renderer ShineSprite;
        public Sprite_Renderer InnerShineSprite;

        private float TimeSpawnedAt;
        private Player LockedOwner; // Useful if we want to prevent other players from picking up the item
        public MyPlayer OpeningPlayer; // Player who opened the chest that spawned this item

        public enum LerpType
        {
            Lob,
            Linear,
        }

        public CustomItemDefinition Item;

        public bool CreatedByChest = false; // True means spawned by a chest, false means dropped by a player
        public ItemRarity PickupRarity;
        public int PickupAmount;

        private float LerpTime;
        private float MaxLerpTime;
        private Vector2 LerpStart;
        private Vector2 LerpEnd;
        private LerpType UseLerpType;
        private bool MarkedForDestroy;
        private float DestroyTimer;

        public const float ARC_MAX_HEIGHT = 0.35f;

        [ClientRpc]
        public void Initialize(string itemDefId, int rarity, int amount, bool createdByChest, Player lockedOwner)
        {
            Interactable.CanUseCallback += p => LerpTime >= MaxLerpTime && !MarkedForDestroy && CheckIfPlayerCanPickUp((MyPlayer)p);
            Interactable.OnInteract += OnInteract;

            Item = GameManager.Instance.GameItems.GetCustomItemDefByID(itemDefId);

            PickupRarity = (ItemRarity)rarity;
            PickupAmount = amount;
            CreatedByChest = createdByChest;
            LockedOwner = lockedOwner;

            SetupVisuals();
        }

        public void SetupVisuals()
        {
            ShineSprite.Tint = GameItems.GetColorForRarity(PickupRarity);
            InnerShineSprite = Entity.Create().AddComponent<Sprite_Renderer>();
            InnerShineSprite.Entity.SetParent(ShineSprite.Entity, false);
            InnerShineSprite.Sprite = ShineSprite.Sprite;
            InnerShineSprite.Tint = new Vector4(MathF.Min(ShineSprite.Tint.X * 2, 1f), MathF.Min(ShineSprite.Tint.Y * 2, 1f), MathF.Min(ShineSprite.Tint.Z * 2, 1f), 1f);
            InnerShineSprite.Entity.Scale = ShineSprite.Entity.Scale * 0.5f;

            ItemSprite.Sprite = Assets.GetAsset<Texture>(Item.ItemDefinition.Icon);
            if (Item.ItemCategory == ItemCategory.Ammo)
            {
                ItemSprite.Entity.Scale = new Vector2(0.3f, 0.3f);
            }
            else
            {
                ItemSprite.Entity.Scale = new Vector2(0.65f, 0.65f);
            }

            TimeSpawnedAt = Time.TimeSinceStartup;

            SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/drop_item.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Entity, SpeedPerturb = 0.1f });
        }

        [ClientRpc]
        public void LerpItem(Vector2 fromLocation, Vector2 toLocation, float time, LerpType type)
        {
            LerpTime = 0;
            MaxLerpTime = time;
            LerpStart = fromLocation;
            LerpEnd = toLocation;
            UseLerpType = type;
        }

        [ClientRpc]
        public void MoveToPosition(Vector2 position)
        {
            Entity.Position = position;
        }

        public bool CheckIfPlayerCanPickUp(MyPlayer player)
        {
            return !LockedOwner.Alive()
                || LockedOwner == player
                || (LockedOwner as MyPlayer).HealthManager.Health <= 0.0f;
        }

        public override void Update()
        {
            if (Network.IsServer)
            {
                if (TimeSpawnedAt + 60 < Time.TimeSinceStartup && LerpTime >= MaxLerpTime)
                {
                    MarkedForDestroy = true;
                }

                if (MarkedForDestroy)
                {
                    DestroyTimer -= Time.DeltaTime;

                    if (DestroyTimer <= 0)
                    {
                        Network.Despawn(Entity);
                        Entity.Destroy();
                    }
                }
            }

            if (Item != null)
            {
                Interactable.Text = $"Grab {Item.ItemDefinition.Name}";
                Interactable.PromptOffset = new Vector2(-Interactable.Text.Length / 2f * 0.1f, 1.5f);
            }

            var shineOffset = MathF.Sin(Time.TimeSinceStartup * 2f) * 0.15f + 0.15f;
            var spriteOffset = MathF.Sin(Time.TimeSinceStartup * 2f + 0.6f) * 0.15f + 0.15f;

            ShineSprite.Entity.Rotation = Time.TimeSinceStartup * 60;
            ShineSprite.Entity.LocalY = shineOffset;
            ItemSprite.Entity.LocalY = spriteOffset;
            ItemSprite.Entity.Rotation = MathF.Sin(Time.TimeSinceStartup * 2f) * 7 - 2.5f;

            ItemSprite.DepthOffset = -spriteOffset - 0.4f;
            ShineSprite.DepthOffset = -shineOffset + 0.1f;
            InnerShineSprite.DepthOffset = ShineSprite.DepthOffset - 0.2f;

            if (LerpTime < MaxLerpTime)
            {
                var ratio = LerpTime / MaxLerpTime;

                switch (UseLerpType)
                {
                    case LerpType.Lob:
                        var range = Vector2.Distance(LerpStart, LerpEnd);
                        var distanceTravelled = range * ratio;
                        if (distanceTravelled <= range)
                        {
                            var height = ParabolaArcHeight(ARC_MAX_HEIGHT * range, range, distanceTravelled);
                            ItemSprite.Entity.LocalY = height;
                            ShineSprite.Entity.LocalY = height;
                            ItemSprite.Entity.Rotation = 180 * ratio;
                        }
                        Entity.Position = Vector2.Lerp(LerpStart, LerpEnd, ratio);
                        break;
                    default:
                        Entity.Position = Vector2.Lerp(LerpStart, LerpEnd, ratio);
                        break;
                }

                LerpTime += Time.DeltaTime;
                if (LerpTime >= MaxLerpTime)
                {
                    Entity.Position = LerpEnd;
                    ItemSprite.Entity.Rotation = 0;
                }
            }
        }

        public float ParabolaArcHeight(float height, float range, float x)
        {
            return -height * MathF.Pow(x / (0.5f * range) - 1, 2) + height;
        }

        public void OnInteract(Player player)
        {
            var myPlayer = (MyPlayer)player;
            if (player.IsLocal)
            {
                // Use the same sound as when the player drops an item
                SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/drop_item.wav"), new SFX.PlaySoundDesc() { SpeedPerturb = 0.1f });
            }

            if (!Network.IsServer) return;

            if (Item.ItemDefinition.Id.StartsWith("__AMMO__"))
            {
                if (Enum.TryParse<AmmoType>(Item.ItemDefinition.Id.Substring(8), out var ammoType))
                {
                    myPlayer.ServerSyncAmmoAmount(ammoType, myPlayer.AmmoAmounts[ammoType].CurrentAmount + 24);
                    myPlayer.CallClient_ThrowMoney(myPlayer);

                    DestroyTimer = 0.075f;

                    MarkedForDestroy = true;
                    LerpItem(Entity.Position, player.Position, 0.075f, LerpType.Linear);

                    OnPickedUp?.Invoke(this);
                }
                else
                {
                    Log.Error($"Failed to parse ammo type from item: {Item.ItemDefinition.Id}");
                    return;
                }
            }
            else
            {
                if (ServerTryGrantItem((MyPlayer)player))
                {
                    DestroyTimer = 0.075f;

                    MarkedForDestroy = true;
                    LerpItem(Entity.Position, player.Position, 0.075f, LerpType.Linear);

                    OnPickedUp?.Invoke(this);
                }
                else
                {
                    GameManager.CallClient_SendTargetedMessage("You can't pick that up! Try clearing some space!", new RPCOptions() { Target = myPlayer });
                }
            }

        }

        public virtual bool ServerTryGrantItem(MyPlayer player)
        {
            if (!Network.IsServer) return false;

            var metadata = new List<(string, string)> { ("rarity", PickupRarity.ToString()) };

            // Add level metadata for weapons based on the player who opened the chest
            if (Item.ItemCategory == ItemCategory.Weapon && OpeningPlayer != null && OpeningPlayer.Alive())
            {
                // Generate a level near the opening player's level (within +/- 2 levels, but at least 1)
                var playerLevel = OpeningPlayer.Level;
                var levelVariance = 2;
                var minLevel = Math.Max(1, playerLevel - levelVariance);
                var maxLevel = playerLevel + levelVariance;
                var weaponLevel = Random.Shared.Next(minLevel, maxLevel + 1);

                metadata.Add(("level", weaponLevel.ToString()));
            }

            return player.ServerTryAddItem(Item.ItemDefinition, PickupAmount, metadata: metadata);
        }
    }
}