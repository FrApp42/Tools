﻿// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

using FrApp42.System.Computer.Awake.Models;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FrApp42.System.Computer.Awake.Natives
{
    internal sealed class Bridge
    {
        [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true)]
        internal delegate int WndProcDelegate(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam);

        [DllImport("Powrprof.dll", SetLastError = true)]
        internal static extern bool GetPwrCapabilities(out SystemPowerCapabilities lpSystemPowerCapabilities);

        //[DllImport("kernel32.dll", SetLastError = true)]
        //internal static extern bool SetConsoleCtrlHandler(ConsoleEventHandler handler, bool add);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr CreateFile(
        [MarshalAs(UnmanagedType.LPWStr)] string filename,
        [MarshalAs(UnmanagedType.U4)] uint access,
        [MarshalAs(UnmanagedType.U4)] FileShare share,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
        IntPtr templateFile);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool InsertMenu(IntPtr hMenu, uint uPosition, uint uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool TrackPopupMenuEx(IntPtr hMenu, uint uFlags, int x, int y, IntPtr hWnd, IntPtr lptpm);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, nuint wParam, string lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll")]
        internal static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool TranslateMessage(ref Msg lpMsg);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr DispatchMessage(ref Msg lpMsg);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr RegisterClassEx(ref WndClassEx lpwcx);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int DefWindowProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool GetMessage(out Msg lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetMenuInfo(IntPtr hMenu, ref MenuInfo lpcmi);

        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
