using System.Net.Mime;
using AO;
using ReusableWeapons;
using System.Collections.Generic;

public enum ItemCategory
{
    Material,
    Healing,
    Weapon,
    Ammo,
    Armor,
    Defence,
    Furniture,
    Car,
    None,
    Plane,
    Boat
}

public class GameItems
{
    public static readonly Vector4 COMMON_COLOR = new(1.1f, 1.1f, 1.1f, 1f); // white
    public static readonly Vector4 UNCOMMON_COLOR = new(0.425f, 1, 0.107f, 1f); // Green
    public static readonly Vector4 RARE_COLOR = new(0, 0.978f, 1, 1f); // Blue
    public static readonly Vector4 EPIC_COLOR = new(0.867f, 0, 1f, 1f); // Purple
    public static readonly Vector4 LEGENDARY_COLOR = new(1f, 0.75f, 0f, 1f); // Yellow
    public static readonly Vector4 MYTHIC_COLOR = new(0.8f, 0.044f, 0.05f, 1f); // Red

    // Cache mapping from ItemDefinition Id -> ItemCategory for quick lookup
    static Dictionary<string, ItemCategory> _itemCategoryCache = new();

    public List<CustomItemDefinition> ItemPool;

    // Healing Items
    public CustomItemDefinition Apple;
    public CustomItemDefinition EnergyDrink;
    public CustomItemDefinition Burger;
    public CustomItemDefinition Popcorn;
    public CustomItemDefinition Hotdog;
    public CustomItemDefinition Smoothie;

    // Weapons (see ammo types below)
    public ExplosiveShotgun ExplosiveShotgun;
    public Pistol Pistol;
    public Fists Fists;
    public Blunderbuss Blunderbuss;
    public PoisonGrenade PoisonGrenade;

    public GatlingGun GatlingGun;
    public WaterBalloonRPG WaterBalloonRPG;
    public AssaultRifle AssaultRifle;
    public SubmachineGun SubmachineGun;
    public Boomwheel BoomWheel;
    public VoidSplitter VoidSplitter;
    public AkimboPistols AkimboPistols;


    // Ammo Types
    public BasicItem LightAmmo;
    public BasicItem MediumAmmo;
    public BasicItem HeavyAmmo;
    public BasicItem ShotgunShells;
    public BasicItem Grenades;

    // Armor
    public Item_Definition BulletProofVest;

