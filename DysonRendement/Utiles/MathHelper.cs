namespace DysonRendement.Utiles;

public class MathHelper
{
    public static float ToDegrees(float radians)
    {
        return radians * (180.0f / (float)Math.PI);
    }
    public double ToRadians(double angleDegrees)
    {
        return (Math.PI / 180) * angleDegrees;
    }
}