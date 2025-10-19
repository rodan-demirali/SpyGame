using SpyGame.Resources.Strings;

namespace SpyGame;

public partial class GameTutorialPage : ContentPage
{
	public GameTutorialPage()
	{
		InitializeComponent();

        var pages = new List<TutorialPageContent>
        {
            new TutorialPageContent
            {
                Content = $"{AppResources.lblPage1Paragrapgh1Text}\n\n{AppResources.lblPage1Paragrapgh2Text}\n\n{AppResources.lblPage1Paragrapgh3Text}"
            },
            new TutorialPageContent
            {
                Content = $"{AppResources.lblPage2Paragrapgh1Text}\n\n{AppResources.lblPage2Paragrapgh2Text}"
            }
        };

        TutorialCarousel.ItemsSource = pages;
    }

    public class TutorialPageContent
    {
        public string Content { get; set; }
    }

    private async void btnDone_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();

    }
}