using System.Text.Json.Serialization;

namespace Web.Test.Models
{
	public class HttpBinPostResponse : HttpBinResponseBase
	{
		[JsonPropertyName("data")]
		public string Data { get; set; }

		[JsonPropertyName("files")]
		public Dictionary<string, string> Files { get; set; }

		[JsonPropertyName("form")]
		public Dictionary<string, string> Form { get; set; }

		[JsonPropertyName("json")]
		public object Json { get; set; }
	}
}
