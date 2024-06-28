using FrenchyApps42.Web.ApiRequest.Helpers;
using FrenchyApps42.Web.ApiRequest.Interfaces;
using FrenchyApps42.Web.ApiRequest.Structs;
using Newtonsoft.Json;
using System.Text;

namespace FrenchyApps42.Web.ApiRequest
{
    public class ApiRequest : IApiRequest
    {
        private HttpClient _httpClient = new();

        public string URL { get; private set; } = string.Empty;
        public HttpMethod Method { get; private set; } = HttpMethod.Get;

        public Dictionary<string, string> RequestHeaders { get; private set; } = [];
        public Dictionary<string, string> ContentHeaders { get; private set; } = [];
        public Dictionary<string, string> QueryParams { get; private set; } = [];

        public object Body { get; private set; } = null;

        private JsonSerializerSettings _jsonSerializerSettings = new()
        {
            ContractResolver = new JsonPropAttrResolver(),
            Formatting = Formatting.Indented
        };

        public ApiRequest()
        {
        }

        public ApiRequest(string url)
        {
            this.URL = url;
        }

        public ApiRequest(string url, HttpMethod method)
        {
            this.URL = url;
            this.Method = method;
        }

        public ApiRequest AddHeader(string key, string value)
        {
            this.RequestHeaders.Add(key, value);

            return this;
        }

        public ApiRequest AddHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                this.RequestHeaders.Add(header.Key, header.Value);
            }

            return this;
        }

        public ApiRequest AddContentHeader(string key, string value)
        {
            this.ContentHeaders.Add(key, value);

            return this;
        }

        public ApiRequest AddContentHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                this.ContentHeaders.Add(header.Key, header.Value);
            }

            return this;
        }

        public ApiRequest AddQueryParam(string key, string value)
        {
            QueryParams.Add(key, value);

            return this;
        }

        public ApiRequest AddQueryParams(Dictionary<string, string> queryParams)
        {
            foreach (KeyValuePair<string, string> query in queryParams)
            {
                QueryParams.Add(query.Key, query.Value);
            }

            return this;
        }

        public ApiRequest AcceptJson()
        {
            RequestHeaders.Add("Accept", "application/json");

            return this;
        }

        public ApiRequest AddJsonBody(object body)
        {
            Body = body;

            return this;
        }

        public async Task<Result<T>> Run<T>()
        {
            StringBuilder builder = new(URL);
            string fullUrl = URL;

            if (fullUrl.EndsWith("/"))
                builder.Remove(fullUrl.Length - 1, 1);

            if (QueryParams.Count() > 0)
            {
                builder.Append("?");

                for (int i = 0; i < QueryParams.Count(); i++)
                {
                    KeyValuePair<string, string> query = QueryParams.ElementAt(i);
                    builder.Append($"{query.Key}={query.Value}");

                    if (!(i == QueryParams.Count() - 1))
                        builder.Append("&");
                }
            }

            HttpRequestMessage request = new(Method, builder.ToString());

            for (int i = 0; i <= this.RequestHeaders.Count(); i++)
            {
                KeyValuePair<string, string> header = RequestHeaders.ElementAt(i);
                request.Headers.Add(header.Key, header.Value);
            }

            if (Body != null)
            {
                string json = JsonConvert.SerializeObject(Body, _jsonSerializerSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentType = new("application/json");
            }

            HttpResponseMessage response;
            Result<T> result = new();

            try
            {
                response = await this._httpClient.SendAsync(request);
                result.StatusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    Stream ContentResponse = await response.Content?.ReadAsStreamAsync();
                    result.Value = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(ContentResponse);
                }
                else
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Error = ex.Message;
            }

            return result;
        }
    }
}
