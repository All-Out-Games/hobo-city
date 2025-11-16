using AO;
using System;

public class AbilityDodgeRoll : MyAbility
{
    public override Type Effect => typeof(EffectDodgeRoll);
    public override bool MonitorEffectDuration => true;
    public override TargettingMode TargettingMode => TargettingMode.Self;
    public override float Cooldown => 1f;
    public override float MaxDistance => 1.5f;
    public override Texture Icon => Assets.GetAsset<Texture>("roll-icon.png");

    public override bool CanUse()
    {
        if (!base.CanUse()) return false;
        if (Player.HealthManager.Health <= 0) return false;
        return true;
    }
}


