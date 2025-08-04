using AO;

public class Shield : Component
{
  public Spine_Animator SpineAnimator;
  public float AppearedAt = 0f;
  public float ShutdownAt = 0f;
  public bool hasPlayedLoop = false;

  public override void Awake()
  {
    SpineAnimator = AddComponent<Spine_Animator>();
    SpineAnimator.SpineInstance.SetSkeleton(Assets.GetAsset<SpineSkeletonAsset>("rigs/shield/reflective_shield.spine"));
    SpineAnimator.SpineInstance.SetAnimation("activate", false);
    SpineAnimator.Layer = 1000;
    AppearedAt = Time.TimeSinceStartup;
  }

  public override void Update()
  {
    if (Time.TimeSinceStartup - AppearedAt > 0.5f && !hasPlayedLoop)
    {
      SpineAnimator.SpineInstance.SetAnimation("loop", true);
      hasPlayedLoop = true;
    }

    if (ShutdownAt > 0f && Time.TimeSinceStartup - ShutdownAt > 0.5f)
    {
      Entity.Destroy();
      Network.Despawn(Entity);
    }
  }

  public void Shutdown()
  {
    SpineAnimator.SpineInstance.SetAnimation("end", false);
    ShutdownAt = Time.TimeSinceStartup;
  }
}