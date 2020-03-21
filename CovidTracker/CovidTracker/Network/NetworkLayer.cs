using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CovidTracker
{
    public class NetworkLayer
    {
        private class JsonId
        {
            public string id;
        }

        public static async Task<string> RegisterAndGetId()
        {
            HttpClient client = new HttpClient();
            Debug.WriteLine(AppConfiguration.REGISTER_ID_JSON, "[RegisterAndGetId]");

            try {
                HttpResponseMessage response = await client.PostAsync(AppConfiguration.REGISTRATION_SERVER_URL.Uri,
                                                                      new StringContent(AppConfiguration.REGISTER_ID_JSON));
                if (response.IsSuccessStatusCode) {
                    Debug.WriteLine("Success", "[RegisterAndGetId]");
                    string rawData = await response.Content.ReadAsStringAsync();
                    JsonId jsonId = JsonConvert.DeserializeObject<JsonId>(rawData);
                    Debug.WriteLine("Id: " + jsonId.id, "[RegisterAndGetId]");
                    return jsonId.id;
                }
                else {
                    Debug.WriteLine("FAILED: " + response.ReasonPhrase, "[RegisterAndGetId]");
                }
            }
            catch (Exception e) {
                Debug.WriteLine("Exception: " + e.Message, "[RegisterAndGetId]");
            }

            return "null";
        }


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
