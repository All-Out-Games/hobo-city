using System.Collections;
using System.Data;
using AO;

namespace ReusableWeapons
{
    /// <summary>
    /// The system for the loot chest that is in Red Sun
    /// Has a normal and a 'legendary' variant
    /// The legendary variant has a higher chance of spawning good loot, and also has a unique skin and animations
    /// 
    /// The chest can be interacted with by players to open it
    /// When opened, it will randomly select a loot table based on the chest tier and roll for each item in the set
    /// The loot tables are defined below
    /// 
    /// The chest will despawn after a short duration after being opened
    /// </summary>
    public partial class LootChest : Component
    {
        private const float BASIC_OPEN_DURATION = 0.15f;
        private const float EXCITING_OPEN_DURATION = 1.75f;

        private const float LEGENDARY_CHEST_CHANCE = 0.04f;

        public Vector2 SpawnPosition;

        #region Loot Table Randomizations
        /// <summary>
        /// Defines the information of a single item in the loot table
        /// </summary>
        public struct LootChestDrop
        {
            public CustomItemDefinition ItemDef;
            public int MinQuantity;
            public int MaxQuantity;

            public LootChestDrop(CustomItemDefinition def, int min = 1, int max = 1)
            {
                ItemDef = def;
                MinQuantity = min;
                MaxQuantity = max;
            }
        }

        /// <summary>
        /// The loot table for weapons
        /// Can add as many weapons as you want here, and also add other loot tables like this
        /// The number is the weighting for that item in the table
        /// The weights are relative to each other, so if you want a weapon to drop more often than another, you can set it higher
        /// If all the weights are the same, the chances for each weapon will be identical
        /// </summary>
        private static readonly WeightedList<LootChestDrop> WeaponLootTable = new()
        {
            {new LootChestDrop(GameManager.Instance.GameItems.Blunderbuss), 2},
            {new LootChestDrop(GameManager.Instance.GameItems.Pistol), 6},
            {new LootChestDrop(GameManager.Instance.GameItems.SubmachineGun), 3},
            {new LootChestDrop(GameManager.Instance.GameItems.ExplosiveShotgun), 1},
            {new LootChestDrop(GameManager.Instance.GameItems.AssaultRifle), 3},
        };

        /// <summary>
        /// The loot table for ammo
        /// Works the same way as the weapon loot table
        /// </summary>
        public static readonly WeightedList<LootChestDrop> AmmoLootTable = new()
        {
            {new LootChestDrop(GameManager.Instance.GameItems.LightAmmo), 3},
            {new LootChestDrop(GameManager.Instance.GameItems.MediumAmmo), 4},
            {new LootChestDrop(GameManager.Instance.GameItems.HeavyAmmo), 2},
            {new LootChestDrop(GameManager.Instance.GameItems.ShotgunShells), 2},
        };
        #endregion

        [Serialized] public Interactable Interactable;
        [Serialized] public Spine_Animator Skeleton;

        private SyncVar<int> _chestTier = new(0);
        public ItemRarity ChestTier
        {
            get => (ItemRarity)_chestTier.Value;
            set
            {
                if (Network.IsServer)
                {
                    _chestTier.Set((int)value);
                }
            }
        }

