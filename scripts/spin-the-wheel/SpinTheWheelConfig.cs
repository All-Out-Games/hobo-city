using AO;
using System;
using System.Collections.Generic;
using System.Linq;
using static SpinWheelUI;

public static class SpinTheWheelConfig
{
    // -- REWARDS CONFIG -- //
    // MUST be either 4 or 8 rewards
    // Reward callbacks run on both the client and the server, to allow you to play custom sounds/animations.
    public static readonly Reward[] SliceRewards =
    {
        // 1) 400 Cash (Common)
        new Reward("$400", Assets.GetAsset<Texture>("icons/cash.png"), RewardRarity.Common, (Player player) => {
            if (!Network.IsServer) return;
            Economy.DepositCurrency(player, GameManager.CASH_CURRENCY, 400);
        }),

        // // 2) Private Jet (Mythic)
        // new Reward("Private Jet", Assets.GetAsset<Texture>("icons/private-jet.png"), RewardRarity.Mythic, (Player player) => {
        //     if (!Network.IsServer) return;
        //     var myPlayer = player as MyPlayer;
        //     if (myPlayer == null || !myPlayer.Alive()) return;

        //     var itemDef = GameManager.Instance.GameItems.PrivateJet.ItemDefinition;

        //     // Check if player already owns the item
        //     bool alreadyHas = myPlayer.DefaultInventory.Items.Any(it => it != null && it.Definition.Id == itemDef.Id);
        //     if (alreadyHas) return;

        //     var instance = Inventory.CreateItem(itemDef, 1);
        //     if (Inventory.CanMoveItemToInventory(instance, myPlayer.DefaultInventory, out var _))
        //     {
        //         Inventory.MoveItemToInventory(instance, myPlayer.DefaultInventory);
        //     }
        // }),

        // 3) Jetski (Legendary)
        // new Reward("Jetski", Assets.GetAsset<Texture>("icons/jetski.png"), RewardRarity.Legendary, (Player player) => {
        //     if (!Network.IsServer) return;
        //     var myPlayer = player as MyPlayer;
        //     if (myPlayer == null || !myPlayer.Alive()) return;

        //     var itemDef = GameManager.Instance.GameItems.Jetski.ItemDefinition;
        //     bool alreadyHas = myPlayer.DefaultInventory.Items.Any(it => it != null && it.Definition.Id == itemDef.Id);
        //     if (alreadyHas) return;

        //     var instance = Inventory.CreateItem(itemDef, 1);
        //     if (Inventory.CanMoveItemToInventory(instance, myPlayer.DefaultInventory, out var _))
        //     {
        //         Inventory.MoveItemToInventory(instance, myPlayer.DefaultInventory);
        //     }
        // }),

        // 4) $1M Cash (Mythic)
        new Reward("$1M", Assets.GetAsset<Texture>("icons/cash.png"), RewardRarity.Mythic, (Player player) => {
            if (!Network.IsServer) return;
            Economy.DepositCurrency(player, GameManager.CASH_CURRENCY, 1000000);
        }),

        // 5) Tall Car (Rare)
        // new Reward("Tall Car", Assets.GetAsset<Texture>("icons/car-icons/tall.png"), RewardRarity.Rare, (Player player) => {
        //     if (!Network.IsServer) return;
        //     var myPlayer = player as MyPlayer;
        //     if (myPlayer == null || !myPlayer.Alive()) return;

        //     var itemDef = GameManager.Instance.GameItems.TallCar.ItemDefinition;
        //     bool alreadyHas = myPlayer.DefaultInventory.Items.Any(it => it != null && it.Definition.Id == itemDef.Id);
        //     if (alreadyHas) return;

        //     var instance = Inventory.CreateItem(itemDef, 1);
        //     if (Inventory.CanMoveItemToInventory(instance, myPlayer.DefaultInventory, out var _))
        //     {
        //         Inventory.MoveItemToInventory(instance, myPlayer.DefaultInventory);
        //     }
        // }),

        // 6) Void Splitter Weapon (Mythic)
        new Reward("Void Splitter", Assets.GetAsset<Texture>("sprites/reusable-weapons/weapon_icons/weapon_icons_250x250/void_splitter.png"), RewardRarity.Mythic, (Player player) => {
            if (!Network.IsServer) return;
            var myPlayer = player as MyPlayer;
            if (myPlayer == null || !myPlayer.Alive()) return;

            var itemDef = GameManager.Instance.GameItems.VoidSplitter.ItemDefinition;
            bool alreadyHas = myPlayer.DefaultInventory.Items.Any(it => it != null && it.Definition.Id == itemDef.Id);
            if (alreadyHas) return;

            var instance = Inventory.CreateItem(itemDef, 1);
            if (Inventory.CanMoveItemToInventory(instance, myPlayer.DefaultInventory, out var _))
            {
                Inventory.MoveItemToInventory(instance, myPlayer.DefaultInventory);
            }
        }),

        // 7) 100 Bitcoin (Legendary)
        new Reward("$15k", Assets.GetAsset<Texture>("icons/cash.png"), RewardRarity.Rare, (Player player) => {
            if (!Network.IsServer) return;
            Economy.DepositCurrency(player, GameManager.CASH_CURRENCY, 15000);
        }),

        // // 8) Capybara Pet (Mythic)
        // new Reward("Capybara", Assets.GetAsset<Texture>("icons/capy.png"), RewardRarity.Epic, (Player player) => {
        //     if (!Network.IsServer) return;
        //     var myPlayer = player as MyPlayer;
        //     if (myPlayer == null) return;
        //     var petManager = myPlayer.GetComponent<PetManager>();
        //     if (petManager == null) return;

        //     // Check if player already owns Capybara pet
        //     if (petManager.OwnedPets.Any(p => p.DefinitionId == "temp-Capybara")) return;

        //     string newPetId = Guid.NewGuid().ToString();
        //     petManager.AddPet(newPetId, "temp-Capybara");
        // }),
    };

    // -- ODDS CONFIG -- //
    // The chance of rolling each rarity is defined by these weights. They do not need to add up to 1 or 100 – any proportional values will do.
    public static readonly Dictionary<RewardRarity, float> RarityWeights = new()
    {
        { RewardRarity.Common,    60f },
        { RewardRarity.Rare,      25f },
        { RewardRarity.Epic,      10f },
        { RewardRarity.Legendary,  4f },
        { RewardRarity.Mythic,     1f },
    };
}
