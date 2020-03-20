using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CovidTracker
{
    public class DeviceLocation
    {
        public static string Id = null;
        public static string Os = null;
        public long timestamp;
        public double Latitude;
        public double Longitude;
        public double? Altitude;

        private DeviceLocation(double latitude, double longitude, double? altitude)
        {
            if (Id == null) {
                Id = Preferences.Get("COVID_TRACKER_ID", "null");
            }
            if (Os == null) {
                if (Device.RuntimePlatform == Device.iOS) {
                    Os = "iOS";
                }
                else {
                    Os = "Android";
                }
            }
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        private static async Task<DeviceLocation> GetDeviceLocationObject()
        {
            DeviceLocation deviceLocation = null;

            try {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best);
                Location location = await Geolocation.GetLocationAsync(request);

                if (location != null) {
                    deviceLocation = new DeviceLocation(location.Latitude, location.Longitude, location.Altitude);
                }
            }
            catch (FeatureNotSupportedException fnsEx) {
                Console.WriteLine(fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx) {
                Console.WriteLine(fneEx.Message);
            }
            catch (PermissionException pEx) {
                // TODO -- display message to the user
                Console.WriteLine(pEx.Message);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return deviceLocation;
        }

        public static async Task SendLocationInformationToServer()
        {
            DeviceLocation deviceLocation = await GetDeviceLocationObject();
            if (deviceLocation == null) {
                return;
            }

            String jsonString = JsonConvert.SerializeObject(deviceLocation);
            Debug.WriteLine(jsonString);


        }

    }
}
