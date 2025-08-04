using AO;

public class HiddenArea : Component
{
  public Box_Collider Collider;
  public List<MyPlayer> PlayersInArea = new();
  public bool IsActive = true;

  public override void Awake()
  {
    return;

    // Collider = GetComponent<Box_Collider>();


    // Collider.OnCollisionEnter += (other) =>
    // {
    //   if (!IsActive) return;

    //   if (other.GetComponent<MyPlayer>().Alive())
    //   {
    //     Log.Info($"Player entered hidden area ({Collider.Entity.Parent.Name}): {other.GetComponent<MyPlayer>().Entity.Name}");
    //     if (!PlayersInArea.Contains(other.GetComponent<MyPlayer>()))
    //     {
    //       Log.Info($"Player entered hidden AREA ({Collider.Entity.Parent.Name}): {other.GetComponent<MyPlayer>().Entity.Name}");
    //       PlayersInArea.Add(other.GetComponent<MyPlayer>());
    //     }

    //     other.GetComponent<MyPlayer>().IsBehindSomething = true;
    //   }
    // };

    // Collider.OnCollisionExit += (other) =>
    // {
    //   if (!IsActive) return;

    //   if (other.GetComponent<MyPlayer>().Alive())
    //   {
    //     Log.Info($"Player exited hidden area ({Collider.Entity.Parent.Name}): {other.GetComponent<MyPlayer>().Entity.Name}");
    //     if (PlayersInArea.Contains(other.GetComponent<MyPlayer>()))
    //     {
    //       Log.Info($"Player exited hidden AREA ({Collider.Entity.Parent.Name}): {other.GetComponent<MyPlayer>().Entity.Name}");
    //       PlayersInArea.Remove(other.GetComponent<MyPlayer>());
    //     }

    //     other.GetComponent<MyPlayer>().IsBehindSomething = false;
    //   }
    // };
  }

  public void OnItemDestroyed()
  {
    IsActive = false;

    foreach (var player in PlayersInArea)
    {
      player.IsBehindSomething = false;
    }
  }

  public void OnItemRespawned()
  {
    IsActive = true;
  }
}
