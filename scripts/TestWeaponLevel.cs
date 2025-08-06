using AO;
using System;

/// <summary>
/// Test component to verify weapon level metadata is working correctly.
/// Attach this to a player to see their weapon levels in the console.
/// </summary>
public partial class TestWeaponLevel : Component
{
    MyPlayer player;
    float lastCheckTime = 0;

    public override void Awake()
    {
        player = GetComponent<MyPlayer>();
    }

    public override void Update()
    {
        if (!Network.IsClient || player == null || !player.Alive()) return;

        // Check every 2 seconds
        if (Time.TimeSinceStartup - lastCheckTime < 2f) return;
        lastCheckTime = Time.TimeSinceStartup;

        // Check all items in inventory for level metadata
        foreach (var item in player.DefaultInventory.Items)
        {
            if (item == null || item.Definition == null) continue;

            var customDef = GameManager.Instance.GameItems.GetCustomItemDefByID(item.Definition.Id);
            if (customDef == null || customDef.ItemCategory != ItemCategory.Weapon) continue;

            var levelMetadata = item.GetMetadata("level");
            var rarityMetadata = item.GetMetadata("rarity");

            if (!string.IsNullOrEmpty(levelMetadata))
            {
                Log.Info($"Weapon: {item.Definition.Name} - Level: {levelMetadata}, Rarity: {rarityMetadata ?? "Unknown"}");
            }
            else
            {
                Log.Info($"Weapon: {item.Definition.Name} - No level metadata, Rarity: {rarityMetadata ?? "Unknown"}");
            }
        }
    }
}