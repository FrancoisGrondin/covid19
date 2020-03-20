using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CovidTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void ClickedSymptoms(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("Got it!", "Thank you for letting us know. This information will stay confidential.", "OK");
        }

        void ClickedRisk(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("Your risk level", "According to our calculations your risk is X. [plus additional recommandations]", "OK");
        }
    }
}
