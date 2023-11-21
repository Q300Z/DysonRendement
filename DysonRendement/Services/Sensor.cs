namespace DysonRendement.Services;

public interface ISensor
{
    bool ToggleCompass();
    string CompassText { get; set; }
}

public class Sensor : ISensor
{
    public string CompassText { get; set; }

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
        CompassText = $"Compass: {e.Reading.HeadingMagneticNorth.ToString()}";
    }
}