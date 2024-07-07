using FrenchyApps42.System.Shutdowner.Helpers;
using FrenchyApps42.System.Shutdowner.Models;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace FrenchyApps42.System.Shutdowner
{
    public class Shutdowner
    {
        /// <summary>
        /// StringBuilder instance used to build the shutdown command.
        /// </summary>
        private readonly StringBuilder _commandBuilder;

        /// <summary>
        /// Machine hostname.
        /// </summary>
        private string Hostname = "";

        /// <summary>
        /// Will the machine be pinged before shutdowning it?
        /// </summary>
        private bool PingEnabled = false;

        /// <summary>
        /// Timeout of machine ping.
        /// </summary>
        private int PingTimeout = 5;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Shutdowner()
        {
            this._commandBuilder = new StringBuilder(ShutdownArgs.BaseCommand);
        }

        /// <summary>
        /// Adds a shutdown argument to the command if it is not already present.
        /// </summary>
        /// <param name="arg">The argument to add.</param>
        /// <param name="value">Optional value associated with the argument.</param>
        private void AddArgument(string arg, string value = "")
        {
            string currentCommand = this._commandBuilder.ToString();

            // Prevent adding /l if /m or /t is already present
            if (arg == ShutdownArgs.LogoutArg && (currentCommand.Contains(ShutdownArgs.MachineArg) || currentCommand.Contains(ShutdownArgs.TimeoutArg)))
                return;

            // Prevent adding /m or /t if /l is already present
            if ((arg == ShutdownArgs.MachineArg || arg == ShutdownArgs.TimeoutArg) && currentCommand.Contains(ShutdownArgs.LogoutArg))
                return;

            if (!this._commandBuilder.ToString().Contains(arg))
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._commandBuilder.Append($" {arg}");
                }
                else
                {
                    this._commandBuilder.Append($" {arg} {value}");
                }
            }
        }

        /// <summary>
        /// Logs off the current user immediately, with no time-out period.
        /// Can't be used with SetMachine and/or SetTimeOut options.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdowner LogoutUser()
        {
            AddArgument(ShutdownArgs.LogoutArg);
            return this;
        }

        /// <summary>
        /// Shuts down the computer.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdowner ShutdownComputer()
        {
            this.AddArgument(ShutdownArgs.ShutdownArg);
            return this;
        }

        /// <summary>
        /// Shuts down the computer. On the next boot, if Automatic Restart Sign-On is enabled,
        /// the device automatically signs in and locks based on the last interactive user.
        /// After sign in, it restarts any registered applications.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdowner ShutdownAndSignOn()
        {
            this.AddArgument(ShutdownArgs.ShutdownAndSignOnArg);
            return this;
        }

        /// <summary>
        /// Restarts the computer after shutdown.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdowner RebootAfterShutdown()
        {
            this.AddArgument(ShutdownArgs.RebootArg);
            return this;
        }

        /// <summary>
        /// Puts the local computer into hibernation, if hibernation is enabled.
        /// The ForceShutdown option can be used with the Hibernate option.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdowner Hibernate()
        {
            this.AddArgument(ShutdownArgs.HibernateArg);
            return this;
        }

        /// <summary>
        /// Allows running processes and applications to gracefully close instead of forcibly terminating.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdowner Soft()
        {
            this.AddArgument(ShutdownArgs.SoftArg);
            return this;
        }

        /// <summary>
        /// Goes to the Advanced boot options menu and restarts the device.
        /// This option must be used with the /r option.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdowner OpenBootOptions()
        {
            this.AddArgument(ShutdownArgs.BootOptionsArg);
            return this;
        }

        /// <summary>
        /// Forces running applications to close without warning users.
        /// </summary>
        /// <returns></returns>
        public Shutdowner ForceShutdown()
        {
            this.AddArgument(ShutdownArgs.ForceArg);
            return this;
        }

        /// <summary>
        /// Specifies the target computer.
        /// Can't be used with LogoutUser option.
        /// </summary>
        /// <param name="machineName">Machine hostname</param>
        /// <returns>Instance</returns>
        public Shutdowner SetMachine(string machineName)
        {
            this.Hostname = machineName;
            this.AddArgument(ShutdownArgs.MachineArg, machineName);
            return this;
        }

        /// <summary>
        /// Sets the timeout period for the shutdown command. Min = 0, Max = 315360000 (10 years)
        /// Can't be used with LogoutUser option.
        /// </summary>
        /// <param name="time">Timeout period in seconds</param>
        /// <returns>Instance</returns>
        public Shutdowner SetTimeOut(int time)
        {
            if (time < 0) time = 0;
            if (time > 315360000) time = 315360000;

            this.AddArgument(ShutdownArgs.TimeoutArg, time.ToString());
            return this;
        }

        /// <summary>
        /// Creates a custom reason for the system shutdown or restart, which must be enclosed.
        /// You can use a maximum of 512 characters. If the comment is longer, it will be shortened to 512.
        /// </summary>
        /// <param name="comment">Custom reason for shutdown, max 512 characters.</param>
        /// <returns>Instance</returns>
        public Shutdowner SetComment(string comment)
        {
            if (comment.Length > 512)
                comment = comment.Substring(0, 512);

            this.AddArgument(ShutdownArgs.CommentArg, $"\"{comment}\"");
            return this;
        }

        /// <summary>
        /// Enables the ping before running the shutdown command.
        /// </summary>
        /// <param name="timeout">Timeout of ping to the machine</param>
        /// <returns></returns>
        public Shutdowner PingBefore(int timeout = 5)
        {
            if (timeout < 0) timeout = 0;

            this.PingEnabled = true;
            this.PingTimeout = timeout;

            return this;
        }

        /// <summary>
        /// Runs the built shutdown command.
        /// If ping is enabled, it will try to ping the machine. If the ping if unsuccesful it returns null.
        /// </summary>
        /// <returns>A tuple containing the exit code and an error message if an error occurred.</returns>
        public async Task<CommandResult?> Run()
        {
            if (this.PingEnabled)
            {
                if (this.SendPing() == false)
                    return null;
            }

            return await Task.Run(() =>
            {
                ProcessStartInfo processStartInfo = new("cmd.exe", this._commandBuilder.ToString())
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                };

                using (Process process = new()
                {
                    StartInfo = processStartInfo
                })
                {
                    process.Start();

                    string errorMessage = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    return new CommandResult()
                    {
                        ExitCode = process.ExitCode,
                        ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage
                    };
                }
            });
        }

        /// <summary>
        /// Sends a ping to the machine.
        /// </summary>
        /// <returns>A bool depending of the success of the ping</returns>
        private bool SendPing()
        {
            if (string.IsNullOrEmpty(this.Hostname))
                return false;

            Ping sender = new();
            PingOptions options = new()
            {
                DontFragment = true,
            };

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            PingReply reply = sender.Send(this.Hostname, this.PingTimeout, buffer, options);

            if (reply.Status == IPStatus.Success)
                return true;

            return false;
        }

        /// <summary>
        /// Displays the command of the current command.
        /// </summary>
        /// <returns>Current built command</returns>
        public string Display()
        {
            return this._commandBuilder.ToString();
        }
    }
}
