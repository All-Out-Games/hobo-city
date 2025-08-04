using AO;
using System;
using System.Linq;

public partial class PresidentAimingUI
{
    public struct TargetActionResult
    {
        public bool FireClicked;
        public bool SkipClicked;
    }

    public static bool nukeUsed = false;
    static float targetUIStartTime = -1f;
    static float nukeUIStartTime = 0f;

    static Texture ButtonRed = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_3.png"); // red
    static Texture ButtonGreen = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_2.png"); // green
    static Texture ButtonOrange = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_1.png"); // orange
    static Texture NukeIcon = Assets.GetAsset<Texture>("nuke-icon.png");

    static UI.TextSettings labelSettings = new UI.TextSettings()
    {
        Font = UI.Fonts.BarlowBold,
        Size = 48,
        Color = Vector4.White,
        HorizontalAlignment = UI.HorizontalAlignment.Center,
        VerticalAlignment = UI.VerticalAlignment.Center,
        Outline = true,
        OutlineThickness = 3,
    };

    static UI.TextSettings buttonTextSettings = new UI.TextSettings()
    {
        Font = UI.Fonts.BarlowBold,
        Size = 42,
        Color = Vector4.White,
        HorizontalAlignment = UI.HorizontalAlignment.Center,
        VerticalAlignment = UI.VerticalAlignment.Center,
        Outline = true,
        OutlineThickness = 3,
    };

    // Helper function to kill target and create explosion
    [ServerRpc]
    public static void KillTargetWithExplosion(MyPlayer target)
    {
        CallClient_KillTargetWithExplosionClient(target);
    }

    // Helper function to send spare notification
    [ServerRpc]
    public static void SendSpareNotification(MyPlayer target, string presidentName)
    {
        GameManager.CallClient_SendTargetedMessage($"You were spared by president {presidentName}", new RPCOptions() { Target = target });
    }

    [ClientRpc]
    public static void KillTargetWithExplosionClient(MyPlayer target)
    {
        if (!target.Alive()) return;

        // Kill the target
        target.HealthManager.Health = 0;

        // Create explosion effect
        var particlePrefab = Assets.GetAsset<Prefab>("RuntimeParticle.prefab");
        SFX.Play(Assets.GetAsset<AudioAsset>("sounds/reusable-weapons/grenade_explode.wav"), new SFX.PlaySoundDesc()
        {
            Volume = 0.5f,
            Position = target.Entity.Position,
            Positional = true,
        });
        var entity = particlePrefab.Instantiate(onBeforeAwake: (entity) =>
        {
            var particle = entity.GetComponent<Particle>();
            entity.Position = target.Entity.Position;
            particle.SkeletonAsset = Assets.GetAsset<SpineSkeletonAsset>("rigs/explosion/014ANT_Big_Explosion.spine");
            particle.AnimationName = "Oblivion_Explosion";
            particle.Duration = 1.9f;
        });
    }

