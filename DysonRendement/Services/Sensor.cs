﻿using System.Numerics;
using DysonRendement.Models;
using DysonRendement.Utiles;

namespace DysonRendement.Services;

// Interface pour le service Sensor
public interface ISensor
{
    CompasModel CompassText { get; }
    OrientationModel OrientationText { get; }
    bool ToggleCompass();
    bool ToggleOrientation();
}

// Service pour les capteurs (compas et orientation) du téléphone qui implémente l'interface ISensor et qui permet de récupérer les valeurs des capteurs.
public class Sensor : ISensor
{
    // Propriétés
    public CompasModel CompassText { get; private set; } = new(0);

    public OrientationModel OrientationText { get; private set; } = new(0, 0, 0, 0);

    // Méthodes pour activer ou désactiver le compas
    public bool ToggleCompass()
    {
        // Vérifie si le compas est supporté sur le téléphone
        if (Compass.Default.IsSupported)
        {
            // Vérifie si le compas est monitoré
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

    // Méthodes pour activer ou désactiver l'orientation
    public bool ToggleOrientation()
    {
        // Vérifie si l'orientation est supportée sur le téléphone
        if (OrientationSensor.Default.IsSupported)
        {
            // Vérifie si l'orientation est monitorée
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

    // Méthodes pour mettre à jour les valeurs du compas
    private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        // Vérifie si le compas est nul
        if (CompassText != null)
            CompassText.Angle = Convert.ToDouble($"{e.Reading.HeadingMagneticNorth:0.00}");
        else
            CompassText = new CompasModel(e.Reading.HeadingMagneticNorth);
    }

    // Méthodes pour mettre à jour les valeurs de l'orientation
    private void Orientation_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
    {
        // Vérifie si l'orientation est nulle
        if (OrientationText != null)
        {
            // Récupérer les valeurs du quaternion
            var qx = e.Reading.Orientation.X;
            var qy = e.Reading.Orientation.Y;
            var qz = e.Reading.Orientation.Z;
            var qw = e.Reading.Orientation.W;

            // Convertir quaternion en angles d'Euler (yaw, pitch, roll) en radians
            var quaternion = new Quaternion(qx, qy, qz, qw);
            var eulerRotation = QuaternionToEulerAngles(quaternion);

            // Convertir radians en degrés
            var pitch = MathHelper.ToDegrees(eulerRotation.X);
            var roll = MathHelper.ToDegrees(eulerRotation.Y);
            var yaw = MathHelper.ToDegrees(eulerRotation.Z);

            // Mettre à jour les valeurs dans votre classe OrientationText
            OrientationText.Pitch = pitch;
            OrientationText.Roll = roll;
            OrientationText.Yaw = yaw;
        }
        else
        {
            OrientationText =
                new OrientationModel(e.Reading.Orientation.X, e.Reading.Orientation.Y, e.Reading.Orientation.Z, e.Reading.Orientation.W);
        }
    }

    // Méthode pour convertir un quaternion en angles d'Euler (yaw, pitch, roll) en degrés
    private static Vector3 QuaternionToEulerAngles(Quaternion quaternion)
    {
        // Les liens sont chier par CoPilot car bon courage pour comprendre ce qu'il se passe ici :
        // Documentation : https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
        // StackOverflow : https://stackoverflow.com/questions/11492299/quaternion-to-euler-angles-algorithm-how-to-convert-to-y-up-and-between-ha
        // StackOverflow : https://stackoverflow.com/questions/596216/formula-to-convert-quaternion-rotation-to-rotation-axis-or-euler-angles
        // StackOverflow : https://stackoverflow.com/questions/1556260/convert-quaternion-rotation-to-rotation-around-axis

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
}