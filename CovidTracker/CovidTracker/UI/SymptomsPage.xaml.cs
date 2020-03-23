using System;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

namespace CovidTracker
{
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
                DisplayAlert("Report sent", "Thank you for letting us know. The information will stay confidential.", "OK");
            }
            else if (status == SymptomsPageVM.Status.Error) {
                DisplayAlert("Not sent", "The report could not be sent due to a network error. Please check your connection and try again.", "OK");
            }
            OnPageExit(this, status);
            Navigation.PopPopupAsync(false);
        }

    }
}
