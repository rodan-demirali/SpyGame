using System.Diagnostics;
using System.Text.Json;

namespace SpyGame;

public partial class GamePage : ContentPage
{
    string sKelime = "Polonya";
    Random rnd = new Random();
    List<string> selectedSpies = new List<string>();
    bool bCountDownContinue = false;
    bool bShowOtherSpiesMode = true;
    int iSpyCountGeneral = 1;

    public GamePage()
	{
		InitializeComponent();

        gameOverArea.IsVisible = false;
        selectionArea.IsVisible = true;
        timerArea.IsVisible = false;
        decideSpies();

        decideTheWord();

        bShowOtherSpiesMode = Preferences.Get("ShowOtherSpiesMode", true);
        iSpyCountGeneral = Preferences.Get("SpyCount", 0);
    }


    public void decideTheWord()
    {
        string savedJson = Preferences.Get("SectionWordList", "[]");
        List<string> savedWordList = JsonSerializer.Deserialize<List<string>>(savedJson);

        int iWordCount = savedWordList.Count();
        int iRndWord = rnd.Next(iWordCount);
        sKelime = savedWordList[iRndWord];


    }


    public void decideSpies()
    {
        var spyCount = Preferences.Get("SpyCount", 0);
        string savedJson = Preferences.Get("PlayerNames", "[]");
        List<string> savedPlayerNames = JsonSerializer.Deserialize<List<string>>(savedJson);
        string sPlayerName = savedPlayerNames[0];
        cardFront.Text = sPlayerName;


        for (int i = 1; i <= spyCount; i++)
        {
            int restPlayersCount = savedPlayerNames.Count();
            int rndSpy = rnd.Next(restPlayersCount);
            string sSpyName = savedPlayerNames[rndSpy];
            //lblTime.Text = lblTime.Text + " " + sSpyName;

            selectedSpies.Add(sSpyName);
            savedPlayerNames.Remove(sSpyName);

        }

        //lblTime.Text = sKelime;
    }


    bool isFlipped = false;
    int sayac = 0;
    private async void OnCardTapped(object sender, EventArgs e)
    {
        string savedJson = Preferences.Get("PlayerNames", "[]");
        List<string> savedPlayerNames = JsonSerializer.Deserialize<List<string>>(savedJson);
        int iPlayerCounter = sayac / 2;

        if(iPlayerCounter < savedPlayerNames.Count())
        {
            if (!isFlipped)
            {
                // Front → Back
                await frontCard.RotateYTo(90, 200, Easing.Linear);
                frontCard.IsVisible = false;
                backCard.IsVisible = true;

                string sPlayerName = savedPlayerNames[iPlayerCounter];
                //lblDurum.Text = sayac + " -- " + sPlayerName;
                if(selectedSpies.Contains(sPlayerName))
                {
                    lblDurum.Text = sPlayerName + ", You're a SPY.";

                    if(iSpyCountGeneral > 1 && bShowOtherSpiesMode)
                        otherSpiesContainer.IsVisible = true;

                    stkOtherSpies.Clear();
                    int iSpyCount = selectedSpies.Count;
                    for (int i = 0; i < iSpyCount; i++)
                    {
                        string playerName = selectedSpies[i];
                        if (playerName == sPlayerName)
                            continue;

                        var entry = new Label
                        {
                            Text = playerName,
                            HorizontalOptions = LayoutOptions.Fill,
                            Margin = new Thickness(0, 5)
                        };

                        stkOtherSpies.Children.Add(entry);
                    }



                }
                else
                    lblDurum.Text = sKelime;

                backCard.RotationY = -90;
                await backCard.RotateYTo(0, 200, Easing.Linear);
            }
            else
            {
                // Back → Front
                await backCard.RotateYTo(-90, 200, Easing.Linear);
                backCard.IsVisible = false;
                frontCard.IsVisible = true;
                otherSpiesContainer.IsVisible = false;

                if(iPlayerCounter < savedPlayerNames.Count() - 1)
                {
                    string sPlayerName = savedPlayerNames[iPlayerCounter + 1];
                    cardFront.Text = sPlayerName;
                }

                frontCard.RotationY = 90;
                await frontCard.RotateYTo(0, 200, Easing.Linear);
            }

            sayac++;
            isFlipped = !isFlipped;
        }

        if(sayac == (savedPlayerNames.Count * 2))
        {
            selectionArea.IsVisible = false;
            timerArea.IsVisible = true;

            bCountDownContinue = true;
            startTheGame();
        }
    }

    public void startTheGame()
    {
        var timerCount = Preferences.Get("Time", 0);
        int timerTime = (int)timerCount;

        //make the lblTime label to count down from timerTime minute to 0 seconds.
        //show minutes, seconds and miliseconds reamining continuously on the lbltime.

        TimeSpan totalTime = TimeSpan.FromMinutes(timerTime);

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // DispatcherTimer her 50ms'de bir çalışsın (milisaniyeler için yeterli)
        var dispatcherTimer = Application.Current.Dispatcher.CreateTimer();
        dispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);

        dispatcherTimer.Tick += (s, e) =>
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            TimeSpan remaining = totalTime - elapsed;

            if (remaining <= TimeSpan.Zero)
            {
                lblTime.FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span { Text = "00:00", FontSize = 32 },  // Bigger minutes:seconds
                        new Span { Text = ":000", FontSize = 18 }   // Smaller milliseconds
                    }
                };
                dispatcherTimer.Stop();
                stopwatch.Stop();

                finishTheGame();
            }
            else
            {
                string minutesSeconds = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
                string milliseconds = $":{remaining.Milliseconds:D3}";

                lblTime.FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span { Text = minutesSeconds, FontSize = 64 }, // Big
                        new Span { Text = milliseconds, FontSize = 36 }   // Small
                    }
                };
            }
        };

        dispatcherTimer.Start();
    }

    public void finishTheGame()
    {
        timerArea.IsVisible = false;
        gameOverArea.IsVisible = true;

        int iSpyCount = selectedSpies.Count;

        gameOverWord.Text = sKelime;

        stkSpies.Children.Clear(); // Önce eski isimleri temizle

        for (int i = 0; i < iSpyCount; i++)
        {
            string playerName = selectedSpies[i];

            var entryFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#4C4CFF"), // hoş bir mavi ton
                CornerRadius = 15,
                Padding = new Thickness(10, 5),
                Margin = new Thickness(0, 5),
                HorizontalOptions = LayoutOptions.Center,
                HasShadow = true
            };

            var nameLabel = new Label
            {
                Text = playerName,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            entryFrame.Content = nameLabel;
            stkSpies.Children.Add(entryFrame);
        }
    }


    private async void GoBackMenu(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


    private void ShowSpiesClicked(object sender, EventArgs e)
    {
        bCountDownContinue = false;
        finishTheGame();


    }


}