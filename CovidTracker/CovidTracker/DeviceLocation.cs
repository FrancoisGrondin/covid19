using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CovidTracker
{
    public class DeviceLocation
    {
        public static string StaticId = null;
        public static string StaticOs = null;
        public string action;
        public string id;
        public string os;
        public long timestamp;
        public double latitude;
        public double longitude;
        public double? speed;
        public double? course;
        public double? accuracy;

        private DeviceLocation(double latitude, double longitude, double? speed, double? course, double? accuracy)
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

            action = "track";
            id = StaticId;
            os = StaticOs;
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.latitude = latitude;
            this.longitude = longitude;

            // TODO -- Remove the checks once the server will deal with 'null'
            this.speed = speed != null ? speed : 0;
            this.course = course != null ? course : 0;
            this.accuracy = accuracy != null ? accuracy : 0;
        }

        public static async Task SendLocationInformationToServer(double latitude, double longitude,
                                                                 double? speed, double? course, double? accuracy)
        {
            DeviceLocation[] deviceLocation = new DeviceLocation[1];
            deviceLocation[0] = new DeviceLocation(latitude, longitude, speed, course, accuracy);

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
