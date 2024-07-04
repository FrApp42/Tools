using FrenchyApps42.Web.ApiRequest.Helpers;
using FrenchyApps42.Web.ApiRequest.Interfaces;
using FrenchyApps42.Web.ApiRequest.Structs;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FrenchyApps42.Web.ApiRequest
{
    public class ApiRequest : IApiRequest
    {
        /// <summary>
        /// The HTTP client used to send requests.
        /// </summary>
        private readonly HttpClient _httpClient = new();

        /// <summary>
        /// Settings for JSON serialization.
        /// </summary>
        private readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            ContractResolver = new JsonPropAttrResolver(),
            Formatting = Formatting.Indented
        };

        /// <summary>
        /// Gets the URL to send the request to.
        /// </summary>
        public string URL { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the HTTP method to use for the request.
        /// </summary>
        public HttpMethod Method { get; private set; } = HttpMethod.Get;

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; private set; } = [];

        /// <summary>
        /// Gets the content headers.
        /// </summary>
        public Dictionary<string, string> ContentHeaders { get; private set; } = [];

        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        public Dictionary<string, string> QueryParams { get; private set; } = [];

        /// <summary>
        /// Gets the JSON body content of the request.
        /// </summary>
        public object Body { get; private set; } = null;

        /// <summary>
        /// Gets the binary document content of the request.
        /// </summary>
        public byte[] DocumentBody { get; private set; } = null;

        /// <summary>
        /// Gets the name of the binary document file.
        /// </summary>
        public string DocumentFileName { get; private set; } = null;

        /// <summary>
        /// Gets the content type of the request.
        /// </summary>
        public string ContentType { get; private set; } = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ApiRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ApiRequest class with the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        public ApiRequest(string url)
        {
            this.URL = url;
        }

        /// <summary>
        /// Initializes a new instance of the ApiRequest class with the specified URL and HTTP method.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        public ApiRequest(string url, HttpMethod method)
        {
            this.URL = url;
            this.Method = method;
        }

        /// <summary>
        /// Adds a header to the request.
        /// </summary>
        /// <param name="key">The header key.</param>
        /// <param name="value">The header value.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddHeader(string key, string value)
        {
            this.RequestHeaders.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds multiple headers from a dictionary of key-value pairs to the request.
        /// </summary>
        /// <param name="headers">Headers in dictionary.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                this.RequestHeaders.Add(header.Key, header.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds a content header to the request
        /// </summary>
        /// <param name="key">The content header key.</param>
        /// <param name="value">The content header value.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddContentHeader(string key, string value)
        {
            this.ContentHeaders.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds multiple content headers from a dictionary of key-value pairs to the request.
        /// </summary>
        /// <param name="headers">Content headers in dictionary.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddContentHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                this.ContentHeaders.Add(header.Key, header.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds a query parameter to the request.
        /// </summary>
        /// <param name="key">The query parameter key.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddQueryParam(string key, string value)
        {
            this.QueryParams.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds multiple content headers from a dictionary of key-value pairs to the request.
        /// </summary>
        /// <param name="queryParams">Query parameters in dictionary.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddQueryParams(Dictionary<string, string> queryParams)
        {
            foreach (KeyValuePair<string, string> query in queryParams)
            {
                this.QueryParams.Add(query.Key, query.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds an "Accept" header with the value "application/json" to the request.
        /// </summary>
        /// <returns>Instance</returns>
        public ApiRequest AcceptJson()
        {
            this.RequestHeaders.Add("Accept", "application/json");

            return this;
        }

        /// <summary>
        /// Adds a JSON body to the request content.
        /// </summary>
        /// <param name="body">The JSON body content.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddJsonBody(object body)
        {
            this.Body = body;

            return this;
        }

        /// <summary>
        /// Adds a binary document to the request content.
        /// </summary>
        /// <param name="document">The binary file content.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>Instance</returns>
        public ApiRequest AddDocumentBody(byte[] document, string fileName)
        {
            this.DocumentBody = document;
            this.DocumentFileName = fileName;

            return this;
        }

        /// <summary>
        /// Sets the content type of the request.
        /// </summary>
        /// <param name="contentType">The content type of the request.</param>
        /// <returns>Instance</returns>
        public ApiRequest SetContentType(string contentType)
        {
            this.ContentType = contentType;

            return this;
        }

        /// <summary>
        /// Executes the HTTP request with the JSON body content included.
        /// </summary>
        /// <typeparam name="T">The type of the response expected from the request.</typeparam>
        /// <returns>A Result object containing the response.</returns>
        public async Task<Result<T>> RunObject<T>()
        {
            HttpRequestMessage request = this.BuildBaseRequest();

            if (this.Body != null)
            {
                string json = JsonConvert.SerializeObject(this.Body, _jsonSerializerSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentType = new("application/json");
            }

            Result<T> result = await this.ProcessRequest<T>(request);

            return result;
        }

        /// <summary>
        /// Executes the HTTP request with the binary document content included.
        /// </summary>
        /// <typeparam name="T">The type of the response expected from the request.</typeparam>
        /// <returns>A Result object containing the response.</returns>
        public async Task<Result<T>> RunDocument<T>()
        {
            HttpRequestMessage request = this.BuildBaseRequest();

            if (this.DocumentBody == null)
            {
                return new Result<T>()
                {
                    Error = "Document cannot be null",
                    StatusCode = 500,
                };
            }

            MultipartFormDataContent content = new();
            ByteArrayContent fileContent = new(this.DocumentBody);

            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("binary")
            {
                Name = "file",
                FileName = this.DocumentFileName
            };

            content.Add(fileContent);

            request.Content = content;
            request.Content.Headers.ContentType = new(this.ContentType);

            Result<T> result = await this.ProcessRequest<T>(request);

            return result;
        }

        /// <summary>
        /// Constructs the URL for the request, including any query parameters if present.
        /// </summary>
        /// <returns>Request URL with added query parameters.</returns>
        private string BuildUrl()
        {
            StringBuilder builder = new(this.URL);
            string fullUrl = this.URL;

            if (fullUrl.EndsWith("/"))
                builder.Remove(fullUrl.Length - 1, 1);

            if (this.QueryParams.Count() > 0)
            {
                builder.Append("?");

                for (int i = 0; i < this.QueryParams.Count(); i++)
                {
                    KeyValuePair<string, string> query = this.QueryParams.ElementAt(i);
                    builder.Append($"{query.Key}={query.Value}");

                    if (!(i == this.QueryParams.Count() - 1))
                        builder.Append("&");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Constructs the base HTTP request message by adding the specified headers.
        /// </summary>
        /// <returns>Base request message.</returns>
        private HttpRequestMessage BuildBaseRequest()
        {
            HttpRequestMessage request = new(this.Method, this.BuildUrl());

            for (int i = 0; i < this.RequestHeaders.Count(); i++)
            {
                KeyValuePair<string, string> header = this.RequestHeaders.ElementAt(i);
                request.Headers.Add(header.Key, header.Value);
            }

            return request;
        }

        /// <summary>
        /// Executes the HTTP request and processes the response.
        /// </summary>
        /// <typeparam name="T">The type of the response expected from the request.</typeparam>
        /// <param name="request">The prepared HTTP request message.</param>
        /// <returns>A Result object containing the response.</returns>
        private async Task<Result<T>> ProcessRequest<T>(HttpRequestMessage request)
        {
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
