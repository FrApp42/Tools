// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

using System.Drawing;

namespace FrApp42.System.Computer.Awake.Models
{
    internal struct Msg
    {
        public IntPtr HWnd;
        public uint Message;
        public IntPtr WParam;
        public IntPtr LParam;
        public uint Time;
        public Point Pt;
    }
}
