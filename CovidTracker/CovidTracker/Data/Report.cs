using System;
using Xamarin.Essentials;

namespace CovidTracker
{
    public class Report
    {
        public string action = "track";
        public string id;
        public long timestamp;
        public Symptoms symptoms;

        public Report(Symptoms symptoms)
        {
            this.id = Preferences.Get(AppConfiguration.PREF_ID, "null");
            this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.symptoms = symptoms;
        }
    }
}
