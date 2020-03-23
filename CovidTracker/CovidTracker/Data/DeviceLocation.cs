using System;

namespace CovidTracker
{
    public class DeviceLocation
    {
        public long timestamp;
        public double latitude;
        public double longitude;
        public double? speed;
        public double? course;
        public double? accuracy;


        public DeviceLocation(double latitude, double longitude, double? speed, double? course, double? accuracy)
        {
            this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.latitude = latitude;
            this.longitude = longitude;
            this.speed = speed;
            this.course = course;
            this.accuracy = accuracy;
        }

    }
}
