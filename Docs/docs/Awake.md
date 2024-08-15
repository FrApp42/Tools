# Awake

Module is exported from Microsoft Powertoys under license MIT.

>[!NOTE]
>Works only for Windows host


## V1

Simple usage

```C#
using FrApps42.System.Computer.Awake.v1;
...

// Keep Screen on
Awake..SetIndefiniteKeepAwake(true);
// Keep Screen off
Awake..SetIndefiniteKeepAwake(false);

// Disable Keep Awake
Awake.SetNoKeepAwake();

...

Awake.CompleteExit(0, false, "AppName");

```

If you want to log Awake error

```C#
using FrApps42.System.Computer.Awake.v1;
...

private static void LogUnexpectedOrCancelledKeepAwakeThreadCompletion(){
    Console.WriteLine("The keep-awake thread was terminated early.");
}

private static void LogCompletedKeepAwakeThread(bool result)
{
    Console.WriteLine($"Exited keep-awake thread successfully: {result}");
}

// Keep Screen on
Awake..SetIndefiniteKeepAwake(LogCompletedKeepAwakeThread, LogUnexpectedOrCancelledKeepAwakeThreadCompletion,true);
// Keep Screen off
Awake..SetIndefiniteKeepAwake(LogCompletedKeepAwakeThread, LogUnexpectedOrCancelledKeepAwakeThreadCompletion,false);

// Disable Keep Awake
Awake.SetNoKeepAwake();

...

Awake.CompleteExit(0, false, "AppName");

```

## V2

Updated version of Power Awake

```C#
using FrApps42.System.Computer.Awake.v1;
...

private static void LogUnexpectedOrCancelledKeepAwakeThreadCompletion(){
    Console.WriteLine("The keep-awake thread was terminated early.");
}

private static void LogCompletedKeepAwakeThread(bool result)
{
    Console.WriteLine($"Exited keep-awake thread successfully: {result}");
}

// Keep Screen on
Awake..SetIndefiniteKeepAwake(true);
// Keep Screen off
Awake..SetIndefiniteKeepAwake(false);

// Keep Awake for a specified seconds with screen on
Awake.SetTimedKeepAwake(3600, true);
// Keep Awake for a specified seconds with screen off
Awake.SetTimedKeepAwake(3600, false);

// Disable Keep Awake
Awake.SetNoKeepAwake();

```

In V2, be sure to disable KeepAwake before app closing.