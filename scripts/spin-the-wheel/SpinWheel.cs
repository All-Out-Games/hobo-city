using AO;

// NOTE: If you have your own ad handler, delete this and integrate the wheel_spin ad id into your own ad handler.
public class AdManagerSystem : System<AdManagerSystem>
{
    public override void Awake()
    {
        Economy.RegisterCurrency("__wheelspins__", "$AO/new/icons/Video.png");

        if (Network.IsServer)
        {
            Ads.SetRewardHandler(OnAdReward);
        }
    }

    private bool OnAdReward(Player player, string adId)
    {
        if (adId == "wheel_spin")
        {
            Economy.DepositCurrency(player, "__wheelspins__", 1);
            return true;
        }

        return false;
    }
}

public class WorldWheelInteractable : Component
{
    public Interactable Interactable;
    public Spine_Animator SpineAnimator;
    [Serialized] public Entity Floaties;

    // Bobbing parameters and caches for each floaty child
    public List<Entity> FloatieEntities = new List<Entity>();
    public List<float> FloatieStartY = new List<float>();
    public List<float> FloatieBobSpeed = new List<float>();
    public List<float> FloatieBobPhase = new List<float>();
    float bobAmount = 0.2f;

    public override void Awake()
    {
        Interactable = GetComponent<Interactable>();
        SpineAnimator = GetComponent<Spine_Animator>();
        SpineAnimator.SpineInstance.SetAnimation("Spin_Loop", true);
        SpineAnimator.SpineInstance.Speed = 0.1f;

        Interactable.OnInteract += OnInteract;
        Interactable.CanUseCallback = (player) => !player.HasEffect<SpinWheelUIEffect>();

        // Initialize bobbing parameters for each child of Floaties
        if (Floaties != null && Floaties.Alive())
        {
            foreach (var child in Floaties.Children)
            {
                if (!child.Alive()) continue;
                FloatieEntities.Add(child);
                FloatieStartY.Add(child.Position.Y);

                var seed = RNG.Seed(child.Id);
                FloatieBobSpeed.Add(RNG.RangeFloat(ref seed, 0.4f, 0.7f));
                FloatieBobPhase.Add(RNG.RangeFloat(ref seed, 0f, (float)Math.PI * 2f));
            }
        }
    }

    public void OnInteract(Player player)
    {
        if (Network.LocalPlayer != player) return;

        player.AddEffect<SpinWheelUIEffect>();
    }

    public override void Update()
    {
        if (!Network.LocalPlayer.Alive()) return;

        // Update bobbing for each floaty child
        for (int i = 0; i < FloatieEntities.Count; i++)
        {
            var e = FloatieEntities[i];
            if (!e.Alive()) continue;

            float bobOffset = (float)Math.Sin((Time.TimeSinceStartup * FloatieBobSpeed[i]) + FloatieBobPhase[i]) * bobAmount;
            Vector2 currentPos = e.Position;
            Vector2 targetPos = new Vector2(currentPos.X, FloatieStartY[i] + bobOffset);
            e.Position = Vector2.Lerp(currentPos, targetPos, Time.DeltaTime * 3.0f);
        }
    }
}

public partial class MyPlayer
{
    // Need to keep this state on the player to prevent cheat redeems etc... just make sure your myplayer class is partial
    public int PendingSliceIndex = -1;

    [ServerRpc]
    public void RequestSpinWheel()
    {
        Log.Info("Requesting spin from server");
        if (!(Game.IsEditor || Game.LaunchedFromEditor) && Economy.GetBalance(this, "__wheelspins__") <= 0)
        {
            Log.Info("No spins");
            return;
        }

        int sliceIndex = SpinWheelUI.GetWeightedRandomSliceIndex();
        PendingSliceIndex = sliceIndex;

        // Send the chosen slice back to the owning client only
        CallClient_StartSpinWheel(sliceIndex, new RPCOptions() { Target = this });
    }

    [ClientRpc]
    public void StartSpinWheel(int sliceIndex)
    {
        SpinWheelUI.StartSpin(sliceIndex);
    }

    [ServerRpc]
    public void ClaimWheelReward(int sliceIndex)
    {
        var player = Network.GetRemoteCallContextPlayer();

        if (sliceIndex != PendingSliceIndex)
        {
            Log.Warn("Invalid reward claim attempt - slice mismatch");
            return;
        }

        // no double-claim
        PendingSliceIndex = -1;

        if (!(Game.IsEditor || Game.LaunchedFromEditor) && Economy.GetBalance(this, "__wheelspins__") > 0)
        {
            Economy.WithdrawCurrency(this, "__wheelspins__", 1);
        }

        CallClient_ClaimWheelRewardClient(player, sliceIndex);
    }

