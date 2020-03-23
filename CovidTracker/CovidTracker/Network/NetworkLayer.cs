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
            ServerResponse response = await IssueRequest(Request.REGISTER, AppConfiguration.REGISTRATION_SERVER_URL.Uri);
            return response?.id;
        }


        public static async Task<string> GetRisk()
        {
            ServerResponse response = await IssueRequest(Request.GET_RISK, AppConfiguration.LOCATION_SERVER_URL.Uri, true);
            return response?.level;
        }


        private static async Task<ServerResponse> IssueRequest(string action, Uri uri, bool addId = false)
        {
            HttpClient client = new HttpClient();
            Request request = new Request(action);
            if (addId) {
                request.id = await LocationsAggregator.RetrieveId();
            }
            string requestString = JsonConvert.SerializeObject(request);
            Debug.WriteLine(requestString, "[IssueRequest]");

            try {
                HttpResponseMessage response = await client.PostAsync(uri, new StringContent(requestString));
                if (response.IsSuccessStatusCode) {
                    Debug.WriteLine("Success", "[IssueRequest]");
                    string rawResult = await response.Content.ReadAsStringAsync();
                    try {
                        return JsonConvert.DeserializeObject<ServerResponse>(rawResult);
                    }
                    catch (JsonSerializationException e) {
                        Debug.WriteLine("Exception: " + e.Message, "[IssueRequest]");
                    }
                    return new ServerResponse();
                }
                else {
                    Debug.WriteLine("FAILED: " + response.ReasonPhrase, "[IssueRequest]");
                }
            }
            catch (Exception e) {
                Debug.WriteLine("Exception: " + e.Message, "[IssueRequest]");
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
