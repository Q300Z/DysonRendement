using System.ComponentModel;

namespace DysonRendement.Models;

public class GpsModel : INotifyPropertyChanged
{
    private double _latitude;
    private double _longitude;
    private double _altitude;
    private bool _error;
    private string _errorMessage;

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

    public double Latitude
    {
        get => _latitude;
        set
        {
            _latitude = value;
            OnPropertyChanged(nameof(Latitude));
        }
    }

    public double Longitude
    {
        get => _longitude;
        set
        {
            _longitude = value;
            OnPropertyChanged(nameof(Longitude));
        }
    }

    public double Altitude
    {
        get => _altitude;
        set
        {
            _altitude = value;
            OnPropertyChanged(nameof(Altitude));
        }
    }

    public bool Error
    {
        get => _error;
        set
        {
            _error = value;
            OnPropertyChanged(nameof(Error));
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}