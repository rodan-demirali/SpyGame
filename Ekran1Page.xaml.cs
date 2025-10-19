using SpyGame.Resources.Strings;
using System.Text.Json;

namespace SpyGame;

public partial class Ekran1Page : ContentPage
{
    string label = AppResources.PlayerLabel;

    public Ekran1Page()
    {
        InitializeComponent();

        // PlayerCount'ý al veya varsayýlan 5
        int playerCount = Preferences.Get("PlayerCount", 5);
        //lblPlayerCount.Text = playerCount.ToString();
        lblStepperValue.Text = playerCount.ToString();

        // SpyCount kontrolü varsa minimum player sayýsý ayarla
        int spyCount = Preferences.Get("SpyCount", 0);
        if (spyCount > 2)
            playerCount = Math.Max(playerCount, spyCount + 1);

        Preferences.Set("PlayerCount", playerCount);

        setPlayerNames();
    }

    public void setPlayerNames()
    {
        stkPlayerNames.Children.Clear();

        int playerCount = Preferences.Get("PlayerCount", 5);

        string savedJson = Preferences.Get("PlayerNames", "[]");
        List<string> savedPlayerNames = JsonSerializer.Deserialize<List<string>>(savedJson);



        for (int i = 1; i <= playerCount; i++)
        {
            string playerName = savedPlayerNames != null && savedPlayerNames.Count >= i
                ? savedPlayerNames[i - 1]
                : $"{label} {i}";

            var entry = new Entry
            {
                Text = playerName,
                HorizontalOptions = LayoutOptions.Fill,
                //Margin = new Thickness(0, 5),
                //BackgroundColor = Color.FromArgb("#2C2C3E"),
                TextColor = Colors.White,
                Placeholder = $"{label} {i}",
                PlaceholderColor = Colors.LightGray,
                HeightRequest = 40,
                WidthRequest = 120,
                FontSize = 14,
            };

            stkPlayerNames.Children.Add(entry);
        }

        savePlayerNames();
    }

    public void savePlayerNames()
    {
        int playerCount = Preferences.Get("PlayerCount", 5);
        List<string> lstPlayerNames = new List<string>();

        for (int i = 0; i < playerCount; i++)
        {
            if (stkPlayerNames.Children[i] is Entry entry)
            {
                string sPlayerName = entry.Text;
                if (string.IsNullOrWhiteSpace(sPlayerName))
                    sPlayerName = $"{label} {i + 1}";

                lstPlayerNames.Add(sPlayerName);
            }
        }

        string json = JsonSerializer.Serialize(lstPlayerNames);
        Preferences.Set("PlayerNames", json);
    }

    // + Butonu
    private void IncreasePlayerCount(object sender, EventArgs e)
    {
        int count = Preferences.Get("PlayerCount", 5);
        int spyCount = Preferences.Get("SpyCount", 0);
        int max = 10;

        if (count < max)
            count++;

        if (count <= spyCount)
            count = spyCount + 1;

        Preferences.Set("PlayerCount", count);
        //lblPlayerCount.Text = count.ToString();
        lblStepperValue.Text = count.ToString();

        setPlayerNames();
    }

    // - Butonu
    private void DecreasePlayerCount(object sender, EventArgs e)
    {
        int count = Preferences.Get("PlayerCount", 5);
        int spyCount = Preferences.Get("SpyCount", 0);
        int min = 3;

        if (count > min)
            count--;

        if (count <= spyCount)
            count = spyCount + 1;

        Preferences.Set("PlayerCount", count);
        //lblPlayerCount.Text = count.ToString();
        lblStepperValue.Text = count.ToString();

        setPlayerNames();
    }

    private async void ConfirmAndGoBack(object sender, EventArgs e)
    {
        savePlayerNames();
        await Navigation.PopAsync();
    }

    private void clearPlayerNames(object sender, EventArgs e)
    {
        int iStkCount = stkPlayerNames.Children.Count;

        for (int i = 0; i < iStkCount; i++)
        {
            if (stkPlayerNames.Children[i] is Entry entry)
            {
                entry.Text = $"{label} {i + 1}";
            }
        }

        savePlayerNames();
    }

}
