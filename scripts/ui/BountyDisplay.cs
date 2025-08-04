using AO;
using System;

public static class BountyDisplay
{
  // [UIPreview]
  // public static void PreviewBountyDisplay()
  // {
  //   // Example data for preview
  //   var exampleRect = UI.ScreenRect.CenterRect().Grow(100, 150, 20, 150);
  //   DrawBountyReward(exampleRect, 30);
  // }

  public static void DrawBountyReward(Entity player, int bountyAmount, int bountyTier)
  {
    if (!player.GetComponent<MyPlayer>().PVPEnabled.Value) return;

    using var _1 = UI.PUSH_CONTEXT(UI.Context.WORLD);
    using var _2 = IM.PUSH_Z(player.GetComponent<MyPlayer>().GetZOffset() - 0.002f);

    // Position above the player
    var position = player.Position;

    // Create text settings for the bounty reward
    var ts = new UI.TextSettings()
    {
      Font = UI.Fonts.BarlowBold,
      Size = 0.375f,
      Color = new Vector4(1.0f, 0.84f, 0, 1f), // Gold color
      DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.5f),
      DropShadowOffset = new Vector2(0f, -0.01f),
      HorizontalAlignment = UI.HorizontalAlignment.Left,
      VerticalAlignment = UI.VerticalAlignment.Center,
      WordWrap = false,
      Outline = true,
      OutlineThickness = 3,
      Offset = Vector2.Zero
    };

    // Draw the bounty text with a small pulse effect
    if (bountyTier >= 3)
    {
      float scale = 1.0f + MathF.Sin(Time.TimeSinceStartup * 2.0f) * 0.1f;
      ts.Size *= scale;
    }

    // Get the appropriate skull texture based on tier
    var skullTexture = Assets.GetAsset<Texture>($"icons/bounties/skull_{(bountyTier >= 1 ? bountyTier : 1)}.png");

    // Draw skull icon and text side by side
    float bounceAmount = MathF.Sin(Time.TimeSinceStartup * 3.0f) * 0.05f;
    var yOffsetFromNameplate = 2.35f;
    var xOffsetFromNameplate = bountyTier >= 1 ? -0.20f : 0.2f;

    // Skull icon on the left with proper sizing
    float iconSize = 0.35f;
    if (bountyTier >= 3) iconSize = 0.4f;
    if (bountyTier >= 4) iconSize = 0.45f;

    var iconPos = new Vector2((position.X + xOffsetFromNameplate) - 0.25f, position.Y + yOffsetFromNameplate + bounceAmount);
    var iconRect = new Rect(iconPos, iconPos).Grow(iconSize);

    // Use FitAspect to maintain proper aspect ratio of the skull image
    iconRect = iconRect.FitAspect(skullTexture.Aspect);
    UI.Image(iconRect, skullTexture);

    // Add a subtle glow effect for higher tier bounties
    if (bountyTier >= 3)
    {
      float glowPulse = 0.5f + MathF.Sin(Time.TimeSinceStartup * 4.0f) * 0.2f;
      var glowColor = new Vector4(1.0f, 0.4f, 0.0f, glowPulse);
      UI.Image(iconRect.Grow(0.05f), skullTexture, glowColor);
    }

    if (bountyTier <= 0) return;

    // Bounty text on the right
    var textPos = new Vector2((position.X + xOffsetFromNameplate) + 0.55f, position.Y + yOffsetFromNameplate + bounceAmount);
    var textRect = new Rect(textPos, textPos).Grow(0.3f);
    UI.TextAsync(textRect, $"${bountyAmount}", ts);
  }
}