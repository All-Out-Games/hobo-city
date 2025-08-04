using AO;

public partial class Store
{
    public const string StarterPackProductId = "67c6032156b855d48dbbd74e";

    public static List<ShopCategory.ProductDescription> BlackMarketProducts = new()
    {
        new () {
            Id = "__WEAPON__water_balloon_rpg",
            Rarity = ItemRarity.Mythic,
            Price = 200 * 100,
            Icon = "sprites/reusable-weapons/weapon_icons/waterballoonrpg.png",
            Currency = GameManager.BITCOIN_CURRENCY,
            Name = "Water Balloon RPG",
            Description = "A weapon that shoots water balloons at a rapid rate (**Price is ACTUALLY 200 BTC**)",
        },
        new () {
            Id = "__WEAPON__gatling_gun",
            Rarity = ItemRarity.Mythic,
            Price = 500 * 100,
            Icon = "sprites/reusable-weapons/weapon_icons/gatlinggun.png",
            Currency = GameManager.BITCOIN_CURRENCY,
            Name = "Gatling Gun",
            Description = "A weapon that shoots bullets at a rapid rate (**Price is ACTUALLY 600 BTC**)",
        },
        new () {
            Id = "__WEAPON__void_splitter",
            Rarity = ItemRarity.Mythic,
            Price = 1250 * 100,
            Icon = "sprites/reusable-weapons/weapon_icons/weapon_icons_250x250/void_splitter.png",
            Currency = GameManager.BITCOIN_CURRENCY,
            Name = "Void Splitter",
            Description = "A weapon that splits the void, killing everyone in its path. (**Price is ACTUALLY 1250 BTC**)",
        },
        new () {
            Id = "__AMMO__SolarCores",
            Rarity = ItemRarity.Mythic,
            Price = 3 * 100,
            Icon = "sprites/reusable-weapons/ammo_icons/ammo_icons_250x250/solarcores.png",
            Currency = GameManager.BITCOIN_CURRENCY,
            Name = "Solar Cores",
            Description = "Ammo for BTC based weapons (**Price is ACTUALLY 3 BTC**)",
        },
    };

    public static List<ShopCategory.ProductDescription> UpgeadesShopProducts = new()
    {
        new () {
            Id = "__UPGRADE__PrivateJet",
            Rarity = ItemRarity.Legendary,
            SparksProductId = "683675f5dd8d800d99696d94",
            Price = 199,
            Icon = "icons/private-jet.png",
            Name = "Private Jet",
            Description = "Get the Private Jet for free",
        },


    };
}