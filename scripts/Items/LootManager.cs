using AO;
using System.Collections.Generic;

namespace ReusableWeapons
{
    /// <summary>
    /// Handles the spawning of loot chests
    /// Currently spawns a chest on every spawn point but can easily be modified to randomly choose spawn points (see Red Sun)
    /// NavMesh is linked here so that items that are dropped are always dropped into a place you can reach
    /// </summary>

    public struct ChestRespawnInfo
    {
        public Vector2 SpawnPosition;
        public float DespawnedAt;
        public float RespawnTime;

        public ChestRespawnInfo(Vector2 position, float despawnTime, float respawnDelay)
        {
            SpawnPosition = position;
            DespawnedAt = despawnTime;
            RespawnTime = respawnDelay;
        }
    }

    public class LootManager : Singleton<_LootManager> { }
    public partial class _LootManager : Component
    {
        [Serialized] public Navmesh RootNavmesh;
        [Serialized] public Entity ChestSpawnPoints;

        public static List<ChestRespawnInfo> ChestRespawnQueue = new();
        public const float CHEST_RESPAWN_TIME = 30f; // 30 seconds

        public override void Awake()
        {
            if (Network.IsServer)
            {
                InitLootChests();
            }
        }

        public override void Update()
        {
            if (Network.IsServer)
            {
                // Check chest respawns
                for (int i = ChestRespawnQueue.Count - 1; i >= 0; i--)
                {
                    var respawnInfo = ChestRespawnQueue[i];
                    if (Time.TimeSinceStartup - respawnInfo.DespawnedAt > respawnInfo.RespawnTime)
                    {
                        SpawnChestAt(respawnInfo.SpawnPosition);
                        ChestRespawnQueue.RemoveAt(i);
                    }
                }
            }
        }

        private void InitLootChests()
        {
            foreach (var chestSpawnPoint in ChestSpawnPoints.Children)
            {
                SpawnChestAt(chestSpawnPoint.Position);
            }
        }

        public static void SpawnChestAt(Vector2 position)
        {
            var chestInstance = Network.InstantiateAndSpawn(WeaponReferences.Instance.Loot_Chest, entity => entity.Position = position);

            var chest = chestInstance.GetComponent<LootChest>();
            chest.ServerRandomizeChestType();
            chest.CallClient_SpawnChest(position);
        }

        public static void AddChestToRespawnQueue(Vector2 position)
        {
            ChestRespawnQueue.Add(new ChestRespawnInfo(position, Time.TimeSinceStartup, CHEST_RESPAWN_TIME));
        }
    }
}