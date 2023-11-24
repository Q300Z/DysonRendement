using System.ComponentModel;

namespace DysonRendement.Models;

// Modèle représentant les coordonnées GPS avec la latitude, la longitude et l'altitude.
public class GpsModel : INotifyPropertyChanged
{
    // Propriétés
    private double _altitude;
    private bool _error;
    private string _errorMessage;
    private double _latitude;
    private double _longitude;

    // Constructeurs pour les erreurs
    public GpsModel(bool error, string errorMessage)
    {
        Latitude = 0;
        Longitude = 0;
        Altitude = 0;
        Error = error;
        ErrorMessage = errorMessage;
    }

    // Constructeur pour les coordonnées GPS
    public GpsModel(double latitude, double longitude, double altitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
        Error = false;
        ErrorMessage = "";
    }

    // Propriétés avec notification de changement de valeur
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

    // Événement pour notifier le changement de propriété à la vue
    public event PropertyChangedEventHandler PropertyChanged;

    // Méthode pour notifier le changement de propriété à la vue
    private void OnPropertyChanged(string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}