# Api Request

## About

This classes helps you making call to APIs

Source: *Coming Soon*  
Authors:
* [AnthoDingo](https://github.com/AnthoDingo) - Original author by taking inspiration from the community
* [Sikelio](https://github.com/Sikelio) - Contributor by adding error property & reverse model data annotation

## Usage

```cs
using FrenchyApps42.Tools.Api;
using System.Text.Json.Serialization;

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

public class SomeModel {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
```
