using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidTracker
{
    public class NetworkLayer
    {
        public static async Task<bool> SendDataToServer(string data)
        {
            HttpClient client = new HttpClient();
            Debug.WriteLine(data, "[SendDataToServer]");

            try {
                HttpResponseMessage response = await client.PostAsync(AppConfiguration.LOCATION_SERVER_URL.Uri, new StringContent(data));
                if (response.IsSuccessStatusCode) {
                    Debug.WriteLine("Success", "[SendDataToServer]");
                    return true;
                }
                else {
                    Debug.WriteLine("FAILED: " + response.ReasonPhrase, "[SendDataToServer]");
                }
            }
            catch (Exception e) {
                Debug.WriteLine("Exception: " + e.Message, "[SendDataToServer]");
            }

            return false;
        }
    }
}
