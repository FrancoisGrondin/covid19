using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CovidTracker
{
    public class GeoTracker
    {
        public GeoTracker()
        {
        }

        public async Task GetLocationAsync()
        {
            try {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null) {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx) {
                Console.WriteLine(fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx) {
                Console.WriteLine(fneEx.Message);
            }
            catch (PermissionException pEx) {
                // TODO -- display message
                Console.WriteLine(pEx.Message);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
