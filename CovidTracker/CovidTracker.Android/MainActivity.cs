﻿using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;

namespace CovidTracker.Droid
{
    [Activity(Label = "CovidTracker",
              Icon = "@mipmap/icon",
              Theme = "@style/MainTheme",
              LaunchMode = LaunchMode.SingleTask,
              MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              ScreenOrientation = ScreenOrientation.Portrait)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CheckPermissionsAndStart();
        }

        private void StartGeoTrackingService()
        {
            Intent startServiceIntent = new Intent(this, typeof(GeoTrackingService));
            startServiceIntent.SetAction(GeoTrackingService.ACTION_START_SERVICE);
            StartService(startServiceIntent);
        }

        protected override void OnDestroy()
        {
            Intent stopServiceIntent = new Intent(this, typeof(GeoTrackingService));
            stopServiceIntent.SetAction(GeoTrackingService.ACTION_STOP_SERVICE);
            StopService(stopServiceIntent);
            base.OnDestroy();
        }

        private void CheckPermissionsAndStart()
        {
            string[] required = new string[] { Android.Manifest.Permission.AccessFineLocation,
                                               Android.Manifest.Permission.AccessCoarseLocation,
                                               Android.Manifest.Permission.AccessNetworkState };
            bool missingPermission = false;
            foreach (string permission in required) {
                if (((int)Build.VERSION.SdkInt > 22) && (CheckSelfPermission(permission) != Permission.Granted)) {
                    missingPermission = true;
                }
            }

            if (missingPermission) {
                RequestPermissions(required, required.Length);
            }
            else {
                StartGeoTrackingService();
                LoadApplication(new App());
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            StartGeoTrackingService();
            LoadApplication(new App());
        }

    }
}