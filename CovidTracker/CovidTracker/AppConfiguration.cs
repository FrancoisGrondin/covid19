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
        public static readonly string BG_WHITE = "#ffffff";


        /* ---------------------------------------------------------------------------------- 
         * SERVERS CONFIGURATION
         */

        public static readonly UriBuilder REGISTRATION_SERVER_URL = new UriBuilder("http", "192.168.0.104", 3000);
        public static readonly UriBuilder LOCATION_SERVER_URL = new UriBuilder("http", "192.168.0.104", 8000);


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
        public static readonly string VERSION_NAME = "1.0.0";
        public static readonly int VERSION_CODE = 010000;

    }
}
