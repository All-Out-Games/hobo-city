using AO;

public partial class WorkoutEffect : MyEffect
{
    public override bool IsActiveEffect => true;
    public override bool BlockAbilityActivation => false;
    public override float DefaultDuration => 9.7f;
    public override bool FreezePlayer => true;

    public override void OnEffectStart(bool isDropIn)
    {
        if (!isDropIn)
        {
            Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("workout");
        }
    }

    public override void OnEffectEnd(bool interrupt)
    {
        // Workout effect no longer increases SwoleLevel
        // Player size is now based on damage leaderboard position
    }
}