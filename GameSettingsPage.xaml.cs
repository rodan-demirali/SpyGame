namespace SpyGame;

public partial class GameSettingsPage : ContentPage
{
    public class LanguageOption
    {
        public string Name { get; set; }
        public string Icon { get; set; } // image path (e.g., "icon.png")
    }

    List<LanguageOption> languages = new List<LanguageOption>
    {
        new LanguageOption { Name = "Turkce", Icon = "dotnet_bot.png" },
        new LanguageOption { Name = "English", Icon = "dotnet_bot.png" },
        new LanguageOption { Name = "Deutsch", Icon = "dotnet_bot.png" },
    };

    public GameSettingsPage()
	{
		InitializeComponent();
        languageCollection.ItemsSource = languages;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Preferences.ContainsKey("ShowOtherSpiesMode"))
            showOtherSpiesSwitch.IsToggled = Preferences.Get("ShowOtherSpiesMode", true);

        if (Preferences.ContainsKey("GameLanguage"))
            languageButton.Text = Preferences.Get("GameLanguage", "Select Language");
    }

    private void OnLanguageButtonClicked(object sender, EventArgs e)
    {
        languagePopup.IsVisible = !languagePopup.IsVisible;
    }

    private async void ApplySettings(object sender, EventArgs e)
    {
        string sSelectedLanguage = languageButton.Text.ToString();
        Preferences.Set("GameLanguage", sSelectedLanguage);

        bool isOtherSpiesOn = showOtherSpiesSwitch.IsToggled;
        Preferences.Set("ShowOtherSpiesMode", isOtherSpiesOn);

        App.SetAppLanguage(sSelectedLanguage);
        Application.Current.MainPage = new AppShell();

        //await Navigation.PopAsync();
    }

    private void OnLanguageSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is LanguageOption selectedLanguage)
        {
            languageButton.Text = selectedLanguage.Name;
            languagePopup.IsVisible = false;

        }
    }

    private void showOtherSpiesSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        bool isOn = e.Value;
    }


}