﻿using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CovidTracker
{
    public class DeviceLocation
    {
        public static string StaticId = null;
        public static string StaticOs = null;
        public string Id;
        public string Os;
        public long Timestamp;
        public double Latitude;
        public double Longitude;
        public double? Altitude;
        public double? Speed;
        public double? Course;

        private DeviceLocation(long timestamp, double latitude, double longitude, double? altitude, double? speed, double? course)
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
            Id = StaticId;
            Os = StaticOs;
            Timestamp = timestamp;
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            Speed = speed;
            Course = course;
        }

        private static async Task<DeviceLocation> GetDeviceLocationObject()
        {
            DeviceLocation deviceLocation = null;

            try {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best);
                Location location = await Geolocation.GetLocationAsync(request);

                if (location != null) {
                    deviceLocation = new DeviceLocation(location.Timestamp.ToUnixTimeMilliseconds(), location.Latitude, location.Longitude,
                                                        location.Altitude, location.Speed, location.Course);
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
            DeviceLocation[] deviceLocation = new DeviceLocation[1];
            deviceLocation[0] = await GetDeviceLocationObject();
            if (deviceLocation == null) {
                return;
            }

            String jsonString = JsonConvert.SerializeObject(deviceLocation);
            Debug.WriteLine(jsonString);


        }

    }
}
