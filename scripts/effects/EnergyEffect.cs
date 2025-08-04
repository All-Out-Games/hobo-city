using AO;

public partial class EnergyEffect : MyEffect
{
  public override bool IsActiveEffect => false;
  public override bool BlockAbilityActivation => false;
  public override float DefaultDuration => 10f;

  public override void OnEffectStart(bool isDropIn)
  {

  }

  public override void OnEffectEnd(bool interrupt)
  {
  }
  public override void OnEffectLateUpdate()
  {

  }
}
