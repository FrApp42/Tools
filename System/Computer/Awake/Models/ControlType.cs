// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

namespace FrApp42.System.Computer.Awake.Models
{
    /// <summary>
    /// The type of control signal received by the handler.
    /// </summary>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/windows/console/handlerroutine">HandlerRoutine callback function</see>.
    /// </remarks>
    internal enum ControlType
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6,
    }
}
