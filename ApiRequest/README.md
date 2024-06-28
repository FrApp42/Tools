# ApiRequest

## About

This classes helps you making call to APIs

Authors:
* [AnthoDingo](https://github.com/AnthoDingo) - Initiator by taking inspiration from the dev community
* [Sikelio](https://github.com/Sikelio) - Contributor

## Usage

```cs
using FrenchyApps42.Web.ApiRequest.Structs;
using FrenchyApps42.Web.ApiRequest;
using Program.Models;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            string url = "";                    // <-- Your API url
            HttpMethod method = HttpMethod.Get; // <-- Your method. Default is Get

            ApiRequest request = new(url, method);
            request
                .AddContentHeader("", "")       // <-- Any content header (optional)
                .AddHeader("", "")              // <-- Any header (optional)
                .AddJsonBody("")                // <-- Any json body (optional)
                .AddQueryParam("", "")          // <-- Any query parameter (optional)
                .AcceptJson();                  // <-- Adds "Accept", "application/json" in the headers (optional)

            Result<SomeModel> result = await request.Run<SomeModel>();

            if (result.StatusCode == 200)       // <-- Depends of the status code your waiting for
            {
                // If your request succeded
            }
            else
            {
                // Other cases
            }
        }
    }
}
```

```cs
using System.Text.Json.Serialization;

namespace Program.Models
{
    public class SomeModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}
```
