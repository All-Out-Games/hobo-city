using AO;

public class Particle : Component
{
    [Serialized] public SpineSkeletonAsset SkeletonAsset;
    [Serialized] public string AnimationName;
    [Serialized] public float Duration = 0.95f;
    [Serialized] public float Speed = 1f;

    public Spine_Animator SpineAnimator;
    float startTime;

    public override void Awake()
    {
        SpineAnimator = GetComponent<Spine_Animator>();
        SpineAnimator.Awaken();
        SpineAnimator.SpineInstance.SetSkeleton(SkeletonAsset);
        SpineAnimator.SpineInstance.SetAnimation(AnimationName, false);
        SpineAnimator.Layer = 100;
        SpineAnimator.SpineInstance.Speed = Speed;
        startTime = Time.TimeSinceStartup;
    }

    public override void Update()
    {
        if (Time.TimeSinceStartup - startTime >= Duration)
        {
            Entity.Destroy();
        }
    }
}
