using AO;
using System;
using System.Collections.Generic;
using System.Linq;

// Structure to hold leaderboard data
public class PlayerLeaderboardData
{
    public string Name;
    public int Points;
    public float CurrentYOffset; // For smooth animation
    public float TargetYOffset;

    public PlayerLeaderboardData(string name, int points)
    {
        Name = name;
        Points = points;
        CurrentYOffset = 0f;
        TargetYOffset = 0f;
    }
}

// Track damage dealt over time
public class DamageEntry
{
    public float Timestamp;
    public int Amount;

    public DamageEntry(float timestamp, int amount)
    {
        Timestamp = timestamp;
        Amount = amount;
    }
}

// Component to track damage dealt by players
public partial class DamageTracker : Component
{
    // Singleton instance
    static DamageTracker _instance;

    // Rolling window duration in seconds
    public const float DAMAGE_WINDOW_DURATION = 120f;

    // Dictionary to track damage entries per player (server only)
    Dictionary<Entity, List<DamageEntry>> playerDamageEntries = new Dictionary<Entity, List<DamageEntry>>();

    // Cache for leaderboard data
    List<PlayerLeaderboardData> cachedLeaderboardData = new List<PlayerLeaderboardData>();
    float lastLeaderboardUpdate = -1f;
    const float LEADERBOARD_UPDATE_INTERVAL = 0.5f; // Update every 0.5 seconds

    // For syncing to clients
    float lastSyncTime = -1f;
    const float SYNC_INTERVAL = 1f; // Sync every 1 second

    // Client-side leaderboard data
    List<PlayerLeaderboardData> clientLeaderboardData = new List<PlayerLeaderboardData>();
    public static DamageTracker Instance
    {
        get
        {
            if (_instance.Alive()) return _instance;
            foreach (var c in Scene.Components<DamageTracker>(false))
            {
                _instance = c;
                _instance.Awaken();
                break;
            }
            return _instance;
        }
    }
    public override void Awake()
    {
        _instance = this;
    }

    // Record damage dealt by a player
    public void RecordDamage(Entity player, int damageAmount)
    {
        if (!player.Alive()) return;

        // Initialize list if player doesn't exist
        if (!playerDamageEntries.ContainsKey(player))
        {
            playerDamageEntries[player] = new List<DamageEntry>();
        }

        // Add new damage entry
        playerDamageEntries[player].Add(new DamageEntry(Time.TimeSinceStartup, damageAmount));

        // Mark cache as dirty
        lastLeaderboardUpdate = -1f;
    }

    // Get total damage dealt by a player in the rolling window
    public int GetPlayerDamage(Entity player)
    {
        if (!playerDamageEntries.ContainsKey(player))
            return 0;

        float currentTime = Time.TimeSinceStartup;
        float cutoffTime = currentTime - DAMAGE_WINDOW_DURATION;

        return playerDamageEntries[player]
            .Where(entry => entry.Timestamp > cutoffTime)
            .Sum(entry => entry.Amount);
    }

    // Clean up old damage entries
    void CleanupOldEntries()
    {
        float currentTime = Time.TimeSinceStartup;
        float cutoffTime = currentTime - DAMAGE_WINDOW_DURATION;

        List<Entity> keysToRemove = new List<Entity>();

        foreach (var kvp in playerDamageEntries)
        {
            // Remove old entries
            kvp.Value.RemoveAll(entry => entry.Timestamp <= cutoffTime);

            // If player has no entries left or entity is dead, mark for removal
            if (kvp.Value.Count == 0 || !kvp.Key.Alive())
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        // Remove empty entries
        foreach (var key in keysToRemove)
        {
            playerDamageEntries.Remove(key);
        }
    }

    // Get leaderboard data for all players
    public List<PlayerLeaderboardData> GetLeaderboardData()
    {
        // Return cached data if recent enough
        if (Time.TimeSinceStartup - lastLeaderboardUpdate < LEADERBOARD_UPDATE_INTERVAL)
        {
            return cachedLeaderboardData;
        }

        // Clean up old entries first
        CleanupOldEntries();

        // Build new leaderboard data
        cachedLeaderboardData.Clear();

        foreach (var player in Scene.Components<MyPlayer>())
        {
            if (!player.Alive()) continue;

            int totalDamage = GetPlayerDamage(player.Entity);

            // Only include players who have dealt damage
            if (totalDamage > 0)
            {
                cachedLeaderboardData.Add(new PlayerLeaderboardData(
                    player.Name,
                    totalDamage
                ));
            }
        }

        // Sort by damage (highest first)
        cachedLeaderboardData.Sort((a, b) => b.Points.CompareTo(a.Points));

        // Update animation targets
        for (int i = 0; i < cachedLeaderboardData.Count; i++)
        {
            cachedLeaderboardData[i].TargetYOffset = i * GameManager.LEADERBOARD_ENTRY_HEIGHT;
        }

        lastLeaderboardUpdate = Time.TimeSinceStartup;
        return cachedLeaderboardData;
    }

    public override void Update()
    {
        // Server: Send leaderboard updates to clients
        if (Network.IsServer)
        {
            if (Time.TimeSinceStartup - lastSyncTime >= SYNC_INTERVAL)
            {
                // Get current leaderboard data
                var leaderboardData = GetLeaderboardData();

                // Prepare data for RPC (names and points)
                string[] names = new string[leaderboardData.Count];
                int[] points = new int[leaderboardData.Count];

                for (int i = 0; i < leaderboardData.Count; i++)
                {
                    names[i] = leaderboardData[i].Name;
                    points[i] = leaderboardData[i].Points;
                }

                // Send to all clients
                CallClient_UpdateLeaderboard(names, points);

                lastSyncTime = Time.TimeSinceStartup;
            }
        }

        // Update animation offsets (use appropriate list based on server/client)
        var dataToAnimate = Network.IsServer ? cachedLeaderboardData : clientLeaderboardData;
        foreach (var data in dataToAnimate)
        {
            // Lerp the Y offset for smooth animation
            float lerpSpeed = Time.DeltaTime * 10f;
            data.CurrentYOffset = data.CurrentYOffset + (data.TargetYOffset - data.CurrentYOffset) * Math.Min(lerpSpeed, 1f);
        }
    }

    [ClientRpc]
    public void UpdateLeaderboard(string[] names, int[] points)
    {
        // Create a dictionary to preserve existing animation states
        var existingDataMap = new Dictionary<string, PlayerLeaderboardData>();
        foreach (var data in clientLeaderboardData)
        {
            existingDataMap[data.Name] = data;
        }

        // Clear and rebuild client leaderboard data
        clientLeaderboardData.Clear();

        for (int i = 0; i < names.Length && i < points.Length; i++)
        {
            var data = new PlayerLeaderboardData(names[i], points[i]);
            data.TargetYOffset = i * GameManager.LEADERBOARD_ENTRY_HEIGHT;

            // Preserve existing animation state if this player was already in the list
            if (existingDataMap.TryGetValue(names[i], out var existingData))
            {
                data.CurrentYOffset = existingData.CurrentYOffset;
            }
            else
            {
                // New entry starts at its target position to avoid sliding in from top
                data.CurrentYOffset = data.TargetYOffset;
            }

            clientLeaderboardData.Add(data);
        }
    }

    // Get leaderboard data appropriate for client/server
    public List<PlayerLeaderboardData> GetClientLeaderboardData()
    {
        return Network.IsClient ? clientLeaderboardData : GetLeaderboardData();
    }
}
