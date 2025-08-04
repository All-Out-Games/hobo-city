using AO;

public partial class SitEffect : MyEffect
{
    public override bool IsActiveEffect => true;
    public override bool BlockAbilityActivation => false;
    public override float DefaultDuration => float.MaxValue; // Sit indefinitely until cancelled
    public override bool FreezePlayer => true; // Prevent movement while sitting
    public bool HasEnteredSitState = false;
    [Serialized] public Vector2 StartPosition;
    [Serialized] public bool IsTropical = false;

    public override void OnEffectStart(bool isDropIn)
    {
        if (!isDropIn)
        {
            if (IsTropical)
            {
                Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("afk");
            }
            else
            {
                Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("sit");
            }
        }

    }

    public override void OnEffectEnd(bool interrupt)
    {
        Player.SpineAnimator.SpineInstance.Speed = 1f;

        if (StartPosition != Vector2.Zero)
        {
            Player.Teleport(StartPosition);
        }
    }

    public override void OnEffectLateUpdate()
    {
        if (DurationRemaining - float.MaxValue < 1f && !HasEnteredSitState)
        {
            HasEnteredSitState = true;
            Player.SpineAnimator.SpineInstance.Speed = 0.25f;
        }

        if (HasEnteredSitState && (Math.Abs(Player.InputThisFrame.X) > 0.1f || Math.Abs(Player.InputThisFrame.Y) > 0.1f))
        {
            Player.RemoveEffect<SitEffect>(true);
        }
    }
}