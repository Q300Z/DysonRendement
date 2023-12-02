using System.ComponentModel;
using DysonRendement.Utiles;

namespace DysonRendement.Models;

public class ViewModel : INotifyPropertyChanged
{
    // Propriétés
    private CompasModel _compasModel;
    private GpsModel _gpsModel;
    private OrientationModel _orientationModel;
    private double _rendement;

    // Constructeur
    public ViewModel(CompasModel compasModel, GpsModel gpsModel, OrientationModel orientationModel)
    {
        CompasModel = compasModel;
        GpsModel = gpsModel;
        OrientationModel = orientationModel;
    }

    // Propriétés avec notification de changement de valeur
    public CompasModel CompasModel
    {
        get => _compasModel;
        set
        {
            _compasModel = value;
            UpdateRendement();
            OnPropertyChanged(nameof(CompasModel));
        }
    }

    public GpsModel GpsModel
    {
        get => _gpsModel;
        set
        {
            _gpsModel = value;
            OnPropertyChanged(nameof(GpsModel));
        }
    }

    public OrientationModel OrientationModel
    {
        get => _orientationModel;
        set
        {
            _orientationModel = value;
            UpdateRendement();
            OnPropertyChanged(nameof(OrientationModel));
        }
    }

    public double Rendement
    {
        get => _rendement;
        set
        {
            _rendement = value;
            OnPropertyChanged(nameof(Rendement));
        }
    }

    // Événement pour notifier le changement de propriété à la vue
    public event PropertyChangedEventHandler PropertyChanged;

    private void UpdateRendement()
    {
        if (GpsModel == null || OrientationModel == null) return;
        var rendement = MathHelper.CalculerRendement(0.85, GpsModel.Latitude, OrientationModel.Yaw, OrientationModel.Pitch, DateTime.Now);
        Console.WriteLine(rendement);
        Rendement = rendement;
    }

    // Méthode pour notifier le changement de propriété à la vue
    private void OnPropertyChanged(string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}