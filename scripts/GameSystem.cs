using AO;

public class GameSystem : System<GameSystem>
{
  public override void Awake()
  {
    Keybinds.OverrideKeybindDefault("Ability 1", Input.UnifiedInput.MOUSE_LEFT);
    Economy.RegisterCurrency(GameManager.CASH_CURRENCY, "icons/cash.png");
    Economy.RegisterCurrency(GameManager.XP_CURRENCY, "icons/xp.png");
    Economy.RegisterCurrency("play_time_10s", "icons/bitcoin.png");

    // if (!Network.IsServer)
    // {
    //   Analytics.EnableAutomaticAnalytics("92e46d4144f82b3d5953a8dec0575714", "3d67d59d0befc79a5b19abcd3bd3137ae8eef6bb");
    // }

  }
}