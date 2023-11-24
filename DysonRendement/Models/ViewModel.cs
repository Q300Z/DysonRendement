using System.ComponentModel;

namespace DysonRendement.Models;

public class ViewModel : INotifyPropertyChanged
{
    public ViewModel(CompasModel compasModel, GpsModel gpsModel, OrientationModel orientationModel)
    {
        CompasModel = compasModel;
        GpsModel = gpsModel;
        OrientationModel = orientationModel;
    }

    private CompasModel _compasModel { get; set; }
    private GpsModel _gpsModel { get; set; }
    private OrientationModel _orientationModel { get; set; }

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

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}