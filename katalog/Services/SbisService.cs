using katalog.Services.Interfaces;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using static System.Net.WebRequestMethods;
using katalog.SbisData;

namespace katalog.Services
{
    public class SbisService : ISbisService
    {
        private string? token;
        private string? sid;
        private string baseUrl = "https://online.sbis.ru";
        private string clientId = "3067279040044919";
        private string appSecret = "KM59TNUJ17XGC4MTNX9ZUCNB";
        private string secretKey = "8AcBv0Gw5wi5jZoJqgqyMkLQSLwynBk8Y4pMmRMiAjDBMHLfeVv015uvL3ZCBRbMf1dU0D8n1VlgaCwKBUxKogSXaynBjgsV6u4DfO9l3TxXdRbvu6Kd6j";

        public async Task Auth(string login, string password)
        {
            if (String.IsNullOrEmpty(token))
            {
                var response = await GetServiceToken();
                token = response?.AccessToken;
                sid = response?.Sid;
            }
        }
        private async Task<AuthResponse?> GetServiceToken()
        {
            string authUrl = "oauth/service";
            NameValueCollection requestParams = new()
              {
                { "app_client_id", clientId },
                { "app_secret", appSecret },
                { "secret_key", secretKey }
              };
            var request = await SendRequestAsync<AuthResponse>(HttpMethod.Post, authUrl, requestParams);
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
        private async Task<T?> SendRequestAsync<T>(HttpMethod httpMethod, string entity, NameValueCollection parameters)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var queryString = SerializeParameters(parameters);
            Console.WriteLine(queryString);
            var url = BuildUrl(entity: entity, queryString: queryString);
            using (var httpRequest = CreateHttpRequest(verb: httpMethod, url: url))
            using (var httpResponse = await client.SendAsync(httpRequest))
            {
                httpResponse.EnsureSuccessStatusCode();
                string apiResponse = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(apiResponse);
                return System.Text.Json.JsonSerializer.Deserialize<T>(apiResponse);
            }
        }
        private async Task<T?> SendRequestAsync<T>(HttpMethod httpMethod, string entity)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url = BuildUrl(entity: entity);
            using (var httpRequest = CreateHttpRequest(verb: httpMethod, url: url))
            using (var httpResponse = await client.SendAsync(httpRequest))
            {
                httpResponse.EnsureSuccessStatusCode();
                string apiResponse = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(apiResponse);
                return System.Text.Json.JsonSerializer.Deserialize<T>(apiResponse);
            }
        }
        private string BuildUrl(string entity, string? queryString = null)
        {
            var url = baseUrl;
            if (!String.IsNullOrEmpty(entity))
            {
                url = String.Format("{0}/{1}", url, entity);
            }
            if (queryString != null)
            {
                url += "?" + queryString;
            }
            return url;
        }
        private HttpRequestMessage CreateHttpRequest(HttpMethod verb, string url)
        {
            var request = new HttpRequestMessage(verb, url);
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("X-SBISAccessToken", this.token);
            }
            return request;
        }
    }

}
