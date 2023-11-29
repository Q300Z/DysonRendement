using Plugin.Maui.Audio;
using System.Diagnostics;

namespace DysonRendement;

public partial class Parametre : ContentPage
{
    public string[] choixMusique { get; set; }
    private IAudioPlayer audioPlayer;
    public Parametre(IAudioPlayer _audioPlayer)
	{
		InitializeComponent();
        choixMusique = new string[] { "Musique1", "Musique2" };
        audioPlayer = _audioPlayer;
        BindingContext = this;
        ChoixMusiqueFond.SelectedIndex = 1;

    }

    private async void ChoixMusiqueFond_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (audioPlayer != null)
        {
            if (audioPlayer.IsPlaying)
            {
                audioPlayer.Stop();
            }
        }
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
            default:
                res = "musique_fond1.mp3";
                break;
        }
        Debug.WriteLine(res);
        audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(res));
        audioPlayer.Volume = 1;
        audioPlayer.Loop = true;
        audioPlayer.Play();
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
}