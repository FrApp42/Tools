using FrApp42.Web.API.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FrApp42.Web.API
{
    public class Request
    {
        #region Variables
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
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Request class with the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        public Request(string url)
        {
            URL = url;
        }

        /// <summary>
        /// Initializes a new instance of the Request class with the specified URL and HTTP method.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        public Request(string url, HttpMethod method): this(url)
        {
            Method = method;
        }

        #endregion

        #region Functions to update variables

        /// <summary>
        /// Adds a header to the request.
        /// </summary>
        /// <param name="key">The header key.</param>
        /// <param name="value">The header value.</param>
        /// <returns>Instance</returns>
        public Request AddHeader(string key, string value)
        {
            RequestHeaders.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds multiple headers from a dictionary of key-value pairs to the request.
        /// </summary>
        /// <param name="headers">Headers in dictionary.</param>
        /// <returns>Instance</returns>
        public Request AddHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                RequestHeaders.Add(header.Key, header.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds a content header to the request
        /// </summary>
        /// <param name="key">The content header key.</param>
        /// <param name="value">The content header value.</param>
        /// <returns>Instance</returns>
        public Request AddContentHeader(string key, string value)
        {
            ContentHeaders.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds multiple content headers from a dictionary of key-value pairs to the request.
        /// </summary>
        /// <param name="headers">Content headers in dictionary.</param>
        /// <returns>Instance</returns>
        public Request AddContentHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                ContentHeaders.Add(header.Key, header.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds a query parameter to the request.
        /// </summary>
        /// <param name="key">The query parameter key.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>Instance</returns>
        public Request AddQueryParam(string key, string value)
        {
            QueryParams.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds multiple content headers from a dictionary of key-value pairs to the request.
        /// </summary>
        /// <param name="queryParams">Query parameters in dictionary.</param>
        /// <returns>Instance</returns>
        public Request AddQueryParams(Dictionary<string, string> queryParams)
        {
            foreach (KeyValuePair<string, string> query in queryParams)
            {
                QueryParams.Add(query.Key, query.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds an "Accept" header with the value "application/json" to the request.
        /// </summary>
        /// <returns>Instance</returns>
        public Request AcceptJson()
        {
            RequestHeaders.Add("Accept", "application/json");

            return this;
        }

        /// <summary>
        /// Adds a JSON body to the request content.
        /// </summary>
        /// <param name="body">The JSON body content.</param>
        /// <returns>Instance</returns>
        public Request AddJsonBody(object body)
        {
            Body = body;

            return this;
        }

        /// <summary>
        /// Adds a binary document to the request content.
        /// </summary>
        /// <param name="document">The binary file content.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>Instance</returns>
        public Request AddDocumentBody(byte[] document, string fileName)
        {
            DocumentBody = document;
            DocumentFileName = fileName;

            return this;
        }

        /// <summary>
        /// Sets the content type of the request.
        /// </summary>
        /// <param name="contentType">The content type of the request.</param>
        /// <returns>Instance</returns>
        public Request SetContentType(string contentType)
        {
            ContentType = contentType;

            return this;
        }

        #endregion

        #region Peform request

        /// <summary>
        /// Executes the HTTP request with the JSON body content included.
        /// </summary>
        /// <typeparam name="T">The type of the response expected from the request.</typeparam>
        /// <returns>A Result object containing the response.</returns>
        public async Task<Result<T>> Run<T>()
        {
            HttpRequestMessage request = BuildBaseRequest();

            if (Body != null)
            {
                string json = JsonConvert.SerializeObject(Body, _jsonSerializerSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentType = new("application/json");
            }

            Result<T> result = await Process<T>(request);

            return result;
        }

        /// <summary>
        /// Executes the HTTP request with the binary document content included.
        /// </summary>
        /// <typeparam name="T">The type of the response expected from the request.</typeparam>
        /// <returns>A Result object containing the response.</returns>
        public async Task<Result<T>> RunDocument<T>()
        {
            HttpRequestMessage request = BuildBaseRequest();

            if (DocumentBody == null)
            {
                return new Result<T>()
                {
                    Error = "Document cannot be null",
                    StatusCode = 500,
                };
            }

            MultipartFormDataContent content = new();
            ByteArrayContent fileContent = new(DocumentBody);

            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("binary")
            {
                Name = "file",
                FileName = DocumentFileName
            };

            content.Add(fileContent);

            request.Content = content;
            request.Content.Headers.ContentType = new(ContentType);

            Result<T> result = await Process<T>(request);

            return result;
        }

        #endregion

        #region Private function

        /// <summary>
        /// Constructs the URL for the request, including any query parameters if present.
        /// </summary>
        /// <returns>Request URL with added query parameters.</returns>
        private string BuildUrl()
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

            return builder.ToString();
        }

        /// <summary>
        /// Constructs the base HTTP request message by adding the specified headers.
        /// </summary>
        /// <returns>Base request message.</returns>
        private HttpRequestMessage BuildBaseRequest()
        {
            HttpRequestMessage request = new(Method, BuildUrl());

            for (int i = 0; i < RequestHeaders.Count(); i++)
            {
                KeyValuePair<string, string> header = RequestHeaders.ElementAt(i);
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
        private async Task<Result<T>> Process<T>(HttpRequestMessage request)
        {
            HttpResponseMessage response;
            Result<T> result = new();

            try
            {
                response = await _httpClient.SendAsync(request);
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

        #endregion
    }
}
