# FrApp42.System

FrApp42.System is collection of classes for system operations:
* IsOnline
* Shutdown
* WakeOnLan

## How to Use this Library?

```nuget
Install-Package FrApp42.System
```

## Examples

### Check if device is online

```csharp
using namespace FrApp42.System.Net;

string address = "your-address";
string timeout = 5;

IsOnline online = new(address, timeout);

bool isOnline = online.Check();
```

### Shutdown through SMB (Windows Only)

```csharp
using namespace FrApp42.System.Computer.Shutdown;

string hostname = "your-hostname";

Shutdown shutdown = new();
shutdown
    .ShutdownComputer()
    .Soft()
    .SetTimeOut(5)
    .SetComment("Custom reason");

ShutdownResult shutdownResult = await shutdown.Run();
```

### Wake up a computer (WOL)

```csharp
using namespace FrApp42.System.Net;

string macAddress = "your-mac-address";

await WakeOnLan.WakeUp(macAddress);
```
