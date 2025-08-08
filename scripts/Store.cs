using AO;
using System;
using System.Linq;
using System.Collections.Generic;
using TinyJson;

public partial class Store : System<Store>
{

    public Shop sparksShop, gunShop, generalShop, blackMarket, generateFurnitureShop, petShop, upgradesShop, weaponSellShop;
    public ShopCategory ammo, weapons, consumables, blackMarketCat, generateFurnitureCat, weaponSellCat;
    public string lastGenerativeMessage = "";
    public string generativeMessage = "";

    // Placeholder product lists (to be populated elsewhere) to satisfy compilation
    public static List<ShopCategory.ProductDescription> WeaponsProducts = new();
    public static List<ShopCategory.ProductDescription> AmmoProducts = new();
    public static List<ShopCategory.ProductDescription> ConsumablesProducts = new();

    public void CreateGunShop()
    {
        gunShop = Economy.CreateShop("Gun Shop");
        gunShop.SetPurchaseModifier(OnBeforeItemPurchase);
        if (Network.IsClient)
        {
            gunShop.SetCustomDisplay(CustomItemShopDisplay);
        }
        if (Network.IsServer)
        {
            gunShop.SetPurchaseHandler(OnItemPurchaseSuccessfull);
        }


        var weaponCat = gunShop.AddCategory("Weapons");
        weaponCat.Icon = "icons/weapons/assault_rifle.png";
        foreach (var p in WeaponsProducts)
        {
            weaponCat.AddProduct(p);
        }

        var ammoCat = gunShop.AddCategory("Ammo");
        ammoCat.Icon = "icons/weapons/ammo/smg-ammo.png";
        foreach (var p in AmmoProducts)
        {
            ammoCat.AddProduct(p);
        }
    }

    public override void Awake()
    {
        if (Network.IsServer) Purchasing.SetPurchaseHandler(SparksPurchaseHandler);

        // CreateGeneralShop();
        // CreateGunShop();
        // CreateBlackMarket();
        // CreateUpgradesShop();
        CreateWeaponSellShop();
    }

    public void CreateGeneralShop()
    {
        generalShop = Economy.CreateShop("General Shop");
        generalShop.SetPurchaseModifier(OnBeforeItemPurchase);
        if (Network.IsClient)
        {
            generalShop.SetCustomDisplay(CustomItemShopDisplay);
        }
        if (Network.IsServer)
        {
            generalShop.SetPurchaseHandler(OnItemPurchaseSuccessfull);
        }

        var consumablesCat = generalShop.AddCategory("Consumables");
        consumablesCat.Icon = "icons/food-icon.png";
        foreach (var p in ConsumablesProducts)
        {
            consumablesCat.AddProduct(p);
        }
    }

    // public void CreateBlackMarket()
    // {
    //     blackMarket = Economy.CreateShop("Black Market");
    //     blackMarket.SetPurchaseModifier(OnBeforeItemPurchase);
    //     if (Network.IsClient)
    //     {
    //         blackMarket.SetCustomDisplay(CustomItemShopDisplay);
    //     }
    //     if (Network.IsServer)
    //     {
    //         blackMarket.SetPurchaseHandler(OnItemPurchaseSuccessfull);
    //     }

    //     var blackMarketCat = blackMarket.AddCategory("Black Market");
    //     blackMarketCat.Icon = "housing/crypto/black-market-icon.png";
    //     foreach (var p in BlackMarketProducts)
    //     {
    //         blackMarketCat.AddProduct(p);
    //     }
    // }

    // -------------------- UPGRADES SHOP --------------------
    public void CreateUpgradesShop()
    {
        upgradesShop = Economy.CreateShop("Upgrades Shop");

        // Re-use generic purchase modifiers and handlers
        upgradesShop.SetPurchaseModifier(OnBeforeItemPurchase);
        if (Network.IsClient)
        {
            upgradesShop.SetCustomDisplay(CustomItemShopDisplay);
        }

        // NOTE: For now, most upgrades are spark-based and handled via Purchasing handler.
        // If server-side item grants are required, hook up purchase handler similarly to other shops.

        var upgradesCat = upgradesShop.AddCategory("Upgrades");
        upgradesCat.Icon = "spark-shop.png";

        foreach (var p in UpgeadesShopProducts)
        {
            upgradesCat.AddProduct(p);
        }
    }

    private bool LookupAndAwardItem(MyPlayer player, string itemId)
    {
        // Find the lambo item definition
        Item_Definition item = null;
        foreach (var i in GameManager.Instance.GameItems.ItemPool)
        {
            if (i.ItemDefinition.Id == itemId)
            {
                item = i.ItemDefinition;
                break;
            }
        }

        if (item != null)
        {
            player.ServerTryAddItem(item);
            GameManager.CallClient_PlayTargetedSFX("sfx/purchase.wav", 0.5f, player.Entity.Position, new RPCOptions(target: player));
            Log.Info($"Gave {itemId} to player {player.Name}");
            return true;
        }
        else
        {
            Log.Error($"{itemId} item not found in GameItems.ItemPool");
            return false;
        }
    }

