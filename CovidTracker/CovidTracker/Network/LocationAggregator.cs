using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CovidTracker
{
    public class LocationsAggregator
    {
        private static LocationsAggregator Instance;
        private DeviceLocationsBundle DeviceLocationsBundle;


        public static async Task<LocationsAggregator> GetInstance()
        {
            if (Instance == null) {
                string id = await CheckOrGetId();
                Instance = new LocationsAggregator(id);
            }

            return Instance;
        }


        private static async Task<string> CheckOrGetId()
        {
            string id = Preferences.Get(AppConfiguration.PREF_ID, "null");
            if (id.Equals("null")) {
                id = await NetworkLayer.RegisterAndGetId();
                // Get ID from network
                Preferences.Set(AppConfiguration.PREF_ID, id);
            }
            return id;
        }


        public LocationsAggregator(string id)
        {
            DeviceLocationsBundle = new DeviceLocationsBundle(id);
        }


        public async Task RecordLocation(double latitude, double longitude,
                                         double? speed, double? course, double? accuracy)
        {
            await CheckOrGetId();
            DeviceLocationsBundle.AddLocation(new DeviceLocation(latitude, longitude, speed, course, accuracy));
            if (DeviceLocationsBundle.IsFull()) {
                string data = JsonConvert.SerializeObject(DeviceLocationsBundle);
                await NetworkLayer.SendDataToServer(data);
                DeviceLocationsBundle.ClearLocations();
            }

        }


    }
}
