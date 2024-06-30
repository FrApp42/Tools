# WakeOnLan

## About

This class creates and send a magic packet for Wake On Lan.

Source: [Link](https://stackoverflow.com/a/58043033)  
Author:
* [Poul Bak](https://stackoverflow.com/users/5741643/poul-bak) - Original author
* [Sikelio](https://github.com/Sikelio) - Contributor: Edited the code to match the today's conventions

## Usage

```cs
using FrenchyApps42.System.WakeOnLan;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            string macAddress = ""; // <-- your MAC address (eg: XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX)

            await WakeOnLan.WakeUp(macAddress);
        }
    }
}
```