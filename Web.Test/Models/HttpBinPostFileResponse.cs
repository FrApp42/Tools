using System.Text.Json.Serialization;

namespace Web.Test.Models
{
	public class HttpBinPostFileResponse
	{
		[JsonPropertyName("args")]
		public Dictionary<string, string> Args { get; set; }

		[JsonPropertyName("data")]
		public string Data { get; set; }

		[JsonPropertyName("files")]
		public Dictionary<string, string> Files { get; set; }

		[JsonPropertyName("form")]
		public Dictionary<string, string> Form { get; set; }

		[JsonPropertyName("headers")]
		public HttpBinPostFileResponseHeaders Headers { get; set; }

		[JsonPropertyName("json")]
		public object Json { get; set; }

		[JsonPropertyName("origin")]
		public string Origin { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }
	}

	public class HttpBinPostFileResponseHeaders
	{
		[JsonPropertyName("Accept")]
		public string Accept { get; set; }

		[JsonPropertyName("Accept-Encoding")]
		public string AcceptEncoding { get; set; }

		[JsonPropertyName("Cache-Control")]
		public string CacheControl { get; set; }

		[JsonPropertyName("Content-Length")]
		public string ContentLength { get; set; }

		[JsonPropertyName("Content-Type")]
		public string ContentType { get; set; }

		[JsonPropertyName("Host")]
		public string Host { get; set; }

		[JsonPropertyName("Postman-Token")]
		public string PostmanToken { get; set; }

		[JsonPropertyName("User-Agent")]
		public string UserAgent { get; set; }

		[JsonPropertyName("X-Amzn-Trace-Id")]
		public string AmazonTraceId { get; set; }
	}
}
