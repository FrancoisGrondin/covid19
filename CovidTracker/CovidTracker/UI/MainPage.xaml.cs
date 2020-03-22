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
                await Navigation.PushPopupAsync(GetSymptomsPage());
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
                await DisplayAlert("Your risk level", "According to our calculations your risk is " + risk + ".", "OK");
                ButtonLock = 0;
            }
        }
    }
}
