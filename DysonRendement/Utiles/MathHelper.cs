namespace DysonRendement.Utiles;

public class MathHelper
{
    public static double ToDegrees(double radians)
    {
        return radians * (180.0f / Math.PI);
    }

    public static double ToRadians(double angleDegrees)
    {
        return Math.PI / 180 * angleDegrees;
    }


    public static double CalculerRendement(double roll, string angletext)
    {
        double val1 = angletext switch
        {
            "Est" => 42,
            "Sud-Est" => 46,
            "Sud" => 50,
            "Sud-Ouest" => 45,
            "Ouest" => 42,
            "Nord-Ouest" => 10,
            "Nord-Est" => 10,
            _ => 0
        };

        double val2 = 0;
        val2 = roll switch
        {
            >= 0 and < 10 => 1.5,
            >= 10 and < 25 => 1.65,
            >= 25 and < 30 => 1.80,
            >= 30 and < 35 => 2,
            >= 35 and < 45 => 1.90,
            >= 45 and < 65 => 1.75,
            >= 65 and < 75 => 1.60,
            _ => 0.1
        };

        return val1 * val2;
    }
}