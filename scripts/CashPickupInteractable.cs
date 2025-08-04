using AO;

public partial class CashPickupInteractable : Component
{
    public Interactable Interactable;
    public Sprite_Renderer SpriteRenderer;
    public SyncVar<bool> IsShowing = new SyncVar<bool>(true);
    public float RespawnTime = 90f;
    public float HideStartTime;

    public override void Awake()
    {
        Interactable = Entity.GetComponent<Interactable>();
        SpriteRenderer = Entity.GetComponent<Sprite_Renderer>();

        Interactable.OnInteract += OnInteract;
        Interactable.CanUseCallback = (Player _player) =>
        {
            var player = (MyPlayer)_player;
            if (!IsShowing.Value)
            {
                return false;
            }
            return player.HealthManager.Health > 0;
        };

        IsShowing.OnSync += (oldValue, newValue) =>
        {
            SpriteRenderer.Tint = newValue == true ? Vector4.One : Vector4.Zero;
        };
    }

    public override void Update()
    {
        // Only run respawn logic on server
        if (Network.IsServer && !IsShowing.Value)
        {
            if (Time.TimeSinceStartup - HideStartTime >= RespawnTime)
            {
                IsShowing.Set(true);
            }
        }
    }

    public void OnInteract(Player player)
    {
        // Only process on server to avoid duplicate rewards
        if (!Network.IsServer) return;

        // Only allow interaction if showing
        if (!IsShowing.Value) return;

        // Give $45 to the player with visual effect
        Economy.DepositCurrency(player, GameManager.CASH_CURRENCY, 250);

        // Hide the cash pickup and start respawn timer
        IsShowing.Set(false);
        HideStartTime = Time.TimeSinceStartup;
    }
}