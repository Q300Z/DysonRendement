namespace DysonRendement.Models;

public class GpsModel
{
    private double Latitude { get; set; }
    private double Longitude { get; set; }
    private double Altitude { get; set; }
    private bool Error { get; set; }
    private string ErrorMessage { get; set; }

    public GpsModel(bool error, string errorMessage)
    {
        Latitude = 0;
        Longitude = 0;
        Altitude = 0;
        Error = error;
        ErrorMessage = errorMessage;
    }

    public GpsModel(double latitude, double longitude, double altitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
        Error = false;
        ErrorMessage = "";
    }

    public String Text()
    {
        return $"Latitude: {Latitude}, Longitude: {Longitude}, Altitude: {Altitude}";
    }
}