using System.ComponentModel;
using System.Diagnostics;

namespace DysonRendement.Models;

public class ViewModel : INotifyPropertyChanged
{
    // Propriétés
    private CompasModel _compasModel;
    private GpsModel _gpsModel;

    private OrientationModel _orientationModel;
    private double _perform;

    // Constructeur
    public ViewModel(CompasModel compasModel, GpsModel gpsModel, OrientationModel orientationModel)
    {
        CompasModel = compasModel;
        GpsModel = gpsModel;
        OrientationModel = orientationModel;
        _perform = Performance();
    }

    // Méthode pour obtenire la performance
    public double Performance()
    {
        var angle = _compasModel.AngleText;
        var oriante = _orientationModel.Roll;
        double val1 = 0;
        switch (angle)
        {
            case "Est":
                val1 = 42;
                break;
            case "Sud-Est":
                val1 = 46;
                break;
            case "Sud":
                val1 = 50;
                break;
            case "Sud-Ouest":
                val1 = 45;
                break;
            case "Ouest":
                val1 = 42;
                break;
            case "Nord-Ouest":
                val1 = 10;
                break;
            case "Nord-Est":
                val1 = 10;
                break;
            

        };

        Debug.WriteLine(angle +" "+ val1.ToString());
        double val2 = 0;
        val2 = oriante switch
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
        Debug.WriteLine(oriante +" " + val2.ToString());

        return val1 * val2;
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

    public double Perform
    {
        get => _perform;
        set
        {
            _perform = value;
            OnPropertyChanged(nameof(Perform));
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