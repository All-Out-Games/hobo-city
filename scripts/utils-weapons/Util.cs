using AO;
using System.Collections;

public static class CustomUtil
{
    public static Vector2 CalculateWorldPosition(this Entity entity, Vector2 localPosition)
    {
        Vector2 parentPosition = entity.Position;
        float parentRotation = entity.Rotation;
        Vector2 parentScale = entity.Scale;

        float scaledX = localPosition.X * parentScale.X;
        float scaledY = localPosition.Y * parentScale.Y;

        float radians = parentRotation * (float)Math.PI / 180; 
        float cos = (float)Math.Cos(radians);
        float sin = (float)Math.Sin(radians);

        float rotatedX = cos * scaledX - sin * scaledY;
        float rotatedY = sin * scaledX + cos * scaledY;

        // Step 3: Translate by the parent's position
        float worldX = parentPosition.X + rotatedX;
        float worldY = parentPosition.Y + rotatedY;

        return new Vector2(worldX, worldY);
    }

    public static Vector2 GetRotatedVector(Vector2 vector, float angleDeg)
    {
        float degToRad = (float)Math.PI * 2.0f / 360.0f;
        float angleRad = angleDeg * degToRad;

        float rotatedX = (float)Math.Cos(angleRad) * vector.X - (float)Math.Sin(angleRad) * vector.Y;
        float rotatedY = (float)Math.Sin(angleRad) * vector.X + (float)Math.Cos(angleRad) * vector.Y;

        return new Vector2(rotatedX, rotatedY);
    }

    public static void FlashSprite(Entity entity, Spine_Animator animator, Vector4 color, float totalFlashDuration = 0.5f, int numFlashes = 3)
    {
        float flashDuration = totalFlashDuration / numFlashes;
        float halfFlashDuration = flashDuration * 0.5f;

        color.W = 0.8f;
        Coroutine.Start(entity, DoFlash());
        IEnumerator DoFlash()
        {
            for (int i = 0; i < numFlashes; i++)
            {
                animator.SpineInstance.ColorMultiplier = color;
                yield return new WaitForSeconds(halfFlashDuration);
                animator.SpineInstance.ColorMultiplier = Vector4.White;
                yield return new WaitForSeconds(halfFlashDuration);
            }
        }
    }

    public static Vector2 RandomInsideCircle(float radius, Vector2 origin)
    {
        var angle = Random.Shared.NextDouble() * Math.PI * 2;
        return new Vector2(origin.X + radius * (float)Math.Cos(angle), origin.Y + radius * (float)Math.Sin(angle));
    }
}
