# Shutdowner

## About

Shutdowner is a class that sends a command to shutdown a computer with its hostname or perform various other shutdown-related operations.

Authors:
* [Sikelio](https://github.com/Sikelio)

## Features

* Log off the current user
* Shutdown the computer
* Shutdown and sign on automatically
* Reboot after shutdown
* Hibernate the computer
* Perform a soft shutdown
* Open advanced boot options
* Force shutdown
* Specify target computer by hostname
* Set a timeout for the shutdown
* Add a custom reason for the shutdown
* Ping before running command

## Usage

```cs
using FrenchyApps42.System.Shutdowner;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            string hostname = "your-computer-name"; // <-- the hostname of the machine you want to shutdown.

            Shutdowner shutdowner = new();
            shutdowner
                .SetMachine(hostname)
                .ShutdownComputer();

            CommandResult result = await shutdowner.Run();

            if (result.ExitCode == 0)
            {
                Console.WriteLine("Shutdown command executed successfully.");
            }
            else
            {
                Console.WriteLine($"Shutdown command failed with exit code {result.ExitCode}. Error: {result.ErrorMessage}");
            }
        }
    }
}
```

## Advanced Usage
### Ping before shutdown
```cs
using FrenchyApps42.System.Shutdowner;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            Shutdowner shutdowner = new();
            shutdowner
                .PingBefore(5);     // <-- Timeout of the ping

            CommandResult result = await shutdowner.Run();

            if (result.ExitCode == 0)
            {
                Console.WriteLine("Shutdown command executed successfully.");
            }
            else
            {
                Console.WriteLine($"Shutdown command failed with exit code {result.ExitCode}. Error: {result.ErrorMessage}");
            }
        }
    }
}
```

### Log off the current user
```cs
using FrenchyApps42.System.Shutdowner;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            Shutdowner shutdowner = new();
            shutdowner
                .LogoutUser();

            CommandResult result = await shutdowner.Run();

            if (result.ExitCode == 0)
            {
                Console.WriteLine("Shutdown command executed successfully.");
            }
            else
            {
                Console.WriteLine($"Shutdown command failed with exit code {result.ExitCode}. Error: {result.ErrorMessage}");
            }
        }
    }
}
```

### Shutdown and sign on automatically
```cs
using FrenchyApps42.System.Shutdowner;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            Shutdowner shutdowner = new();
            shutdowner
                .ShutdownAndSignOn();

            CommandResult result = await shutdowner.Run();

            if (result.ExitCode == 0)
            {
                Console.WriteLine("Shutdown command executed successfully.");
            }
            else
            {
                Console.WriteLine($"Shutdown command failed with exit code {result.ExitCode}. Error: {result.ErrorMessage}");
            }
        }
    }
}
```

### Set a timeout and add a custom reason for the shutdown
```cs
using FrenchyApps42.System.Shutdowner;

namespace Program
{
    public class Program
    {
        public static async Task Main()
        {
            Shutdowner shutdowner = new();
            shutdowner
                .SetTimeOut(60) // Set timeout to 60 seconds
                .SetComment("Scheduled maintenance"); // Max 512 characters

            CommandResult result = await shutdowner.Run();

            if (result.ExitCode == 0)
            {
                Console.WriteLine("Shutdown command executed successfully.");
            }
            else
            {
                Console.WriteLine($"Shutdown command failed with exit code {result.ExitCode}. Error: {result.ErrorMessage}");
            }
        }
    }
}
```

## License

This project is licensed under the GPLv3 License - see the [LICENSE](./LICENCE.md) file for details.
