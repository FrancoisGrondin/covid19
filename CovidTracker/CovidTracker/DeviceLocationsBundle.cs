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
            return (deviceLocations.Count + 1 > AppConfiguration.LOCATIONS_BUFFER);
        }


        public void AddLocation(DeviceLocation deviceLocation)
        {
            this.deviceLocations.Add(deviceLocation);
            Debug.WriteLine("Recorded locations: " + deviceLocations.Count);
        }


        public void ClearLocations()
        {
            this.deviceLocations.Clear();
        }
    }
}