    private bool SparksPurchaseHandler(Player _player, string productId)
    {
        var player = (MyPlayer)_player;

        Log.Info($"Player {player.Name} purchasing item: {productId}");

        switch (productId)
        {
            case "685afb236b535435039888bc": // Epic Crypto Pack
                var suc1 = player.ServerTryAddItem(GameManager.Instance.GameItems.ItemPool.First(i => i.ItemDefinition.Id == "__FURNITURE__PowerNuclearPlant").ItemDefinition);
                var suc2 = player.ServerTryAddItem(GameManager.Instance.GameItems.ItemPool.First(i => i.ItemDefinition.Id == "__FURNITURE__MiningRackMega").ItemDefinition);
                var suc3 = player.ServerTryAddItem(GameManager.Instance.GameItems.ItemPool.First(i => i.ItemDefinition.Id == "__FURNITURE__BTC_Collector_Mega").ItemDefinition);
                // If any succeed we grant so we don't end up in a scenario where we flood inventory with one item every time they join. Still not ideal. 
                return suc1 || suc2 || suc3;
            default:
                Log.Info($"Unknown product (probably game pass! np): {productId} for player {player.UserId}");
                return false;
        }
    }

    public void CreateWeaponSellShop()
    {
        weaponSellShop = Economy.CreateShop("Weapon Sell Shop");
        weaponSellShop.SetPurchaseModifier(OnBeforeItemPurchase);
        if (Network.IsClient)
        {
            weaponSellShop.SetCustomDisplay(CustomItemShopDisplay);
        }
        if (Network.IsServer)
        {
            weaponSellShop.SetPurchaseHandler(OnItemPurchaseSuccessfull);
        }

        weaponSellCat = weaponSellShop.AddCategory("Sell Weapons");
        weaponSellCat.Icon = "icons/weapons/assault_rifle.png";
    }

    #region ItemShop
    public PurchaseModification OnBeforeItemPurchase(Player _player, GameProduct product)
    {
        var player = (MyPlayer)_player;

        var modification = new PurchaseModification(product)
        {
            ModifyProduct = false
        };

        if (MyPlayer.localPlayer != null && product.Id.StartsWith("__GENERATIVE__"))
        {
            if (generativeMessage.Length < 3)
            {
                modification.ModifyProduct = true;
                modification.PurchaseButtonText = "Type a prompt";
                modification.Color = PurchaseButtonColor.Grey;
                modification.OnBuyButtonClicked = () => PurchaseFail(_player);
            }
            else
            {
                modification.ModifyProduct = true;
                modification.PurchaseButtonText = "Create";
                modification.Color = PurchaseButtonColor.Green;
            }
        }

        if (MyPlayer.localPlayer != null && product.Id.StartsWith("__WEAPON__"))
        {
            var weaponName = product.Id.Substring(11);

            var ownsWeapon = player.DefaultInventory.Items.FirstOrDefault(item => item != null && item.Definition.Id == product.Id);

            if (ownsWeapon != null)
            {
                modification.ModifyProduct = true;
                modification.PurchaseButtonText = "Owned";
                modification.Color = PurchaseButtonColor.Grey;
                modification.OnBuyButtonClicked = () => PurchaseFail(_player);
            }
        }

        if (product.Id.StartsWith("WEAPON_SELL_"))
        {
            modification.ModifyProduct = true;
            modification.PurchaseButtonText = "Sell";
            int slotIndex = int.Parse(product.Id.Substring(12, product.Id.IndexOf('_', 12) - 12));
            modification.OnBuyButtonClicked = () => MyPlayer.localPlayer.RequestSellWeapon(slotIndex);
        }

        return modification;
    }


    void PurchaseFail(Player player)
    {
        MyPlayer p = (MyPlayer)player;
        if (p.IsLocal)
        {
            //SFXE.Play(Assets.GetAsset<AudioAsset>("sfx/retro_fail_sound_05.wav"), new() { });
            //p.ShakeScreen(0.35f, 0.1f);
        }
    }


    public void CustomItemShopDisplay(GameProduct product, Rect rect)
    {
        rect.CutTop(10);
        var descriptionRect = rect.CutTop(200).Inset(0, 15, 0, 15);
        UI.TextAsync(descriptionRect, product.Description, new UI.TextSettings()
        {
            Font = UI.Fonts.Barlow,
            Size = 32,
            VerticalAlignment = UI.VerticalAlignment.Top,
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            Color = Vector4.White,
            WordWrap = true,
            Outline = true,
            OutlineThickness = 3.0f,
            DoAutofit = true,
            AutofitMinSize = 16,
            AutofitMaxSize = 32,
        });
        rect.CutTop(25);
    }


