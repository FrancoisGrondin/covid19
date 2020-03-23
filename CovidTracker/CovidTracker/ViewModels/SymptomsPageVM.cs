using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CovidTracker
{
    public class SymptomsPageVM : BaseVM
    {
        public enum Status
        {
            Success = 0,
            Canceled = 1,
            Error = 2
        };

        public event EventHandler<Status> OnPageExit;

        ObservableCollection<Symptom> _testsLists = new ObservableCollection<Symptom>();
        public ObservableCollection<Symptom> TestsList { get { return _testsLists; } }

        ObservableCollection<Symptom> _symptomsLists = new ObservableCollection<Symptom>();
        public ObservableCollection<Symptom> SymptomsList { get { return _symptomsLists; } }


        public SymptomsPageVM()
        {
            foreach (FieldInfo field in typeof(Symptoms).GetFields()) {
                if (field.Name.Equals("tested_positive")) {
                    Symptom symptom = new Symptom(field.Name);
                    TestsList.Add(symptom);
                }
            }
            foreach (FieldInfo field in typeof(Symptoms).GetFields()) {
                if (!field.Name.Equals("tested_positive")) {
                    Symptom symptom = new Symptom(field.Name);
                    SymptomsList.Add(symptom);
                }
            }
        }


        public ICommand ReportSymptomsCommand => new Command(ReportSymptoms);
        async void ReportSymptoms()
        {
            Symptoms symptoms = new Symptoms();
            foreach (Symptom symptom in TestsList) {
                typeof(Symptoms).GetField(symptom.Id).SetValue(symptoms, symptom.IsChecked);
            }
            foreach (Symptom symptom in SymptomsList) {
                typeof(Symptoms).GetField(symptom.Id).SetValue(symptoms, symptom.IsChecked);
            }

            ProcessingApiCall = ProcessState.RUNNING;
            bool success = await SendSymptoms(symptoms);
            ProcessingApiCall = ProcessState.IDLE;

            if (success) {
                OnPageExit(this, Status.Success);
            }
            else {
                OnPageExit(this, Status.Error);
            }
        }


        private async Task<bool> SendSymptoms(Symptoms symptoms)
        {
            Report report = new Report(symptoms);
            report.id = await LocationsAggregator.RetrieveId();
            if (report.id == null) {
                return false;
            }

            string data = JsonConvert.SerializeObject(report);
            bool success = await NetworkLayer.SendDataToServer(data);
            return success;
        }


        public ICommand CancelCommand => new Command(Cancel);
        void Cancel()
        {
            OnPageExit(this, Status.Canceled);
        }

    }
}
