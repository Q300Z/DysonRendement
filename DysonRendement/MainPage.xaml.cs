using DysonRendement.Services;

namespace DysonRendement;

public partial class MainPage : ContentPage
{
    int count = 0;

    private readonly Gps _gps = new Gps();

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";
        var a = await _gps.GetCurrentLocation();
        await DisplayAlert("Alert", a.Text(), "OK");
        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}