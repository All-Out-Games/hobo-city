using AO;

public static class Util2
{
    public static Vector3 V3Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * t;

    public static Vector2 RandomInsideCircle(float radius, Vector2 origin)
    {
        var angle = Random.Shared.NextDouble() * Math.PI * 2;
        return new Vector2(origin.X + radius * (float)Math.Cos(angle), origin.Y + radius * (float)Math.Sin(angle));
    }
}