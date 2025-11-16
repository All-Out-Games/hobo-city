using AO;

public class GameSystem : System<GameSystem>
{
  public override void Awake()
  {
    Keybinds.OverrideKeybindDefault("Ability 1", Input.UnifiedInput.MOUSE_LEFT);
    Keybinds.OverrideKeybindDefault("Ability 2", Input.UnifiedInput.KEYCODE_SPACE);
    Economy.RegisterCurrency(GameManager.CASH_CURRENCY, "icons/cash.png");
    Economy.RegisterCurrency(GameManager.XP_CURRENCY, "icons/xp.png");
    Economy.RegisterCurrency("play_time_10s", "icons/bitcoin.png");

    if (!Network.IsServer)
    {
      Analytics.EnableAutomaticAnalytics("e167b72bbe4c3cf6672000d5d2f7aa9f", "b0dbcb4f65d428b37f5ba8767513702e7112c8e0");
    }

  }
}