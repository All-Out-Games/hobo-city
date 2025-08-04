using AO;

// Can't rename this since it's used in so many places
public class GenericReturnTeleporter : Component
{
  public Interactable Interactable;
  [Serialized] public Entity ReturnToPoint;
  [Serialized] public Room ReturnToRoom;
  [Serialized] public string SparksProductId;

  public override void Awake()
  {
    Interactable = Entity.GetComponent<Interactable>();
    Interactable.OnInteract += OnInteract;

    Interactable.CanUseCallback = (Player _player) =>
    {
      var player = (MyPlayer)_player;
      return player.HealthManager.Health > 0;
    };
  }

  public void OnInteract(Player _player)
  {
    var player = (MyPlayer)_player;

    if (Network.IsClient && SparksProductId != null && !Purchasing.OwnsGamePassLocal(SparksProductId))
    {
      if (player.IsLocal)
      {
        Purchasing.PromptPurchase(SparksProductId);
      }
    }

    if (SparksProductId != null && !Purchasing.OwnsGamePass(player, SparksProductId))
    {
      return;
    }

    if (Network.IsServer && player.HealthManager.Health <= 0)
    {
      GameManager.CallClient_SendTargetedMessage("You can't teleport right now", new RPCOptions() { Target = player });
      return;
    }

    player.Teleport(ReturnToPoint.Position);
    player.Agent.LockToNavmesh = false;

    if (Network.IsServer)
    {
      player.CurrentRoom = ReturnToRoom;
    }
  }
}
