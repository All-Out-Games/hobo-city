using AO;
using System;

public partial class WeaponLevelUpper : Component
{
    private Interactable interactable;
    private Spine_Animator spineAnimator;
    private const int MAX_WEAPON_LEVEL = 50;
    private const long TOTAL_COST_TO_MAX = 750000; // $1M total to reach level 50

    public override void Awake()
    {
        interactable = Entity.GetComponent<Interactable>();
        spineAnimator = Entity.GetComponent<Spine_Animator>();

        interactable.CanUseCallback += (Player p) =>
        {
            // Always allow interaction - we'll check everything in OnInteract
            return true;
        };

        interactable.OnInteract = (Player p) =>
        {
            if (!Network.IsServer) return;

            var myPlayer = (MyPlayer)p;
            var equippedItem = myPlayer.DefaultInventory.Items[myPlayer.CurrentHoveredSlot];

            if (equippedItem == null)
            {
                // No item equipped
                myPlayer.CallClient_NoWeaponEquipped(new RPCOptions() { Target = myPlayer });
                return;
            }

            // Get weapon info
            if (!GameItems.TryCreateCustomInstance(equippedItem, out var customItem))
            {
                myPlayer.CallClient_NoWeaponEquipped(new RPCOptions() { Target = myPlayer });
                return;
            }

            if (customItem.CustomDefinition.ItemCategory != ItemCategory.Weapon)
            {
                // Item is not a weapon
                myPlayer.CallClient_NotAWeapon(customItem.CustomDefinition.ItemDefinition.Name, new RPCOptions() { Target = myPlayer });
                return;
            }

            var levelStr = equippedItem.GetMetadata("level");
            int currentLevel = string.IsNullOrEmpty(levelStr) ? 1 : int.Parse(levelStr);

            if (currentLevel >= MAX_WEAPON_LEVEL)
            {
                // Weapon is already max level
                myPlayer.CallClient_WeaponMaxLevel(customItem.CustomDefinition.ItemDefinition.Name, new RPCOptions() { Target = myPlayer });
                return;
            }

            ItemRarity rarity = customItem.CustomDefinition.ItemRarity;
            string rarityStr = equippedItem.GetMetadata("rarity");
            if (!string.IsNullOrEmpty(rarityStr) && Enum.TryParse<ItemRarity>(rarityStr, out var metadataRarity))
            {
                rarity = metadataRarity;
            }

            long upgradeCost = CalculateUpgradeCost(rarity, currentLevel);

            long currentBalance = Economy.GetBalance(myPlayer, GameManager.CASH_CURRENCY);

            if (currentBalance >= upgradeCost)
            {
                // Deduct money
                Economy.DepositCurrency(myPlayer, GameManager.CASH_CURRENCY, -upgradeCost);

                // Update weapon level
                int newLevel = currentLevel + 1;
                equippedItem.SetMetadata("level", newLevel.ToString());

                // Send success notification
                myPlayer.CallClient_WeaponUpgraded(customItem.CustomDefinition.ItemDefinition.Name, newLevel, upgradeCost, new RPCOptions() { Target = myPlayer });

                // Update the equipped item to refresh any damage calculations
                myPlayer.CallClient_UpdateCurrentHoveredSlot();

                // Play upgrade sound and animation
                myPlayer.CallClient_PlayUpgradeSound(new RPCOptions() { Target = myPlayer });
                CallClient_PlayUpgradeAnimation(new RPCOptions() { Target = myPlayer });
            }
            else
            {
                // Not enough money - send notification
                long shortBy = upgradeCost - currentBalance;
                myPlayer.CallClient_NotEnoughMoneyForUpgrade(customItem.CustomDefinition.ItemDefinition.Name, upgradeCost, currentBalance, new RPCOptions() { Target = myPlayer });
            }
        };
    }

    public override void Update()
    {
        if (Network.IsServer) return;

        // Set dynamic text based on current equipped weapon
        var localPlayer = MyPlayer.localPlayer;
        if (localPlayer == null || !localPlayer.Alive())
        {
            interactable.Text = "Upgrade Weapon";
            return;
        }

        var equippedItem = localPlayer.DefaultInventory.Items[localPlayer.CurrentHoveredSlot];
        if (equippedItem == null)
        {
            interactable.Text = "No weapon equipped";
            return;
        }

        // Check if it's a weapon
        if (!GameItems.TryCreateCustomInstance(equippedItem, out var customItem))
        {
            interactable.Text = "No weapon equipped";
            return;
        }

        if (customItem.CustomDefinition.ItemCategory != ItemCategory.Weapon)
        {
            interactable.Text = "Equip a weapon to upgrade";
            return;
        }

        // Get current level
        var levelStr = equippedItem.GetMetadata("level");
        int currentLevel = string.IsNullOrEmpty(levelStr) ? 1 : int.Parse(levelStr);

        if (currentLevel >= MAX_WEAPON_LEVEL)
        {
            interactable.Text = $"{customItem.CustomDefinition.ItemDefinition.Name} is MAX LEVEL!";
            return;
        }

        ItemRarity rarity = customItem.CustomDefinition.ItemRarity;
        string rarityStr = equippedItem.GetMetadata("rarity");
        if (!string.IsNullOrEmpty(rarityStr) && Enum.TryParse<ItemRarity>(rarityStr, out var metadataRarity))
        {
            rarity = metadataRarity;
        }

        // Calculate upgrade cost
        long upgradeCost = CalculateUpgradeCost(rarity, currentLevel);

        // Set text
        interactable.Text = $"Upgrade {customItem.CustomDefinition.ItemDefinition.Name} to Lv.{currentLevel + 1} (${upgradeCost:N0})";
    }

