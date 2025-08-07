using AO;
using System;

public static class WeaponCooldownIndicator
{
    // Main entry: compute effective cooldown and draw if needed
    public static void DrawForPlayerGun(MyPlayer player)
    {
        if (!Network.IsClient) return;
        if (player == null || !player.Alive()) return;
        if (!player.IsLocal) return;
        if (player.CurrentEquippedItem == null) return;

        var weapon = player.CurrentEquippedItem.CustomDefinition as ReusableWeapons.Weapon;
        if (weapon == null) return;

        // Compute effective cooldown with rarity modifiers
        float baseCooldown = weapon.BaseTimeBetweenShots;
        float effectiveCooldown = weapon.ApplyRarityToCooldown(baseCooldown, player);
        if (effectiveCooldown <= 0.5f) return; // Only show for slower weapons

        float timeSince = Time.TimeSinceStartup - player.LastShootTime;
        float progress = Math.Clamp(timeSince / Math.Max(0.0001f, effectiveCooldown), 0f, 1f);
        if (progress >= 1f) return; // Ready; hide the indicator

        // Position over gun muzzle using the PROJECTILE bone
        var localBone = player.SpineAnimator.SpineInstance.GetBonePosition("PROJECTILE");
        var worldPos = player.Entity.CalculateWorldPosition(localBone);

        // Radius in world meters; larger on mobile for readability
        float radius = Game.IsMobile ? 0.65f : 0.45f;

        DrawCooldownAt(worldPos, progress, radius, player);
    }

    // Pure draw: worldPos and progress in [0,1]
    public static void DrawCooldownAt(Vector2 worldPos, float progress01, float radius, MyPlayer playerForLayering)
    {
        using var _ctx = UI.PUSH_CONTEXT(UI.Context.WORLD);
        if (playerForLayering != null && playerForLayering.Alive())
        {
            using var _z = IM.PUSH_Z(playerForLayering.GetZOffset() - 0.001f);
            InternalDraw(worldPos, progress01, radius);
        }
        else
        {
            InternalDraw(worldPos, progress01, radius);
        }
    }

    static void InternalDraw(Vector2 worldPos, float progress01, float radius)
    {
        progress01 = Math.Clamp(progress01, 0f, 1f);

        // Base faint circle for context
        var circle = Assets.GetAsset<Texture>("$AO/circle.png");
        var hs = new Vector2(radius, radius);
        var backColor = new Vector4(0f, 0f, 0f, 0.35f);
        UI.Image(new Rect(worldPos - hs, worldPos + hs), circle, backColor);

        // Segmented progress ring: light up segments according to progress
        int segments = 24;
        float litSegments = progress01 * segments;
        float segmentThickness = radius * 0.18f;   // radial thickness
        float segmentLength = radius * 0.42f;      // along the radius
        float innerRadius = radius - segmentThickness * 0.5f - 0.02f;

        for (int i = 0; i < segments; i++)
        {
            float t = (i + 1) / (float)segments;
            bool isLit = progress01 >= t;

            float angleDeg = -90f + (360f * (i / (float)segments)); // start at top, clockwise
            float angleRad = angleDeg * (float)(Math.PI / 180.0);
            var dir = new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad));
            var center = worldPos + dir * innerRadius;

            // Rectangle centered at segment center, elongated along dir; rotate to align
            float halfLen = segmentLength * 0.5f;
            float halfThk = segmentThickness * 0.5f;
            var segRect = new Rect(center, center).Grow(halfLen, halfThk, halfLen, halfThk);

            // Color: cyan when lit, dim cyan otherwise
            var litColor = new Vector4(0.2f, 0.9f, 1f, 1f);
            var dimColor = new Vector4(0.2f, 0.9f, 1f, 0.15f);
            var color = isLit ? litColor : dimColor;

            UI.Image(segRect, null, color, default, angleDeg);
        }

        // Sweeping hand to reinforce progress
        float handAngle = -90f + (360f * progress01);
        float handLen = radius * 0.78f;
        float handThk = radius * 0.08f;
        var handRect = new Rect(worldPos, worldPos).Grow(handLen * 0.5f, handThk * 0.5f, handLen * 0.5f, handThk * 0.5f);
        var handColor = new Vector4(1f, 1f, 1f, 0.7f);
        UI.Image(handRect, null, handColor, default, handAngle);

        // Center dot
        float dot = radius * 0.12f;
        UI.Image(new Rect(worldPos - new Vector2(dot, dot), worldPos + new Vector2(dot, dot)), circle, new Vector4(1f, 1f, 1f, 0.9f));
    }

    [UIPreview]
    public static void PreviewWeaponCooldownIndicator()
    {
        // Example: draw at origin with 60% progress
        var pos = new Vector2(0, 0);
        DrawCooldownAt(pos, 0.6f, 0.5f, null);
    }
}


