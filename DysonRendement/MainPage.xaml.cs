﻿using DysonRendement.Models;
using DysonRendement.Services;

namespace DysonRendement;

public partial class MainPage : ContentPage
{
    // Propriétés statiques pour les modèles de données des capteurs
    private static GpsModel _gpsModel = new(0, 0, 0);
    private static CompasModel _compasModel = new(0);

    private static OrientationModel _orientationModel = new(0, 0, 0, 0);

    // Propriétés pour les services
    private readonly IGps _gps = MauiApplication.Current.Services.GetService<IGps>();
    private readonly ISensor _sensor = MauiApplication.Current.Services.GetService<ISensor>();

    private readonly ViewModel _viewModel = new(_compasModel, _gpsModel, _orientationModel);

    public MainPage()
    {
        InitializeComponent();
        // Vérifie si le GPS est supporté sur le téléphone et affiche une alerte si ce n'est pas le cas sinon démarre le GPS
        if (!_sensor.ToggleCompass()) DisplayAlert("Alert", "Compass not supported on device", "OK");
        // Vérifie si le compas est supporté sur le téléphone et affiche une alerte si ce n'est pas le cas sinon démarre le compas
        if (!_sensor.ToggleOrientation()) DisplayAlert("Alert", "Gyroscope not supported on device", "OK");
        // Définit le BindingContext de la page sur le ViewModel
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Récupère les données du compas et les affiches dans les labels
        if (_compasModel != null)
        {
            _compasModel = _sensor.CompassText;
            _viewModel.CompasModel = _compasModel;
        }

        // Récupère les données du gyroscope et les affiches dans les labels
        if (_orientationModel != null)
        {
            _orientationModel = _sensor.OrientationText;
            _viewModel.OrientationModel = _orientationModel;
        }

        // Récupère les données du GPS et les affiches dans les labels
        var gps = await _gps.GetCurrentLocation();

        if (gps == null) return;
        _gpsModel = gps;
        _viewModel.GpsModel = _gpsModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}