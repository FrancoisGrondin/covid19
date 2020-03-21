using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace CovidTracker
{
    public partial class SymptomsPage : PopupPage
    {
        public event EventHandler<bool> OnPageExit;

        SymptomsPageVM SymptomsPageVM;

        public SymptomsPage()
        {
            InitializeComponent();
            SymptomsPageVM = new SymptomsPageVM();
            SymptomsPageVM.OnPageExit += PageExit;
            BindingContext = SymptomsPageVM;
            CloseWhenBackgroundIsClicked = false;
        }


        protected override bool OnBackgroundClicked()
        {
            return false;
        }


        void PageExit(object sender, bool reportSent)
        {
            if (reportSent) {
                DisplayAlert("Report sent", "Thank you for letting us know. The information will stay confidential.", "OK");
            }
            Navigation.PopPopupAsync();
        }


    }
}
