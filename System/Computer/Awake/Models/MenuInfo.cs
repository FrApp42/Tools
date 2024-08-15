// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

using System.Runtime.InteropServices;

namespace FrApp42.System.Computer.Awake.Models
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MenuInfo
    {
        public uint CbSize;             // Size of the structure, in bytes
        public uint FMask;              // Specifies which members of the structure are valid
        public uint DwStyle;            // Style of the menu
        public uint CyMax;              // Maximum height of the menu, in pixels
        public IntPtr HbrBack;          // Handle to the brush used for the menu's background
        public uint DwContextHelpID;    // Context help ID
        public IntPtr DwMenuData;       // Pointer to the menu's user data
    }
}
