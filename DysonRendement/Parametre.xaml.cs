using Plugin.Maui.Audio;
using System.Diagnostics;

namespace DysonRendement;

public partial class Parametre : ContentPage
{
    public string[] choixMusique { get; set; }
    private IAudioPlayer audioPlayer;
    private bool arriver = true;
    public Parametre(IAudioPlayer _audioPlayer)
	{
		InitializeComponent();
        choixMusique = new string[] { "Can't Sleep - WaterFlame", "Tsuki sayu Yoru - Fu rin Ka zan", "Ambiance Espace - Papy Nounn" };
        audioPlayer = _audioPlayer;
        if (!audioPlayer.IsPlaying)
        {
            MuteMusique.Text = "Activer la musique";
        }
        else
        {
            MuteMusique.Text = "Couper la musique";
        }
        BindingContext = this;
        ChoixMusiqueFond.SelectedIndex = 3;


    }

    private async void ChoixMusiqueFond_SelectedIndexChanged(object sender, EventArgs e)
    {
        SonBouton();
        if (!arriver)
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
        }
        arriver = false;
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