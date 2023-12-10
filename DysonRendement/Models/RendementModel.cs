using System.ComponentModel;

namespace DysonRendement.Models;

public class RendementModel : INotifyPropertyChanged
{
    private double _rendement;

    public RendementModel(double rendement)
    {
        Rendement = rendement;
    }

    public double Rendement
    {
        get => _rendement;
        set
        {
            _rendement = value;
            OnPropertyChanged(nameof(Rendement));
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