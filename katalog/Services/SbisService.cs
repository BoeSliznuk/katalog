using katalog.Services.Interfaces;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace katalog.Services
{
    public class SbisService : ISbisService
    {
        private string? token;
        private async Task<string?> GetServiceToken(string login, string password)
        {
            NameValueCollection requestParams = new()
              {
                { "app_client_id", "3067279040044919" },
                { "app_secret", "KM59TNUJ17XGC4MTNX9ZUCNB" },
                { "secret_key", "8AcBv0Gw5wi5jZoJqgqyMkLQSLwynBk8Y4pMmRMiAjDBMHLfeVv015uvL3ZCBRbMf1dU0D8n1VlgaCwKBUxKogSXaynBjgsV6u4DfO9l3TxXdRbvu6Kd6j" }
              };
            string json = SerializeParameters(requestParams);
            var request = await SendPostRequestAsync<string>("https://online.sbis.ru/oauth/service/", json);
            return request;
        }
        private string? SerializeParameters(NameValueCollection parameters)
        {
            if (parameters.Count != 0)
            {
                List<string> parts = new List<string>();
                foreach (String? key in parameters.AllKeys)
                    parts.Add(String.Format("{0}={1}", key, parameters[key]));
                return String.Join("&", parts);
            }
            else
            {
                return null;
            }
        }
        private async Task<T?> SendPostRequestAsync<T>(string url, string data)
        {
            var client = new HttpClient();
            using (var httpRequest = CreateHttpRequest(verb: HttpMethod.Post, url: url))
            {
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var options = new JsonSerializerOptions()
                { WriteIndented = true };
                //string data = System.Text.Json.JsonSerializer.Serialize((Tarif)request, options);
                using (var httpContent = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    httpRequest.Content = httpContent;
                    Console.WriteLine(httpRequest);
                    Console.WriteLine(data);
                    using (var httpResponse = await client.SendAsync(httpRequest))
                    {
                        httpResponse.EnsureSuccessStatusCode();
                        string apiResponse = await httpResponse.Content.ReadAsStringAsync();
                        return System.Text.Json.JsonSerializer.Deserialize<T>(apiResponse);
                    }
                }
            }
        }
        private HttpRequestMessage CreateHttpRequest(HttpMethod verb, string url)
        {
            var request = new HttpRequestMessage(verb, url);
            //if (!string.IsNullOrWhiteSpace(token))
            //{
            //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
            //}
            return request;
        }
    }

}