    public static TargetActionResult DrawTargetActions(MyPlayer target, string presidentName)
    {
        // Initialize start time if not set
        if (targetUIStartTime < 0)
        {
            targetUIStartTime = Time.TimeSinceStartup;
        }

        // Don't show UI for 1 second after it's added
        if (Time.TimeSinceStartup - targetUIStartTime < 1f)
        {
            return new TargetActionResult()
            {
                FireClicked = false,
                SkipClicked = false,
            };
        }

        using var _layer = UI.PUSH_LAYER(1000000);
        // Dim background
        UI.Image(UI.ScreenRect, null, new Vector4(0, 0, 0, 0.4f));

        var centerRect = UI.ScreenRect.CenterRect().Grow(150, 250, 150, 250);
        var imageRect = centerRect.Offset(0, 25).Inset(45).FitAspect(Assets.GetAsset<Texture>("$AO/new/Ability Buttons/v2/small_ability_button_aim_pressed.png").Aspect);
        {
            using var _rotate = UI.PUSH_ROTATE_ABOUT_POINT(Time.TimeSinceStartup * 180f, imageRect.Center);
            UI.Image(imageRect, Assets.GetAsset<Texture>("$AO/new/Ability Buttons/v2/small_ability_button_aim_pressed.png"), new Vector4(0.05f, 0.05f, 0.05f, 0.9f));
        }

        var titleRect = centerRect.TopCenterRect().Offset(0, 50);
        UI.TextAsync(titleRect, $"TARGET: {target.Name.ToUpper()}", labelSettings);

        // Buttons area
        var buttonWidth = 300f;
        var buttonHeight = 120f;
        var spacing = 75f;

        var fireRect = centerRect.BottomCenterRect().Offset(-buttonWidth / 2 - spacing / 2, -100).Grow(buttonHeight / 2, buttonWidth / 2, buttonHeight / 2, buttonWidth / 2);
        var skipRect = centerRect.BottomCenterRect().Offset(buttonWidth / 2 + spacing / 2, -100).Grow(buttonHeight / 2, buttonWidth / 2, buttonHeight / 2, buttonWidth / 2);

        // FIRE button
        var fireButtonSettings = new UI.ButtonSettings()
        {
            Sprite = ButtonRed,
            PressScaling = 0.95f,
            ColorMultiplier = Vector4.White
        };

        var skipButtonSettings = new UI.ButtonSettings()
        {
            Sprite = ButtonGreen,
            PressScaling = 0.95f,
            ColorMultiplier = Vector4.White
        };

        bool fireClicked;
        bool skipClicked;

        using (var _id1 = UI.PUSH_ID("fire_button"))
        {
            fireClicked = UI.Button(fireRect, "FIRE", fireButtonSettings, buttonTextSettings).Clicked;
            if (fireClicked)
            {
                CallServer_KillTargetWithExplosion(target);
            }
        }
        using (var _id2 = UI.PUSH_ID("skip_button"))
        {
            skipClicked = UI.Button(skipRect, "SPARE", skipButtonSettings, buttonTextSettings).Clicked;
            if (skipClicked)
            {
                CallServer_SendSpareNotification(target, presidentName);
            }
        }

        return new TargetActionResult()
        {
            FireClicked = fireClicked,
            SkipClicked = skipClicked,
        };
    }

    public static bool DrawNukeButton()
    {
        // If nuke has already been used, don't show the button
        if (nukeUsed)
        {
            return false;
        }

        // Initialize start time if not set
        if (nukeUIStartTime < 0)
        {
            nukeUIStartTime = Time.TimeSinceStartup;
        }

        // Don't show UI for 1 second after it's added
        if (Time.TimeSinceStartup - nukeUIStartTime < 1f)
        {
            return false;
        }

        using var _layer = UI.PUSH_LAYER(1000000);

        var buttonSize = 400f;
        var buttonRect = UI.ScreenRect.BottomCenterRect().Offset(0, 250).Grow(buttonSize / 3, buttonSize, buttonSize / 3, buttonSize);

        var nukeButtonSettings = new UI.ButtonSettings()
        {
            Sprite = ButtonOrange,
            PressScaling = 0.9f,
            ColorMultiplier = Vector4.White
        };

        bool clicked;
        using (var _id3 = UI.PUSH_ID("nuke_button"))
        {
            clicked = UI.Button(buttonRect, "NUKE", nukeButtonSettings, buttonTextSettings).Clicked;
        }

        // Add nuke icon to the button
        var iconSize = 35f;
        var iconRect = buttonRect.Offset(-60, 0).Grow(iconSize / 2).FitAspect(NukeIcon.Aspect);
        UI.Image(iconRect, NukeIcon, Vector4.White);

        // If button was clicked, mark nuke as used
        if (clicked)
        {
            nukeUsed = true;
        }

        return clicked;
    }

    // ---------------- PREVIEW -----------------
    // [UIPreview]
    public static void PreviewTargetUI()
    {
        // Create mock player for preview
        var mockPlayer = Scene.Components<MyPlayer>().FirstOrDefault(); // might be null in editor but fine
        if (mockPlayer == null)
        {
            return;
        }
        DrawTargetActions(mockPlayer, "TestPresident");
    }

    // [UIPreview]
    public static void PreviewNukeUI()
    {
        DrawNukeButton();
    }
}