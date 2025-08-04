using AO;

namespace ReusableWeapons
{
    /// <summary>
    /// Wraps around the Item_Definition class and adds some extra functionality
    /// Basically, it defines an item and how that item actually behaves in the game flow
    /// Important that most of the reusable-weapons flow is built around this
    /// Generally, if you want to make a new type of item, you will want to inherit from this OR the BasicItem class below instead
    /// All weapons inherit from this
    /// </summary>
    public abstract class CustomItemDefinition
    {
        public CustomItemDefinition(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount)
        {
            ItemDefinition = itemDef;
            ItemRarity = rarity;
            ItemCategory = category;
            BasePickupAmount = basePickupAmount;
        }

        public Item_Definition ItemDefinition;
        public ItemCategory ItemCategory;
        public ItemRarity ItemRarity;
        public int BasePickupAmount;

        //--- Abilities ---//
        /// <summary>
        /// This is tied to the main button for this (ie: shoot for guns)
        /// The GameManagerSystem defines the main button to be left click
        /// This CAN be null, in which case the main ability slot will just be empty (ex: for ammo items)
        /// </summary>
        public abstract Ability GetPrimaryAbility(MyPlayer player);

        /// <summary>
        /// This is any secondary abilities for this (ie: secondary fire for guns, etc)  
        /// None of the base 26 guns have secondary abilities currently but this is here for future use
        /// The player's second ability slot is defaulted to be Melee so these will be ability #3 and onwards
        /// </summary>
        public virtual List<Ability> GetSecondaryAbilities(MyPlayer player) => new();

        //--- PC-Specific Controls ---//
        /// <summary>
        /// If this is enabled, will manually update the player's CurrentTargettingDirection and CurrentTargettingMagnitude so they look directly at the mouse
        /// This is done in the MyPlayer.Update() function
        /// This only works on PC, it does not do the same on mobile since aiming works a bit differently there
        /// For pretty much all weapons, this should be true
        /// </summary>
        public virtual bool OverrideTargettingOnPC => true;

        /// <summary>
        /// Only called on the local client, not automatically synced!
        /// Will handle when the left mouse is clicked while this item is equipped
        /// This is synced in the MyPlayer class through RPCs
        /// </summary>
        public virtual void TryHandleMouseDown(MyPlayer inputPlayer) { }

        /// <summary>
        /// Only called on the local client, not automatically synced!
        /// Will handle when the left mouse is released while this item is equipped
        /// This is synced in the MyPlayer class through RPCs
        /// </summary>  
        public virtual void TryHandleMouseUp(MyPlayer inputPlayer) { }

        //--- Item equipping ---//
        /// <summary>
        /// This is called when the player selects this item in their hotbar
        /// For weapons, this plays the equip sound
        /// For some weapons, like the octo siphoner, it also creates the radius indicator around the player
        /// </summary>
        public abstract void OnEquip(MyPlayer player);

        /// <summary>
        /// This is called when the player unselects this item in their hotbar
        /// For some weapons, like the octo siphoner, it also removes the radius indicator around the player
        /// </summary>
        public abstract void OnUnequip(MyPlayer player);

        /// <summary>
        /// Defines if the player can equip an item or not
        /// If this return false, the abilties will not be able to be used
        /// Usually the IsDead check makes sense, but in cases like the bandages it is useful to use them when dead for self-revive
        /// So, can be custom to each item
        /// </summary>
        public virtual bool IsEquippableByPlayer(MyPlayer player)
        {
            return player.Alive()
            && player.HealthManager.Health > 0;
        }
    }

    /// <summary>
    /// A simple item that doesn't have any abilities or secondary abilities
    /// Can be inherited from for more complex items
    /// Or can just be used for things like ammo that you want to be able to hold in the inventory but that don't actually do anything
    /// </summary>
    public class BasicItem : CustomItemDefinition
    {
        public BasicItem(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount) : base(itemDef, rarity, category, basePickupAmount) { }

        public override Ability GetPrimaryAbility(MyPlayer player) => null;

        public override void OnEquip(MyPlayer player) { }
        public override void OnUnequip(MyPlayer player) { }
    }

    /// <summary>
    /// Wraps around an Item_Instance and a CustomItemDefinition
    /// Works similarly to the base Item_Instance as it represents a specific item instead of a general definition of one
    /// ---> Ex: the Blunderbuss ItemDefinition has a default rarity of Common, but a *specific* Blunderbuss item instance could have a rarity of Legendary
    /// The player's CurrentEquippedItem is of this type
    /// </summary>
    public class CustomItemInstance
    {
        public CustomItemDefinition CustomDefinition;
        public Item_Instance Instance;

        public ItemRarity ItemRarity => OverrideItemRarity ?? CustomDefinition.ItemRarity;
        private ItemRarity? OverrideItemRarity = null;

        public CustomItemInstance(CustomItemDefinition customDefinition, Item_Instance itemInstance)
        {
            Instance = itemInstance;
            CustomDefinition = customDefinition;

            var rarityMetadata = itemInstance.GetMetadata("rarity");
            if (!string.IsNullOrEmpty(rarityMetadata))
            {
                if (Enum.TryParse<ItemRarity>(rarityMetadata, out var rarity))
                {
                    OverrideItemRarity = rarity;
                }
            }
        }
    }
}
