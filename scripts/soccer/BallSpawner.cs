using AO;

public class BallSpawner : Component
{
    public override void Awake()
    {
        SpawnBall();
    }

    public void SpawnBall()
    {
        if (!Network.IsServer) return;

        var ballPrefab = Assets.GetAsset<Prefab>("SoccerBall.prefab");
        Network.InstantiateAndSpawn(ballPrefab, onBeforeAwake: (entity) =>
        {
            entity.Position = Entity.Position;
        });
    }
}