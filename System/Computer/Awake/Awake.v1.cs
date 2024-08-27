// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

using FrApp42.System.Computer.Awake.Models;
using FrApp42.System.Computer.Awake.Natives;
using NLog;

namespace FrApp42.System.Computer.Awake.v1
{
    public class Awake
    {

        private static readonly Logger _log;
        private static CancellationTokenSource _cts;
        private static CancellationToken _ct;

        private static Task? _runnerThread;

        static Awake() {
            _log = LogManager.GetCurrentClassLogger();
            _cts = new CancellationTokenSource();
        }

        #region Public Functions

        /// <summary>
        /// Set Indefinite Keep Awake
        /// </summary>
        /// <param name="keepDisplayOn"></param>
        public static void SetIndefiniteKeepAwake(bool keepDisplayOn = false)
        {
            SetIndefiniteKeepAwake(null, null, keepDisplayOn);
        }

        /// <summary>
        /// Set Indefinite Keep Awake
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="failureCallback"></param>
        /// <param name="keepDisplayOn"></param>
        public static void SetIndefiniteKeepAwake(Action<bool> callback, Action? failureCallback, bool keepDisplayOn = false)
        {
            _cts.Cancel();

            callback ??= (bool success) => { };
            failureCallback ??= () => { };

            try
            {
                if (_runnerThread != null && !_runnerThread.IsCanceled)
                {
                    _runnerThread.Wait(_ct);
                }
            }
            catch (OperationCanceledException)
            {
                _log.Info("Confirmed background thread cancellation when setting indefinite keep awake.");
            }

            _cts = new CancellationTokenSource();
            _ct = _cts.Token;

            try
            {
                _runnerThread = Task.Run(() => RunIndefiniteLoop(keepDisplayOn), _ct)
                    .ContinueWith((result) => callback(result.Result), TaskContinuationOptions.OnlyOnRanToCompletion)
                    .ContinueWith((result) => failureCallback, TaskContinuationOptions.NotOnRanToCompletion);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
        }

        /// <summary>
        /// Disable Awake
        /// </summary>
        public static void SetNoKeepAwake()
        {
            _cts.Cancel();

            try
            {
                if (_runnerThread != null && !_runnerThread.IsCanceled)
                {
                    _runnerThread.Wait(_ct);
                }
            }
            catch (OperationCanceledException)
            {
                _log.Info("Confirmed background thread cancellation when disabling explicit keep awake.");
            }
        }

        #endregion

        #region Private Functions

        private static ExecutionState ComputeAwakeState(bool keepDisplayOn)
        {
            return keepDisplayOn
                ? ExecutionState.ES_SYSTEM_REQUIRED | ExecutionState.ES_DISPLAY_REQUIRED | ExecutionState.ES_CONTINUOUS
                : ExecutionState.ES_SYSTEM_REQUIRED | ExecutionState.ES_CONTINUOUS;
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


        private static bool RunIndefiniteLoop(bool keepDisplayOn = false)
        {
            bool success;
            if (keepDisplayOn)
            {
                success = SetAwakeState(ExecutionState.ES_SYSTEM_REQUIRED | ExecutionState.ES_DISPLAY_REQUIRED | ExecutionState.ES_CONTINUOUS);
            }
            else
            {
                success = SetAwakeState(ExecutionState.ES_SYSTEM_REQUIRED | ExecutionState.ES_CONTINUOUS);
            }

            try
            {
                if (success)
                {
                    _log.Info($"Initiated indefinite keep awake in background thread: {Bridge.GetCurrentThreadId()}. Screen on: {keepDisplayOn}");

                    WaitHandle.WaitAny(new[] { _ct.WaitHandle });
                }
                else
                {
                    _log.Info("Could not successfully set up indefinite keep awake.");
                }
            }
            catch (OperationCanceledException ex)
            {
                // Task was clearly cancelled.
                _log.Info($"Background thread termination: {Bridge.GetCurrentThreadId()}. Message: {ex.Message}");
            }
            return success;
        }

        #endregion

    }
}
