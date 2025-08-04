using AO;

public abstract class MyAbility : Ability
{
  public new MyPlayer Player => (MyPlayer)base.Player;

  protected float CalculateCooldown(float baseCooldown) => baseCooldown;

  public override bool CanUse()
  {
    if (Player.HealthManager.Health <= 0) return false;
    return true;
  }

  public override bool CanTarget(Player player)
  {
    var myPlayer = (MyPlayer)player;

    if (myPlayer.HealthManager.IsInvulnerable) return false;
    return true;
  }
}