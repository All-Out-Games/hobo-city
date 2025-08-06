using AO;

public class GameTeleporter : Component
{
    public class GameData
    {
        public string GameName;
        public string GameId;
        public Texture Thumbnail;
    }
    public static Dictionary<string, GameData> Games = new()
    {
        { "675ca48b432135705eb966f9", new GameData() { GameName = "Bed Wars", GameId = "675ca48b432135705eb966f9", Thumbnail = Assets.GetAsset<Texture>("thumbnails/bed-wars.png") } },
        { "660dbd382928ad7252a47ec9", new GameData() { GameName = "Murder Mystery", GameId = "660dbd382928ad7252a47ec9", Thumbnail = Assets.GetAsset<Texture>("thumbnails/mm.png") } },
        { "675095a0e905a4db3fee01af", new GameData() { GameName = "Fishermon", GameId = "675095a0e905a4db3fee01af", Thumbnail = Assets.GetAsset<Texture>("thumbnails/fishermon.png") } },
    };

    public void UpdateBannerDisplay()
    {
        var banner = Entity.TryGetChildByName("Banner");
        banner.GetComponent<UIText>().Text = Game.GameName;
        banner.GetComponent<Sprite_Renderer>().Texture = Game.Thumbnail;

        var interactable = Entity.GetComponent<Interactable>();
        if (interactable == null) { Log.Error($"No Interactable component on {Entity.Name}"); return; }
        interactable.Text = "Go to " + Game.GameName + " (leaves game)";

    }

    [Serialized] public string GameId;
    public GameData Game;

    public override void Awake()
    {
        var interactable = Entity.GetComponent<Interactable>();
        interactable.OnInteract += OnInteract;
        interactable.CanUseCallback += (Player p) =>
{
    if (GameId == "none")
    {
        return false;
    }

    var myPlayer = (MyPlayer)p;

    // Check if player is already teleporting
    if (myPlayer.TeleportingToGameId.Value != "none")
    {
        return false;
    }

    // Check if player is on teleport cooldown
    if (myPlayer.IsTeleportOnCooldown)
    {
        return false;
    }

    return true;
};

        var spineAnimator = Entity.GetComponent<Spine_Animator>();
        if (spineAnimator.Alive())
        {
            spineAnimator.Awaken();
            spineAnimator.Entity.Scale = new Vector2(1.25f, 1.25f);
            spineAnimator.SpineInstance.SetAnimation("idle", true);
        }

        if (GameId != "none")
        {
            if (!Games.ContainsKey(GameId))
            {
                Log.Warn($"GameId {GameId} not found in Games dictionary");
                return;
            }

            Game = Games[GameId];
            UpdateBannerDisplay();
        }
    }

    public void OnInteract(Player p)
    {
        if (!Network.IsServer) return;

        var myPlayer = (MyPlayer)p;

        // Check damage cooldown
        if (myPlayer.IsTeleportOnCooldown)
        {
            GameManager.CallClient_SendTargetedMessage($"You can't teleport for {myPlayer.TeleportCooldownRemaining.Value:F1} more seconds after taking damage!", new RPCOptions() { Target = myPlayer });
            return;
        }

        myPlayer.TeleportingToGameId.Set(GameId);
        myPlayer.StartedTeleportingAt.Set(Time.TimeSinceStartup);
        Network.QueueAddPlayer("__TRANSFER:" + GameId, p);
    }
}