using Plugin.Maui.Audio;

namespace DysonRendement;

public partial class Parametre : ContentPage
{
    private bool arriver = true;
    private IAudioPlayer audioPlayer;

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
        ChoixMusiqueFond.SelectedIndex = 3;
    }

    public string[] choixMusique { get; set; }

    private async void ChoixMusiqueFond_SelectedIndexChanged(object sender, EventArgs e)
    {
        SonBouton();
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        if (!arriver)
        {
            if (audioPlayer != null)
                if (audioPlayer.IsPlaying)
                    audioPlayer.Stop();
            var choix = ChoixMusiqueFond.SelectedIndex;
            var res = choix switch
            {
                0 => "musique_fond1.mp3",
                1 => "musique_fond2.mp3",
                2 => "musique_fond3.mp3",
                _ => "musique_fond3.mp3"
            };
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
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
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
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
    }
}