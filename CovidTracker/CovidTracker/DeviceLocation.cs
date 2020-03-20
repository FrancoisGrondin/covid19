using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace CovidTracker
{
    public class DeviceLocation
    {
        public static string StaticId = null;
        public static string StaticOs = null;
        public string Action;
        public string Id;
        public string Os;
        public long Timestamp;
        public double Latitude;
        public double Longitude;
        public double? Altitude;
        public double? Speed;
        public double? Course;
        public double? Accuracy;

        private DeviceLocation(double latitude, double longitude, double? altitude, double? speed, double? course, double? accuracy)
        {
            if (StaticId == null) {
                StaticId = Preferences.Get("COVID_TRACKER_ID", "testingId");
            }
            if (StaticOs == null) {
                if (Device.RuntimePlatform == Device.iOS) {
                    StaticOs = "iOS";
                }
                else {
                    StaticOs = "Android";
                }
            }

            Action = "track";
            Id = StaticId;
            Os = StaticOs;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Latitude = latitude;
            Longitude = longitude;

            // TODO -- Remove the checks once the server will deal with 'null'
            Altitude = altitude != null ? altitude : 0;
            Speed = speed != null ? speed : 0;
            Course = course != null ? course : 0;
            Accuracy = accuracy != null ? accuracy : 0;
        }

        public static async Task SendLocationInformationToServer(double latitude, double longitude, double? altitude,
                                                                 double? speed, double? course, double? accuracy)
        {
            DeviceLocation[] deviceLocation = new DeviceLocation[1];
            deviceLocation[0] = new DeviceLocation(latitude, longitude, altitude, speed, course, accuracy);

            await SendDeviceLocationToServer(deviceLocation);
        }

        public static async Task SendDeviceLocationToServer(DeviceLocation[] deviceLocation)
        {
            try {
                HttpClient client = new HttpClient();
                string data = JsonConvert.SerializeObject(deviceLocation);
                Debug.WriteLine("jsonString: " + data, "[SendDeviceLocationToServer]");
                HttpResponseMessage response = await client.PostAsync(AppConfiguration.LOCATION_SERVER_URL.Uri, new StringContent(data));
                if (response.IsSuccessStatusCode) {
                    Debug.WriteLine("Success", "[SendDeviceLocationToServer]");
                }
                else {
                    Debug.WriteLine("FAILED: " + response.ReasonPhrase, "[SendDeviceLocationToServer]");
                }
            }
            catch (Exception e) {
                Debug.WriteLine("Exception: " + e.Message, "[SendDeviceLocationToServer]");
            }

        }

    }
}