    public List<BasicItem> FurniturePool;
    public void CreateItemDefinitions()
    {
        // Healing Items
        Apple = new Apple(Item_Definition.Create(new ItemDescription { Icon = "consumables/apple.png", Id = "__HEALING__Apple", Name = "Apple", StackSize = 64 }), ItemRarity.Common, ItemCategory.Healing, 1);
        EnergyDrink = new EnergyDrink(Item_Definition.Create(new ItemDescription { Icon = "consumables/energy_drink.png", Id = "__HEALING__EnergyDrink", Name = "Energy Drink", StackSize = 64 }), ItemRarity.Common, ItemCategory.Healing, 1);
        Burger = new Burger(Item_Definition.Create(new ItemDescription { Icon = "consumables/burger.png", Id = "__HEALING__Burger", Name = "Burger", StackSize = 64 }), ItemRarity.Common, ItemCategory.Healing, 1);
        Popcorn = new Popcorn(Item_Definition.Create(new ItemDescription { Icon = "popcorn.png", Id = "__HEALING__Popcorn", Name = "Popcorn", StackSize = 64 }), ItemRarity.Common, ItemCategory.Healing, 1);
        Hotdog = new Hotdog(Item_Definition.Create(new ItemDescription { Icon = "consumables/hotdog.png", Id = "__HEALING__Hotdog", Name = "Hotdog", StackSize = 64 }), ItemRarity.Common, ItemCategory.Healing, 1);
        Smoothie = new Smoothie(Item_Definition.Create(new ItemDescription { Icon = "consumables/smoothie.png", Id = "__HEALING__Smoothie", Name = "Smoothie", StackSize = 64 }), ItemRarity.Common, ItemCategory.Healing, 1);

        // Weapons
        Fists = new Fists(Item_Definition.Create(new ItemDescription { Icon = "icons/weapons/punch-inactive.png", Id = "__WEAPON__fists", Name = "Fists", StackSize = 1 }), ItemRarity.Common, ItemCategory.Healing);
        Pistol = new Pistol(Item_Definition.Create(new ItemDescription { Icon = "icons/weapons/pistol.png", Id = "__WEAPON__pistol", Name = "Pistol", StackSize = 1 }), ItemRarity.Common, ItemCategory.Weapon);
        AkimboPistols = new AkimboPistols(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/weapon_icons_250x250/akimbo_pistols_alt.png", Id = "__WEAPON__akimbo_pistols", Name = "Akimbo Pistols", StackSize = 1 }), ItemRarity.Rare, ItemCategory.Weapon);
        Blunderbuss = new Blunderbuss(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/blunderbuss.png", Id = "__WEAPON__blunderbuss", Name = "Blunderbuss", StackSize = 1 }), ItemRarity.Rare, ItemCategory.Weapon);
        PoisonGrenade = new PoisonGrenade(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/ammo_icons/grenade.png", Id = "__WEAPON__poison_grenade", Name = "Poison Grenade", StackSize = 10 }), ItemRarity.Epic, ItemCategory.Weapon);
        GatlingGun = new GatlingGun(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/gatlinggun.png", Id = "__WEAPON__gatling_gun", Name = "Gatling Gun", StackSize = 1 }), ItemRarity.Mythic, ItemCategory.Weapon);
        WaterBalloonRPG = new WaterBalloonRPG(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/waterballoonrpg.png", Id = "__WEAPON__water_balloon_rpg", Name = "Water Balloon RPG", StackSize = 1 }), ItemRarity.Mythic, ItemCategory.Weapon);
        AssaultRifle = new AssaultRifle(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/assaultrifle.png", Id = "__WEAPON__assault_rifle", Name = "Assault Rifle", StackSize = 1 }), ItemRarity.Epic, ItemCategory.Weapon);
        SubmachineGun = new SubmachineGun(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/smg.png", Id = "__WEAPON__submachine_gun", Name = "Submachine Gun", StackSize = 1 }), ItemRarity.Rare, ItemCategory.Weapon);
        ExplosiveShotgun = new ExplosiveShotgun(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/weapon_icons_250x250/shotgun.png", Id = "__WEAPON__explosive_shotgun", Name = "Explosive Shotgun", StackSize = 1 }), ItemRarity.Legendary, ItemCategory.Weapon);
        BoomWheel = new Boomwheel(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/weapon_icons_250x250/boomwheel.png", Id = "__WEAPON__boomwheel", Name = "Boomwheel", StackSize = 1 }), ItemRarity.Mythic, ItemCategory.Weapon);
        VoidSplitter = new VoidSplitter(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/weapon_icons/weapon_icons_250x250/void_splitter.png", Id = "__WEAPON__void_splitter", Name = "Void Splitter", StackSize = 1 }), ItemRarity.Mythic, ItemCategory.Weapon);

        // Ammo Types
        LightAmmo = new BasicItem(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/ammo_icons/bullet.png", Id = "__AMMO__LightAmmo", Name = "Light Ammo", StackSize = 100 }), ItemRarity.Common, ItemCategory.Ammo, 24);
        MediumAmmo = new BasicItem(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/ammo_icons/bullet_alt.png", Id = "__AMMO__MediumAmmo", Name = "Medium Ammo", StackSize = 100 }), ItemRarity.Common, ItemCategory.Ammo, 24);
        HeavyAmmo = new BasicItem(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/ammo_icons/cannonball.png", Id = "__AMMO__HeavyAmmo", Name = "Heavy Ammo", StackSize = 100 }), ItemRarity.Common, ItemCategory.Ammo, 24);
        Grenades = new BasicItem(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/ammo_icons/grenade.png", Id = "__AMMO__Grenades", Name = "Grenades", StackSize = 100 }), ItemRarity.Common, ItemCategory.Ammo, 24);
        ShotgunShells = new BasicItem(Item_Definition.Create(new ItemDescription { Icon = "icons/weapons/ammo/shotgun-ammo.png", Id = "__AMMO__ShotgunShells", Name = "Shotgun Shells", StackSize = 100 }), ItemRarity.Common, ItemCategory.Ammo, 24);

        // Armor
        // BulletProofVest = new BulletProofVest(Item_Definition.Create(new ItemDescription { Icon = "sprites/reusable-weapons/armor icons/bullet_proof_vest.png", Id = "__ARMOR__bullet_proof_vest", Name = "Bullet Proof Vest", StackSize = 1 }), ItemRarity.Common, ItemCategory.Armor);

        // Add the furniture items to the item pool
        ItemPool = new List<CustomItemDefinition>()
        {
            Apple, EnergyDrink, Burger, Popcorn, Hotdog, Smoothie,
            Fists, Pistol, Blunderbuss, PoisonGrenade, GatlingGun, WaterBalloonRPG, AssaultRifle, SubmachineGun, ExplosiveShotgun, BoomWheel, VoidSplitter, AkimboPistols,
            LightAmmo, MediumAmmo, HeavyAmmo, ShotgunShells, Grenades,
        };

    }

    /// <summary>
    /// Creates a custom item instance based on the internal one
    /// Should really only be called by the item equipping system in MyPlayer
    /// </summary>
    public static bool TryCreateCustomInstance(Item_Instance itemInstance, out CustomItemInstance newItem)
    {
        var customItemDef = GameManager.Instance.GameItems.ItemPool.FirstOrDefault(x => x.ItemDefinition == itemInstance.Definition);

        if (customItemDef == null)
        {
            newItem = null;
            Log.Warn($"Failed to create custom item instance for {itemInstance.Definition.Id}");
            return false;
        }

        var customItemInstance = new CustomItemInstance(customItemDef, itemInstance);
        newItem = customItemInstance;

        return true;
    }

    public static Vector4 GetItemTierColor(ItemTier tier)
    {
        switch (tier)
        {
            case ItemTier.Common:
                return new Vector4(1, 1, 1, 1); // White
            case ItemTier.Uncommon:
                return new Vector4(0, 1, 0, 1); // Green
            case ItemTier.Rare:
                return new Vector4(0, 0, 1, 1); // Blue
            case ItemTier.Epic:
                return new Vector4(1, 0, 1, 1); // Purple
            case ItemTier.Legendary:
                return new Vector4(1, 1, 0, 1); // Yellow
            default:
                return new Vector4(1, 1, 1, 0); // Transparent white
        }
    }


    /// <summary>
    /// Creates an item pickup of the specified type on the ground at a given spot
    /// This is called by the LootChest when spawning in the loot
    /// It is also called when dropping items out of the inventory
    /// </summary>
    public static Entity SpawnLootInstance(CustomItemDefinition itemDef, Vector2 dropStartingPoint, bool doLobAnimation, bool createdByChest, ItemRarity? rarityOverride = null, int? amountOverride = null, bool requireNavmesh = true, Player lockedOwner = null)
    {
        return Network.InstantiateAndSpawn(WeaponReferences.Instance.Loot_Pickup, entity =>
        {
            var simplePickup = entity.GetComponent<LootPickup>();
            ItemRarity pickupRarity = rarityOverride ?? itemDef.ItemRarity;
            int pickupAmount = amountOverride ?? itemDef.BasePickupAmount;
            simplePickup.CallClient_Initialize(itemDef.ItemDefinition.Id, (int)pickupRarity, pickupAmount, createdByChest, lockedOwner);

            var dropDestination = (doLobAnimation) ? dropStartingPoint + Util.RandomPositionOnUnitCircle(ref GameManager.Instance.GlobalRng) * Random.Shared.NextFloat(1.5f, 3f) : dropStartingPoint;
            if (requireNavmesh)
            {
                LootManager.Instance.RootNavmesh.TryFindClosestPointOnNavmesh(dropDestination, out dropDestination);
            }

            if (doLobAnimation)
            {
                simplePickup.CallClient_LerpItem(dropStartingPoint, dropDestination, 0.35f, LootPickup.LerpType.Lob);
            }
            else
            {
                simplePickup.CallClient_MoveToPosition(dropDestination);
            }
        });
    }

    /// <summary>
    /// Gets a custom item definition by its ID
    /// The ID is the same as the internal item definition ID
    /// IMPORTANT: Items need to be in the ItemPool array to be found by this function
    /// </summary>
    public CustomItemDefinition GetCustomItemDefByID(string itemDefId)
    {
        var item = ItemPool.FirstOrDefault(x => x.ItemDefinition.Id == itemDefId);
        if (item == null)
        {
            Log.Error($"Failed to find item definition for {itemDefId} in GetCustomItemDefByID");
        }

        return item;
    }

    public static Vector4 GetColorForRarity(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Uncommon => UNCOMMON_COLOR,
            ItemRarity.Rare => RARE_COLOR,
            ItemRarity.Epic => EPIC_COLOR,
            ItemRarity.Legendary => LEGENDARY_COLOR,
            ItemRarity.Mythic => MYTHIC_COLOR,
            _ => COMMON_COLOR
        };
    }

    public static ItemCategory GetItemCategory(Item_Definition itemDef)
    {
        if (itemDef?.Id == null) return ItemCategory.None;

        // Check cache first to avoid repeated string operations
        if (_itemCategoryCache.TryGetValue(itemDef.Id, out var cachedCategory))
        {
            return cachedCategory;
        }

        // Fallback to evaluating the category via string prefix checks
        ItemCategory category = itemDef.Id switch
        {
            var id when id.StartsWith("__CAR__") => ItemCategory.Car,
            var id when id.StartsWith("__PLANE__") => ItemCategory.Plane,
            var id when id.StartsWith("__BOAT__") => ItemCategory.Boat,
            var id when id.StartsWith("__WEAPON__") => ItemCategory.Weapon,
            var id when id.StartsWith("__AMMO__") => ItemCategory.Ammo,
            var id when id.StartsWith("__ARMOR__") => ItemCategory.Armor,
            var id when id.StartsWith("__HEALING__") => ItemCategory.Healing,
            var id when id.StartsWith("__FURNITURE__") => ItemCategory.Furniture,
            _ => ItemCategory.None
        };

        // Store in cache for next time
        _itemCategoryCache[itemDef.Id] = category;
        return category;
    }
}

public enum ItemTier
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythic,
}

// Not an actual equippable item, but a way to track ammo for weapons
// This way, it doesn't go into the player's inventory slots
public class AmmoData
{
    public AmmoType Type;
    public Texture Icon;
    public int MaxAmount = -1; // -1 means unlimited
    public int CurrentAmount = 0;
    public MyPlayer Player;
    public string CurrentAmountFormatted;

    public AmmoData(AmmoType type, string icon, int startingCount, MyPlayer player)
    {
        Type = type;
        Icon = Assets.GetAsset<Texture>(icon);
        CurrentAmount = startingCount;
        Player = player;

        RefreshFormattedAmount();
    }

    public void RefreshFormattedAmount()
    {
        CurrentAmountFormatted = $"x{CurrentAmount}";
    }

    public void SetAmount(int amount)
    {
        int previousAmount = CurrentAmount;

        if (MaxAmount == -1)
        {
            // No capped ammo, just stay above 0
            CurrentAmount = Math.Max(0, amount);
        }
        else
        {
            // Capped ammo, clamp to max
            CurrentAmount = Math.Clamp(amount, 0, MaxAmount);
        }

        if (Player.IsLocal && previousAmount > 0 && CurrentAmount <= 0)
        {
            Notifications.Show($"Out of Ammo! Buy more at the Gun Store!");
        }

        RefreshFormattedAmount();
    }


}

public enum AmmoType
{
    HeavyAmmo,
    Grenades,
    LMGRounds,
    MediumAmmo,
    LightAmmo,
    ShotgunShells,
    SolarCores,

    None
}
