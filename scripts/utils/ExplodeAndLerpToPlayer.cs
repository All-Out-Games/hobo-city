using AO;

public class ExplodeAndLerpToPlayer : Component
{
  [Serialized] public Texture Texture;
  [Serialized] public Player Player;
  [Serialized] public int Count;
  private float explodeStartTime;
  private float explosionDuration = 0.5f;
  private float pauseDuration = 0.1f; // Short pause for dramatic effect
  private float lerpDuration = 0.5f; // Shorter lerp for snappier effect

  public Entity[] Entities;
  private Vector2[] directions;
  private Vector2[] startLerpPositions;
  private float explosionForce = 3.5f; // Reduced from 5f
  private Vector4[] particleColors;
  private float[] rotationSpeeds;

  public override void Awake()
  {
    ulong rngSeed = (ulong)Time.TimeSinceStartup;
    RNG.Seed(1337); // Adding a seed for consistency
    explodeStartTime = Time.TimeSinceStartup;

    if (Count > 25)
    {
      Count = 25;
    }

    Entities = new Entity[Count];
    directions = new Vector2[Count];
    startLerpPositions = new Vector2[Count];
    particleColors = new Vector4[Count];
    rotationSpeeds = new float[Count];

    for (int i = 0; i < Count; i++)
    {
      var entity = Entity.Create();
      var sr = entity.AddComponent<Sprite_Renderer>();
      sr.Texture = Texture;
      entity.SetParent(Entity, false);
      Entities[i] = entity;

      // Generate random explosion direction
      float angle = RNG.RangeFloat(ref rngSeed, 0, 6.28f); // 0 to 2π
      directions[i] = new Vector2(
        (float)System.Math.Cos(angle),
        (float)System.Math.Sin(angle)
      );

      // Random size for variety
      float scale = RNG.RangeFloat(ref rngSeed, 0.7f, 1.3f);
      entity.LocalScale = new Vector2(scale, scale);

      // Random rotation speed
      rotationSpeeds[i] = RNG.RangeFloat(ref rngSeed, 360f, 720f) * (RNG.RangeInt(ref rngSeed, 0, 2) == 0 ? -1 : 1);

      // Randomize colors slightly for visual effect
      float colorVar = RNG.RangeFloat(ref rngSeed, 0.8f, 1.0f);
      particleColors[i] = new Vector4(colorVar, colorVar, colorVar, 1f);

      // Apply initial color
      sr.Tint = particleColors[i];
    }
  }

  public override void Update()
  {
    float timeSinceExplosion = Time.TimeSinceStartup - explodeStartTime;
    float totalDuration = explosionDuration + pauseDuration + lerpDuration - 0.2f;

    if (timeSinceExplosion <= explosionDuration)
    {
      // Explosion phase - particles moving outward
      float explosionProgress = timeSinceExplosion / explosionDuration;
      float easeOutFactor = 1 - (1 - explosionProgress) * (1 - explosionProgress) * (1 - explosionProgress); // Cubic ease out

      for (int i = 0; i < Count; i++)
      {
        if (!Entities[i].Alive()) continue;

        // Calculate position based on explosion direction
        float distance = explosionForce * easeOutFactor;
        Vector2 offset = new Vector2(
          directions[i].X * distance,
          directions[i].Y * distance
        );

        Entities[i].Position = new Vector2(
          Entity.Position.X + offset.X,
          Entity.Position.Y + offset.Y
        );

        // Dynamic rotation based on individual speed
        Entities[i].Rotation += rotationSpeeds[i] * Time.DeltaTime;

        // If this is the end of explosion phase, store positions for lerp
        if (explosionProgress > 0.99f)
        {
          startLerpPositions[i] = Entities[i].Position;
        }
      }
    }
    else if (timeSinceExplosion <= explosionDuration + pauseDuration)
    {
      // Pause phase - just a brief moment before particles start homing
      // This creates a nice anticipation effect

      for (int i = 0; i < Count; i++)
      {
        if (!Entities[i].Alive()) continue;

        // Store positions for lerp phase
        startLerpPositions[i] = Entities[i].Position;

        // Continue rotation during pause
        Entities[i].Rotation += rotationSpeeds[i] * Time.DeltaTime * 0.5f;
      }
    }
    else if (timeSinceExplosion <= totalDuration)
    {
      // Lerp to player phase - more exponential for snappiness
      float lerpProgress = (timeSinceExplosion - explosionDuration - pauseDuration) / lerpDuration;

      // More exponential curve for snappier effect
      float easedProgress = 1 - (1 - lerpProgress) * (1 - lerpProgress) * (1 - lerpProgress) * (1 - lerpProgress); // Quartic ease out

      for (int i = 0; i < Count; i++)
      {
        if (!Entities[i].Alive() || !Player.Alive() || !Player.Entity.Alive())
        {
          continue;
        }

        var sr = Entities[i].GetComponent<Sprite_Renderer>();

        // Lerp from start position to player position
        Vector2 targetPos = Player.Entity.Position;
        Vector2 startPos = startLerpPositions[i];

        Entities[i].Position = new Vector2(
          startPos.X + (targetPos.X - startPos.X) * easedProgress,
          startPos.Y + (targetPos.Y - startPos.Y) * easedProgress
        );

        // Increase rotation speed as approaching player
        Entities[i].Rotation += rotationSpeeds[i] * Time.DeltaTime * (1.0f + lerpProgress * 5.0f);

        // Change color as it gets closer to player - glow effect
        float alpha = 1.0f - (0.5f * easedProgress);
        sr.Tint = new Vector4(
          1.0f + easedProgress, // Increase red component
          1.0f + easedProgress * 0.6f, // Increase green less
          1.0f, // Keep blue the same
          alpha // Fade out slightly
        );
      }
    }
    else
    {
      // Clean up completed particles
      for (int i = 0; i < Count; i++)
      {
        if (Entities[i].Alive())
        {
          Entities[i].Destroy();
        }
      }

      Entity.Destroy();
    }
  }
}
