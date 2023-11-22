using DysonRendement.Models;

namespace DysonRendement.Services;

public interface ISensor
{
    bool ToggleCompass();
    bool ToggleOrientation();
    CompasModel CompassText { get; }
    OrientationModel OrientationText { get; }
}

public class Sensor : ISensor
{
    public CompasModel CompassText { get; private set; }
    public OrientationModel OrientationText { get; private set; }

    public bool ToggleCompass()
    {
        if (Compass.Default.IsSupported)
        {
            if (!Compass.Default.IsMonitoring)
            {
                // Turn on compass
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                Compass.Default.Start(SensorSpeed.UI);
                return true;
            }
            else
            {
                // Turn off compass
                Compass.Default.Stop();
                Compass.Default.ReadingChanged -= Compass_ReadingChanged;
                return true;
            }
        }
        else
        {
            // Compass not supported on device
            return false;
        }
    }

    private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        // Update UI Label with compass state
        // CompassLabel.TextColor = Colors.Green;
        // CompassLabel.Text = $"Compass: {e.Reading}";
        CompassText = new CompasModel(e.Reading.HeadingMagneticNorth);
    }
    
    public bool ToggleOrientation()
    {
        if (OrientationSensor.Default.IsSupported)
        {
            if (!OrientationSensor.Default.IsMonitoring)
            {
                // Turn on orientation
                OrientationSensor.Default.ReadingChanged += Orientation_ReadingChanged;
                OrientationSensor.Default.Start(SensorSpeed.UI);
                return true;
            }
            else
            {
                // Turn off orientation
                OrientationSensor.Default.Stop();
                OrientationSensor.Default.ReadingChanged -= Orientation_ReadingChanged;
                return true;
            }
        }else
        {
            // Orientation not supported on device
            return false;
        }
    }

    private void Orientation_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
    {
        // Update UI Label with orientation state
        // OrientationLabel.TextColor = Colors.Green;
        // OrientationLabel.Text = $"Orientation: {e.Reading}";
        OrientationText = new OrientationModel(e.Reading.Orientation.X, e.Reading.Orientation.Y, e.Reading.Orientation.Z);
    }
}