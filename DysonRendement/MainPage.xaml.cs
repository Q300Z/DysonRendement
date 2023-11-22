using DysonRendement.Services;

namespace DysonRendement;

public partial class MainPage : ContentPage
{
    int count = 0;

    private readonly IGps _gps = MauiApplication.Current.Services.GetService<IGps>();
    private readonly ISensor _sensor = MauiApplication.Current.Services.GetService<ISensor>();

    public MainPage()
    {
        InitializeComponent();

        if (!_sensor.ToggleCompass())
        {
            DisplayAlert("Alert", "Compass not supported on device", "OK");
        }

        if (!_sensor.ToggleOrientation())
        {
            DisplayAlert("Alert", "Gyroscope not supported on device", "OK");
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";
        var a = await _gps.GetCurrentLocation();
        var b = _sensor.CompassText;
        var c = _sensor.OrientationText;

        var viewModel = new Models.ViewModel(b, a, c);
        BindingContext= viewModel;

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}