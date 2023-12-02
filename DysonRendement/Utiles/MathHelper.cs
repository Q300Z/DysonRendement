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

    public static double AngleElevationSolaire(double latitude, DateTime dateHeure)
    {
        // Calcul simplifié de l'angle d'élévation solaire en fonction de la latitude, la date et l'heure
        // Calcul de la déclinaison solaire en fonction de la date et de l'heure : https://fr.wikipedia.org/wiki/Position_du_Soleil#D%C3%A9clinaison_du_Soleil%2C_vu_de_la_Terre
        var jourJulien = dateHeure.DayOfYear + (dateHeure.Hour - 12) / 24;
        var declinaisonSolaire = 23.44 * Math.Sin(ToRadians(360 / 365 * (jourJulien - 81)));
        var angleElevation = Math.Asin(Math.Sin(ToRadians(latitude)) * Math.Sin(ToRadians(declinaisonSolaire)) +
                                       Math.Cos(ToRadians(latitude)) * Math.Cos(ToRadians(declinaisonSolaire)) * Math.Cos(ToRadians(15 * (dateHeure.Hour - 12) + dateHeure.Minute / 4.0)));

        return ToDegrees(angleElevation);
    }

    public static double CalculerRendement(double rendementMax, double latitude, double yaw, double pitch, DateTime dateHeure)
    {
        // Conversion de l'angle d'orientation par rapport au nord en radians
        var betaRad = ToRadians(pitch);

        // Calcul de l'angle d'élévation solaire en fonction de la latitude, la date et l'heure
        var alpha = AngleElevationSolaire(latitude, dateHeure);

        // Conversion de l'angle d'élévation solaire en radians
        var alphaRad = ToRadians(alpha);

        // Calcul du rendement en utilisant l'équation donnée
        var rendement = rendementMax * (Math.Cos(alphaRad - betaRad) / Math.Cos(alphaRad)) * (Math.Cos(ToRadians(yaw)) / 2 + 0.5);

        return rendement;
    }
}