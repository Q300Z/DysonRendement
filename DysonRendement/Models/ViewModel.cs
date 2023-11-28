using System.ComponentModel;

namespace DysonRendement.Models;

public class ViewModel : INotifyPropertyChanged
{
    // Propriétés
    private CompasModel _compasModel;
    private GpsModel _gpsModel;

    private OrientationModel _orientationModel;

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
            OnPropertyChanged(nameof(OrientationModel));
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