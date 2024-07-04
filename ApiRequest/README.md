# ApiRequest

## About

This classes helps you making call to APIs

Authors:
* [AnthoDingo](https://github.com/AnthoDingo) - Initiator by taking inspiration from the dev community
* [Sikelio](https://github.com/Sikelio) - Contributor

## Usage
### Simple request
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

## Advanced usage
### Sending binary file

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
            string url = "";
            string filePath = "path_to_your_file.ext";

            if (File.Exists(filePath))                                                  // <-- Check if your file exist at the specified path
            {
                try
                {
                    byte[] fileBytes = File.ReadAllBytes(filePath);                     // <-- Convert your file into byte[]

                    ApiRequest request = new(url, HttpMethod.Post);
                    request
                        .SetContentType("application/type")                             // <-- Set your content type
                        .AddDocumentBody(fileBytes, "your_file_name.ext");              // <-- Add your file in byte[] format with it's name

                    Result<FileModel> result = await request.RunDocument<FileModel>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("File do not exist");
            }
        }
    }
}
```

```cs
using System.Text.Json.Serialization;

namespace Program.Models
{
    public class FileModel
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
```
