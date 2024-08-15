// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

namespace FrApp42.System.Computer.Awake.Statics
{
    internal static class Constants
    {
        internal const string AppName = "Awake";
        internal const string FullAppName = "PowerToys " + AppName;
        internal const string TrayWindowId = "Awake.MessageWindow";
        internal const string BuildRegistryLocation = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";

        // PowerToys Awake build code name. Used for exact logging
        // that does not map to PowerToys broad version schema to pinpoint
        // internal issues easier.
        // Format of the build ID is: CODENAME_MMDDYYYY, where MMDDYYYY
        // is representative of the date when the last change was made before
        // the pull request is issued.
        internal const string BuildId = "DAISY023_04102024";
    }
}
