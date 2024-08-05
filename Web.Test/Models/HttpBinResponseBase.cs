using System.Text.Json.Serialization;

namespace Web.Test.Models
{
	public class HttpBinResponseBase
	{
		[JsonPropertyName("args")]
		public Dictionary<string, string> Args { get; set; }

		[JsonPropertyName("headers")]
		public HttpBinResponseHeaders Headers { get; set; }

		[JsonPropertyName("origin")]
		public string Origin { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }
	}

	public class HttpBinResponseHeaders
	{
		[JsonPropertyName("Accept")]
		public string Accept { get; set; }

		[JsonPropertyName("Accept-Encoding")]
		public string AcceptEncoding { get; set; }

		[JsonPropertyName("Accept-Language")]
		public string AcceptLanguage { get; set; }

		[JsonPropertyName("Host")]
		public string Host { get; set; }

		[JsonPropertyName("User-Agent")]
		public string UserAgent { get; set; }

		[JsonPropertyName("X-Amzn-Trace-Id")]
		public string AmazonTraceId { get; set; }
	}
}
