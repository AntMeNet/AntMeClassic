using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Online.Client
{
    internal static class HttpConnection
    {
        private static int timeout = 1000;

        //private static Uri authUri = new Uri("http://antmeservice.azurewebsites.net/");
        private static Uri authUri = new Uri("https://localhost:44303/");

        

        private static Uri baseUri = new Uri("http://antmeapi.azurewebsites.net/");

        public static string Token = string.Empty;

        public static Guid ClientId = Guid.Empty;

        public static string ClientVersion = string.Empty;

        internal static HttpClient CreateClient()
        {
            var client = new HttpClient();

            client.BaseAddress = baseUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AntMeClient", ClientVersion));
            client.DefaultRequestHeaders.From = ClientId.ToString() + "@antme.net";

            if (!string.IsNullOrEmpty(Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            return client;
        }

        internal static HttpClient CreateAuthClient()
        {
            var client = new HttpClient();

            client.BaseAddress = authUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AntMeClient", ClientVersion));
            client.DefaultRequestHeaders.From = ClientId.ToString() + "@antme.net";

            return client;
        }

        internal static T Post<T>(string url, object request)
        {
            using (var client = CreateClient())
            {
                HttpContent content = new StringContent(
                    JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> requestTask = client.PostAsync(url, content);
                HandleTask(requestTask);

                if (requestTask.Result.IsSuccessStatusCode)
                {
                    Task<string> responseTask = requestTask.Result.Content.ReadAsStringAsync();
                    HandleTask(responseTask);

                    return JsonConvert.DeserializeObject<T>(responseTask.Result);
                }
                else
                {
                    Task<string> errorTask = requestTask.Result.Content.ReadAsStringAsync();
                    HandleTask(errorTask);
                    throw new Exception(errorTask.Result);
                }
            }
        }

        internal static void HandleTask(Task t)
        {
            if (!t.Wait(timeout))
                throw new TimeoutException();
            if (t.IsFaulted)
                throw t.Exception;
        }
    }
}
