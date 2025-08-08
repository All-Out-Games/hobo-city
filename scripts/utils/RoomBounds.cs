using AO;
public enum Room
{
    STADIUM,
    DEALERSHIP,
    CLUB,
    BANK,
    CLOTHING_STORE,
    GUN_STORE,
    GENERAL_STORE,
    HOSPITAL,
    DINER,
    POLICE_STATION,
    BEACH,
    OUTSIDE,
    GARBAGE,
    PIZZA_PLACE,
    OCEAN,
    PIZZA_KITCHEN,
    BANK_VAULT,
    CINEMA,
    BLACK_MARKET,
    FORGE,
    ISLAND,
    None,
}

public class RoomBounds : Component
{
    [Serialized]
    public Room RoomName;

    public override void Awake()
    {
        Entity.GetComponent<Box_Collider>().OnCollisionEnter = (Entity other) =>
        {
            var op = other.GetComponent<MyPlayer>();
            if (!op.Alive()) return;

            // Store the previous room for event metadata
            Room previousRoom = op.CurrentRoom;
            op.CurrentRoom = RoomName;

            // Add invulnerability effect when entering safe areas
            if (Network.IsServer && (RoomName == Room.FORGE || RoomName == Room.GUN_STORE || RoomName == Room.HOSPITAL))
            {
                if (!op.HasEffect<InvulnerabilityEffect>())
                {
                    op.CallClient_SetInvulnerable(true, false);
                }
            }

            // Fire the room enter event
            string metadata = $"{previousRoom}:{RoomName}";
            EventSystem.FireEvent(GameEventType.PlayerEnterRoom, op, metadata);
        };

        Entity.GetComponent<Box_Collider>().OnCollisionExit = (Entity other) =>
        {
            var player = other.GetComponent<MyPlayer>();
            if (!player.Alive()) return;

            // Store the previous room for event metadata
            Room previousRoom = player.CurrentRoom;
            player.CurrentRoom = Room.OUTSIDE;

            // Remove invulnerability effect when exiting safe areas
            if (Network.IsServer && (RoomName == Room.FORGE || RoomName == Room.GUN_STORE || RoomName == Room.HOSPITAL))
            {
                if (player.HasEffect<InvulnerabilityEffect>())
                {
                    player.CallClient_SetInvulnerable(false, true);
                }
            }

            // Fire the room exit event
            string metadata = $"{previousRoom}:{Room.OUTSIDE}";
            EventSystem.FireEvent(GameEventType.PlayerExitRoom, player, metadata);
        };
    }

    public static List<Player> GetPlayersInRoom(Room room)
    {
        var players = new List<Player>();

        foreach (var player in Scene.Components<MyPlayer>())
        {
            var op = (MyPlayer)player;
            if (op.CurrentRoom == room) players.Add(player);
        }

        return players;
    }

    public List<Player> GetPlayersInRoom()
    {
        var players = new List<Player>();

        foreach (var player in Scene.Components<MyPlayer>())
        {
            var op = (MyPlayer)player;
            if (op.CurrentRoom == RoomName) players.Add(player);
        }

        return players;
    }
}
