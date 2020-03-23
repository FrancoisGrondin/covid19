using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace CovidTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();


        }

        protected override void OnStart()
        {
            AppCenter.Start("ios=dd7d91e1-ddce-45df-a353-70c77eb75040;" +
                            "android=547b2b02-721e-451f-a9fd-9279b3adbe61",
                            typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