    public static long CalculateUpgradeCost(ItemRarity rarity, int currentLevel)
    {
        // Rarity multipliers affect the distribution of cost across levels
        float rarityMultiplier = rarity switch
        {
            ItemRarity.Common => 0.2f,      // Cheaper upgrades
            ItemRarity.Uncommon => 0.3f,
            ItemRarity.Rare => 0.4f,       // Moderate upgrades
            ItemRarity.Epic => 0.8f,        // Standard upgrades
            ItemRarity.Legendary => 1f,   // Expensive upgrades
            ItemRarity.Mythic => 1.0f,      // Very expensive upgrades
            _ => 1.0f
        };

        // Calculate cumulative cost using exponential formula
        // Total cost to reach level n = A * (e^(k*n) - e^k) / (e^k - 1)
        // Where A is base cost, k is growth rate

        float k = 0.08f; // Growth rate
        float A = 100f; // Base cost

        // Calculate total cost to reach currentLevel + 1
        float totalCostToNext = A * (MathF.Exp(k * (currentLevel + 1)) - MathF.Exp(k)) / (MathF.Exp(k) - 1);

        // Calculate total cost to reach currentLevel
        float totalCostToCurrent = A * (MathF.Exp(k * currentLevel) - MathF.Exp(k)) / (MathF.Exp(k) - 1);

        // The upgrade cost is the difference
        float upgradeCost = totalCostToNext - totalCostToCurrent;

        // Apply rarity multiplier
        upgradeCost *= rarityMultiplier;

        // Scale to ensure total cost to max is approximately $5M for Epic weapons
        float scaleFactor = TOTAL_COST_TO_MAX / (A * (MathF.Exp(k * MAX_WEAPON_LEVEL) - MathF.Exp(k)) / (MathF.Exp(k) - 1));
        upgradeCost *= scaleFactor;

        return (long)Math.Round(upgradeCost);
    }

    [ClientRpc]
    public void PlayUpgradeAnimation()
    {
        if (spineAnimator != null && spineAnimator.Alive())
        {
            spineAnimator.SpineInstance.SetAnimation("switch", false);
        }
    }
}

// Extension for MyPlayer to handle client notifications
public partial class MyPlayer
{
    [ClientRpc]
    public void WeaponUpgraded(string weaponName, int newLevel, long cost)
    {
        Notifications.Show($"{weaponName} upgraded to Level {newLevel}! (-${cost:N0})");
    }

    [ClientRpc]
    public void PlayUpgradeSound()
    {
        // Play a satisfying upgrade sound
        AudioAsset upgradeSound = Assets.GetAsset<AudioAsset>("sfx/pull_lever.wav");
        if (upgradeSound != null)
        {
            SFX.Play(upgradeSound, new SFX.PlaySoundDesc()
            {
                EntityToFollow = Entity,
                Volume = 0.4f,
                SpeedPerturb = 0.1f
            });
        }
    }

    [ClientRpc]
    public void NotEnoughMoneyForUpgrade(string weaponName, long upgradeCost, long currentBalance)
    {
        long shortBy = upgradeCost - currentBalance;
        Notifications.Show($"Need ${shortBy:N0} more to upgrade {weaponName}! (Cost: ${upgradeCost:N0})");

        // Play error sound
        AudioAsset errorSound = Assets.GetAsset<AudioAsset>("sfx/error.wav");
        if (errorSound != null)
        {
            SFX.Play(errorSound, new SFX.PlaySoundDesc()
            {
                EntityToFollow = Entity,
                Volume = 0.6f
            });
        }
    }

    [ClientRpc]
    public void NoWeaponEquipped()
    {
        Notifications.Show("Equip a weapon first to upgrade it!");

        // Play error sound
        AudioAsset errorSound = Assets.GetAsset<AudioAsset>("sfx/error.wav");
        if (errorSound != null)
        {
            SFX.Play(errorSound, new SFX.PlaySoundDesc()
            {
                EntityToFollow = Entity,
                Volume = 0.6f
            });
        }
    }

    [ClientRpc]
    public void NotAWeapon(string itemName)
    {
        Notifications.Show($"{itemName} is not a weapon! Equip a weapon to upgrade it.");

        // Play error sound
        AudioAsset errorSound = Assets.GetAsset<AudioAsset>("sfx/error.wav");
        if (errorSound != null)
        {
            SFX.Play(errorSound, new SFX.PlaySoundDesc()
            {
                EntityToFollow = Entity,
                Volume = 0.6f
            });
        }
    }

    [ClientRpc]
    public void WeaponMaxLevel(string weaponName)
    {
        Notifications.Show($"{weaponName} is already at MAX LEVEL!");

        // Play error sound
        AudioAsset errorSound = Assets.GetAsset<AudioAsset>("sfx/error.wav");
        if (errorSound != null)
        {
            SFX.Play(errorSound, new SFX.PlaySoundDesc()
            {
                EntityToFollow = Entity,
                Volume = 0.6f
            });
        }
    }
}