using System.ComponentModel;

namespace DysonRendement.Models;

public class OrientationModel : INotifyPropertyChanged
{
    private double _pitch;
    private double _roll;
    private double _yaw;


    public OrientationModel(double x, double y, double z, double w)
    {
        Roll = x;
        Yaw = y;
        Pitch = z;
    }

    public double Roll
    {
        get => _roll;
        set
        {
            _roll = value;
            OnPropertyChanged(nameof(Roll));
        }
    }

    public double Yaw
    {
        get => _yaw;
        set
        {
            _yaw = value;
            OnPropertyChanged(nameof(Yaw));
        }
    }

    public double Pitch
    {
        get => _pitch;
        set
        {
            _pitch = value;
            OnPropertyChanged(nameof(Pitch));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}