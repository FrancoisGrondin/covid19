using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CovidTracker
{
    public class Symptom : INotifyPropertyChanged
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
                OnPropertyChanged("IsChecked");

            }
        }


        public Symptom(string ID, string Description)
        {
            this.ID = ID;
            this.Description = Description;
            this._isChecked = Preferences.Get(ID, false);
        }


        public ICommand TapCommand => new Command(Tapped);

        void Tapped()
        {
            IsChecked = !IsChecked;
        }


        #region INotifyChangedProperties
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyChangedProperties

    }
}
