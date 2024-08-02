using System.Text.Json.Serialization;

namespace Web.Test.Models
{
	public class HttpBinGetResponse
	{
		[JsonPropertyName("args")]
		public Dictionary<string, string> Args { get; set; }

		[JsonPropertyName("headers")]
		public HttpBinGetResponseHeaders Headers { get; set; }

		[JsonPropertyName("origin")]
		public string Origin { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }
	}

	public class HttpBinGetResponseHeaders
	{
		[JsonPropertyName("Accept")]
		public string Accept { get; set; }

		[JsonPropertyName("Accept-Encoding")]
		public string AcceptEncoding { get; set; }

		[JsonPropertyName("Accept-Language")]
		public string AcceptLanguage { get; set; }

		[JsonPropertyName("Host")]
		public string Host { get; set; }

		[JsonPropertyName("Priority")]
		public string Priority { get; set; }

		[JsonPropertyName("Sec-Fetch-Dest")]
		public string SecFetchDest { get; set; }

		[JsonPropertyName("Sec-Fetch-Mode")]
		public string SecFecthMode { get; set; }

		[JsonPropertyName("Sec-Fetch-Site")]
		public string SecFetchSite { get; set; }

		[JsonPropertyName("Sec-Fetch-User")]
		public string SecFetchUser { get; set; }

		[JsonPropertyName("Upgrade-Insecure-Requests")]
		public string UpgradeInsecureRequests { get; set; }

		[JsonPropertyName("User-Agent")]
		public string UserAgent { get; set; }

		[JsonPropertyName("X-Amzn-Trace-Id")]
		public string AmazonTraceId { get; set; }
	}
}
