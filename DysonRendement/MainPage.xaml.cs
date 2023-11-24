using DysonRendement.Models;
using DysonRendement.Services;

namespace DysonRendement;

public partial class MainPage : ContentPage
{
    private static GpsModel _gpsModel = new(0, 0, 0);
    private static CompasModel _compasModel = new(0);
    private static OrientationModel _orientationModel = new(0, 0, 0, 0);
    private readonly IGps _gps = MauiApplication.Current.Services.GetService<IGps>();
    private readonly ISensor _sensor = MauiApplication.Current.Services.GetService<ISensor>();
    private readonly ViewModel _viewModel = new(_compasModel, _gpsModel, _orientationModel);
    private int count;

    public MainPage()
    {
        InitializeComponent();
        if (!_sensor.ToggleCompass()) DisplayAlert("Alert", "Compass not supported on device", "OK");

        if (!_sensor.ToggleOrientation()) DisplayAlert("Alert", "Gyroscope not supported on device", "OK");
        BindingContext = _viewModel;
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        var gps = await _gps.GetCurrentLocation();

        if (gps != null)
            _gpsModel = gps;
        if (_compasModel != null)
            _compasModel = _sensor.CompassText;
        if (_orientationModel != null) _orientationModel = _sensor.OrientationText;
        _viewModel.GpsModel = _gpsModel;
        _viewModel.CompasModel = _compasModel;
        _viewModel.OrientationModel = _orientationModel;

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}