    [ClientRpc]
    public void ClaimWheelRewardClient(Player player, int sliceIndex)
    {
        Log.Info($"Claiming reward for slice {sliceIndex} for player {player.Name}");
        var reward = SpinTheWheelConfig.SliceRewards[sliceIndex];
        reward.OnWon?.Invoke(player);
    }
}

public partial class SpinWheelUIEffect : AEffect
{
    public override bool IsActiveEffect => true;
    public override float DefaultDuration => float.MaxValue;

    public WorldWheelInteractable WorldWheel;
    public float WorldSpinAnimationStartedAt;

    public override void OnEffectStart(bool isDropIn)
    {

    }

    public void TriggerSpinAnimationOnWorldWheel()
    {
        WorldSpinAnimationStartedAt = Time.TimeSinceStartup;
        WorldWheel.GetComponent<Spine_Animator>().SpineInstance.SetAnimation("Spin_End", false);
    }

    public override void OnEffectUpdate()
    {
        SpinWheelUI.Draw();
    }
}

// Confetti instance management class
public class ConfettiInstance
{
    public SpineInstance Instance;
    public Vector2 Position;
    public Vector2 Scale;
    public float StartTime;
    public float Duration;
    public float Rotation;

    public ConfettiInstance(Vector2 position, Vector2 scale, float startTime, float duration, float rotation)
    {
        Instance = SpineInstance.Make();
        Instance.SetSkeleton(Assets.GetAsset<SpineSkeletonAsset>("spin-the-wheel/confetti/confetti_.spine"));
        Instance.SetAnimation("Confetti", false);
        Position = position;
        Scale = scale;
        StartTime = startTime;
        Duration = duration;
        Rotation = rotation;
    }

    public bool IsFinished(float currentTime)
    {
        return currentTime - StartTime >= Duration;
    }
}

