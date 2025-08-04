using AO;

public class SoccerBall : Component
{
    public Circle_Collider Collider;
    float lastSoundTime;
    float soundDebounceTime = 0.45f;

    // For tracking velocity and rotation
    Vector2 previousPosition;
    float rotationSpeed = 25.0f;

    public override void Awake()
    {
        Collider = GetComponent<Circle_Collider>();
        Collider.OnCollisionEnter += OnCollisionEnter;
        lastSoundTime = -soundDebounceTime;
        previousPosition = Entity.Position;
    }

    public override void Update()
    {
        // Calculate velocity based on position change
        Vector2 currentPosition = Entity.Position;
        Vector2 velocity = (currentPosition - previousPosition) / Time.DeltaTime;
        previousPosition = currentPosition;

        // Apply rotation based on velocity magnitude and direction
        // Mainly using X velocity to determine rotation direction
        if (velocity.Length > 0.1f)
        {
            float rotationAmount = -velocity.X * rotationSpeed * Time.DeltaTime;
            Entity.Rotation += rotationAmount;
        }
    }

    public void OnCollisionEnter(Entity other)
    {
        if (!other.Alive()) return;

        if (other.GetComponent<MyPlayer>().Alive())
        {
            float currentTime = Time.TimeSinceStartup;
            if (currentTime - lastSoundTime >= soundDebounceTime)
            {
                SFX.Play(Assets.GetAsset<AudioAsset>("sfx/soccer-kick.wav"), new SFX.PlaySoundDesc() { Volume = 0.5f, Position = Entity.Position, Positional = true, SpeedPerturb = 0.3f, VolumePerturb = 0.3f });
                lastSoundTime = currentTime;
            }
        }
    }

    public void Reset()
    {
        SFX.Play(Assets.GetAsset<AudioAsset>("sfx/soccer-reset.wav"), new SFX.PlaySoundDesc() { Volume = 0.5f, Position = Entity.Position, Positional = true, SpeedPerturb = 0.3f, VolumePerturb = 0.3f });
        Scene.Components<BallSpawner>().FirstOrDefault().SpawnBall();

        if (!Network.IsServer) return;

        Network.Despawn(Entity);
        Entity.Destroy();
    }
}
