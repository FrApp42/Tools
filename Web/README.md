﻿# FrApp42.System

FrApp42.Web is collection of classes for web operations:
* Request

## How to Use this Library?

```nuget
Install-Package FrApp42.Net
```

## Examples

### Make an API request

```csharp
using FrApp42.Web.API;
using System.Text.Json.Serialization;

string url = "your-url";

Request request = new(url);
request
    .AddHeader("key", "value")
    .AcceptJson();

Result<MyModel> result = await request.Run<MyModel>();

if (result.StatusCode == 200 && result.Value != null)
{
    Console.WriteLine(result.Value.Name);
    Console.WriteLine(result.Value.Description);
}

class MyModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
```

### Get an image
```csharp
using FrApp42.Web.API;

string url = "your-url";

Request request = new(url);
request
    .AddHeader("Accept", "image/png");

Result<byte[]> result = await request.RunGetBytes();

if (result.StatusCode == 200 && result.Value != null)
{
    File.WriteAllBytes($"image.png", result.Value);
}
```
