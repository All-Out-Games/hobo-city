using AO;
using System;

public static class WastedUI
{
  // Animation parameters
  public static float fadeInDuration = 0.8f;
  public static float wastedTextScale = 2.0f;
  public static Vector4 wastedColor = new Vector4(0.8f, 0.1f, 0.1f, 1.0f); // Red color
  public static Vector4 moneyLostColor = new Vector4(1.0f, 0.84f, 0, 1.0f); // Gold color

  // Audio handle for wasted sound
  public static ulong wastedSoundHandle = 0;

  public static float startedAt = -1f;
  public static int moneyLost = 0; // Track how much money was lost
  public static string selectedTip = ""; // Store the selected tip

  // Array of tips to display
  public static string[] tips = new string[]
  {
    "Police chasing you? Hide behind an object to escape",
    "Need cash? Set up a Bitcoin miner in your housey",
    "Completing jobs around the city is an easy way to make money",
    "Buy food and drinks for more health!",
    "Be careful out there!",
    "Police will chase when you hurt players or destroy things!"
  };

  // [UIPreview]
  public static void Show(int lostAmount = 0)
  {
    startedAt = Time.TimeSinceStartup;
    moneyLost = lostAmount;

    // Select a random tip
    ulong rngSeed = RNG.Seed((ulong)(Time.TimeSinceStartup * 1000));
    int tipIndex = RNG.RangeInt(ref rngSeed, 0, tips.Length - 1);
    selectedTip = tips[tipIndex];
  }

  [UIPreview]
  public static void Draw()
  {
    if (startedAt == -1f) return;

    // Play sound effect when wasted UI first appears
    if (Time.TimeSinceStartup - startedAt < 0.1f && wastedSoundHandle == 0)
    {
      wastedSoundHandle = SFX.Play(Assets.GetAsset<AudioAsset>("sfx/wasted.wav"), new SFX.PlaySoundDesc() { Volume = 0.8f });
    }

    // Calculate animation progress (0 to 1)
    float fadeInProgress = Math.Min(1.0f, (Time.TimeSinceStartup - startedAt) / fadeInDuration);

    // Desaturate and darken the screen with a fade-in effect
    var screenRect = UI.ScreenRect;

    // Draw full-screen darkening overlay
    float overlayAlpha = 0.5f * fadeInProgress;
    UI.Image(screenRect, null, new Vector4(0, 0, 0, overlayAlpha));

    // Calculate wasted text animation
    float textProgress = Math.Max(0, fadeInProgress - 0.3f) / 0.7f; // Start text slightly after overlay

    if (textProgress > 0)
    {
      // Scale up animation for the text
      float currentScale = 2.0f + (wastedTextScale - 1.0f) * (1.0f - Ease.OutQuart(textProgress));

      using var _ = UI.PUSH_SCALE_FACTOR(currentScale);

      // Center text rect
      var wastedTextRect = UI.ScreenRect.CenterRect();

      // Text settings for "WASTED" text
      var wastedTextSettings = new UI.TextSettings()
      {
        Font = UI.Fonts.BarlowBold,
        Size = 80,
        Color = new Vector4(wastedColor.X, wastedColor.Y, wastedColor.Z, textProgress), // Fade in the color
        HorizontalAlignment = UI.HorizontalAlignment.Center,
        VerticalAlignment = UI.VerticalAlignment.Center,
        DropShadowColor = new Vector4(0f, 0f, 0f, 0.7f * textProgress),
        DropShadowOffset = new Vector2(3f, -3f),
        Outline = true,
        OutlineThickness = 3,
      };

      // Add a subtle shake effect
      ulong rngSeed = (ulong)(Time.TimeSinceStartup * 1000);
      RNG.Seed(rngSeed);
      float shakeIntensity = 5.0f * (1.0f - textProgress); // Shake intensity decreases as animation progresses
      float shakeX = RNG.RangeFloat(ref rngSeed, -shakeIntensity, shakeIntensity);
      float shakeY = RNG.RangeFloat(ref rngSeed, -shakeIntensity, shakeIntensity);

      wastedTextRect = wastedTextRect.Offset(shakeX, shakeY);

      // Draw the "WASTED" text
      UI.TextAsync(wastedTextRect, "WASTED", wastedTextSettings);

      // Display money lost if PVP was enabled
      if (moneyLost > 0)
      {
        var moneyLostRect = wastedTextRect.CutBottom(120);

        // Add a pulsing effect to the money lost text
        float pulseScale = 1.0f + MathF.Sin(Time.TimeSinceStartup * 4.0f) * 0.1f;

        var moneyLostSettings = new UI.TextSettings()
        {
          Font = UI.Fonts.BarlowBold,
          Size = 50 * pulseScale,
          Color = new Vector4(moneyLostColor.X, moneyLostColor.Y, moneyLostColor.Z, textProgress),
          HorizontalAlignment = UI.HorizontalAlignment.Center,
          VerticalAlignment = UI.VerticalAlignment.Center,
          DropShadowColor = new Vector4(0f, 0f, 0f, 0.5f * textProgress),
          DropShadowOffset = new Vector2(2f, -2f),
          Outline = true,
          OutlineThickness = 2,
        };

        UI.TextAsync(moneyLostRect, $"-${moneyLost:N0}", moneyLostSettings);
      }

      // Display respawn countdown
      if (textProgress >= 1.0f)
      {
        var countdownTime = Math.Max(0, MyPlayer.RESPAWN_TIME - (Time.TimeSinceStartup - startedAt - fadeInDuration));
        var countdownRect = wastedTextRect.CutBottomUnscaled(75 + (moneyLost > 0 ? 180 : 100));

        var countdownSettings = new UI.TextSettings()
        {
          Font = UI.Fonts.BarlowBold,
          Size = 40,
          Color = Vector4.White,
          HorizontalAlignment = UI.HorizontalAlignment.Center,
          VerticalAlignment = UI.VerticalAlignment.Center,
          DropShadowColor = new Vector4(0f, 0f, 0f, 0.5f),
          DropShadowOffset = new Vector2(2f, -2f),
          Outline = true,
          OutlineThickness = 2,
        };

        UI.TextAsync(countdownRect, $"RESPAWNING IN {countdownTime:F1}s", countdownSettings);

        // Display random tip below the countdown
        var tipRect = countdownRect.Offset(0, 100);

        var tipSettings = new UI.TextSettings()
        {
          Font = UI.Fonts.BarlowBold,
          Size = 32,
          Color = Vector4.White,
          HorizontalAlignment = UI.HorizontalAlignment.Center,
          VerticalAlignment = UI.VerticalAlignment.Center,
          DropShadowColor = new Vector4(0f, 0f, 0f, 0.3f),
          DropShadowOffset = new Vector2(1f, -1f),
          Outline = false,
        };

        UI.TextAsync(tipRect, $"{selectedTip}", tipSettings);
      }
    }
  }

  // Call this method when player gets revived
  public static void Reset()
  {
    startedAt = -1f;
    moneyLost = 0;
    selectedTip = "";

    if (wastedSoundHandle != 0)
    {
      SFX.Stop(wastedSoundHandle);
      wastedSoundHandle = 0;
    }
  }
}