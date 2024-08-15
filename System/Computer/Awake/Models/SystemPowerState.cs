// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

namespace FrApp42.System.Computer.Awake.Models
{
    /// <summary>
    /// Represents the system power state.
    /// </summary>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/windows/win32/power/system-power-states">System power states</see>.
    /// </remarks>
    internal enum SystemPowerState
    {
        PowerSystemUnspecified = 0,
        PowerSystemWorking = 1,
        PowerSystemSleeping1 = 2,
        PowerSystemSleeping2 = 3,
        PowerSystemSleeping3 = 4,
        PowerSystemHibernate = 5,
        PowerSystemShutdown = 6,
        PowerSystemMaximum = 7,
    }
}
