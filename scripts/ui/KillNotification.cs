using AO;
using System;
using System.Collections.Generic;

public class KillNotificationData
{
    public string KilledPlayerName;
    public int XPAmount;
    public int MoneyAmount;
    public bool IsAssist;
    public float StartTime;
    public float DisplayedXP; // For lerping
    public float DisplayedMoney; // For lerping
}

public class KillNotification : Component
{
    public static List<KillNotificationData> ActiveNotifications = new List<KillNotificationData>();

    public const float NOTIFICATION_DURATION = 4.0f;
    public const float ANIMATION_IN_DURATION = 0.8f;
    public const float ANIMATION_OUT_DURATION = 0.6f;
    public const float XP_LERP_DURATION = 1.5f;

    public override void Update()
    {
        if (!Network.IsClient) return;

        DrawKillNotifications();

        // Clean up old notifications
        ActiveNotifications.RemoveAll(notif => Time.TimeSinceStartup - notif.StartTime > NOTIFICATION_DURATION);
    }

    public static void ShowKillNotification(string killedPlayerName, int xpAmount, int moneyAmount, bool isAssist = false)
    {
        if (!Network.IsClient) return;

        var notification = new KillNotificationData
        {
            KilledPlayerName = killedPlayerName,
            XPAmount = xpAmount,
            MoneyAmount = Math.Max(0, moneyAmount),
            IsAssist = isAssist,
            StartTime = Time.TimeSinceStartup,
            DisplayedXP = 0,
            DisplayedMoney = 0
        };

        ActiveNotifications.Add(notification);

        // Play kill XP sound effect
        AudioAsset killXpSound = Assets.GetAsset<AudioAsset>("sfx/kill-xp-sound.wav");
        if (killXpSound != null)
        {
            SFX.Play(killXpSound, new SFX.PlaySoundDesc() { Volume = 0.7f });
        }
    }

    // [UIPreview]
    // public static void PreviewKillNotifications()
    // {
    //     // Show example notifications for preview
    //     ActiveNotifications.Clear();
    //     ActiveNotifications.Add(new KillNotificationData
    //     {
    //         KilledPlayerName = "Enemy_Player",
    //         XPAmount = 100,
    //         IsAssist = false,
    //         StartTime = Time.TimeSinceStartup - 0.5f,
    //         DisplayedXP = 0
    //     });

    //     ActiveNotifications.Add(new KillNotificationData
    //     {
    //         KilledPlayerName = "Another_Enemy",
    //         XPAmount = 50,
    //         IsAssist = true,
    //         StartTime = Time.TimeSinceStartup - 1.5f,
    //         DisplayedXP = 0
    //     });

    //     DrawKillNotifications();
    // }

    public static void DrawKillNotifications()
    {
        if (!Network.IsClient) return;

        var baseRect = UI.ScreenRect.TopCenterRect().Offset(0, -175).Grow(400, 80, 400, 0);

        // Draw notifications in reverse order so newest appears on top
        for (int i = ActiveNotifications.Count - 1; i >= 0; i--)
        {
            var notification = ActiveNotifications[i];
            var timeSinceStart = Time.TimeSinceStartup - notification.StartTime;

            // Calculate notification position (newer notifications at top, older ones below)
            var positionIndex = ActiveNotifications.Count - 1 - i;
            var notifRect = baseRect.Offset(0, positionIndex * 100);

            // Animation calculations
            var animationProgress = timeSinceStart / ANIMATION_IN_DURATION;
            var fadeOutProgress = Math.Max(0, (timeSinceStart - (NOTIFICATION_DURATION - ANIMATION_OUT_DURATION)) / ANIMATION_OUT_DURATION);

            // Pop-in animation with overshoot
            var popScale = 1.0f;
            var yOffset = 0f;

            if (animationProgress < 1.0f)
            {
                // Ease out back for pop effect
                var t = Math.Min(1.0f, animationProgress);
                var backEase = EaseOutBack(t);
                popScale = 0.3f + (backEase * 0.7f);
                yOffset = (1.0f - backEase) * 200f; // Slide down from above
            }

            // Fade out animation
            var alpha = 1.0f;
            if (fadeOutProgress > 0)
            {
                alpha = 1.0f - EaseInQuart(fadeOutProgress);
                yOffset += fadeOutProgress * 100f; // Slide up when fading out
            }

            // XP/Money lerping animation
            var xpLerpProgress = Math.Min(1.0f, timeSinceStart / XP_LERP_DURATION);
            notification.DisplayedXP = EaseOutQuart(xpLerpProgress) * notification.XPAmount;
            notification.DisplayedMoney = EaseOutQuart(xpLerpProgress) * notification.MoneyAmount;

            // Apply animations to rect
            var animatedRect = notifRect.Offset(0, yOffset).Scale(popScale);

            using var _layer = UI.PUSH_LAYER(1000); // High layer to appear above everything

            DrawSingleNotification(animatedRect, notification, alpha);
        }
    }

