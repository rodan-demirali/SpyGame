namespace SpyGame;

public partial class Ekran3Page : ContentPage
{
    public Ekran3Page()
    {
        InitializeComponent();

        // Baþlangýçta Time Count'u yükle
        int timeCount = Preferences.Get("Time", 3);
        lblStepperValue.Text = timeCount.ToString();
    }

    private void IncreaseTimeCount(object sender, EventArgs e)
    {
        int timeCount = Preferences.Get("Time", 3);
        int max = 5; // Maksimum deðer

        if (timeCount < max)
            timeCount++;

        Preferences.Set("Time", timeCount);
        lblStepperValue.Text = timeCount.ToString();
    }

    private void DecreaseTimeCount(object sender, EventArgs e)
    {
        int timeCount = Preferences.Get("Time", 3);
        int min = 1; // Minimum deðer

        if (timeCount > min)
            timeCount--;

        Preferences.Set("Time", timeCount);
        lblStepperValue.Text = timeCount.ToString();
    }

    private async void ConfirmAndGoBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
