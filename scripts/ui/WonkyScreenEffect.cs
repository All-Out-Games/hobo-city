using AO;

public partial class WonkyScreenEffect : MyEffect
{
  public override bool IsActiveEffect => false;
  public override bool BlockAbilityActivation => false;
  public override float DefaultDuration => 7f;

  public Entity WonkyScreenEntity;

  public SpineInstance WonkyInstance;

  public override void OnEffectStart(bool isDropIn)
  {
    if (Player == Network.LocalPlayer)
    {
      WonkyInstance = SpineInstance.Make();
      WonkyInstance.SetSkeleton(Assets.GetAsset<SpineSkeletonAsset>("housing/party/wonky-screen/HYDR113_screen_effect.spine"));
      WonkyInstance.SetAnimation("wonky_screen_idle", true);
    }
  }

  public override void OnEffectEnd(bool interrupt)
  {

  }


  public override void OnEffectLateUpdate()
  {
    if (Player == Network.LocalPlayer)
    {
      WonkyInstance.Update(Time.DeltaTime);
      UI.DrawSkeleton(UI.ScreenRect.CenterRect().Offset(0, -47), WonkyInstance, new Vector2(250, 250) * (new Vector2(UI.ScreenRect.Width / UI.ScreenRect.Height, 1) / new Vector2(1920f / 1080f, 1)), 0);
    }
  }
}
