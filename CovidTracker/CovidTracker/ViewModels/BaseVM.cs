using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CovidTracker
{
    public class BaseVM : INotifyPropertyChanged
    {
        public enum ProcessState { RUNNING, IDLE };

        private ProcessState _processingApiCall = ProcessState.IDLE;
        public ProcessState ProcessingApiCall {
            set { SetProperty(ref _processingApiCall, value); }
            get { return _processingApiCall; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value)) {
                return false;
            }
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

    }
}
