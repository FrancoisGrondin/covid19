using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CovidTracker
{
    public class NetworkLayer
    {
        public static async Task<string> RegisterAndGetId()
        {
            ServerResponse response = await IssueRequest(Request.REGISTER);
            return response?.id;
        }


        public static async Task<string> GetRisk()
        {
            ServerResponse response = await IssueRequest(Request.GET_RISK);
            return response?.risk;
        }


        private static async Task<ServerResponse> IssueRequest(string action)
        {
            HttpClient client = new HttpClient();
            string request = JsonConvert.SerializeObject(new Request(action));
            Debug.WriteLine(request, "[IssueRequest]");

            try {
                HttpResponseMessage response = await client.PostAsync(AppConfiguration.REGISTRATION_SERVER_URL.Uri,
                                                                      new StringContent(request));
                if (response.IsSuccessStatusCode) {
                    Debug.WriteLine("Success", "[RegisterAndGetId]");
                    string rawResult = await response.Content.ReadAsStringAsync();
                    try {
                        return JsonConvert.DeserializeObject<ServerResponse>(rawResult);
                    }
                    catch (JsonSerializationException e) {
                        Debug.WriteLine("Exception: " + e.Message, "[RegisterAndGetId]");
                    }
                    return new ServerResponse();
                }
                else {
                    Debug.WriteLine("FAILED: " + response.ReasonPhrase, "[RegisterAndGetId]");
                }
            }
            catch (Exception e) {
                Debug.WriteLine("Exception: " + e.Message, "[RegisterAndGetId]");
            }

            return new ServerResponse();
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
