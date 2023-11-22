using System;
using System.ComponentModel;

namespace DysonRendement.Models
{
    public class OrientationModel : INotifyPropertyChanged
    {
        public OrientationModel(double roll, double yaw, double pitch)
        {
            Roll = roll;
            Yaw = yaw;
            Pitch = pitch;
        }
        
        private double _roll;
        private double _yaw;
        private double _pitch;

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

        public void SetQuaternionValues(double quaternionX, double quaternionY, double quaternionZ)
        {
            Roll = ConvertToDegrees(quaternionX, 'x');
            Yaw = ConvertToDegrees(quaternionY, 'y');
            Pitch = ConvertToDegrees(quaternionZ, 'z');
        }

        private double ConvertToDegrees(double quaternion, char axe)
        {
            var sinrCosp = 2 * quaternion;
            var cosrCosp = 1 - 2 * quaternion;
            var roll = Math.Atan2(sinrCosp, cosrCosp);
            var sinp = 2 * quaternion;
            var pitch = Math.Asin(sinp);
            var sinyCosp = 2 * quaternion;
            var cosyCosp = 1 - 2 * quaternion;
            var yaw = Math.Atan2(sinyCosp, cosyCosp);

            // Convertir en degrés
            roll *= (180.0 / Math.PI);
            pitch *= (180.0 / Math.PI);
            yaw *= (180.0 / Math.PI);

            switch (axe)
            {
                case 'x':
                    return roll;
                case 'y':
                    return pitch;
                case 'z':
                    return yaw;
                default:
                    return 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}