using AO;

public static class HealthBar
{
  // @Credit: Lookumz
  public static Rect DrawHealthBar(Rect rect, int currentHealth, int maxHealth, string team = "red", Player player = null)
  {
    if (!Network.IsClient) return rect;

    if (!Camera.GetCurrentCameraWorldRect().Overlaps(rect))
    {
      return rect;
    }

    using var _1 = UI.PUSH_CONTEXT(UI.Context.WORLD);
    if (player != null)
    {
      using var _2 = IM.PUSH_Z(player.GetZOffset() - 0.0001f); // minus an epsilon so the health bar draws over the entity
      using var _4 = UI.PUSH_PLAYER_MATERIAL(player);
    }
    else
    {
      using var _2 = IM.PUSH_Z(1000000000);
    }
    using var _3 = UI.PUSH_SCALE_FACTOR(5.0f / 540.0f);

    var healthRect = rect.TopCenterRect().Offset(0, 5);
    healthRect = healthRect.Grow(13, 70, 0, 70).Offset(0, 0);
    var borderRect = healthRect.Grow(5.5f, 4, 5.5f, 4).Offset(0, -2);

    var back = team == "red" ? References.HealthBarBack_RedTeam : References.HealthBarBack_BlueTeam;
    var fill = team == "red" ? References.HealthBarFill_RedTeam : References.HealthBarFill_BlueTeam;
    var pip = team == "red" ? References.Pip_RedTeam : References.Pip_BlueTeam;

    // Draw bar background
    UI.Image(borderRect, back, Vector4.White, new UI.NineSlice());

    // Draw health percentage
    var healthPercent = currentHealth / (float)maxHealth;
    var healthPercentRect = healthRect.SubRect(0, 0, healthPercent, 1, 0, 0, 0, 0);
    UI.Image(healthPercentRect, fill, Vector4.White, new UI.NineSlice());
    DrawPipOnHealthBar(healthRect, currentHealth, maxHealth, 6, pip, 0.05f, 0.05f, Vector4.White);

    return healthRect;
  }

  public static void DrawPipOnHealthBar(Rect barRect, float currentHealth, float maxHealth, int totalPips, Texture pipTexture, float leftOffset, float rightOffset, Vector4 color, float shrinkFactor = 0.3f)
  {
    float healthPerPip = maxHealth / (float)totalPips;

    float totalOffset = leftOffset + rightOffset;
    float availableWidth = 1.0f - totalOffset;  // Total width available for all pips
    float pipWidth = availableWidth / totalPips; // Width of each pip

    for (int i = 0; i < totalPips; i++)
    {
      // Calculate the threshold for this pip
      float pipThreshold = (i + 1) * healthPerPip;

      float xMin = leftOffset + i * pipWidth; // Start after the left offset for all pips
      float xMax = leftOffset + (i + 1) * pipWidth; // Extend by pipWidth

      // Only draw the pip if the current health is greater than the threshold for this pip
      if (currentHealth >= pipThreshold)
      {
        float newWidth = pipWidth * shrinkFactor;

        // Center the smaller pip rectangle
        float newXMin = xMin + (pipWidth - newWidth) / 2;
        float newXMax = xMax - (pipWidth - newWidth) / 2;

        var smallerPipRect = barRect.SubRect(newXMin, 0, newXMax, 1, 0, 0, 0, 0);
        UI.Image(smallerPipRect, pipTexture, color, new UI.NineSlice());
      }
    }
  }

}