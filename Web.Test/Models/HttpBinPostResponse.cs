using System.Text.Json.Serialization;

namespace Web.Test.Models
{
	public class HttpBinPostResponse
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
		public HttpBinPostResponseHeaders Headers { get; set; }

		[JsonPropertyName("json")]
		public object Json { get; set; }

		[JsonPropertyName("origin")]
		public string Origin { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }
	}

	public class HttpBinPostResponseHeaders
	{
		[JsonPropertyName("Accept")]
		public string Accept { get; set; }

		[JsonPropertyName("Accept-Encoding")]
		public string AcceptEncoding { get; set; }

		[JsonPropertyName("Accept-Language")]
		public string AcceptLanguage { get; set; }

		[JsonPropertyName("Content-Length")]
		public string ContentLength { get; set; }

		[JsonPropertyName("Host")]
		public string Host { get; set; }

		[JsonPropertyName("Origin")]
		public string Origin { get; set; }

		[JsonPropertyName("Priority")]
		public string Priority { get; set; }

		[JsonPropertyName("Referer")]
		public string Referer { get; set; }

		[JsonPropertyName("Sec-Fetch-Dest")]
		public string SecFetchDest { get; set; }

		[JsonPropertyName("Sec-Fetch-Mode")]
		public string SecFetchMode { get; set; }

		[JsonPropertyName("Sec-Fetch-Site")]
		public string SecFetchSite { get; set; }

		[JsonPropertyName("User-Agent")]
		public string UserAgent { get; set; }

		[JsonPropertyName("X-Amzn-Trace-Id")]
		public string AmazonTraceId { get; set; }
	}
}
