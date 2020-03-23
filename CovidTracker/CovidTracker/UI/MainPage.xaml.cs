using System;
using System.ComponentModel;
using System.Threading;
using CovidTracker.Localization;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CovidTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private SymptomsPage SymptomsPage;
        private int ButtonLock;

        private SymptomsPage GetSymptomsPage()
        {
            if (SymptomsPage == null) {
                SymptomsPage = new SymptomsPage();
                SymptomsPage.OnPageExit += SymptomsPageExit;
            }

            return SymptomsPage;
        }


        public MainPage()
        {
            InitializeComponent();
            MainPageVM mainPageVM = new MainPageVM();
            mainPageVM.OnRiskResult += RiskResult;
            BindingContext = mainPageVM;
        }


        void OpenSymptomsPage(System.Object sender, System.EventArgs e)
        {
            if (0 == Interlocked.Exchange(ref ButtonLock, 1)) {
                Navigation.PushPopupAsync(GetSymptomsPage(), false);
            }
        }


        void SymptomsPageExit(object sender, SymptomsPageVM.Status status)
        {
            ButtonLock = 0;
        }


        void RiskResult(System.Object sender, string risk)
        {
            DisplayAlert(LocResources.RiskLevelTitle, LocResources.RiskLevelBody + risk, LocResources.Ok);
        }


        void NewServerIp(System.Object sender, System.EventArgs e)
        {
            string newIP = IpAddress.Text != null ? IpAddress.Text : IpAddress.Placeholder;
            AppConfiguration.REGISTRATION_SERVER_URL = new UriBuilder("http", newIP, 8000);
            AppConfiguration.LOCATION_SERVER_URL = new UriBuilder("http", newIP, 8000);
            DisplayAlert("o_O", "---> " + newIP + " <---", "OK");
        }
    }
}