public static partial class SpinWheelUI
{
    public enum RewardRarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythic,
    }

    public struct Reward
    {
        public string Name;
        public Texture Icon;
        public RewardRarity Rarity;
        public Action<Player> OnWon;

        public Reward(string name, Texture icon, RewardRarity rarity, Action<Player> onWon)
        {
            Name = name;
            Icon = icon;
            Rarity = rarity;
            OnWon = onWon;
        }
    }

    static float LightChangeInterval = 0.5f;

    static readonly Texture BgTexture = Assets.GetAsset<Texture>("spin-the-wheel/lucky_wheel_background.png");
    static readonly Texture Frame1Texture = Assets.GetAsset<Texture>("spin-the-wheel/lucky_wheel_frame_1.png");
    static readonly Texture Frame2Texture = Assets.GetAsset<Texture>("spin-the-wheel/lucky_wheel_frame_2.png");
    static readonly Texture Slice4Texture = Assets.GetAsset<Texture>("spin-the-wheel/slice_divided_by_4.png");
    static readonly Texture Slice8Texture = Assets.GetAsset<Texture>("spin-the-wheel/slice_divided_by_8.png");
    static readonly Texture PointerTexture = Assets.GetAsset<Texture>("spin-the-wheel/pointer.png");

    static readonly Texture CenterBlobTexture = Assets.GetAsset<Texture>("spin-the-wheel/spin_button.png");


    static readonly UI.TextSettings LabelTs = new UI.TextSettings()
    {
        Font = UI.Fonts.BarlowBold,
        Size = 35f,
        Color = new Vector4(1, 1, 1, 1),
        Outline = true,
        OutlineThickness = 5f,
        HorizontalAlignment = UI.HorizontalAlignment.Center,
        VerticalAlignment = UI.VerticalAlignment.Center,
        DropShadowColor = new Vector4(0f, 0f, 0f, 0.5f),
        DropShadowOffset = new Vector2(0f, -3f),
    };

    static readonly Texture ButtonSprite = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_1.png");
    static readonly Texture WinBGPopupTexture = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_9.png");

    static Vector4 GetRarityColor(RewardRarity rarity)
    {
        return rarity switch
        {
            RewardRarity.Common => new Vector4(0.949f, 0.949f, 1.0f, 1.0f),
            RewardRarity.Rare => new Vector4(0.239f, 0.902f, 1.0f, 1.0f),
            RewardRarity.Epic => new Vector4(0.518f, 0.129f, 1.0f, 1.0f),
            RewardRarity.Legendary => new Vector4(1.0f, 0.776f, 0.055f, 1.0f),
            RewardRarity.Mythic => new Vector4(1.0f, 0.137f, 0.463f, 1.0f),
            _ => Vector4.White
        };
    }

    private static bool isSpinning;
    private static float spinStartTime;
    private static float spinDuration;
    private static float startAngle;
    private static float targetAngle;

    // Wheel expansion animation
    private static bool isAnimatingToFullscreen;
    private static float wheelAnimationStartTime;
    private static float wheelAnimationDuration = 0.4f;

    // Button scale animation
    private static float buttonScaleAnimationStartTime;
    private static bool isButtonScaling;

    // Win popup state
    private static bool showWinPopup;
    private static Reward wonReward;
    private static float popupAnimationStartTime;
    private static float popupAnimationDuration = 0.3f;

    // Confetti state
    private static List<ConfettiInstance> confettiInstances = new List<ConfettiInstance>();
    private static readonly int NUM_CONFETTI = 5;
    private static readonly float BASE_CONFETTI_DURATION = 1.467f;

    private static ulong rngSeed = RNG.Seed(1337);

    // slice chosen by the authoritative server for the current spin
    private static int currentSliceIndex = -1;

    // Rarity percentage display toggle
    private static bool showRarityPercentages = false;

    // ---------------------------------------------

    // Entrance animation (wheel pop-in & button slide)
    private static bool entranceInitialized;
    private static bool isEntranceAnimating = true;
    private static float entranceStartTime;
    private static float entranceDuration = 0.4f;

    public static int GetWeightedRandomSliceIndex()
    {
        float total = 0f;
        foreach (var reward in SpinTheWheelConfig.SliceRewards)
        {
            total += SpinTheWheelConfig.RarityWeights.TryGetValue(reward.Rarity, out var w) ? w : 0f;
        }

        // no div by zero if messed up config
        if (total <= 0f)
        {
            Log.Warn("GetWeightedRandomSliceIndex: No slice found because total is <= 0");
            return RNG.RangeInt(ref rngSeed, 0, SpinTheWheelConfig.SliceRewards.Length);
        }

        float roll = RNG.RangeFloat(ref rngSeed, 0f, total);
        float cumulative = 0f;
        for (int i = 0; i < SpinTheWheelConfig.SliceRewards.Length; i++)
        {
            cumulative += SpinTheWheelConfig.RarityWeights.TryGetValue(SpinTheWheelConfig.SliceRewards[i].Rarity, out var w) ? w : 0f;
            if (roll <= cumulative)
            {
                return i;
            }
        }

        Log.Warn("GetWeightedRandomSliceIndex: No slice found because roll is > cumulative");
        return SpinTheWheelConfig.SliceRewards.Length - 1;
    }

    // Pointer deflection state
    static float _pointerDeflectDuration = 0.15f; // seconds the deflection lasts
    static float _pointerDeflectEndTime;
    static float _pointerMaxDeflectAngle = 18f;   // maximum rotation angle in degrees
    static int _lastTickIndex = -1;

    // Icon scale animation state
    static int _lastSelectedSliceIndex = -1;
    static float[] _iconScales = new float[0]; // Will be initialized based on slice count
    static float _iconScaleAnimationSpeed = 8f; // How fast the scale animation happens

    // [UIPreview]
    public static void Draw()
    {
        if (Network.IsServer) return;

        // Ensure entrance animation is initialised once when UI first draws
        if (!entranceInitialized)
        {
            entranceInitialized = true;
            entranceStartTime = GetTime();
            isEntranceAnimating = true;
        }

        // Automatically end entrance animation after its duration
        if (isEntranceAnimating && (GetTime() - entranceStartTime) >= entranceDuration)
        {
            isEntranceAnimating = false;
        }

        // Background dim
        UI.Image(UI.ScreenRect, ButtonSprite, new Vector4(0, 0, 0, 0.2f));

        UpdateSpin();

        if (showWinPopup)
        {
            DrawWinPopup();
        }
        else
        {
            DrawWheel();
            DrawSpinButton();
        }
    }

    private static void UpdateSpin()
    {
        UpdateWheelAnimation();
        UpdateButtonAnimation();
        UpdateIconScales();

        if (!isSpinning)
        {
            // Continuous slow rotation when not spinning (preview rotation)
            float previewRotationSpeed = 10f; // degrees per second
            startAngle += previewRotationSpeed * Time.DeltaTime;
            startAngle = startAngle % 360f; // Keep angle within 0-360 range
            return;
        }

        var t = (GetTime() - spinStartTime) / spinDuration;
        if (t >= 1f)
        {
            isSpinning = false;
            startAngle = targetAngle % 360f;

            // Show win popup with the server-determined slice
            ShowWinPopup(SpinTheWheelConfig.SliceRewards[currentSliceIndex]);

            return;
        }

        var eased = 1f - MathF.Pow(1f - t, 4f);
        startAngle = targetAngle * eased;

        // pointer deflection
        int tickIndex = (int)((startAngle % 360f) / 90f);
        if (tickIndex != _lastTickIndex)
        {
            _lastTickIndex = tickIndex;
            _pointerDeflectEndTime = GetTime() + _pointerDeflectDuration;
        }
    }

    private static float GetTime()
    {
        return Time.TimeSinceStartup;
    }

    private static void UpdateWheelAnimation()
    {
        if (!isAnimatingToFullscreen)
            return;

        var t = (GetTime() - wheelAnimationStartTime) / wheelAnimationDuration;
        if (t >= 1f)
        {
            isAnimatingToFullscreen = false;
        }
    }

    private static void UpdateButtonAnimation()
    {
        if (!isButtonScaling)
            return;

        var t = (GetTime() - buttonScaleAnimationStartTime) / 0.2f; // Quick animation
        if (t >= 1f)
        {
            isButtonScaling = false;
        }
    }

    private static void UpdateIconScales()
    {
        int sliceCount = SpinTheWheelConfig.SliceRewards.Length;

        // Initialize icon scales array if needed
        if (_iconScales.Length != sliceCount)
        {
            _iconScales = new float[sliceCount];
            for (int i = 0; i < sliceCount; i++)
            {
                _iconScales[i] = 1.0f;
            }
        }

        // Calculate which slice is currently selected
        float currentRotation = (startAngle % 360f + 360f) % 360f;
        float angleFromTop = (180f - currentRotation + 360f) % 360f;
        float sliceAngle = 360f / sliceCount;
        int selectedSliceIndex = (int)((angleFromTop + (sliceAngle * 0.5f)) / sliceAngle) % sliceCount;

        // Smoothly lerp all icon scales
        for (int i = 0; i < sliceCount; i++)
        {
            float targetScale = (i == selectedSliceIndex) ? 1.3f : 1.0f;
            _iconScales[i] = AOMath.Lerp(_iconScales[i], targetScale, Time.DeltaTime * _iconScaleAnimationSpeed);
        }
    }

    private static void DrawWheel()
    {
        // Calculate fade opacity based on entrance animation
        var bgOpacity = 0.8f;
        if (isEntranceAnimating)
        {
            var t = (GetTime() - entranceStartTime) / 0.3f; // 0.3s fade duration
            var eased = 1f - MathF.Pow(1f - MathF.Min(t, 1f), 3f); // Cubic ease out, clamped
            bgOpacity *= eased;
        }

        UI.Image(UI.ScreenRect, null, new Vector4(0, 0, 0, bgOpacity));

        // Animate wheel size and position
        var wheelRect = UI.ScreenRect.CenterRect();

        if (isAnimatingToFullscreen)
        {
            var t = (GetTime() - wheelAnimationStartTime) / wheelAnimationDuration;
            var eased = 1f - MathF.Pow(1f - t, 3f); // Smooth easing

            // Lerp from initial size/position to target
            var growAmount = 400f + (500f - 400f) * eased;
            var offsetY = 100f + (0f - 100f) * eased;

            wheelRect = wheelRect.Grow(growAmount).Offset(0, offsetY);
        }
        else if (isEntranceAnimating)
        {
            // Pop-in from scale 0 to full size
            var t = (GetTime() - entranceStartTime) / entranceDuration;
            var eased = 1f - MathF.Pow(1f - t, 3f); // Ease-out cubic

            var safeScale = MathF.Max(0.01f, MathF.Min(eased, 1f));

            var baseRect = wheelRect.Grow(400).Offset(0, 100);
            wheelRect = baseRect.Scale(safeScale);
        }
        else if (isSpinning)
        {
            // Use full screen size during spinning
            wheelRect = wheelRect.Grow(500).Offset(0, 0);
        }
        else
        {
            // Default size and position
            wheelRect = wheelRect.Grow(400).Offset(0, 100);
        }

        var finalRect = wheelRect.FitAspect(BgTexture.Aspect);

        using (var wheelRotScope = UI.PUSH_ROTATE_ABOUT_POINT(startAngle, finalRect.Center))
        {
            UI.Image(finalRect.Grow(-40), BgTexture);

            DrawSlices(finalRect);

            var currentFrame = ((int)(GetTime() / LightChangeInterval) % 2 == 0) ? Frame1Texture : Frame2Texture;
            UI.Image(finalRect, currentFrame);

            UI.Image(finalRect.CenterRect().Grow(50), CenterBlobTexture);
        }

        DrawPointer(finalRect);

        // Only show close button when not spinning
        if (!isSpinning)
        {
            DrawCloseButton(finalRect);
        }
    }

    private static void DrawSlices(Rect finalRect)
    {
        int sliceCount = SpinTheWheelConfig.SliceRewards.Length;
        float sliceAngle = 360f / sliceCount;

        // Calculate which slice is currently under the pointer
        float currentRotation = (startAngle % 360f + 360f) % 360f;
        float angleFromTop = (180f - currentRotation + 360f) % 360f;
        // Add half a slice angle before flooring to effectively round to the nearest slice.
        int selectedSliceIndex = (int)((angleFromTop + (sliceAngle * 0.5f)) / sliceAngle) % sliceCount;

        for (int i = 0; i < sliceCount; i++)
        {
            float rot = (i * sliceAngle) + 180 - (sliceAngle * 1.5f);
            using var sliceRot = UI.PUSH_ROTATE_ABOUT_POINT(rot, finalRect.Center);

            var radius = finalRect.Width / (sliceCount == 8 ? 3.5f : 2.6f);
            var sliceRect = finalRect.CenterRect().GrowUnscaled(radius, 0, 0, radius * (sliceCount == 4 ? Slice4Texture.Aspect : Slice8Texture.Aspect));

            var sliceColor = GetRarityColor(SpinTheWheelConfig.SliceRewards[i].Rarity);

            // Highlight the currently selected slice
            if (i == selectedSliceIndex)
            {
                // Make the slice glow by increasing its brightness
                sliceColor = new Vector4(
                    MathF.Min(1f, sliceColor.X + 0.4f),
                    MathF.Min(1f, sliceColor.Y + 0.4f),
                    MathF.Min(1f, sliceColor.Z + 0.4f),
                    sliceColor.W
                );
            }

            if (sliceCount == 4)
            {
                UI.Image(sliceRect, Slice4Texture, sliceColor);
            }
            else if (sliceCount == 8)
            {
                UI.Image(sliceRect, Slice8Texture, sliceColor);
            }
            else
            {
                throw new Exception("DrawSlices: Unsupported slice count: " + sliceCount);
            }

            var direction = Vector2.Left;
            direction = Vector2.Rotate(direction, AOMath.ToRadians(sliceCount == 8 ? -45f / 2 : -90f / 2), Vector2.Zero);

            var centerOfSlice = finalRect.CenterRect().Grow(10).OffsetUnscaled(direction.X * radius * (sliceCount == 4 ? 0.65f : 0.9f), direction.Y * radius * (sliceCount == 4 ? 0.65f : 0.9f));


            using var textRotScope = UI.PUSH_ROTATE_ABOUT_POINT(-(rot + startAngle), centerOfSlice.Center);
            if (SpinTheWheelConfig.SliceRewards[i].Icon != null)
            {
                // Use smoothly interpolated scale from the animation system
                var iconScale = (_iconScales.Length > i) ? _iconScales[i] : 1.0f;
                var iconSize = 50 * iconScale;
                var iconRect = centerOfSlice.CenterRect().Grow(iconSize).Offset(0, direction.Y * 50);

                UI.Image(iconRect.FitAspect(SpinTheWheelConfig.SliceRewards[i].Icon.Aspect), SpinTheWheelConfig.SliceRewards[i].Icon);
            }

            UI.TextAsync(centerOfSlice.Offset(0, -42.5f), SpinTheWheelConfig.SliceRewards[i].Name, LabelTs);

            // Show rarity percentage if toggled on
            if (showRarityPercentages)
            {
                // Calculate percentage for this slice
                float totalWeight = 0f;
                foreach (var reward in SpinTheWheelConfig.SliceRewards)
                {
                    totalWeight += SpinTheWheelConfig.RarityWeights.TryGetValue(reward.Rarity, out var w) ? w : 0f;
                }

                float sliceWeight = SpinTheWheelConfig.RarityWeights.TryGetValue(SpinTheWheelConfig.SliceRewards[i].Rarity, out var sliceW) ? sliceW : 0f;
                float percentage = totalWeight > 0f ? (sliceWeight / totalWeight) * 100f : 0f;

                var percentageSettings = new UI.TextSettings()
                {
                    Font = UI.Fonts.BarlowBold,
                    Size = 28f,
                    Color = new Vector4(1, 1, 0.7f, 1),
                    Outline = true,
                    OutlineThickness = 3f,
                    HorizontalAlignment = UI.HorizontalAlignment.Center,
                    VerticalAlignment = UI.VerticalAlignment.Center,
                    DropShadowColor = new Vector4(0f, 0f, 0f, 0.5f),
                    DropShadowOffset = new Vector2(0f, -2f),
                };

                UI.TextAsync(centerOfSlice.Offset(0, -60), $"{percentage:F1}%", percentageSettings);
            }
        }
    }

    private static void DrawSpinButton()
    {
        var buttonSize = 110;
        var buttonRect = UI.ScreenRect.BottomCenterRect().Offset(0, 175).Grow(buttonSize / 2f, buttonSize * 1.5f, buttonSize / 2f, buttonSize * 1.5f);

        // Slide-up entrance animation
        if (isEntranceAnimating)
        {
            float t = (GetTime() - entranceStartTime) / entranceDuration;
            float eased = 1f - MathF.Pow(1f - t, 3f);
            float startYOffset = 400f; // off-screen below
            buttonRect = buttonRect.Offset(0, startYOffset * (1f - eased));
        }

        // Apply button scale animation (used when wheel starts spinning)
        var buttonScale = 1f;
        if (isButtonScaling)
        {
            var t = (GetTime() - buttonScaleAnimationStartTime) / 0.2f;
            var eased = 1f - MathF.Pow(1f - t, 2f); // Quick ease out
            buttonScale = 1f - eased; // Scale from 1 to 0
        }
        else if (isSpinning)
        {
            buttonScale = 0f; // Keep at zero while spinning
        }

        // Scale the button rect after positional adjustments
        buttonRect = buttonRect.Scale(buttonScale);

        var bs = new UI.ButtonSettings()
        {
            Sprite = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_2.png"), // Green button for when they can spin
            PressScaling = 0.25f,
        };

        // Don't show button if it's scaled to zero
        if (buttonScale <= 0f)
        {
            return;
        }

        if (!Game.IsMobile && !(Game.IsEditor || Game.LaunchedFromEditor))
        {
            // desktop can't do ads
            buttonRect = UI.ScreenRect.BottomCenterRect().Offset(0, 175).Grow(buttonSize / 2f, buttonSize * 4f, buttonSize / 2f, buttonSize * 4f);
            buttonRect = buttonRect.Scale(buttonScale);
            UI.Button(buttonRect, "Download on your phone/tablet to spin!", bs, LabelTs);
            return;
        }

        if (!(Game.IsEditor || Game.LaunchedFromEditor) && Economy.GetBalance(Network.LocalPlayer, "__wheelspins__") <= 0)
        {

            var adButtonSettings = new UI.ButtonSettings()
            {
                Sprite = ButtonSprite, // Default button color for ad-based spins
                PressScaling = 0.25f,
            };

            if (Ads.IsRewardedAdLoaded())
            {
                // we have an ad ready to show so they can spin
                if (UI.Button(buttonRect, "SPIN", adButtonSettings, LabelTs).Clicked && isSpinning == false && !isButtonScaling)
                {
                    Ads.PromptRewardedAd("wheel_spin", "Wheel Spin", "Watch an ad to win a prize!", Assets.GetAsset<Texture>("$AO/new/icons/Video.png"));
                }
            }
            else
            {
                // Use a greyed out button when ads aren't available
                var greyButtonSettings = new UI.ButtonSettings()
                {
                    Sprite = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_4.png"), // Grey button for unavailable
                    PressScaling = 0.25f,
                };
                UI.Button(buttonRect, "Ad Unavailable", greyButtonSettings, LabelTs);
            }
        }
        else
        {
            if (UI.Button(buttonRect, "SPIN", bs, LabelTs).Clicked && isSpinning == false && !isButtonScaling)
            {
                RequestSpinFromServer();
            }
        }

    }

    private static void RequestSpinFromServer()
    {
        var player = Network.LocalPlayer as MyPlayer;
        if (player.Alive())
        {
            player.CallServer_RequestSpinWheel();
        }
    }

    public static void StartSpin(int chosenSlice)
    {
        currentSliceIndex = chosenSlice;

        SFX.Play(Assets.GetAsset<AudioAsset>("spin-the-wheel/spin-stop.wav"), new SFX.PlaySoundDesc() { Volume = 0.3f });

        // Start wheel expansion animation
        isAnimatingToFullscreen = true;
        wheelAnimationStartTime = GetTime();

        // Start button scale animation
        isButtonScaling = true;
        buttonScaleAnimationStartTime = GetTime();

        // Start spinning
        isSpinning = true;
        spinStartTime = GetTime();

        // matches the sfx length
        spinDuration = 4f;

        var extraRotations = RNG.RangeInt(ref rngSeed, 3, 6);

        int sliceCount = SpinTheWheelConfig.SliceRewards.Length;
        float sliceAngle = 360f / sliceCount;

        // Ensure we never land too close to the boundary between two slices
        // Keep a safety margin of 25% of the slice angle on both sides of the centre.
        float boundaryBuffer = sliceAngle * 0.25f;
        float maxOffset = (sliceAngle * 0.5f) - boundaryBuffer;
        float withinSliceOffset = RNG.RangeFloat(ref rngSeed, -maxOffset, maxOffset);

        // Align the centre of the chosen slice with the pointer that sits at the very top of the wheel.
        // This means we should rotate the wheel so that slice `chosenSlice` (whose centre is at
        // 180° + (chosenSlice * sliceAngle) in its un-rotated state) ends up at 0°.
        float sliceTargetAngle = 180f - (chosenSlice * sliceAngle) + withinSliceOffset;
        sliceTargetAngle = (sliceTargetAngle % 360f + 360f) % 360f;

        targetAngle = 360f * extraRotations + sliceTargetAngle;

        Log.Info($"Spinning wheel - Chosen slice: {chosenSlice}, Slice angle: {sliceAngle}, Target angle: {sliceTargetAngle}, Final angle with rotations: {targetAngle}");

        _lastTickIndex = (int)((startAngle % 360f) / 90f);
    }

    private static void ShowWinPopup(Reward reward)
    {
        showWinPopup = true;
        wonReward = reward;
        popupAnimationStartTime = GetTime();

        SFX.Play(Assets.GetAsset<AudioAsset>("spin-the-wheel/confetti-shoot.wav"), new SFX.PlaySoundDesc() { Volume = 0.4f });

        // Start confetti animations
        if (Network.IsClient && !Game.IsEditor)
        {
            confettiInstances.Clear();
            var currentTime = GetTime();
            var rngSeed = RNG.Seed((ulong)currentTime);

            for (int i = 0; i < NUM_CONFETTI; i++)
            {
                // Randomize position, scale, and timing for each confetti
                var xOffset = RNG.RangeFloat(ref rngSeed, -0.8f, 0.5f);
                var yOffset = RNG.RangeFloat(ref rngSeed, -0.3f, 0.3f);
                var position = new Vector2(xOffset, yOffset);

                var baseScale = 250f;
                var scale = new Vector2(baseScale, baseScale) * RNG.RangeFloat(ref rngSeed, 0.8f, 1.2f);

                var startDelay = RNG.RangeFloat(ref rngSeed, 0f, 0.3f);
                var duration = BASE_CONFETTI_DURATION + RNG.RangeFloat(ref rngSeed, -0.2f, 0.2f);

                var rotation = RNG.RangeFloat(ref rngSeed, -30f, 30f);

                confettiInstances.Add(new ConfettiInstance(position, scale, currentTime + startDelay, duration, rotation));
            }
        }
    }

    private static void DrawWinPopup()
    {
        // Update and draw confetti
        var currentTime = GetTime();

        // Remove finished confetti instances
        confettiInstances.RemoveAll(c => c.IsFinished(currentTime));

        // Update and draw remaining confetti
        foreach (var confetti in confettiInstances)
        {
            confetti.Instance.Update(Time.DeltaTime);
            confetti.Instance.Speed = 0.75f;

            // Draw confetti in screen space with offset position and rotation
            using var rotScope = UI.PUSH_ROTATE_ABOUT_POINT(confetti.Rotation, UI.ScreenRect.CenterRect().Center);
            var screenAspectScale = new Vector2(UI.ScreenRect.Width / UI.ScreenRect.Height, 1) / new Vector2(1920f / 1080f, 1);
            var drawRect = UI.ScreenRect.CenterRect().Offset(confetti.Position.X * UI.ScreenRect.Width, confetti.Position.Y * UI.ScreenRect.Height);
            UI.DrawSkeleton(drawRect, confetti.Instance, confetti.Scale * screenAspectScale, 0);
        }

        // Animation scale for overall popup
        var t = (GetTime() - popupAnimationStartTime) / popupAnimationDuration;
        var scale = t >= 1f ? 1f : 1f - MathF.Pow(1f - t, 3f); // Ease out cubic

        // Popup container
        var popupSize = 500f;
        var popupRect = UI.ScreenRect.CenterRect().Grow(popupSize * 0.5f * scale);

        // Background panel
        UI.Image(popupRect, WinBGPopupTexture, new Vector4(1, 1, 1, 1));

        // Icon with bobble and pop-in animation
        var iconAnimationDuration = 0.8f; // Longer than popup for dramatic effect
        var iconT = (GetTime() - popupAnimationStartTime) / iconAnimationDuration;

        float iconScale = 1f;
        if (iconT < 1f)
        {
            // Pop-in with overshoot effect (elastic ease-out)
            float elasticT = iconT;
            if (elasticT < 0.5f)
            {
                // First half: scale from 0 to overshoot
                elasticT = elasticT * 2f; // Map to 0-1
                iconScale = 1f - MathF.Pow(1f - elasticT, 3f); // Ease out cubic
                iconScale *= 1.4f; // Overshoot to 140%
            }
            else
            {
                // Second half: settle back to normal size with bounce
                elasticT = (elasticT - 0.5f) * 2f; // Map to 0-1
                var bounce = MathF.Sin(elasticT * MathF.PI * 3f) * (1f - elasticT) * 0.15f; // Decreasing bounce
                iconScale = 1.4f - (0.4f * (1f - MathF.Pow(1f - elasticT, 2f))) + bounce; // Settle from 140% to 100% with bounce
            }
        }

        // Add continuous subtle bobble after initial animation
        if (iconT >= 1f)
        {
            var bobbleTime = GetTime() - popupAnimationStartTime - iconAnimationDuration;
            var bobble = MathF.Sin(bobbleTime * 4f) * 0.05f + 1f; // Gentle 5% scale bobble
            iconScale = bobble;
        }

        var iconSize = 180f * scale * iconScale;
        var iconRect = popupRect.TopCenterRect().Offset(0, -120 * scale).Grow(iconSize * 0.5f);
        UI.Image(iconRect.FitAspect(wonReward.Icon.Aspect), wonReward.Icon);

        // Title text
        var titleRect = popupRect.CenterRect().Offset(0, -30 * scale).Grow(180 * scale, 40 * scale, 180 * scale, 40 * scale);
        var titleSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 42f * scale,
            Color = new Vector4(1, 1, 1, 1),
            Outline = true,
            OutlineThickness = 2f,
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
        };
        UI.TextAsync(titleRect, "You Won!", titleSettings);

        // Item name
        var nameRect = popupRect.CenterRect().Offset(0, 20 * scale).Grow(180 * scale, 30 * scale, 180 * scale, 30 * scale);
        var nameSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 48f * scale,
            Color = new Vector4(1, 1, 1, 1),
            Outline = true,
            OutlineThickness = 2f,
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
        };
        UI.TextAsync(nameRect, wonReward.Name, nameSettings);

        // Claim button
        var buttonRect = popupRect.BottomCenterRect().Offset(0, 100 * scale).Grow(45 * scale, 110 * scale, 45 * scale, 110 * scale);
        var bs = new UI.ButtonSettings()
        {
            Sprite = ButtonSprite,
            PressScaling = 0.1f,
        };

        var buttonTextSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 38f * scale,
            Color = new Vector4(1, 1, 1, 1),
            Outline = true,
            OutlineThickness = 2f,
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
        };

        if (UI.Button(buttonRect, "CLAIM", bs, buttonTextSettings).Clicked)
        {
            ClaimReward();
        }
    }

    private static void ClaimReward()
    {
        var player = Network.LocalPlayer as MyPlayer;
        if (player.Alive())
        {
            player.CallServer_ClaimWheelReward(currentSliceIndex);
        }

        showWinPopup = false;
        confettiInstances.Clear(); // Clean up confetti

        isAnimatingToFullscreen = false;
        isButtonScaling = false;
        entranceInitialized = false; // Reset so next open re-plays entrance animation
    }

    private static void DrawCloseButton(Rect wheelRect)
    {
        var buttonSize = 70f;
        var closeButtonRect = wheelRect.TopRightRect()
            .Offset(-20, -150)
            .Grow(buttonSize * 0.5f, buttonSize * 0.5f, buttonSize * 0.5f, buttonSize * 0.5f);

        var redButtonSprite = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_3.png");
        var redButtonSettings = new UI.ButtonSettings()
        {
            Sprite = redButtonSprite,
            PressScaling = 0.15f,
        };

        var regularButtonSettings = new UI.ButtonSettings()
        {
            Sprite = ButtonSprite,
            PressScaling = 0.15f,
        };

        var closeTextSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 32f,
            Color = new Vector4(1, 1, 1, 1),
            Outline = true,
            OutlineThickness = 2f,
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
        };

        // Close button
        if (UI.Button(closeButtonRect, "X", redButtonSettings, closeTextSettings).Clicked)
        {
            CallServer_CloseSpinWheel();
        }

        // Help/Rarity toggle button
        var helpButtonRect = wheelRect.TopRightRect()
            .Offset(-20, -230)
            .Grow(buttonSize * 0.5f, buttonSize * 0.5f, buttonSize * 0.5f, buttonSize * 0.5f);

        var helpTextSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 32f,
            Color = showRarityPercentages ? new Vector4(1, 1, 0.7f, 1) : new Vector4(1, 1, 1, 1),
            Outline = true,
            OutlineThickness = 2f,
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
        };

        if (UI.Button(helpButtonRect, "?", regularButtonSettings, helpTextSettings).Clicked)
        {
            showRarityPercentages = !showRarityPercentages;
        }
    }

    // Wrapper to get the player who requested the close
    [ServerRpc]
    public static void CloseSpinWheel()
    {
        var player = Network.GetRemoteCallContextPlayer();
        if (player.Alive())
        {
            CallClient_CloseSpinWheel(player);
        }
    }

    // Actually do the close on both clients/server
    [ClientRpc]
    public static void CloseSpinWheel(Player player)
    {
        player.RemoveEffect<SpinWheelUIEffect>(true);
    }

    private static void DrawPointer(Rect finalRect)
    {
        float pointerHeight = 150f;
        float pointerWidth = pointerHeight * PointerTexture.Aspect;

        var pointerRect = finalRect.TopCenterRect()
            .Offset(16, -50f)
            .Grow(pointerWidth * 0.5f, pointerWidth * 0.5f, pointerHeight * 0.5f, pointerHeight * 0.5f);

        float remaining = MathF.Max(0f, _pointerDeflectEndTime - GetTime());
        float ratio = remaining / _pointerDeflectDuration;
        float angle = -_pointerMaxDeflectAngle * ratio;

        using var rotScope = UI.PUSH_ROTATE_ABOUT_POINT(angle, pointerRect.Center);
        UI.Image(pointerRect.FitAspect(PointerTexture.Aspect), PointerTexture);
    }
}