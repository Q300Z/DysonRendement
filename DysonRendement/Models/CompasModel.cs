using System.ComponentModel;

namespace DysonRendement.Models;

public class CompasModel : INotifyPropertyChanged
{
    private double _angle;
    private string _angleText;

    public CompasModel(double angle)
    {
        Angle = angle;
    }

    public double Angle
    {
        get => _angle;
        set
        {
            _angle = value;
            AngleText = AngleTextCalcul(value);
            OnPropertyChanged(nameof(Angle));
        }
    }

    public string AngleText
    {
        get => _angleText;
        set
        {
            _angleText = value;
            OnPropertyChanged(nameof(AngleText));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private static string AngleTextCalcul(double angle)
    {
        return angle switch
        {
            (>= 0 and < 22.5) or (>= 337.5 and <= 360) => "Nord", // Nord
            >= 22.5 and < 67.5 => "Nord-Est", // Nord-Est
            >= 67.5 and < 112.5 => "Est", // Est
            >= 112.5 and < 157.5 => "Sud-Est", // Sud-Est
            >= 157.5 and < 202.5 => "Sud", // Sud
            >= 202.5 and < 247.5 => "Sud-Ouest", // Sud-Ouest
            >= 247.5 and < 292.5 => "Ouest", // Ouest
            _ => "Nord-Ouest" // Nord-Ouest
        };
    }

    private void OnPropertyChanged(string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}