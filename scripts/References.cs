using AO;

public static class References
{
  public static Texture HealthBarBack_BlueTeam = Assets.KeepLoaded<Texture>("ui/Health_Bar/health_bar_blue/healthbar_back.png", synchronous: false);
  public static Texture HealthBarBack_RedTeam = Assets.KeepLoaded<Texture>("ui/Health_Bar/health_bar_red/healthbar_back.png", synchronous: false);
  public static Texture HealthBarFill_BlueTeam = Assets.KeepLoaded<Texture>("ui/Health_Bar/health_bar_blue/healthbar_fill.png", synchronous: false);
  public static Texture HealthBarFill_RedTeam = Assets.KeepLoaded<Texture>("ui/Health_Bar/health_bar_red/healthbar_fill.png", synchronous: false);
  public static Texture Pip_RedTeam = Assets.KeepLoaded<Texture>("ui/Health_Bar/health_bar_red/healthbar_pip.png", synchronous: false);
  public static Texture Pip_BlueTeam = Assets.KeepLoaded<Texture>("ui/Health_Bar/health_bar_blue/healthbar_pip.png", synchronous: false);


  public static Prefab ShotgunExplosionVFX = Assets.GetAsset<Prefab>("ShotgunExplosionVFX.prefab");
  public static Prefab WaterBalloonSplashVFX = Assets.GetAsset<Prefab>("WaterBalloonSplashVFX.prefab");


}
