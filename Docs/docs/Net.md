# NET

>[!NOTE]
> Tested on Windows, should works on Linux and MacOS

## IsOnline

Simple class to test if computer is Online.

```C#
using FrApps42.System.Net;`


bool result = (new IsOnline("8.8.8.8")).Check();

```