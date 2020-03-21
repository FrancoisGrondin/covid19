using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CovidTracker
{
    public class SymptomsPageVM
    {
        public event EventHandler<bool> OnPageExit;

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
        void ReportSymptoms()
        {
            Symptoms symptoms = new Symptoms();
            foreach (Symptom symptom in TestsList) {
                typeof(Symptoms).GetField(symptom.Id).SetValue(symptoms, symptom.IsChecked);
            }
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
