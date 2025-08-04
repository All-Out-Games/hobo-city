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
        if (Network.IsServer && !interrupt)
        {
            if (Player.SwoleLevel.Value < 5f)
            {
                Player.SwoleLevel.Set(Player.SwoleLevel.Value + 0.4f);
            }
        }
    }
}