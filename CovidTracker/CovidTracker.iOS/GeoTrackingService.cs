﻿using System;
using CoreLocation;
using Foundation;
using UIKit;

namespace CovidTracker.iOS
{
    public class GeoTrackingService
    {
        protected CLLocationManager LocationManager;
        private LocationsAggregator LocationsAggregator;


        public GeoTrackingService()
        {
            LocationManager = new CLLocationManager();
            LocationManager.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                LocationManager.RequestAlwaysAuthorization();
            }

            // iOS 9 requires the following for background location updates
            // By default this is set to false and will not allow background updates
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0)) {
                LocationManager.AllowsBackgroundLocationUpdates = true;
            }

        }

        public CLLocationManager LocMgr {
            get { return this.LocationManager; }
        }


        public async void StartLocationUpdates()
        {
            // We need the user's permission for our app to use the GPS in iOS. This is done either by the user accepting
            // the popover when the app is first launched, or by changing the permissions for the app in Settings
            if (CLLocationManager.LocationServicesEnabled) {
                //set the desired accuracy, in meters
                LocationManager.DesiredAccuracy = 1;
                LocationManager.DistanceFilter = AppConfiguration.MINIMUM_DISTANCE_M;
                LocationManager.ShowsBackgroundLocationIndicator = true;
                LocationManager.AllowsBackgroundLocationUpdates = true;
                LocationManager.RequestAlwaysAuthorization();

                LocationsAggregator = await LocationsAggregator.GetInstance();

                LocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
                    foreach (CLLocation location in e.Locations) {
                        LocationsAggregator.RecordLocation(location.Coordinate.Longitude, location.Coordinate.Latitude,
                                                           location.Course, location.Speed, location.HorizontalAccuracy);
                    }
                };

                LocationManager.StartUpdatingLocation();
            }
        }
    }
}

