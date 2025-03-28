using Newtonsoft.Json;

namespace Web.Test.Models
{
	public class HttpBinPostResponse : HttpBinResponseBase
	{
		[JsonProperty("data")]
		public string Data { get; set; } = string.Empty;

		[JsonProperty("files")]
		public Dictionary<string, string> Files { get; set; } = [];

		[JsonProperty("form")]
		public Dictionary<string, string> Form { get; set; } = [];

		[JsonProperty("json")]
		public object Json { get; set; } = new();
	}
}
