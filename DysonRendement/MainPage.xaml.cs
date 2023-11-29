using DysonRendement.Models;
using DysonRendement.Services;
using Plugin.Maui.Audio;

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


    private IAudioPlayer _audioPlayer { get; set; }

    // Propriétés
    private int count;

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
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        SonBouton();
        await Task.Delay(700);
        // Récupère la position GPS
        var gps = await _gps.GetCurrentLocation();
        // Vérifie si la position GPS est nulle
        if (gps != null)
            // Met à jour le modèle avec les coordonnées GPS
            _gpsModel = gps;
        // Vérifie si le modèle du compas est null
        if (_compasModel != null)
            // Met à jour le modèle avec l'angle du compas
            _compasModel = _sensor.CompassText;
        // Vérifie si le modèle de l'orientation est null
        if (_orientationModel != null)
            // Met à jour le modèle avec les coordonnées de l'orientation
            _orientationModel = _sensor.OrientationText;

        // Met à jour le ViewModel avec les modèles de données des capteurs
        _viewModel.GpsModel = _gpsModel;
        _viewModel.CompasModel = _compasModel;
        _viewModel.OrientationModel = _orientationModel;

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Parametre(_audioPlayer));
    }

    private async void LanceMusique()
    {
        await Task.Delay(700);
        _audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("musique_fond3.mp3"));
        _audioPlayer.Volume = 1;
        _audioPlayer.Loop = true;
        _audioPlayer.Play();
    }

    private async void SonBouton()
    {
        var audioPlayerBouton = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("son_bouton.mp3"));
        audioPlayerBouton.Volume = 1;
        audioPlayerBouton.Loop = false;
        audioPlayerBouton.Play();
    }
}