        public override void Awake()
        {
            Interactable.OnInteract += OnInteract;
            Interactable.RequiredHoldTime = 0.5f;

            Interactable.CanUseCallback = (player) =>
            {
                var myPlayer = (MyPlayer)player;
                return myPlayer.HealthManager.Alive() && myPlayer.HealthManager.Health > 0;
            };

            Skeleton.Awaken();
            var sm = StateMachine.Make();
            Skeleton.SpineInstance.SetStateMachine(sm, Entity);

            var baseLayer = sm.CreateLayer("main");

            // Basic chests (under gold tier)
            var basicIdleState = baseLayer.CreateState("idle_loop", 0, true);
            baseLayer.CreateGlobalTransition(basicIdleState).CreateTriggerCondition(sm.CreateVariable("idle_basic", StateMachineVariableKind.TRIGGER));

            var baseOpeningState = baseLayer.CreateState("open", 0, false);
            baseLayer.CreateTransition(basicIdleState, baseOpeningState, false).CreateTriggerCondition(sm.CreateVariable("open_basic", StateMachineVariableKind.TRIGGER));

            // Exciting chests (gold, diamond tier)
            var excitingIdleState = baseLayer.CreateState("fall_loop_sparkle", 0, true);
            baseLayer.CreateGlobalTransition(excitingIdleState).CreateTriggerCondition(sm.CreateVariable("idle_exciting", StateMachineVariableKind.TRIGGER));

            var excitingOpeningState = baseLayer.CreateState("open_long", 0, false);
            baseLayer.CreateTransition(excitingIdleState, excitingOpeningState, false).CreateTriggerCondition(sm.CreateVariable("open_exciting", StateMachineVariableKind.TRIGGER));

            baseLayer.InitialState = basicIdleState;

            _chestTier.OnSync += (_, _) =>
            {
                Skeleton.SpineInstance.SetSkin(ChestTier == ItemRarity.Legendary ? "gold" : "iron");
                Skeleton.SpineInstance.RefreshSkins();

                Skeleton.SpineInstance.StateMachine.SetTrigger(ChestTier == ItemRarity.Legendary ? "idle_exciting" : "idle_basic");
            };
        }

        [ClientRpc]
        public void SpawnChest(Vector2 position)
        {
            Entity.Position = position;
            SpawnPosition = position;
            Interactable.LocalEnabled = true;
        }

        [ClientRpc]
        public void DespawnChest()
        {
            Entity.Position = Vector2.One * 1000;

            // Register this chest for respawn (server only)
            if (Network.IsServer)
            {
                _LootManager.AddChestToRespawnQueue(SpawnPosition);
            }
        }

        public void ServerRandomizeChestType()
        {
            if (!Network.IsServer) return;

            ChestTier = Random.Shared.NextFloat() <= LEGENDARY_CHEST_CHANCE ? ItemRarity.Legendary : ItemRarity.Common;
        }

        private void OnInteract(Player player)
        {
            var myPlayer = (MyPlayer)player;
            Interactable.LocalEnabled = false;

            if (Network.IsClient)
            {
                Destructable.CashRewardPrefab.Instantiate(onBeforeAwake: (entity) =>
                {
                    var explodeAndLerp = entity.GetComponent<ExplodeAndLerpToPlayer>();
                    explodeAndLerp.Player = myPlayer;
                    explodeAndLerp.Texture = Assets.GetAsset<Texture>("icons/cash.png");
                    explodeAndLerp.Count = 75;
                    entity.SetParent(Entity, false);
                });
            }

            if (Network.IsServer && myPlayer.Alive())
            {
                Economy.DepositCurrency(myPlayer, GameManager.CASH_CURRENCY, 75);
            }

            Coroutine.Start(myPlayer.Entity, DoOpenSequence());
            IEnumerator DoOpenSequence()
            {
                Skeleton.SpineInstance.StateMachine.SetTrigger(ChestTier == ItemRarity.Legendary ? "open_exciting" : "open_basic");

                SFX.Play(Assets.GetAsset<AudioAsset>(ChestTier == ItemRarity.Legendary ? "sounds/reusable-weapons/open_chest_long.wav" : "sounds/reusable-weapons/open_chest_default.wav"), new SFX.PlaySoundDesc() { Position = Entity.Position, Positional = true });

                var openTime = ChestTier == ItemRarity.Legendary ? EXCITING_OPEN_DURATION : BASIC_OPEN_DURATION;

                yield return new WaitForSeconds(openTime);

                if (Network.IsServer)
                {
                    ServerDetermineLoot(myPlayer);
                }

                yield return new WaitForSeconds(1f);

                if (Network.IsServer)
                {
                    CallClient_DespawnChest();
                }
            }
        }

