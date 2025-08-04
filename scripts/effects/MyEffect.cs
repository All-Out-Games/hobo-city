using AO;

public abstract class MyEffect : AEffect
{
  public new MyPlayer Player => (MyPlayer)base.Player;
}