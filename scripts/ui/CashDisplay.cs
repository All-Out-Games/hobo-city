using AO;
using System;

public static class CashDisplay
{
  // Track animation state for money changes
  private static float LastChangeTime = 0f;
  private static int PreviousAmount = 0;
  private static int CurrentAmount = 0;
  private static bool IsAnimating = false;

  // Cache the formatted text to avoid repeated formatting
  private static string CachedFormattedAmount = "0";

  // [UIPreview]
  // public static void DrawMoneyPreview()
  // {
  //   // For preview, show money with animation between two values
  //   var timeLoop = Time.TimeSinceStartup % 5;
  //   int previewAmount = 1000;

  //   if (timeLoop > 3 && timeLoop < 4)
  //   {
  //     // Simulate money change during preview
  //     DrawMoney(previewAmount, previewAmount + 250, timeLoop - 3);
  //   }
  //   else
  //   {
  //     DrawMoney(previewAmount, previewAmount, 0);
  //   }
  // }

  public static void DrawMoney(int amount)
  {
    // Update animation state
    if (amount != CurrentAmount)
    {
      if (amount > PreviousAmount)
      {
        if (Network.IsClient)
        {
          SFX.Play(Assets.GetAsset<AudioAsset>("sfx/get-cash.wav"), new SFX.PlaySoundDesc() { Volume = 0.6f });
        }
      }
      else
      {
        if (Network.IsClient)
        {
          SFX.Play(Assets.GetAsset<AudioAsset>("sfx/purchase.wav"), new SFX.PlaySoundDesc() { Volume = 0.4f });
        }
      }

      PreviousAmount = CurrentAmount;
      CurrentAmount = amount;
      LastChangeTime = Time.TimeSinceStartup;
      IsAnimating = true;

      // Update cached formatted amount when money changes
      CachedFormattedAmount = BBUtil.FormatNumber(amount);
    }

    // Load money icon
    var cashTexture = Assets.GetAsset<Texture>("icons/cash.png");

    // Calculate animation progress (0-1)
    float animProgress = IsAnimating ? Math.Min((Time.TimeSinceStartup - LastChangeTime) / 0.5f, 1.0f) : 1.0f;
    if (animProgress >= 1.0f)
    {
      IsAnimating = false;
    }

    // Create a compact, cohesive container for money display
    var baseRect = UI.SafeRect.TopCenterRect().Offset(270, 0).Inset(30, 30, 10, 10);
    var containerWidth = 260;
    var containerHeight = 100;
    var containerRect = baseRect.Grow(0, 0, containerHeight, containerWidth);

    // Create rounded background with semi-transparent fill
    UI.Image(containerRect, null, new Vector4(0, 0.1f, 0.2f, 0.8f));

    // Create icon and text in a horizontal layout
    var iconSize = containerHeight * 0.5f;
    var iconRect = containerRect.LeftRect().Grow(iconSize).FitAspect(cashTexture.Aspect);

    // Align text to be inside the container - fix positioning
    var textRect = containerRect.CenterRect().Inset(0, 20, 0, iconSize + 10);

    // Animate the coin when money changes
    if (IsAnimating && amount > PreviousAmount)
    {
      // Bounce effect for positive change
      var bounce = 1.0f + 0.2f * (float)Math.Sin(animProgress * Math.PI);
      var bounceRect = iconRect.Scale(bounce);
      UI.Image(bounceRect, cashTexture, Vector4.White);
    }
    else
    {
      // Normal display with a slight wobble
      var wobble = (float)Math.Sin(Time.TimeSinceStartup * 2) * 3;
      using var rotateScope = UI.PUSH_ROTATE_ABOUT_POINT(wobble, iconRect.Center);
      UI.Image(iconRect, cashTexture, Vector4.White);
    }

    // Text settings for the money amount
    var moneyTextSettings = new UI.TextSettings()
    {
      Font = UI.Fonts.BarlowBold,
      Size = 42,
      Color = new Vector4(1f, 1f, 1f, 1f),
      DropShadowColor = new Vector4(0f, 0f, 0.02f, 0.8f),
      DropShadowOffset = new Vector2(2f, -2f),
      HorizontalAlignment = UI.HorizontalAlignment.Center,
      VerticalAlignment = UI.VerticalAlignment.Center,
      WordWrap = false,
      Outline = true,
      OutlineThickness = 3,
    };

    // If money is changing, animate the display
    if (IsAnimating)
    {
      if (amount > PreviousAmount)
      {
        // Money increasing animation
        var displayedAmount = PreviousAmount + (int)((amount - PreviousAmount) * Ease.OutQuart(animProgress));
        UI.TextAsync(textRect, BBUtil.FormatNumber(displayedAmount), moneyTextSettings);

        // Show the increase amount with animation over the player in world space
        if (MyPlayer.localPlayer != null && MyPlayer.localPlayer.Alive())
        {
          // Switch to world space context
          using var worldContext = UI.PUSH_CONTEXT(UI.Context.WORLD);
          // Properly layer the text above the player
          using var zLayer = IM.PUSH_Z(MyPlayer.localPlayer.GetZOffset() - 0.001f);

          var increaseSettings = moneyTextSettings;
          increaseSettings.Size = 0.5f; // World space sizes are in meters
          increaseSettings.Color = new Vector4(0.2f, 1f, 0.2f, 1f - animProgress);
          increaseSettings.HorizontalAlignment = UI.HorizontalAlignment.Center;
          increaseSettings.VerticalAlignment = UI.VerticalAlignment.Center;

          var increaseText = "+" + (amount - PreviousAmount).ToString();

          // Position above the player and move up over time
          var playerPos = MyPlayer.localPlayer.Entity.Position;
          var floatOffset = new Vector2(0, 1.0f + animProgress * 0.8f);
          var textPos = playerPos + floatOffset;

          // Create a rect in world space centered on the player
          var textSize = 1.0f;
          var halfSize = new Vector2(textSize, textSize);
          var textWorldRect = new Rect(textPos - halfSize, textPos + halfSize);

          UI.TextAsync(textWorldRect, increaseText, increaseSettings);
        }
      }
      else if (amount < PreviousAmount)
      {
        // Money decreasing animation
        var displayedAmount = PreviousAmount - (int)((PreviousAmount - amount) * Ease.OutQuart(animProgress));
        UI.TextAsync(textRect, BBUtil.FormatNumber(displayedAmount), moneyTextSettings);

        // Show the decrease amount with animation over the player in world space
        if (MyPlayer.localPlayer != null && MyPlayer.localPlayer.Alive())
        {
          // Switch to world space context
          using var worldContext = UI.PUSH_CONTEXT(UI.Context.WORLD);
          // Properly layer the text above the player
          using var zLayer = IM.PUSH_Z(MyPlayer.localPlayer.GetZOffset() - 0.001f);

          var decreaseSettings = moneyTextSettings;
          decreaseSettings.Size = 0.5f; // World space sizes are in meters
          decreaseSettings.Color = new Vector4(1f, 0.2f, 0.2f, 1f - animProgress);
          decreaseSettings.HorizontalAlignment = UI.HorizontalAlignment.Center;
          decreaseSettings.VerticalAlignment = UI.VerticalAlignment.Center;

          var decreaseText = "-" + (PreviousAmount - amount).ToString();

          // Position above the player and move up over time
          var playerPos = MyPlayer.localPlayer.Entity.Position;
          var floatOffset = new Vector2(0, 1.0f + animProgress * 0.8f);
          var textPos = playerPos + floatOffset;

          // Create a rect in world space centered on the player
          var textSize = 1.0f;
          var halfSize = new Vector2(textSize, textSize);
          var textWorldRect = new Rect(textPos - halfSize, textPos + halfSize);

          UI.TextAsync(textWorldRect, decreaseText, decreaseSettings);
        }
      }
    }
    else
    {
      // Normal display - use cached formatted amount
      UI.TextAsync(textRect, CachedFormattedAmount, moneyTextSettings);
    }
  }
}