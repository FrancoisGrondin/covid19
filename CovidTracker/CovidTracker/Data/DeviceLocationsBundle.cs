using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CovidTracker
{
    public class DeviceLocationsBundle
    {
        public string action = "track";
        public string id = null;
        public string os = null;
        public List<DeviceLocation> deviceLocations;


        public DeviceLocationsBundle(string id)
        {
            this.id = id;
            this.os = (Device.RuntimePlatform == Device.iOS ? "iOS" : "Android");
            this.deviceLocations = new List<DeviceLocation>();
        }


        public bool IsFull()
        {
            return (deviceLocations.Count + 1 > AppConfiguration.LOCATIONS_BUFFER_MAX);
        }

        public bool SampleFilled()
        {
            return (deviceLocations.Count % AppConfiguration.LOCATIONS_SAMPLING_RATE == 0);
        }

        public void AddLocation(DeviceLocation deviceLocation)
        {
            if (IsFull()) {
                Debug.WriteLine("Buffer is full, removing oldest element", "[DeviceLocationsBundle]");
                this.deviceLocations.RemoveAt(0);
            }
            this.deviceLocations.Add(deviceLocation);
            Debug.WriteLine("Recorded: " + deviceLocations.Count, "[DeviceLocationsBundle]");
        }


        public void ClearLocations()
        {
            this.deviceLocations.Clear();
        }
    }
}
