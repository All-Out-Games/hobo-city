using AO;

public class NpcTrigger : Component
{
    public enum ShopType { Gun, General, BlackMarket, Generative }
    [Serialized] public Interactable Interactable;
    [Serialized] public Spine_Animator SpineAnimator;
    [Serialized] public ShopType shopType;
    [Serialized] public bool overrideSkin = false;
    [Serialized] public int colorIndex = 0;
    [Serialized] public Box_Collider StoreAreaCollider;

    float lastPlayerInteractionTime = -1f;
    float lastIdleBreakTime = -1f;
    const float IDLE_BREAK_DELAY = 5f;
    const float IDLE_BREAK_COOLDOWN = 10f;
    MyPlayer currentPlayer;

    public override void Awake()
    {
        Interactable.Awaken();

        Interactable.CanUseCallback = (Player p) =>
        {
            return !UIManager.IsUIActive();
        };

        Interactable.OnInteract = (Player p) =>
        {
            OpenUI((MyPlayer)p);
        };

        if (StoreAreaCollider.Alive())
        {
            StoreAreaCollider.OnCollisionEnter += (other) =>
            {
                if (!Network.IsClient) return;

                var player = other.GetComponent<MyPlayer>();
                if (player != null)
                {
                    currentPlayer = player;
                    lastPlayerInteractionTime = Time.TimeSinceStartup;
                }
            };

            StoreAreaCollider.OnCollisionExit += (other) =>
            {
                if (!Network.IsClient) return;

                var player = other.GetComponent<MyPlayer>();
                if (player != null && currentPlayer == player)
                {
                    currentPlayer = null;
                    lastPlayerInteractionTime = -1f;
                }
            };
        }

        StartRig();
    }

    public void StartRig()
    {
        SpineAnimator.Awaken();
        if (overrideSkin)
        {
            //SpineAnimator.SpineInstance.SetSkeleton(Assets.GetAsset<SpineSkeletonAsset>("Animations/player.merged_spine_rig#output"));
            SpineAnimator.SetCrewchsia(colorIndex);
        }
        var sm = StateMachine.Make();
        SpineAnimator.SpineInstance.SetStateMachine(sm, SpineAnimator.Entity);
        var mainLayer = sm.CreateLayer("main");
        var idleState = mainLayer.CreateState("Idle", 0, true);
        var idleBreakState = mainLayer.CreateState("016ARP/Idle_Break_2", 0, false);

        // Create trigger for idle break animation
        var idleBreakTrigger = sm.CreateVariable("idle_break_2", StateMachineVariableKind.TRIGGER);

        // Setup transitions
        mainLayer.CreateGlobalTransition(idleBreakState).CreateTriggerCondition(idleBreakTrigger);
        mainLayer.CreateTransition(idleBreakState, idleState, true);

        mainLayer.InitialState = idleState;
    }

    public void OpenUI(MyPlayer p)
    {
        if (p.IsLocal)
        {
            Log.Info($"Opening UI for {shopType}");
            //p.StoreEntity = Entity;
            lastPlayerInteractionTime = Time.TimeSinceStartup;
            switch (shopType)
            {
                case ShopType.Gun:
                    UIManager.OpenPositionalUI(() => Store.DrawShop(Store.Instance.gunShop), Position);
                    break;
                case ShopType.General:
                    UIManager.OpenPositionalUI(() => Store.DrawShop(Store.Instance.generalShop), Position);
                    break;
                case ShopType.BlackMarket:
                    UIManager.OpenPositionalUI(() => Store.DrawShop(Store.Instance.blackMarket), Position);
                    break;
                case ShopType.Generative:
                    UIManager.OpenPositionalUI(() => Store.DrawShop(Store.Instance.generateFurnitureShop), Position);
                    break;
            }
        }
    }

    public override void Update()
    {
        if (!Network.IsClient) return;

        // Check if we should play the idle break animation
        if (currentPlayer != null &&
            lastPlayerInteractionTime > 0 &&
            Time.TimeSinceStartup - lastPlayerInteractionTime >= IDLE_BREAK_DELAY &&
            (lastIdleBreakTime < 0 || Time.TimeSinceStartup - lastIdleBreakTime >= IDLE_BREAK_COOLDOWN))
        {
            SpineAnimator.SpineInstance.StateMachine.SetTrigger("idle_break_2");
            lastIdleBreakTime = Time.TimeSinceStartup;
            lastPlayerInteractionTime = Time.TimeSinceStartup;
        }
    }
}