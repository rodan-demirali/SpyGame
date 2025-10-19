using System.Diagnostics;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;
using SpyGame.Resources.Strings;

namespace SpyGame
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            string text = AppResources.WelcomeText;

            InitializeComponent();

            if (!Preferences.ContainsKey("GameLanguage"))
                Preferences.Set("GameLanguage", "English");

            if (!Preferences.ContainsKey("PlayerCount"))
                Preferences.Set("PlayerCount", 3);

            if (!Preferences.ContainsKey("PlayerCount"))
                Preferences.Set("PlayerCount", 3);

            if (!Preferences.ContainsKey("SpyCount"))
                Preferences.Set("SpyCount", 1);

            if (!Preferences.ContainsKey("ShowOtherSpiesMode"))
                Preferences.Set("ShowOtherSpiesMode", true);


            if (!Preferences.ContainsKey("Time"))
                Preferences.Set("Time", 5);

            if (!Preferences.ContainsKey("Sections"))
                Preferences.Set("Sections", 6);

            if (!Preferences.ContainsKey("SectionList"))
            {
                List<string> lstSections = new List<string> { "Countries", "Places" };

                string json = JsonSerializer.Serialize(lstSections);
                Preferences.Set("SectionList", json);
            }

            if (!Preferences.ContainsKey("PlayerNames"))
            {
                List<string> lstPlayerNames = new List<string> { "Player 1", "Player 2", "Player 3" };

                string json = JsonSerializer.Serialize(lstPlayerNames);
                Preferences.Set("PlayerNames", json);
            }


            setGameSettings();
            setAllWords();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            setGameSettings();
            setAllWords();
        }

        public async void setAllWords()
        {

            string savedJson = Preferences.Get("SectionList", "[]");
            List<string> savedSections = JsonSerializer.Deserialize<List<string>>(savedJson);

            List<string> allWords = await GetSectionWordsAsync(savedSections);
            Debug.WriteLine(allWords);
            if (allWords.Count < 1)
            {
                allWords = new List<string> { "Amerika" };
            }
            string jsonWords = JsonSerializer.Serialize(allWords);
            Preferences.Set("SectionWordList", jsonWords);

        }

        public async Task<List<string>> GetSectionWordsAsync(List<string> savedSections)
        {
            var lang = Preferences.Get("GameLanguage", "English").ToString();

            List<string> lstAllWords = new List<string>();
            foreach (string sectionName in savedSections)
            {
                string fileName = lang + "/" + sectionName + ".txt";
                //string fileName = "Countries.txt";

                try
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                    using var reader = new StreamReader(stream);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine()?.Trim();
                        if (!string.IsNullOrEmpty(line))
                        {
                            lstAllWords.Add(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error reading {fileName}: {ex.Message}");
                }

            }

            return lstAllWords;
        }


        private void setGameSettings()
        {
            var playerCount = Preferences.Get("PlayerCount", 0);
            var spyCount = Preferences.Get("SpyCount", 0);
            var timeVal = Preferences.Get("Time", 0);
            //var sectionVal = Preferences.Get("Sections", 0);

            lblPlayerCount.Text = playerCount.ToString();
            lblSpyCount.Text = spyCount.ToString();
            lblTime.Text = timeVal.ToString();
            //lblSectionCount.Text = sectionVal.ToString();

            string savedJson = Preferences.Get("SectionList", "[]");
            List<string> savedSections = JsonSerializer.Deserialize<List<string>>(savedJson);

            if(savedSections.Count < 1)
            {
                savedSections = new List<string> { "Countries" };
                string json = JsonSerializer.Serialize(savedSections);
                Preferences.Set("SectionList", json);

            }

            lblSectionCount.Text = savedSections.Count.ToString();


        }

        
        private async void OnInfoButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GameSettingsPage());
        }

        private async void OnTutorialButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GameTutorialPage());
        }


        private async void StartPlaying(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }

        private async void OnEkran1Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ekran1Page());
        }

        private async void OnEkran2Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ekran2Page());
        }

        private async void OnEkran3Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ekran3Page());
        }

        private async void OnEkran4Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ekran4Page());
        }

    }

}
