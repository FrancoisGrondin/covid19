using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CovidTracker
{
    public class SymptomsPageVM
    {
        public event EventHandler<bool> OnPageExit;

        ObservableCollection<Symptom> _symptomsLists = new ObservableCollection<Symptom>();
        public ObservableCollection<Symptom> SymptomsList { get { return _symptomsLists; } }

        public SymptomsPageVM()
        {
            foreach (string[] content in Symptoms.LIST) {
                Symptom symptom = new Symptom(content[0], content[1]);
                SymptomsList.Add(symptom);
            }
        }

        public ICommand ReportSymptomsCommand => new Command(ReportSymptoms);
        void ReportSymptoms()
        {
            Symptoms symptoms = new Symptoms();
            foreach (Symptom symptom in SymptomsList) {
                typeof(Symptoms).GetField(symptom.Id).SetValue(symptoms, symptom.IsChecked);
            }
            Report report = new Report(symptoms);
            string data = JsonConvert.SerializeObject(report);
            NetworkLayer.SendDataToServer(data);
            OnPageExit(this, true);
        }

        public ICommand CancelCommand => new Command(Cancel);
        void Cancel()
        {
            OnPageExit(this, false);
        }

    }
}
