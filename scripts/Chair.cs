using AO;

public partial class Chair : Component
{
    public Interactable Interactable;
    [Serialized] public Vector2 Offset = new Vector2(0, 0.5f);
    [Serialized] public bool FaceRight = false;
    [Serialized] public bool IsTropical = false;

    public override void Awake()
    {
        Interactable = Entity.GetComponent<Interactable>();

        Interactable.Text = "Sit";
        Interactable.Radius = 1f;
        Interactable.CanUseCallback = (Player player) =>
        {
            if (player.HasEffect<SitEffect>())
            {
                return false;
            }
            return true;
        };

        Interactable.OnInteract += OnInteract;
    }

    void OnInteract(Player player)
    {
        var myPlayer = player as MyPlayer;
        if (myPlayer.Alive())
        {
            var pos = myPlayer.Position;
            if (IsTropical)
            {
                myPlayer.Teleport((Entity.Position + Offset) + new Vector2(0.8f, -0.2f));
            }
            else
            {
                myPlayer.Teleport(Entity.Position + Offset);
            }
            Log.Info($"Setting facing direction to {FaceRight}");
            myPlayer.SetFacingDirection(FaceRight);
            myPlayer.AddEffect<SitEffect>(preInit: eff =>
            {
                eff.StartPosition = pos;
                eff.IsTropical = IsTropical;
            });
        }

        if (Entity.Name == "Picnic")
        {
            if (Network.IsServer)
            {
                EventSystem.FireEvent(GameEventType.HavePicnic, player);
            }
        }
    }
}