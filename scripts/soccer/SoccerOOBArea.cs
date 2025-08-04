using AO;

public class SoccerOOBArea : Component
{
    public Polygon_Collider Collider;

    public override void Awake()
    {
        Collider = GetComponent<Polygon_Collider>();
        Collider.OnCollisionEnter += OnCollisionEnter;
    }

    public void OnCollisionEnter(Entity other)
    {
        if (!other.Alive()) return;

        if (other.GetComponent<SoccerBall>().Alive())
        {
            SoccerBall ball = other.GetComponent<SoccerBall>();
            ball.Reset();

        }
    }
}
