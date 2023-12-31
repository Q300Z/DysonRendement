﻿using DysonRendement.Models;
using DysonRendement.Services;
using DysonRendement.Utiles;
using Plugin.Maui.Audio;

namespace DysonRendement;

public partial class MainPage : ContentPage
{
    // Propriétés statiques pour les modèles de données des capteurs
    private static GpsModel _gpsModel = new(0, 0, 0);
    private static CompasModel _compasModel = new(0);

    private static OrientationModel _orientationModel = new(0, 0, 0, 0);
    private static readonly RendementModel _rendementModel = new(0);

    // Propriétés pour les services
    private readonly IGps _gps = IPlatformApplication.Current?.Services.GetService<IGps>();
    private readonly ISensor _sensor = IPlatformApplication.Current?.Services.GetService<ISensor>();

    private readonly Timer _timerUpdateRendement;

    private readonly ViewModel _viewModel = new(_compasModel, _gpsModel, _orientationModel, _rendementModel);

    public MainPage()
    {
        InitializeComponent();
        // Vérifie si le GPS est supporté sur le téléphone et affiche une alerte si ce n'est pas le cas sinon démarre le GPS
        if (!_sensor.ToggleCompass()) DisplayAlert("Alert", "Compass not supported on device", "OK");
        // Vérifie si le compas est supporté sur le téléphone et affiche une alerte si ce n'est pas le cas sinon démarre le compas
        if (!_sensor.ToggleOrientation()) DisplayAlert("Alert", "Gyroscope not supported on device", "OK");

        LanceMusique();


        // Définit le BindingContext de la page sur le ViewModel
        BindingContext = _viewModel;

        _timerUpdateRendement = new Timer(UpdateRendement, null, 0, 1000);
    }


    private IAudioPlayer AudioPlayer { get; set; }

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

    private static void UpdateRendement(object state)
    {
        _rendementModel.Rendement = MathHelper.CalculerRendement(_orientationModel.Roll, _compasModel.AngleText);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        SonBouton();
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        Navigation.PushAsync(new Parametre(AudioPlayer));
    }

    private async void LanceMusique()
    {
        AudioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("musique_fond3.mp3"));
        AudioPlayer.Volume = 1;
        AudioPlayer.Loop = true;
        AudioPlayer.Play();
    }

    private static async void SonBouton()
    {
        var audioPlayerBouton = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("son_bouton.mp3"));
        audioPlayerBouton.Volume = 1;
        audioPlayerBouton.Loop = false;
        audioPlayerBouton.Play();
    }
}