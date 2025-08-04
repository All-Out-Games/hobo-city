using AO;

namespace ReusableWeapons
{
    /// <summary>
    /// Handles the spawning of loot chests
    /// Currently spawns a chest on every spawn point but can easily be modified to randomly choose spawn points (see Red Sun)
    /// NavMesh is linked here so that items that are dropped are always dropped into a place you can reach
    /// </summary>
    public class LootManager : Singleton<_LootManager> { }
    public partial class _LootManager : Component
    {
        [Serialized] public Navmesh RootNavmesh;
        [Serialized] public Entity[] ChestSpawnPoints;

        public override void Awake()
        {
            if (Network.IsServer)
            {
                InitLootChests();
            }
        }

        private void InitLootChests()
        {
            foreach (var chestSpawnPoint in ChestSpawnPoints)
            {
                var chestInstance = Network.InstantiateAndSpawn(WeaponReferences.Instance.Loot_Chest, entity => entity.Position = chestSpawnPoint.Position);

                var chest = chestInstance.GetComponent<LootChest>();
                chest.ServerRandomizeChestType();
                chest.CallClient_SpawnChest(chestSpawnPoint.Position);
            }
        }
    }
}