    public static void DrawSingleNotification(Rect rect, KillNotificationData notification, float alpha)
    {
        // Text setup
        var mainTextSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 42,
            Color = new Vector4(1f, 1f, 1f, alpha),
            DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.7f * alpha),
            DropShadowOffset = new Vector2(0f, -4f),
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
            WordWrap = false,
            Outline = true,
            OutlineThickness = 3,
            Offset = new Vector2(0, 10),
        };



        // Main text
        var actionText = notification.IsAssist ? "ASSIST" : "ELIMINATED";
        var fullText = $"{actionText} {notification.KilledPlayerName}";
        UI.TextAsync(rect.Offset(0, 10), fullText, mainTextSettings);

        // XP text with pulsing effect
        var displayedXP = (int)Math.Round(notification.DisplayedXP);
        var xpText = $"+{displayedXP} XP";

        // Add pulsing effect to XP text
        var pulseScale = 1.0f + (float)(Math.Sin(Time.TimeSinceStartup * 8.0f) * 0.1f);

        var xpTextSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 48 * pulseScale,
            Color = notification.IsAssist ? new Vector4(0.0f, 1.0f, 1.0f, alpha) : new Vector4(1.0f, 0.8f, 0.0f, alpha),
            DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.7f * alpha),
            DropShadowOffset = new Vector2(0f, -4f),
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
            WordWrap = false,
            Outline = true,
            OutlineThickness = 4,
            Offset = new Vector2(0, -20),
        };

        // Draw XP slightly left of center
        UI.TextAsync(rect.Offset(-90, -20), xpText, xpTextSettings);

        // Money text with pulsing effect
        var displayedMoney = (int)Math.Round(notification.DisplayedMoney);
        var moneyText = $"+$" + displayedMoney.ToString("N0");

        var moneyTextSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 48 * pulseScale,
            Color = new Vector4(0.3f, 1.0f, 0.3f, alpha),
            DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.7f * alpha),
            DropShadowOffset = new Vector2(0f, -4f),
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
            WordWrap = false,
            Outline = true,
            OutlineThickness = 4,
            Offset = new Vector2(0, -20),
        };

        // Draw Money slightly right of center
        UI.TextAsync(rect.Offset(110, -20), moneyText, moneyTextSettings);

        // Add particle-like effects around the notification
        DrawParticleEffects(rect, notification, alpha);
    }

    public static void DrawParticleEffects(Rect rect, KillNotificationData notification, float alpha)
    {
        var timeSinceStart = Time.TimeSinceStartup - notification.StartTime;

        // Draw floating "+XP" particles
        for (int i = 0; i < 6; i++)
        {
            var particleTime = timeSinceStart - (i * 0.2f);
            if (particleTime < 0 || particleTime > 2.0f) continue;

            // Use deterministic values based on particle index and start time to avoid jittering
            var baseAngle = (notification.StartTime * 1000) % 1000; // Use start time for variation
            var angle = (float)((baseAngle + i * 60) * (Math.PI / 180.0)); // 60 degrees apart
            var baseDistance = 70 + (i * 15); // Stagger distances

            // Animate distance over time for smooth movement
            var distance = baseDistance * particleTime;
            var particleAlpha = alpha * Math.Max(0, 1.0f - (particleTime / 2.0f));

            var particlePos = rect.Center + new Vector2(
              MathF.Cos(angle) * distance,
              MathF.Sin(angle) * distance - (particleTime * 30) // Float upward
            );

            var particleSettings = new UI.TextSettings()
            {
                Font = UI.Fonts.BarlowBold,
                Size = 24,
                Color = notification.IsAssist ? new Vector4(0.0f, 1.0f, 1.0f, particleAlpha) : new Vector4(1.0f, 0.8f, 0.0f, particleAlpha),
                HorizontalAlignment = UI.HorizontalAlignment.Center,
                VerticalAlignment = UI.VerticalAlignment.Center,
                Outline = true,
                OutlineThickness = 2,
            };
            var particleRect = new Rect(particlePos - new Vector2(20, 10), particlePos + new Vector2(20, 10));
            UI.TextAsync(particleRect, "+XP", particleSettings);
        }
    }

    // Easing functions for animations
    public static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * MathF.Pow(t - 1f, 3f) + c1 * MathF.Pow(t - 1f, 2f);
    }

    public static float EaseInQuart(float t)
    {
        return t * t * t * t;
    }

    public static float EaseOutQuart(float t)
    {
        return 1f - MathF.Pow(1f - t, 4f);
    }
}