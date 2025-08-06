using AO;

// TODO: move to particle system
public class Confetti : Component
{
    public Spine_Animator SpineAnimator;
    float startTime;

    public override void Awake()
    {
        SpineAnimator = GetComponent<Spine_Animator>();
        SpineAnimator.SpineInstance.SetAnimation("Confetti_Explosion", false);
        SpineAnimator.SpineInstance.Speed = 0.7f;
        startTime = Time.TimeSinceStartup;
    }

    public override void Update()
    {
        if (Time.TimeSinceStartup - startTime >= 0.95f)
        {
            Entity.Destroy();
        }
    }
}
