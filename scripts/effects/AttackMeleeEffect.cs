using AO;

public partial class AttackMeleeEffect : MyEffect
{
    // This effect is short-lived and purely cosmetic, it simply triggers the attack animation.
    public override bool IsActiveEffect => true;
    public override bool BlockAbilityActivation => false;
    public override bool FreezePlayer => false;
    public override float DefaultDuration => 0.6f;

    public override void OnEffectStart(bool isDropIn)
    {
        // Prevent retriggering when the player joins an ongoing effect midway.
        if (isDropIn) return;

        // Fire the animation trigger we registered in the state machine.
        Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("attack_melee_1");
    }
}