using System;
using System.Collections.ObjectModel;

namespace CovidTracker
{
    public class SymptomsPageVM
    {
        ObservableCollection<Symptom> _symptomsLists = new ObservableCollection<Symptom>();
        public ObservableCollection<Symptom> SymptomsList { get { return _symptomsLists; } }

        public SymptomsPageVM()
        {
            foreach (string[] content in AppConfiguration.SYMPTOMS) {
                Symptom symptom = new Symptom(content[0], content[1]);
                SymptomsList.Add(symptom);
            }
        }

        public void CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            object x = sender;
        }

    }
}
