// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

using FrApp42.System.Computer.Awake.Models;
using FrApp42.System.Computer.Awake.Natives;
using FrApp42.System.Computer.Awake.Statics;
using NLog;
using System.Collections.Concurrent;
using System.Reactive.Linq;

namespace FrApp42.System.Computer.Awake.v2
{
    public class Awake
    {

        private static readonly Logger _log;
        private static CancellationTokenSource _cts;
        private static CancellationToken _ct;

        private static readonly BlockingCollection<ExecutionState> _stateQueue;

        static Awake()
        {
            _log = LogManager.GetCurrentClassLogger();
            _cts = new CancellationTokenSource();
            StartMonitor();
        }

        #region Public Functions

        /// <summary>
        /// Set Indefinite Keep Awake
        /// </summary>
        /// <param name="keepDisplayOn"></param>
        public static void SetIndefiniteKeepAwake(bool keepDisplayOn = false)
        {
            CancelExistingThread();

            _stateQueue.Add(ComputeAwakeState(keepDisplayOn));
        }

        /// <summary>
        /// Set Keep Awake until specified DateTimeOffset
        /// </summary>
        /// <param name="expireAt"></param>
        /// <param name="keepDisplayOn"></param>
        public static void SetExpirableKeepAwake(DateTimeOffset expireAt, bool keepDisplayOn = true)
        {
            _log.Info($"Expirable keep-awake. Expected expiration date/time: {expireAt} with display on setting set to {keepDisplayOn}.");

            CancelExistingThread();

            if (expireAt > DateTimeOffset.Now)
            {
                _log.Info($"Starting expirable log for {expireAt}");
                _stateQueue.Add(ComputeAwakeState(keepDisplayOn));

                Observable.Timer(expireAt - DateTimeOffset.Now).Subscribe(
                _ =>
                {
                    _log.Info($"Completed expirable keep-awake.");
                    CancelExistingThread();

                    _log.Info("Exiting after expirable keep awake.");
                    CompleteExit(Environment.ExitCode);
                },
                _cts.Token);
            }
            else
            {
                _log.Info("The specified target date and time is not in the future.");
                _log.Info($"Current time: {DateTimeOffset.Now}\tTarget time: {expireAt}");
            }
        }

        /// <summary>
        /// Set Keep Awake for specified seconds
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="keepDisplayOn"></param>
        /// <param name="logElapsedSeconds"></param>
        public static void SetTimedKeepAwake(uint seconds, bool keepDisplayOn = true, bool logElapsedSeconds = false)
        {
            _log.Info($"Timed keep-awake. Expected runtime: {seconds} seconds with display on setting set to {keepDisplayOn}.");
            CancelExistingThread();

            _log.Info($"Timed keep awake started for {seconds} seconds.");
            _stateQueue.Add(ComputeAwakeState(keepDisplayOn));

            IObservable<long> timerObservable = Observable.Timer(TimeSpan.FromSeconds(seconds));
            IObservable<long> intervalObservable = Observable.Interval(TimeSpan.FromSeconds(1)).TakeUntil(timerObservable);

            IObservable<long> combinedObservable = Observable.CombineLatest(intervalObservable, timerObservable.StartWith(0), (elapsedSeconds, _) => elapsedSeconds + 1);

            combinedObservable.Subscribe(
                elapsedSeconds =>
                {
                    if(logElapsedSeconds)                    
                    {
                        uint timeRemaining = seconds - (uint)elapsedSeconds;
                        if (timeRemaining >= 0)
                        {
                            _log.Info($"[Awake]\n{TimeSpan.FromSeconds(timeRemaining).ToHumanReadableString()}");

                        }
                    }
                },
                () =>
                {
                    Console.WriteLine("Completed timed thread.");
                    CancelExistingThread();

                    _log.Info("Exiting after timed keep-awake.");
                    CompleteExit(Environment.ExitCode);
                },
                _cts.Token);
        }

        /// <summary>
        /// Disable Kepp Awake
        /// </summary>
        public static void SetNoKeepAwake()
        {
            CancelExistingThread();
        }

        #endregion

        #region Private Functions

        private static void StartMonitor()
        {
            Thread monitorThread = new(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    ExecutionState state = _stateQueue.Take();

                    _log.Info($"Setting state to {state}");

                    SetAwakeState(state);
                }
            });
            monitorThread.Start();
        }

        /// <summary>
        /// Sets the computer awake state using the native Win32 SetThreadExecutionState API. This
        /// function is just a nice-to-have wrapper that helps avoid tracking the success or failure of
        /// the call.
        /// </summary>
        /// <param name="state">Single or multiple EXECUTION_STATE entries.</param>
        /// <returns>true if successful, false if failed</returns>
        private static bool SetAwakeState(ExecutionState state)
        {
            try
            {
                var stateResult = Bridge.SetThreadExecutionState(state);
                return stateResult != 0;
            }
            catch
            {
                return false;
            }
        }

        private static ExecutionState ComputeAwakeState(bool keepDisplayOn)
        {
            return keepDisplayOn
                ? ExecutionState.ES_SYSTEM_REQUIRED | ExecutionState.ES_DISPLAY_REQUIRED | ExecutionState.ES_CONTINUOUS
                : ExecutionState.ES_SYSTEM_REQUIRED | ExecutionState.ES_CONTINUOUS;
        }

        private static void CancelExistingThread()
        {
            _log.Info($"Attempting to ensure that the thread is properly cleaned up...");

            // Resetting the thread state.
            _stateQueue.Add(ExecutionState.ES_CONTINUOUS);

            // Next, make sure that any existing background threads are terminated.
            _cts.Cancel();
            _cts.Dispose();

            _cts = new CancellationTokenSource();
            _log.Info("Instantiating of new token source and thread token completed.");
        }

        /// <summary>
        /// Performs a clean exit from Awake.
        /// </summary>
        /// <param name="exitCode">Exit code to exit with.</param>
        private static void CompleteExit(int exitCode)
        {
            CancelExistingThread();

            Bridge.PostQuitMessage(exitCode);
            Environment.Exit(exitCode);
        }

        #endregion
    }
}
