using AO;
using System;

public class XPProgressBar : Component
{
    public override void Update()
    {
        if (!Network.IsClient) return;

        DrawXPProgressBar();
    }

    // [UIPreview]
    public static void PreviewXPProgressBar()
    {
        // Create example data for preview
        DrawXPProgressBar(25, 1500, 500, 26);
    }

    public static void DrawXPProgressBar()
    {
        // Get local player data
        var localPlayer = MyPlayer.localPlayer;
        if (localPlayer == null || !localPlayer.Alive()) return;

        var currentLevel = localPlayer.Level;
        var currentXP = localPlayer.XP.Value;
        var xpToNextLevel = localPlayer.XPToNextLevel;
        var nextLevel = currentLevel + 1;

        DrawXPProgressBar(currentLevel, currentXP, xpToNextLevel, nextLevel);
    }

    public static void DrawXPProgressBar(int currentLevel, int currentXP, int xpToNextLevel, int nextLevel)
    {
        // Position at top of screen - using SafeRect to avoid notches on mobile
        // Offset down from top to avoid overlapping with other UI
        var baseRect = UI.SafeRect.TopCenterRect().Offset(0, -40).Grow(15, 300, 15, 300);

        // Background for the entire progress bar area
        var bgRect = baseRect.Grow(10, 50, 10, 50);
        var barRect = baseRect;

        // Background with rounded corners effect
        UI.Image(bgRect, null, new Vector4(0.1f, 0.1f, 0.15f, 0.8f));

        // Calculate progress (0.0 to 1.0)
        var xpForCurrentLevel = MyPlayer.CalculateXPForLevel(currentLevel);
        var xpForNextLevel = MyPlayer.CalculateXPForLevel(nextLevel);
        var xpInCurrentLevel = currentXP - xpForCurrentLevel;
        var xpNeededForLevel = xpForNextLevel - xpForCurrentLevel;
        var progress = xpNeededForLevel > 0 ? (float)xpInCurrentLevel / xpNeededForLevel : 1.0f;
        progress = Math.Clamp(progress, 0.0f, 1.0f);

        // Create the progress bar background
        UI.Image(barRect, null, new Vector4(0.2f, 0.2f, 0.3f, 0.9f));

        // Create the filled portion of the progress bar with gradient effect
        if (progress > 0)
        {
            // Calculate the fill rectangle properly - fill from left to the progress percentage
            var fillWidth = barRect.Width * progress;
            var fillRect = new Rect(barRect.Min, new Vector2(barRect.Min.X + fillWidth, barRect.Max.Y));

            // Add a beautiful gradient effect by drawing multiple layers
            var glowColor = new Vector4(0.3f, 0.8f, 1.0f, 0.8f); // Cyan glow
            var coreColor = new Vector4(0.1f, 0.6f, 0.9f, 0.9f); // Blue core

            // Outer glow
            UI.Image(fillRect.Grow(2), null, new Vector4(glowColor.X, glowColor.Y, glowColor.Z, glowColor.W * 0.3f));
            // Main fill
            UI.Image(fillRect, null, coreColor);
        }

        // Text settings for level numbers
        var levelTextSettings = new UI.TextSettings()
        {
            Font = UI.Fonts.BarlowBold,
            Size = 36,
            Color = new Vector4(1f, 1f, 1f, 1f),
            DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.8f),
            DropShadowOffset = new Vector2(0f, -3f),
            HorizontalAlignment = UI.HorizontalAlignment.Center,
            VerticalAlignment = UI.VerticalAlignment.Center,
            WordWrap = false,
            Outline = true,
            OutlineThickness = 3,
        };

        // Current level (left side)
        var currentLevelRect = bgRect.LeftCenterRect().Offset(-55, 0).Grow(0, 25, 0, 25);
        var currentLevelColor = new Vector4(0.3f, 0.8f, 1.0f, 1.0f); // Cyan for current level
        levelTextSettings.Color = currentLevelColor;
        levelTextSettings.HorizontalAlignment = UI.HorizontalAlignment.Center;
        UI.TextAsync(currentLevelRect, "lvl " + currentLevel.ToString(), levelTextSettings);

        // Next level (right side)
        var nextLevelRect = bgRect.RightCenterRect().Offset(55, 0).Grow(0, 25, 0, 25);
        var nextLevelColor = new Vector4(1.0f, 0.8f, 0.3f, 1.0f); // Golden yellow for next level
        levelTextSettings.Color = nextLevelColor;

        // Check if player is at max level
        if (currentLevel >= 50)
        {
            UI.TextAsync(nextLevelRect, "MAX", levelTextSettings);
        }
        else
        {
            UI.TextAsync(nextLevelRect, "lvl " + nextLevel.ToString(), levelTextSettings);
        }

        // Add subtle decorative elements
        DrawXPProgressDecorations(bgRect, progress, currentLevel);
    }

    public static void DrawXPProgressDecorations(Rect bgRect, float progress, int currentLevel)
    {
        // Add progress indicator diamond at the end of the filled portion
        if (progress > 0 && progress < 1.0f)
        {
            // Calculate position based on the inner bar rect (account for padding)
            var barRect = bgRect.Grow(-10, -50, -10, -50); // Match the barRect from DrawXPProgressBar
            var progressPos = new Vector2(barRect.Min.X + barRect.Width * progress, barRect.Center.Y);
            var diamondSize = 4f;
            var diamondRect = new Rect(
                progressPos - new Vector2(diamondSize, diamondSize),
                progressPos + new Vector2(diamondSize, diamondSize)
            );

            // Pulsing diamond at progress position
            var pulsePower = (float)(Math.Sin(Time.TimeSinceStartup * 4.0) * 0.3 + 0.7);
            var diamondColor = new Vector4(1.0f, 1.0f, 1.0f, pulsePower);
            UI.Image(diamondRect, null, diamondColor);
        }
    }
}