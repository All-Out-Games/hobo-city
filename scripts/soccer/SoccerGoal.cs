using AO;

public class SoccerGoal : Component
{
    public Box_Collider Collider;

    public override void Awake()
    {
        Collider = GetComponent<Box_Collider>();
        Collider.OnCollisionEnter += OnCollisionEnter;
    }

    public void OnCollisionEnter(Entity other)
    {
        if (!other.Alive()) return;

        if (other.GetComponent<SoccerBall>().Alive())
        {
            SFX.Play(Assets.GetAsset<AudioAsset>("sfx/woohoo.wav"), new SFX.PlaySoundDesc() { Volume = 0.55f, Position = Entity.Position, Positional = true, SpeedPerturb = 0.2f, VolumePerturb = 0.3f });
            SFX.Play(Assets.GetAsset<AudioAsset>("sfx/disappear_confetti.wav"), new SFX.PlaySoundDesc() { Volume = 1f, Position = Entity.Position, Positional = true, SpeedPerturb = 0.3f, VolumePerturb = 0.3f });

            var confettiPrefab = Assets.GetAsset<Prefab>("Confetti.prefab");
            confettiPrefab.Instantiate(onBeforeAwake: (entity) =>
            {
                entity.Position = Entity.Position;
            });

            SoccerBall ball = other.GetComponent<SoccerBall>();
            ball.Reset();
        }
    }
}