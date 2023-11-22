using System.ComponentModel;

namespace DysonRendement.Models;

public class CompasModel : INotifyPropertyChanged
{
    public CompasModel(double angle)
    {
        Angle = angle;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private double _angle;
    private string _angleText;

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

    private static string AngleTextCalcul(double valeur)
    {
        const double nordmin = 15;
        const double nordmax = 345;
        const double estmin = 75;
        const double estmax = 105;
        const double sudmin = 165;
        const double sudmax = 190;
        const double ouestmin = 260;
        const double ouestmax = 280;

        return valeur switch
        {
            > nordmin and < estmin => "Nord-Est",
            > estmin and < estmax => "Est",
            > estmax and < sudmin => "Sud-Est",
            > sudmin and < sudmax => "Sud",
            > sudmax and < ouestmin => "Sud-Ouest",
            > ouestmin and < ouestmax => "Nord-Ouest",
            > ouestmax and < nordmax => "Nord",
            _ => "Inconnu"
        };
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

    private void OnPropertyChanged(string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}