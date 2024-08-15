// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

using System.Runtime.InteropServices;

namespace FrApp42.System.Computer.Awake.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WndClassEx
    {
        public uint CbSize;
        public uint Style;
        public IntPtr LpfnWndProc;
        public int CbClsExtra;
        public int CbWndExtra;
        public IntPtr HInstance;
        public IntPtr HIcon;
        public IntPtr HCursor;
        public IntPtr HbrBackground;
        public string LpszMenuName;
        public string LpszClassName;
        public IntPtr HIconSm;
    }
}
