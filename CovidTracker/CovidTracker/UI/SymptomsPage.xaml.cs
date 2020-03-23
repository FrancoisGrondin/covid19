using System;
using CovidTracker.Localization;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace CovidTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SymptomsPage : PopupPage
    {
        public event EventHandler<SymptomsPageVM.Status> OnPageExit;

        public SymptomsPage()
        {
            InitializeComponent();
            SymptomsPageVM symptomsPageVM = new SymptomsPageVM();
            symptomsPageVM.OnPageExit += PageExit;
            BindingContext = symptomsPageVM;
            CloseWhenBackgroundIsClicked = false;
        }


        protected override bool OnBackgroundClicked()
        {
            return false;
        }


        void PageExit(object sender, SymptomsPageVM.Status status)
        {
            if (status == SymptomsPageVM.Status.Success) {
                DisplayAlert(LocResources.ReportSentTitle, LocResources.ReportSentBody, LocResources.Ok);
            }
            else if (status == SymptomsPageVM.Status.Error) {
                DisplayAlert(LocResources.ReportNotSentTitle, LocResources.ReportNotSentBody, LocResources.Ok);
            }
            OnPageExit(this, status);
            Navigation.PopPopupAsync(false);
        }

    }
}
