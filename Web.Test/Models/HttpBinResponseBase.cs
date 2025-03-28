using Newtonsoft.Json;

namespace Web.Test.Models
{
	public class HttpBinResponseBase
	{
		[JsonProperty("args")]
		public Dictionary<string, string> Args { get; set; } = [];

		[JsonProperty("headers")]
		public HttpBinResponseHeaders Headers { get; set; } = new();

		[JsonProperty("origin")]
		public string Origin { get; set; } = string.Empty;

		[JsonProperty("url")]
		public string Url { get; set; } = string.Empty;
	}

	public class HttpBinResponseHeaders
	{
		[JsonProperty("Accept")]
		public string Accept { get; set; } = string.Empty;

		[JsonProperty("Accept-Encoding")]
		public string AcceptEncoding { get; set; } = string.Empty;

        [JsonProperty("Accept-Language")]
		public string AcceptLanguage { get; set; } = string.Empty;

        [JsonProperty("Host")]
		public string Host { get; set; } = string.Empty;

        [JsonProperty("User-Agent")]
		public string UserAgent { get; set; } = string.Empty;

        [JsonProperty("X-Amzn-Trace-Id")]
		public string AmazonTraceId { get; set; } = string.Empty;
    }
}
