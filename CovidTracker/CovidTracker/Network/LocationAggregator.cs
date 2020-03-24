using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace CovidTracker
{
    public class LocationsAggregator
    {
        private static LocationsAggregator Instance;
        private DeviceLocationsBundle DeviceLocationsBundle;


        public static async Task<LocationsAggregator> GetInstance()
        {
            if (Instance == null) {
                string id = await RetrieveId();
                Instance = new LocationsAggregator(id);
            }

            return Instance;
        }


        public static async Task<string> RetrieveId(bool force = false)
        {
            string id = Preferences.Get(AppConfiguration.PREF_ID, null);
            if (force || id == null) {
                id = await NetworkLayer.RegisterAndGetId();
                Preferences.Set(AppConfiguration.PREF_ID, id);
            }
            return id;
        }


        public LocationsAggregator(string id)
        {
            DeviceLocationsBundle = new DeviceLocationsBundle(id);
        }


        public void RecordLocation(double latitude, double longitude,
                                   double? speed, double? course, double? accuracy)
        {
            DeviceLocationsBundle.AddLocation(new DeviceLocation(latitude, longitude, speed, course, accuracy));

            if (DeviceLocationsBundle.SampleFilled()) {
                SendDataToServer();
            }
        }


        private async void SendDataToServer()
        {
            // Before sending, make sure we have an Id. Do not send if we can't retrieve it
            DeviceLocationsBundle.id = await RetrieveId();
            if (DeviceLocationsBundle.id == null) {
                return;
            }

            string data = JsonConvert.SerializeObject(DeviceLocationsBundle);
            bool success = await NetworkLayer.SendDataToServer(data);
            if (success) {
                DeviceLocationsBundle.ClearLocations();
            }
        }


    }


}
