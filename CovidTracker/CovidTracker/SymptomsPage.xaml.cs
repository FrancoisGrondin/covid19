using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace CovidTracker
{
    public partial class SymptomsPage : PopupPage
    {
        SymptomsPageVM SymptomsPageVM;

        public SymptomsPage()
        {
            InitializeComponent();
            SymptomsPageVM = new SymptomsPageVM();
            BindingContext = SymptomsPageVM;
            CloseWhenBackgroundIsClicked = false;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        void CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            SymptomsPageVM.CheckedChanged(sender, e);
        }
    }
}