        /// <summary>
        /// Actually defines the loot that will be dropped by this chest
        /// For Red Sun, it was setup so that the chests would drop at least 1 weapon and 1 ammo
        /// The weights of the loot tables can be modified to affect the chances of dropping things
        /// ---> Ex: Notice that the first dropped item is always a weapon, and then it is significantly less likely to drop a second weapon after that, but it is possible
        /// </summary>
        private void ServerDetermineLoot(MyPlayer player)
        {
            if (!Network.IsServer) return;

            // How many sets of loot to pull from the table
            // Note that this set may contain more than 1 item instance, if the quantity for the set is greater than 1
            // This means that this number does not always equal the number of actual grabbable items from the ground
            int numSetsOfLoot = 2;
            int weaponsSpawned = 0;

            for (int lootSetIndex = 0; lootSetIndex < numSetsOfLoot; lootSetIndex++)
            {
                var tableWeighting = new WeightedList<WeightedList<LootChestDrop>>()
                {
                    {WeaponLootTable, 100},
                    {AmmoLootTable, 50},
                };

                switch (weaponsSpawned)
                {
                    case 0:
                        tableWeighting.SetWeightOrRemove(WeaponLootTable, 100);
                        tableWeighting.SetWeightOrRemove(AmmoLootTable, 0);
                        break;
                    case 1:
                        tableWeighting.SetWeightOrRemove(WeaponLootTable, 5);
                        tableWeighting.SetWeightOrRemove(AmmoLootTable, 44);
                        break;
                    default:
                        tableWeighting.SetWeightOrRemove(WeaponLootTable, 0);
                        tableWeighting.SetWeightOrRemove(AmmoLootTable, 47);
                        break;
                }

                var table = tableWeighting.Next();

                var clonedTable = new WeightedList<LootChestDrop>();
                clonedTable.Add(table);

                if (table == WeaponLootTable)
                {
                    weaponsSpawned++;
                }

                LootChestDrop lootDrop = clonedTable.Next();
                ItemRarity? forceRarity = null;

                for (int itemInstanceIndex = 0; itemInstanceIndex < Random.Shared.Next(lootDrop.MinQuantity, lootDrop.MaxQuantity); itemInstanceIndex++)
                {
                    SpawnLootInstance(lootDrop.ItemDef, forceRarity, player);
                }
            }
        }

        private void SpawnLootInstance(CustomItemDefinition itemDef, ItemRarity? forcedItemRarity = null, MyPlayer openingPlayer = null)
        {
            ItemRarity? rarity = null;
            var itemCategory = itemDef.ItemCategory;
            if (itemCategory == ItemCategory.Weapon)
            {
                rarity = forcedItemRarity ?? GetScaledRarities().Next();
            }

            // Pass the opening player so we can set level metadata
            var lootEntity = GameItems.SpawnLootInstance(itemDef, Entity.Position, true, true, rarity);

            // If it's a weapon and we have a player, set the level metadata
            if (itemCategory == ItemCategory.Weapon && openingPlayer != null && lootEntity != null && lootEntity.Alive())
            {
                var lootPickup = lootEntity.GetComponent<LootPickup>();
                if (lootPickup != null)
                {
                    lootPickup.OpeningPlayer = openingPlayer;
                }
            }
        }

        /// <summary>
        /// The likelihood of the weapons having a specific rarity
        /// Can modify these weights at runtime based on some value (ex: in Red Sun, it is based on the Day Count) to make higher value weapons more likely to drop later on
        /// </summary>
        private WeightedList<ItemRarity> GetScaledRarities()
        {
            WeightedList<ItemRarity> table;

            if (ChestTier == ItemRarity.Legendary)
            {
                // Legendary chest: No common items, higher chance of rare items
                table = new()
                {
                    {ItemRarity.Epic, 11 },
                    {ItemRarity.Legendary, 7 },
                };
            }
            else
            {
                // Common chest: No legendary items, higher chance of common items
                table = new()
                {
                    {ItemRarity.Common, 10 },
                    {ItemRarity.Uncommon, 7 },
                    {ItemRarity.Rare, 2 },
                    {ItemRarity.Epic, 1 },
                };
            }

            return table;
        }
    }
}