// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

namespace FrApp42.System.Computer.Awake.Models
{
    [Flags]
    internal enum ExecutionState : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001,
    }
}
