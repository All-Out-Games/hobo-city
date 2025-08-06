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

  // Check if player is alive
  if (player.HealthManager.Health <= 0)
  {
    return false;
  }

  // Check if player is on teleport cooldown
  if (player.IsTeleportOnCooldown)
  {
    return false;
  }

  return true;
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

    if (Network.IsServer)
    {
      if (player.HealthManager.Health <= 0)
      {
        GameManager.CallClient_SendTargetedMessage("You can't teleport right now", new RPCOptions() { Target = player });
        return;
      }

      // Check damage cooldown
      if (player.IsTeleportOnCooldown)
      {
        GameManager.CallClient_SendTargetedMessage($"You can't teleport for {player.TeleportCooldownRemaining.Value:F1} more seconds after taking damage!", new RPCOptions() { Target = player });
        return;
      }
    }

    player.Teleport(ReturnToPoint.Position);
    player.Agent.LockToNavmesh = false;

    if (Network.IsServer)
    {
      player.CurrentRoom = ReturnToRoom;
    }
  }
}
