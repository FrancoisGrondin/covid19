using System;
using System.ComponentModel;
using System.Threading;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace CovidTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private SymptomsPage SymptomsPage;
        private int ButtonLock;

        private SymptomsPage GetSymptomsPage()
        {
            if (SymptomsPage == null) {
                SymptomsPage = new SymptomsPage();
            }

            return SymptomsPage;
        }


        public MainPage()
        {
            InitializeComponent();
        }


        async void ClickedSymptoms(System.Object sender, System.EventArgs e)
        {
            if (0 == Interlocked.Exchange(ref ButtonLock, 1)) {
                await Navigation.PushPopupAsync(GetSymptomsPage(), false);
                ButtonLock = 0;
            }
        }


        async void ClickedRisk(System.Object sender, System.EventArgs e)
        {
            if (0 == Interlocked.Exchange(ref ButtonLock, 1)) {
                string risk = await NetworkLayer.GetRisk();
                if (risk == null) {
                    risk = "unknown";
                }
                await DisplayAlert("Your risk level", "According to our calculations your risk is level " + risk + ".", "OK");
                ButtonLock = 0;
            }
        }


        void NewServerIp(System.Object sender, System.EventArgs e)
        {
            string newIP = IpAddress.Text != null ? IpAddress.Text : IpAddress.Placeholder;
            AppConfiguration.REGISTRATION_SERVER_URL = new UriBuilder("http", newIP, 8000);
            DisplayAlert("Updated", "Server IP updated to " + newIP + ".", "OK");
        }
    }
}
