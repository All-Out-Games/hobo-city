using AO;
using System;

public partial class GunButtonsUI
{
    // Static asset references
    static Texture ButtonGrey = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_8.png");
    static Texture ButtonGreen = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_2.png");
    static Texture ButtonPink = Assets.GetAsset<Texture>("$AO/new/modal/buttons_2/button_7.png");
    static Texture GunIcon = Assets.GetAsset<Texture>("icons/weapons/pistol.png");
    static Texture ForgeIcon = Assets.GetAsset<Texture>("forge-icon.png");
    static AudioAsset ButtonClickSound = Assets.GetAsset<AudioAsset>("sfx/button-click.wav");
    static AudioAsset ButtonClickSpecialSound = Assets.GetAsset<AudioAsset>("sfx/button-click-special.wav");

    static UI.TextSettings SidebarLabelSettings = new UI.TextSettings()
    {
        Font = UI.Fonts.BarlowBold,
        Size = 28,
        Color = Vector4.White,
        HorizontalAlignment = UI.HorizontalAlignment.Center,
        VerticalAlignment = UI.VerticalAlignment.Top,
        DropShadow = true,
        DropShadowColor = new Vector4(0, 0, 0, 0.6f),
        DropShadowOffset = new Vector2(0, -2),
        Outline = true,
        OutlineThickness = 2,
    };

    // [UIPreview]
    // public static void PreviewGunButtons()
    // {
    //     DrawSidebarButtons();
    // }

    public static void DrawSidebarButtons()
    {
        if (!Game.IsEditor && (!Network.IsClient || MyPlayer.localPlayer == null)) return;

        // Don't show buttons in certain states
        if (MyPlayer.localPlayer != null)
        {
            if (MyPlayer.localPlayer.HealthManager.Health <= 0) return;
            if (MyPlayer.localPlayer.InventoryOpen) return;
        }

        using var _1 = UI.PUSH_LAYER(10000000);

        // Check if player is shooting or recently shot
        bool shootingDisable = false;
        bool teleportDisable = false;
        if (!Game.IsEditor && MyPlayer.localPlayer != null)
        {
            bool recentlyShot = (MyPlayer.localPlayer.LastShootTime > 0) && (Time.TimeSinceStartup - MyPlayer.localPlayer.LastShootTime <= 3f);
            shootingDisable = MyPlayer.localPlayer.IsShooting || recentlyShot;
            teleportDisable = MyPlayer.localPlayer.IsTeleportOnCooldown;
        }
        bool anyDisable = shootingDisable || teleportDisable;
        float buttonAlpha = anyDisable ? 0.2f : 1f;

        var buttonSize = 120f;
        var spacing = 100f;

        // Position buttons
        Rect gunStoreRect;
        Rect gunForgeRect;

        // Desktop: vertical stack on the left
        var baseRect = UI.SafeRect.LeftCenterRect().Offset(90, -150).Grow(50, 75, 50, 75);
        gunStoreRect = baseRect.Inset(5);
        gunForgeRect = baseRect.Offset(0, 100).Inset(5);

        // Gun Store Button
        var gunStoreButtonSettings = new UI.ButtonSettings()
        {
            Sprite = ButtonGreen,
            PressScaling = anyDisable ? 0f : 0.95f,
            ColorMultiplier = new Vector4(1f, 1f, 1f, buttonAlpha),
            Slice = new UI.NineSlice() { slice = new Vector4(45, 45, 45, 45), sliceScale = 0.8f }
        };

        using (var id = UI.PUSH_ID("gun_store"))
        {
            var result = UI.Button(gunStoreRect, "", gunStoreButtonSettings, new UI.TextSettings());

            // Draw icon
            var iconRect = gunStoreRect.Inset(15, 15, 15, 15);
            UI.Image(iconRect.FitAspect(1f), GunIcon, new Vector4(1f, 1f, 1f, buttonAlpha));

            // Draw label
            var labelRect = gunStoreRect.Offset(0, -buttonSize / 2 - 5f);
            UI.TextAsync(labelRect, "Gun Store", SidebarLabelSettings);

            if (!anyDisable && result.Clicked)
            {
                SFX.Play(ButtonClickSound, new SFX.PlaySoundDesc() { Volume = 0.4f, SpeedPerturb = 0.15f });
                TeleportToArea(LandmarkArea.GunStore);
            }
        }

        // Gun Forge Button
        var gunForgeButtonSettings = new UI.ButtonSettings()
        {
            Sprite = ButtonPink, // Pink for special/premium look
            PressScaling = anyDisable ? 0f : 0.95f,
            ColorMultiplier = new Vector4(1f, 1f, 1f, buttonAlpha),
            Slice = new UI.NineSlice() { slice = new Vector4(45, 45, 45, 45), sliceScale = 0.8f }
        };

        using (var id = UI.PUSH_ID("gun_forge"))
        {
            var result = UI.Button(gunForgeRect, "", gunForgeButtonSettings, new UI.TextSettings());

            // Draw icon
            var iconRect = gunForgeRect.Inset(15, 15, 15, 15);
            UI.Image(iconRect.FitAspect(1f), ForgeIcon, new Vector4(1f, 1f, 1f, buttonAlpha));

            // Draw label
            var labelRect = gunForgeRect.Offset(0, -buttonSize / 2 - 5f);
            UI.TextAsync(labelRect, "Forge", SidebarLabelSettings);

            if (!anyDisable && result.Clicked)
            {
                SFX.Play(ButtonClickSpecialSound, new SFX.PlaySoundDesc() { Volume = 0.6f });
                TeleportToArea(LandmarkArea.Forge);
            }
        }
    }

    static void TeleportToArea(LandmarkArea targetArea)
    {
        if (MyPlayer.localPlayer == null) return;

        // Check if player can teleport
        if (MyPlayer.localPlayer.HealthManager.Health <= 0)
        {
            Notifications.Show("You can't teleport while dead");
            return;
        }

        // Check damage cooldown
        if (MyPlayer.localPlayer.IsTeleportOnCooldown)
        {
            Notifications.Show($"You can't teleport for {MyPlayer.localPlayer.TeleportCooldownRemaining.Value:F1} more seconds after taking damage!");
            return;
        }

        // Find the area collider
        AreaCollider targetAreaCollider = null;
        foreach (var areaCollider in Scene.Components<AreaCollider>())
        {
            if (areaCollider.AreaName == targetArea)
            {
                targetAreaCollider = areaCollider;
                break;
            }
        }

        if (targetAreaCollider == null)
        {
            Notifications.Show($"Could not find {AreaCollider.GetDisplayName(targetArea)} area");
            return;
        }

        CallServer_ServerTeleportToArea(targetAreaCollider.Entity.Position);
    }

    [ServerRpc]
    public static void ServerTeleportToArea(Vector2 position)
    {
        var player = Network.GetRemoteCallContextPlayer();
        if (player == null) return;

        var myPlayer = player as MyPlayer;
        if (myPlayer == null) return;

        // Check damage cooldown on server side too
        if (myPlayer.IsTeleportOnCooldown)
        {
            GameManager.CallClient_SendTargetedMessage($"You can't teleport for {myPlayer.TeleportCooldownRemaining.Value:F1} more seconds after taking damage!", new RPCOptions() { Target = myPlayer });
            return;
        }

        player.Teleport(position);
        if (!myPlayer.HasEffect<InvulnerabilityEffect>())
        {
            myPlayer.CallClient_SetInvulnerable(true, false);
        }
    }
}
