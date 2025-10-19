namespace SpyGame;

public partial class Ekran2Page : ContentPage
{
    public Ekran2Page()
    {
        InitializeComponent();

        // Baþlangýçta Spy Count'u yükle
        int spyCount = Preferences.Get("SpyCount", 1);
        int playerCount = Preferences.Get("PlayerCount", 5);

        // Maksimum spy sayýsýný belirle
        int newSpyMaxLimit = playerCount - 1;
        if (newSpyMaxLimit > 8) newSpyMaxLimit = 8;

        // Spy Count'u lblSpyCount ve lblStepperValue ile eþitle
        lblStepperValue.Text = spyCount.ToString();
    }

    private void IncreaseSpyCount(object sender, EventArgs e)
    {
        int spyCount = Preferences.Get("SpyCount", 1);
        int playerCount = Preferences.Get("PlayerCount", 5);
        int max = Math.Min(playerCount - 1, 8);

        if (spyCount < max)
            spyCount++;

        Preferences.Set("SpyCount", spyCount);
        lblStepperValue.Text = spyCount.ToString();
    }

    private void DecreaseSpyCount(object sender, EventArgs e)
    {
        int spyCount = Preferences.Get("SpyCount", 1);
        int min = 1;

        if (spyCount > min)
            spyCount--;

        Preferences.Set("SpyCount", spyCount);
        lblStepperValue.Text = spyCount.ToString();
    }

    private async void ConfirmAndGoBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
