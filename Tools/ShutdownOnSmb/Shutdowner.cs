using System.Diagnostics;

namespace FrenchyApps42.Tools.ShutdownOnSmb
{
    public class Shutdowner
    {
        public static async Task Shutdown(string hostname, int time = 5, bool forceAppQuit = false)
        {
            await Task.Run(() =>
            {
                string command = $"shutdown /m {hostname} /f";

                if (forceAppQuit == true)
                {
                    command += " /f";
                }

                command += $" /t {time}";

                ProcessStartInfo processStartInfo = new("cmd.exe", $"/c {command}")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                Process process = new()
                {
                    StartInfo = processStartInfo
                };

                process.Start();
                process.WaitForExit();

                int exitCode = process.ExitCode;
            });
        }
    }
}
