using System.Numerics;
using DysonRendement.Models;

namespace DysonRendement.Services;

public interface ISensor
{
    CompasModel CompassText { get; }
    OrientationModel OrientationText { get; }
    bool ToggleCompass();
    bool ToggleOrientation();
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

            // Turn off compass
            Compass.Default.Stop();
            Compass.Default.ReadingChanged -= Compass_ReadingChanged;
            return true;
        }

        // Compass not supported on device
        return false;
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

            // Turn off orientation
            OrientationSensor.Default.Stop();
            OrientationSensor.Default.ReadingChanged -= Orientation_ReadingChanged;
            return true;
        }

        // Orientation not supported on device
        return false;
    }

    private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        // Update UI Label with compass state
        if (CompassText != null)
            CompassText.Angle = e.Reading.HeadingMagneticNorth;
        else
            CompassText = new CompasModel(e.Reading.HeadingMagneticNorth);
    }

    private void Orientation_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
    {
        // Update UI Label with orientation state
        if (OrientationText != null)
        {
            var qx = e.Reading.Orientation.X;
            var qy = e.Reading.Orientation.Y;
            var qz = e.Reading.Orientation.Z;
            var qw = e.Reading.Orientation.W;

            // Convertir quaternion en angles d'Euler (yaw, pitch, roll) en radians
            var quaternion = new Quaternion(qx, qy, qz, qw);
            var eulerRotation = QuaternionToEulerAngles(quaternion);

            // Convertir radians en degrés
            var roll = MathHelper.ToDegrees(eulerRotation.X);
            var pitch = MathHelper.ToDegrees(eulerRotation.Y);
            var yaw = MathHelper.ToDegrees(eulerRotation.Z);

            // Mettre à jour les valeurs dans votre classe OrientationText
            OrientationText.Pitch = pitch;
            OrientationText.Roll = roll;
            OrientationText.Yaw = yaw;
        }
        else
        {
            OrientationText = new OrientationModel(e.Reading.Orientation.X, e.Reading.Orientation.Y, e.Reading.Orientation.Z, e.Reading.Orientation.W);
        }
    }

    private static Vector3 QuaternionToEulerAngles(Quaternion quaternion)
    {
        var sinr_cosp = 2 * (quaternion.W * quaternion.X + quaternion.Y * quaternion.Z);
        var cosr_cosp = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);
        var roll = (float)Math.Atan2(sinr_cosp, cosr_cosp);

        var sinp = 2 * (quaternion.W * quaternion.Y - quaternion.Z * quaternion.X);
        float pitch;
        if (Math.Abs(sinp) >= 1)
            pitch = (float)Math.CopySign(Math.PI / 2, sinp); // use 90 degrees if out of range
        else
            pitch = (float)Math.Asin(sinp);

        var siny_cosp = 2 * (quaternion.W * quaternion.Z + quaternion.X * quaternion.Y);
        var cosy_cosp = 1 - 2 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
        var yaw = (float)Math.Atan2(siny_cosp, cosy_cosp);

        return new Vector3(roll, pitch, yaw);
    }

    private static class MathHelper
    {
        public static float ToDegrees(float radians)
        {
            return radians * (180.0f / (float)Math.PI);
        }
    }
}