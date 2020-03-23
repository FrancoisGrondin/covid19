using System;

namespace CovidTracker
{
    public static class AppConfiguration
    {
        /* ---------------------------------------------------------------------------------- 
         * UI COLOR DEFINITIONS 
         */

        public static readonly string BG_PRIMARY = "#007bff";
        public static readonly string BG_SECONDARY = "#6c757d";
        public static readonly string BG_SUCCESS = "#28a745";
        public static readonly string BG_DANGER = "#dc3545";
        public static readonly string BG_WARNING = "#ffc107";
        public static readonly string BG_INFO = "#17a2b8";
        public static readonly string BG_LIGHT = "#f8f9fa";
        public static readonly string BG_DARK = "#343a40";
        public static readonly string BG_BLACK = "#000000";
        public static readonly string BG_WHITE = "#ffffff";


        /* ---------------------------------------------------------------------------------- 
         * SERVERS CONFIGURATION
         */

        public static UriBuilder REGISTRATION_SERVER_URL = new UriBuilder("http", "192.222.192.89", 8000);
        public static UriBuilder LOCATION_SERVER_URL = new UriBuilder("http", "192.222.192.89", 8000);


        /* ---------------------------------------------------------------------------------- 
         * LOCATIONS SAMPLING
         */

        // MINIMUM_TIME_MS milliseconds must have elapsed for the 
        // device to trigger a location change (Android only)
        public static readonly int MINIMUM_TIME_MS = 0;

        // The location must have changed by at least a distance
        // of MINIMUM_DISTANCE_M meters for the device to trigger
        // a location change (Android, iOS)
        public static readonly int MINIMUM_DISTANCE_M = 2;

        // Maximum number of locations the buffer can store, 3600 is equivalent to at least 60 minutes
        // If the network is down, the oldest location will be erased to make room for a newest location 
        public static readonly int LOCATIONS_BUFFER_MAX = 3600;

        // Number of location changes to record before sending the bundle to the server
        public static readonly int LOCATIONS_SAMPLING_RATE = 3;


        /* ---------------------------------------------------------------------------------- 
         * PREFERENCE NAMES
         */

        public static readonly string PREF_ID = "COVID_TRACKER_ID";


        /* ---------------------------------------------------------------------------------- 
         * DO NOT CHANGE THE LINES BELOW
         * Instead, run version.sh <version number>
         *     E.g.
         *     $ ./version.sh 2.0.33
         * This will update all the following files properly:
         *     XDocks.Android/Properties/AndroidManifest.xml
         *     XDocks.Android/XDocks.Android.csproj
         *     XDocks.iOS/Info.plist
         *     XDocks.iOS/XDocks.iOS.csproj
         *     XDocks.sln
         *     XDocks/AppConfig.cs
         *     XDocks/XDocks.csproj
         */
        public static readonly string VERSION_NAME = "1.0.3";
        public static readonly int VERSION_CODE = 010003;

    }
}
