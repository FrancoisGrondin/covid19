using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CovidTracker
{
    public class Symptom
    {
        private string ID;
        public string Description { get; set; }

        private bool _isChecked;
        public bool IsChecked {
            get {
                return _isChecked;
            }
            set {
                _isChecked = value;
                Preferences.Set(ID, _isChecked);
            }
        }

        public Symptom(string ID, string Description)
        {
            this.ID = ID;
            this.Description = Description;
            this._isChecked = Preferences.Get(ID, false);
        }

        public ICommand CheckChangedCommand => new Command(CheckChanged);

        void CheckChanged()
        {
            IsChecked = !IsChecked;
        }

    }
}
