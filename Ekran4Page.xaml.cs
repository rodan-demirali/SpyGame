using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;

namespace SpyGame;

public partial class Ekran4Page : ContentPage
{
	public Ekran4Page()
	{
		InitializeComponent();

        colorSelectedSections();
    }

    public void colorSelectedSections()
    {
        Objects.BackgroundColor = Colors.Maroon;
        Places.BackgroundColor = Colors.Maroon;
        Countries.BackgroundColor = Colors.Maroon;
        Sports.BackgroundColor = Colors.Maroon;
        Animals.BackgroundColor = Colors.Maroon;
        Transport.BackgroundColor = Colors.Maroon;

        string savedJson = Preferences.Get("SectionList", "[]");
        List<string> savedSections = JsonSerializer.Deserialize<List<string>>(savedJson);

        foreach (var section in savedSections)
        {
            switch (section)
            {
                case "Countries":
                    Countries.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Places":
                    Places.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Objects":
                    Objects.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Sports":
                    Sports.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Animals":
                    Animals.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Transport":
                    Transport.BackgroundColor = Colors.DarkOliveGreen;
                    break;
            }
        }


    }

    private void SectionOptionClicked(object sender, TappedEventArgs e)
    {
        var sectionName = e.Parameter?.ToString();

        string savedJson = Preferences.Get("SectionList", "[]");
        List<string> savedSections = JsonSerializer.Deserialize<List<string>>(savedJson);

        if(!savedSections.Contains(sectionName))
        {
            savedSections.Add(sectionName);
            string json = JsonSerializer.Serialize(savedSections);
            Preferences.Set("SectionList", json);

            switch (sectionName)
            {
                case "Countries":
                    Countries.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Places":
                    Places.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Objects":
                    Objects.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Sports":
                    Sports.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Animals":
                    Animals.BackgroundColor = Colors.DarkOliveGreen;
                    break;
                case "Transport":
                    Transport.BackgroundColor = Colors.DarkOliveGreen;
                    break;
            }
        }
        else
        {
            savedSections.Remove(sectionName);
            string json = JsonSerializer.Serialize(savedSections);
            Preferences.Set("SectionList", json);


            switch (sectionName)
            {
                case "Countries":
                    Countries.BackgroundColor = Colors.Maroon;
                    break;
                case "Places":
                    Places.BackgroundColor = Colors.Maroon;
                    break;
                case "Objects":
                    Objects.BackgroundColor = Colors.Maroon;
                    break;
                case "Sports":
                    Sports.BackgroundColor = Colors.Maroon;
                    break;
                case "Animals":
                    Animals.BackgroundColor = Colors.Maroon;
                    break;
                case "Transport":
                    Transport.BackgroundColor = Colors.Maroon;
                    break;
            }

        }
    }

    private async void ConfirmAndGoBack(object sender, EventArgs e)
    {
        string savedJson = Preferences.Get("SectionList", "[]");
        List<string> savedSections = JsonSerializer.Deserialize<List<string>>(savedJson);

        if (savedSections.Count < 1)
        {
            savedSections = new List<string> { "Countries" };
            string json = JsonSerializer.Serialize(savedSections);
            Preferences.Set("SectionList", json);

        }

        List<string> allWords = await GetSectionWordsAsync(savedSections);
        Debug.WriteLine(allWords);
        if (allWords.Count < 1)
        {
            allWords = new List<string> { "Amerika" };
        }
        string jsonWords = JsonSerializer.Serialize(allWords);
        Preferences.Set("SectionWordList", jsonWords);


        await Navigation.PopAsync();
    }


    public async Task<List<string>> GetSectionWordsAsync(List<string> savedSections)
    {
        List<string> lstAllWords = new List<string>();
        foreach (string sectionName in savedSections)
        {
            string fileName = sectionName + ".txt";
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

}