    public bool OnItemPurchaseSuccessfull(Player _player, GameProduct product)
    {
        var player = (MyPlayer)_player;

        Item_Definition item = null;

        foreach (var i in GameManager.Instance.GameItems.ItemPool)
        {
            if (i.ItemDefinition.Id.ToLower() == product.Id.ToLower())
            {
                item = i.ItemDefinition;
            }
        }

        if (item == null)
        {
            Log.Error($"Item {product.Id} not found");
            return false;
        }

        GameManager.CallClient_PlayTargetedSFX("sfx/purchase.wav", 0.5f, player.Entity.Position, new RPCOptions(target: player));

        Log.Info($"Purchasing item: {product.Id}");
        if (product.Id.StartsWith("__WEAPON__") || product.Id.StartsWith("__HEALING__"))
        {
            // Check if it's a weapon to add level metadata
            if (product.Id.StartsWith("__WEAPON__"))
            {
                var metadata = new List<(string, string)>();

                // Add level metadata based on player level
                var playerLevel = player.Level;
                metadata.Add(("level", playerLevel.ToString()));

                player.ServerTryAddItem(item, metadata: metadata);
            }
            else
            {
                player.ServerTryAddItem(item);
            }
        }
        else if (product.Id.StartsWith("__AMMO__"))
        {
            Log.Info($"Adding ammo: {product.Id}");
            if (Enum.TryParse<AmmoType>(item.Id.Substring(8), out var ammoType))
            {
                player.AmmoAmounts[ammoType].CurrentAmount += 15;
            }
            else
            {
                Log.Error($"Failed to parse ammo type from item: {item.Id}");
                return false;
            }

            player.ServerSyncAmmoAmount(ammoType, player.AmmoAmounts[ammoType].CurrentAmount);
        }
        else if (product.Id.StartsWith("__GENERATIVE__"))
        {
            if (generativeMessage.Length < 3)
            {
                return false;
            }

        }

        return true;
    }
    #endregion

    public static int CalculateWeaponValue(ItemRarity rarity, int level = 1)
    {
        // Base values for each rarity
        int baseValue = rarity switch
        {
            ItemRarity.Common => 100,
            ItemRarity.Rare => 500,
            ItemRarity.Epic => 2000,
            ItemRarity.Legendary => 5000,
            ItemRarity.Mythic => 10000,
            _ => 50
        };

        // Level multiplier: each level adds 20% more value
        float levelMultiplier = 1.0f + (level - 1) * 0.2f;

        return (int)(baseValue * levelMultiplier);
    }

    public void RefreshWeaponSell()
    {
        if (!Network.IsClient || MyPlayer.localPlayer == null) return;

        weaponSellCat.ClearProducts();

        var gameItems = GameManager.Instance.GameItems;
        var weaponProducts = new List<ShopCategory.ProductDescription>();

        foreach (var item in MyPlayer.localPlayer.DefaultInventory.Items)
        {
            if (item == null || item.Definition == null) continue;

            // Get custom item definition to check if it's a weapon
            var customDef = gameItems.GetCustomItemDefByID(item.Definition.Id);
            if (customDef == null || customDef.ItemCategory != ItemCategory.Weapon) continue;

            // Skip fists
            if (item.Definition.Id == "__WEAPON__fists") continue;

            string levelStr = item.GetMetadata("level");
            int weaponLevel = string.IsNullOrEmpty(levelStr) ? 1 : int.Parse(levelStr);

            // Get rarity from metadata first, fallback to custom definition
            ItemRarity rarity = customDef.ItemRarity;
            string rarityStr = item.GetMetadata("rarity");
            if (!string.IsNullOrEmpty(rarityStr) && Enum.TryParse<ItemRarity>(rarityStr, out var metadataRarity))
            {
                rarity = metadataRarity;
            }

            int price = CalculateWeaponValue(rarity, weaponLevel);
            string id = $"WEAPON_SELL_{item.InventorySlot}_{item.Id}";

            ShopCategory.ProductDescription product = new()
            {
                Id = id,
                Rarity = rarity,
                Price = price,
                Icon = item.Definition.Icon,
                Currency = GameManager.CASH_CURRENCY,
                Name = item.Definition.Name,
                Description = $"Level {weaponLevel} {rarity} Weapon"
            };

            weaponProducts.Add(product);
        }

        // Sort by rarity (lowest to highest)
        weaponProducts.Sort((a, b) => a.Rarity.CompareTo(b.Rarity));

        // Add sorted products to the shop
        foreach (var product in weaponProducts)
        {
            weaponSellCat.AddProduct(product);
        }
    }

    public static bool DrawShop(Shop shop)
    {
        Rect rect = UI.ScreenRect.CenterRect().Grow(300, 500, 300, 500);
        return shop.Draw(rect);
    }
}