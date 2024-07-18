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
        private string? pointId;
        private string? priceListId;
        private string baseUrl = "https://api.sbis.ru";
        private string clientId = "3067279040044919";
        private string appSecret = "KM59TNUJ17XGC4MTNX9ZUCNB";
        private string secretKey = "8AcBv0Gw5wi5jZoJqgqyMkLQSLwynBk8Y4pMmRMiAjDBMHLfeVv015uvL3ZCBRbMf1dU0D8n1VlgaCwKBUxKogSXaynBjgsV6u4DfO9l3TxXdRbvu6Kd6j";

        public async Task Auth()
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
            var client = new HttpClient();
            var request = new AuthRequest()
            {
                ClientId = clientId,
                AppSecret = appSecret,
                SecretKey = secretKey
            };
            using (var httpRequest = CreateHttpRequest(verb: HttpMethod.Post, url: "https://online.sbis.ru/oauth/service/"))
            {
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var options = new JsonSerializerOptions()
                { WriteIndented = true };
                string data = JsonSerializer.Serialize(request, options);
                using (var httpContent = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    httpRequest.Content = httpContent;
                    Console.WriteLine(httpRequest);
                    Console.WriteLine(data);
                    using (var httpResponse = await client.SendAsync(httpRequest))
                    {
                        httpResponse.EnsureSuccessStatusCode();
                        string apiResponse = await httpResponse.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<AuthResponse>(apiResponse);
                    }
                }
            }
        }
        private async Task GetPointId()
        {
            if (pointId == null)
            {
                string pointUrl = "retail/point/list";
                var response = await SendRequestAsync<PointResponse>(HttpMethod.Get, pointUrl);
                if (response != null && response.Points.Count > 0)
                {
                    pointId = response.Points.FirstOrDefault().Id.ToString();
                }
            }
        }
        private async Task GetPriceListId()
        {
            if (priceListId == null)
            {
                await GetPointId();
                string priceListUrl = "retail/nomenclature/price-list";
                NameValueCollection requestParams = new()
              {
                { "pointId", pointId },
                { "actualDate", DateTime.Today.ToShortDateString() }
              };

                var response = await SendRequestAsync<PriceListResponse>(HttpMethod.Get, priceListUrl, requestParams);
                if (response != null && response.PriceLists.Count > 0)
                {
                    priceListId = response.PriceLists.FirstOrDefault().Id.ToString();
                }
            }
        }
        private async Task<List<Product>?> GetCatalog()
        {
            await GetPriceListId();
            string catalogUrl = "retail/nomenclature/list";
            NameValueCollection requestParams = new()
              {
                { "pointId", pointId },
                { "priceListId", priceListId },
                {"onlyPublished", "true" },
                {"pageSize", "10000" }
              };
            var response = await SendRequestAsync<ProductResponse>(HttpMethod.Get, catalogUrl, requestParams);
            if (response != null && response.Products.Count > 0)
            {
                return response.Products;
            }
            return null;
        }
        private async Task<List<Product>?> GetCatalogByName(string name)
        {
            await GetPriceListId();
            string catalogUrl = "retail/nomenclature/list";
            NameValueCollection requestParams = new()
              {
                { "pointId", pointId },
                { "priceListId", priceListId },
                {"onlyPublished", "true" },
                {"pageSize", "10000" },
                {"searchString", name }
              };
            var response = await SendRequestAsync<ProductResponse>(HttpMethod.Get, catalogUrl, requestParams);
            if (response != null && response.Products.Count > 0)
            {
                return response.Products;
            }
            return null;
        }
        public async Task<List<Product>?> GetCategories()
        {
            List<Product>? allProducts = await GetCatalog();
            if (allProducts == null) return null;
            return allProducts.Where(x => x.IsParent != null && (bool)x.IsParent).ToList();
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
            Console.WriteLine(url);
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

        public async Task<List<Product>?> GetProducts()
        {
            List<Product>? allProducts = await GetCatalog();
            if (allProducts == null) return null;
            return allProducts.Where(x => x.HierarchicalParent != null).ToList();
        }

        public async Task<List<Product>?> GetProducts(int hierarchicalParent)
        {
            List<Product>? allProducts = await GetCatalog();
            if (allProducts == null) return null;
            return allProducts.Where(x => x.HierarchicalParent == hierarchicalParent).ToList();
        }
        public async Task<List<Product>?> GetProductsSearched(string search)
        {
            List<Product>? allProducts = await GetCatalogByName(search);
            if (allProducts == null) return null;
            return allProducts.Where(x => x.HierarchicalParent != null && x.Published).ToList();
        }
        public async Task<byte[]> GetImage(string url)
        {
            string imgUrl = "retail" + url;
            var client = new HttpClient();
            var requesrUrl = BuildUrl(entity: imgUrl);
            using (var httpRequest = CreateHttpRequest(verb: HttpMethod.Get, url: requesrUrl))
            using (var httpResponse = await client.SendAsync(httpRequest))
            {
                httpResponse.EnsureSuccessStatusCode();
                return await httpResponse.Content.ReadAsByteArrayAsync();
            }
        }
    }

}
