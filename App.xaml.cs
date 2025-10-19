using SpyGame.Resources.Strings;
using System.Globalization;

namespace SpyGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!Preferences.ContainsKey("GameLanguage"))
                Preferences.Set("GameLanguage", "English");

            var lang = Preferences.Get("GameLanguage", "English");
            SetAppLanguage(lang);

            MainPage = new NavigationPage(new MainPage());


        }

        //protected override void OnStart()
        //{
        //    base.OnStart();
        //
        //    if (!Preferences.ContainsKey("GameLanguage"))
        //        Preferences.Set("GameLanguage", "English");
        //
        //    var lang = Preferences.Get("GameLanguage", "");
        //    SetAppLanguage(lang);
        //}


        public static void SetAppLanguage(string lang)
        {
            string cultureCode;

            switch (lang)
            {
                case "English":
                    cultureCode = "en";
                    break;
                case "Deutsch":
                    cultureCode = "de";
                    break;
                case "Turkce":
                default:
                    cultureCode = "tr";
                    break;
            }

            var culture = new CultureInfo(cultureCode);

            // Thread culture
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // MAUI default culture
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Resource’ların da haberi olsun
            AppResources.Culture = culture;
        }

    }
}
