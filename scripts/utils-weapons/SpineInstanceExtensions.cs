using AO;

public static class SpineInstanceExtensions
{
    public static void SetBonePositionWorld(this Spine_Animator spine, string boneName, Vector2 worldPosition, bool facingRight)
    {
        spine.SpineInstance.SetBonePosition(boneName, GetBonePositionFromWorld(spine, worldPosition, facingRight));
    }

    public static Vector2 GetBonePositionFromWorld(this Spine_Animator spine, Vector2 worldPosition, bool facingRight)
    {
        var localPosition = worldPosition - spine.Entity.Position;
        localPosition.X *= facingRight ? 1 : -1;

        return localPosition;
    }
}