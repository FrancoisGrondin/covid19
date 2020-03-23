using System;

namespace CovidTracker
{
    public class Report
    {
        public string action = "report";
        public string id;
        public long timestamp;
        public Symptoms symptoms;


        public Report(Symptoms symptoms)
        {
            this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.symptoms = symptoms;
        }
    }
}
