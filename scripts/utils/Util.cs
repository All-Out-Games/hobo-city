using System.Collections;
using System.Text;
using AO;

public static partial class BBUtil
{
    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
    {
        Vector2 a = target - current;
        float magnitude = MathF.Sqrt((a.X * a.X) + (a.Y * a.Y));
        if (magnitude <= maxDistanceDelta || magnitude == 0f)
        {
            return target;
        }
        return current + a / magnitude * maxDistanceDelta;
    }

    public static string FormatNumber(int number)
    {
        return number.ToString("N0");
    }

    public static string FormatNumber(float number, int decimalPlaces = 0)
    {
        return number.ToString($"N{decimalPlaces}");
    }

    public static Vector2 RandomPosition(float maxDistance)
    {
        Random random = new Random();
        int distance = (int)(maxDistance * 100);
        return new Vector2(random.Next(-distance, distance) / 100f, random.Next(-distance, distance) / 100f);
    }
    public static Vector2 SeedRandomPosition(float maxDistance, int seed)
    {
        Random random = new Random(seed);
        int distance = (int)(maxDistance * 100);
        return new Vector2(random.Next(-distance, distance) / 100f, random.Next(-distance, distance) / 100f);
    }

    public static Vector2 WorldToLocalPoint(Entity parent, Vector2 worldPoint)
    {
        // First transform to parent space
        Vector2 parentPosition = parent.Position;
        float parentRotation = -parent.Rotation * (float)Math.PI / 180f;
        Vector2 parentScale = parent.LocalScale;

        // Transform to parent space
        Vector2 pointInParentSpace = worldPoint - parentPosition;

        // Rotate in parent space
        float parentCos = (float)Math.Cos(parentRotation);
        float parentSin = (float)Math.Sin(parentRotation);
        Vector2 rotatedInParent = new Vector2(
            pointInParentSpace.X * parentCos - pointInParentSpace.Y * parentSin,
            pointInParentSpace.X * parentSin + pointInParentSpace.Y * parentCos
        );

        // Scale in parent space
        Vector2 localPoint = new Vector2(
            rotatedInParent.X / parentScale.X,
            rotatedInParent.Y / parentScale.Y
        );

        return localPoint;
    }
    public static (Vector2 startPos, Vector2 direction) GetAimingDirectionFromBone(MyPlayer player, string boneName)
    {
        var startPos = player.Entity.CalculateWorldPosition(player.SpineAnimator.SpineInstance.GetBonePosition(boneName));
        var clickedPos = player.Position + player.CurrentTargettingDirection * player.CurrentTargettingMagnitude;
        var direction = (clickedPos - startPos).Normalized;
        return (startPos, direction);
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

        // // Create an overlay entity that will flash
        // var overlay = Entity.Create();
        // overlay.SetParent(entity, false);
        // overlay.LocalPosition = Vector2.Zero;
        // overlay.LocalScale = new Vector2(2f, 2f); // Make it a bit bigger than the player
        // var renderer = overlay.AddComponent<Sprite_Renderer>();
        // renderer.Texture = Assets.GetAsset<Texture>("Sprites/white_large.png");
        // renderer.Tint = new Vector4(0, 0, 0, 0);

        color.W = 0.8f;
        Coroutine.Start(entity, DoFlash());
        IEnumerator DoFlash()
        {
            // Original color multiplier approach:
            for (int i = 0; i < numFlashes; i++)
            {
                animator.SpineInstance.ColorMultiplier = color;
                yield return new WaitForSeconds(halfFlashDuration);
                animator.SpineInstance.ColorMultiplier = Vector4.White;
                yield return new WaitForSeconds(halfFlashDuration);
            }

            // // New overlay approach:
            // for (int i = 0; i < numFlashes; i++)
            // {
            //     renderer.Tint = new Vector4(color.X, color.Y, color.Z, 0.5f);
            //     yield return new WaitForSeconds(halfFlashDuration);
            //     renderer.Tint = new Vector4(0, 0, 0, 0);
            //     yield return new WaitForSeconds(halfFlashDuration);
            // }

            // overlay.Destroy();
        }
    }
}

