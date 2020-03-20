using System;
using CoreLocation;
using Foundation;
using UIKit;

namespace CovidTracker.iOS
{
    public class GeoTrackingService
    {
        protected CLLocationManager locMgr;

        public GeoTrackingService()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                locMgr.RequestAlwaysAuthorization();
            }

            // iOS 9 requires the following for background location updates
            // By default this is set to false and will not allow background updates
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0)) {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }

        }

        public CLLocationManager LocMgr {
            get { return this.locMgr; }
        }

        public void StartLocationUpdates()
        {
            // We need the user's permission for our app to use the GPS in iOS. This is done either by the user accepting
            // the popover when the app is first launched, or by changing the permissions for the app in Settings
            if (CLLocationManager.LocationServicesEnabled) {
                //set the desired accuracy, in meters
                LocMgr.DesiredAccuracy = CLLocation.AccuracyBest;
                LocMgr.DistanceFilter = AppConfiguration.MINIMUM_DISTANCE_M;

                LocMgr.LocationsUpdated += async (object sender, CLLocationsUpdatedEventArgs e) => {
                    foreach (CLLocation location in e.Locations) {
                        await DeviceLocation.SendLocationInformationToServer(location.Coordinate.Longitude, location.Coordinate.Latitude,
                                                                             location.Altitude, location.Course, location.Speed, location.HorizontalAccuracy);
                    }
                };

                LocMgr.StartUpdatingLocation();
            }
        }
    }
}

