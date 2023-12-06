using System.Diagnostics;
using Plugin.Maui.Audio;

namespace DysonRendement;

public partial class Parametre : ContentPage
{
    private bool FirstArriver = true;
    private IAudioPlayer audioPlayer;
    public string[] choixMusique { get; set; }

    public Parametre(IAudioPlayer _audioPlayer)
    {
        InitializeComponent();
        choixMusique = new[] { "Can't Sleep - WaterFlame", "Tsuki sayu Yoru - Fu rin Ka zan", "Ambiance Espace - Papy Nounn" };
        audioPlayer = _audioPlayer;
        if (!audioPlayer.IsPlaying)
            MuteMusique.Text = "Activer la musique";
        else
            MuteMusique.Text = "Couper la musique";
        BindingContext = this;
        ChoixMusiqueFond.SelectedIndex = 2;
    }

    
    private async void ChoixMusiqueFond_SelectedIndexChanged(object sender, EventArgs e)
    {
        SonBouton();
        if (!FirstArriver)
        {
            if (audioPlayer != null)
                if (audioPlayer.IsPlaying)
                    audioPlayer.Stop();
            var choix = ChoixMusiqueFond.SelectedIndex;
            string res;
            switch (choix)
            {
                case 0:
                    res = "musique_fond1.mp3";
                    break;
                case 1:
                    res = "musique_fond2.mp3";
                    break;
                case 2:
                    res = "musique_fond3.mp3";
                    break;
                default:
                    res = "musique_fond3.mp3";
                    break;
            }

            Debug.WriteLine(res);
            audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(res));
            audioPlayer.Volume = 1;
            audioPlayer.Loop = true;
            audioPlayer.Play();

            MuteMusique.Text = "Couper la musique";
        }

        FirstArriver = false;
    }

    private async void MuteMusique_Clicked(object sender, EventArgs e)
    {
        SonBouton();
        await Task.Delay(700);
        if (audioPlayer.IsPlaying)
        {
            MuteMusique.Text = "Activer la musique";
            audioPlayer.Pause();
        }
        else
        {
            MuteMusique.Text = "Couper la musique";
            audioPlayer.Play();
        }
    }

    private async void SonBouton()
    {
        var audioPlayerBouton = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("son_bouton.mp3"));
        audioPlayerBouton.Volume = 1;
        audioPlayerBouton.Loop = false;
        audioPlayerBouton.Play();
    }

    private void ContentPage_Disappearing(object sender, EventArgs e)
    {
        SonBouton();
    }
}