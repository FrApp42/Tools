# Shutdowner

## About

This class send a command that shutdown a computer with its hostname.

Authors:
* [Sikelio](https://github.com/Sikelio)

## Usage

```cs
using FrenchyApps42.System.Shutdowner;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            string hostname = ""; // <-- the hostname of the machine you want to shutdown

            try
            {
                /**
                 * Arg1: Target machine hostname
                 * Arg2: Time before shutdown starts
                 * Arg3: Should the app force quit before shutdown
                 */
                await Shutdowner.Shutdown(hostname, 5, false);
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
        }
    }
}
```
