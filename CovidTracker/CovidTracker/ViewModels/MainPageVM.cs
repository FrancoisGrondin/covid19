using System;
using System.Windows.Input;
using CovidTracker.Localization;
using Xamarin.Forms;

namespace CovidTracker
{
    public class MainPageVM : BaseVM
    {
        public event EventHandler<string> OnRiskResult;


        public ICommand QueryRiskCommand => new Command(QueryRisk);
        async void QueryRisk()
        {
            ProcessingApiCall = ProcessState.RUNNING;
            string risk = await NetworkLayer.GetRisk();
            if (risk == null) {
                risk = LocResources.Unknown;
            }
            ProcessingApiCall = ProcessState.IDLE;
            OnRiskResult(this, risk);
        }

    }
}
