using FrApps42.System.Computer.Shutdown.Helpers;
using FrApps42.System.Net;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace FrApps42.System.Computer.Shutdown
{
    public partial class Shutdown
    {
        /// <summary>
        /// StringBuilder instance used to build the shutdown command.
        /// </summary>
        private readonly StringBuilder _commandBuilder;

        /// <summary>
        /// Machine hostname.
        /// </summary>
        private string _hostname = "";

        /// <summary>
        /// Will the machine be pinged before shutdowning it?
        /// </summary>
        private bool _pingEnabled = false;

        /// <summary>
        /// Timeout of machine ping.
        /// </summary>
        private int _pingTimeout = 5;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Shutdown()
        {
            _commandBuilder = new StringBuilder(ShutdownArgs.BaseCommand);
        }

        /// <summary>
        /// Iniialize Shutdowner with a hostname
        /// </summary>
        /// <param name="hostname"></param>
        public Shutdown(string hostname): this()
        {
            SetMachine(hostname);
        }

        /// <summary>
        /// Iniialize Shutdowner with an IPAddress
        /// </summary>
        /// <param name="address"></param>
        public Shutdown(IPAddress address) : this(address.ToString()) { }

        /// <summary>
        /// Adds a shutdown argument to the command if it is not already present.
        /// </summary>
        /// <param name="arg">The argument to add.</param>
        /// <param name="value">Optional value associated with the argument.</param>
        private void AddArgument(string arg, string value = "")
        {
            string currentCommand = _commandBuilder.ToString();

            // Prevent adding /l if /m or /t is already present
            if (arg == ShutdownArgs.LogoutArg && (currentCommand.Contains(ShutdownArgs.MachineArg) || currentCommand.Contains(ShutdownArgs.TimeoutArg)))
                return;

            // Prevent adding /m or /t if /l is already present
            if ((arg == ShutdownArgs.MachineArg || arg == ShutdownArgs.TimeoutArg) && currentCommand.Contains(ShutdownArgs.LogoutArg))
                return;

            if (!_commandBuilder.ToString().Contains(arg))
            {
                if (string.IsNullOrEmpty(value))
                {
                    _commandBuilder.Append($" {arg}");
                }
                else
                {
                    _commandBuilder.Append($" {arg} {value}");
                }
            }
        }

        /// <summary>
        /// Displays the command of the current command.
        /// </summary>
        /// <returns>Current built command</returns>
        public string GetCommand()
        {
            return _commandBuilder.ToString();
        }

        /// <summary>
        /// Logs off the current user immediately, with no time-out period.
        /// Can't be used with SetMachine and/or SetTimeOut options.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdown LogoutUser()
        {
            AddArgument(ShutdownArgs.LogoutArg);
            return this;
        }

        /// <summary>
        /// Shuts down the computer.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdown ShutdownComputer()
        {
            AddArgument(ShutdownArgs.ShutdownArg);
            return this;
        }

        /// <summary>
        /// Shuts down the computer. On the next boot, if Automatic Restart Sign-On is enabled,
        /// the device automatically signs in and locks based on the last interactive user.
        /// After sign in, it restarts any registered applications.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdown ShutdownAndSignOn()
        {
            AddArgument(ShutdownArgs.ShutdownAndSignOnArg);
            return this;
        }

        /// <summary>
        /// Restarts the computer
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdown Reboot()
        {
            AddArgument(ShutdownArgs.RebootArg);
            return this;
        }

        /// <summary>
        /// Puts the local computer into hibernation, if hibernation is enabled.
        /// The ForceShutdown option can be used with the Hibernate option.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdown Hibernate()
        {
            AddArgument(ShutdownArgs.HibernateArg);
            return this;
        }

        /// <summary>
        /// Allows running processes and applications to gracefully close instead of forcibly terminating.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdown Soft()
        {
            AddArgument(ShutdownArgs.SoftArg);
            return this;
        }

        /// <summary>
        /// Goes to the Advanced boot options menu and restarts the device.
        /// This option must be used with the /r option.
        /// </summary>
        /// <returns>Instance</returns>
        public Shutdown OpenBootOptions()
        {
            AddArgument(ShutdownArgs.BootOptionsArg);
            return this;
        }

        /// <summary>
        /// Forces running applications to close without warning users.
        /// </summary>
        /// <returns></returns>
        public Shutdown ForceShutdown()
        {
            AddArgument(ShutdownArgs.ForceArg);
            return this;
        }

        /// <summary>
        /// Specifies the target computer.
        /// Can't be used with LogoutUser option.
        /// </summary>
        /// <param name="machineName">Machine hostname</param>
        /// <returns>Instance</returns>
        public Shutdown SetMachine(string machineName)
        {
            _hostname = machineName;
            AddArgument(ShutdownArgs.MachineArg, machineName);
            return this;
        }

        /// <summary>
        /// Sets the timeout period for the shutdown command. Min = 0, Max = 315360000 (10 years)
        /// Can't be used with LogoutUser option.
        /// </summary>
        /// <param name="time">Timeout period in seconds</param>
        /// <returns>Instance</returns>
        public Shutdown SetTimeOut(int time)
        {
            if (time < 0) time = 0;
            if (time > 315360000) time = 315360000;

            AddArgument(ShutdownArgs.TimeoutArg, time.ToString());
            return this;
        }

        /// <summary>
        /// Creates a custom reason for the system shutdown or restart, which must be enclosed.
        /// You can use a maximum of 512 characters. If the comment is longer, it will be shortened to 512.
        /// </summary>
        /// <param name="comment">Custom reason for shutdown, max 512 characters.</param>
        /// <returns>Instance</returns>
        public Shutdown SetComment(string comment)
        {
            if (comment.Length > 512)
                comment = comment.Substring(0, 512);

            AddArgument(ShutdownArgs.CommentArg, $"\"{comment}\"");
            return this;
        }

        /// <summary>
        /// Enables the ping before running the shutdown command.
        /// </summary>
        /// <param name="timeout">Timeout of ping to the machine</param>
        /// <returns></returns>
        public Shutdown CheckIsOnline(int timeout = 5)
        {
            if (timeout < 0) timeout = 0;

            _pingEnabled = true;
            _pingTimeout = timeout;

            return this;
        }

        /// <summary>
        /// Runs the built shutdown command.
        /// If ping is enabled, it will try to ping the machine. If the ping if unsuccesful it returns null.
        /// </summary>
        /// <returns>A tuple containing the exit code and an error message if an error occurred.</returns>
        public async Task<ShutdownResult> Run()
        {
            if (_pingEnabled)
            {
                IsOnline isOnline = new IsOnline(_hostname);

                if(!isOnline.Check())
                    return new ShutdownResult()
                    {   
                        ExitCode = 1,
                        ErrorMessage = "Machine is unreachable"
                    };
            }

            return await Task.Run(() =>
            {
                ProcessStartInfo processStartInfo = new("shutdown.exe", _commandBuilder.ToString())
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                };

                using (Process process = new() { StartInfo = processStartInfo })
                {
                    process.Start();

                    string errorMessage = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    return new ShutdownResult()
                    {
                        ExitCode = process.ExitCode,
                        ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage
                    };
                }
            });
        }
    }
}
