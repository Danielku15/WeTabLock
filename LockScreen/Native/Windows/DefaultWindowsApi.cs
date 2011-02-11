using System;
using System.ServiceProcess;
using System.Windows.Forms;
using Gma.UserActivityMonitor;
using NLog;

namespace LockScreen.Native.Windows
{
    /// <summary>
    /// A System API wrapper for default windows systems (without sensor).
    /// Initializes the lockscreen using the <code>Pause</code> Button on the Keyboard.
    /// </summary>
    public class DefaultWindowsApi : SystemApi
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The service name of the tablet input service.
        /// </summary>
        private const string TouchScreenServiceName = "TabletInputService";

        /// <summary>
        /// The service start/stop timeout.
        /// </summary>
        private const int Timeout = 2000;

        /// <summary>
        /// Initializes a new Instance of the <see cref="DefaultWindowsApi"/> class.
        /// </summary>
        public DefaultWindowsApi()
        {
            Logger.Debug("Initialize keyhook");
            // initialize global hooks for activating
            HookManager.KeyDown += OnGlobalKeyDown;
        }

        /// <summary>
        /// Handles global mouse events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGlobalMouseHandler(object sender, MouseEventArgs e)
        {
            OnUnlockRequested();
        }

        /// <summary>
        /// Handles global keydown events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGlobalKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Pause || e.KeyData == Keys.Pause)
            {
                OnLockRequested();
            }
            else
            {
                OnUnlockRequested();
            }
        }

        /// <summary>
        /// Activates and enables the touch screen.
        /// </summary>
        public override void EnableTouchScreen()
        {
            HookManager.MouseDown -= OnGlobalMouseHandler;
            StartService();
            SetScreenPower(true);
        }

        /// <summary>
        /// Deactivates and disables the touchscreen.
        /// </summary>
        public override void DisableTouchScreen()
        {
            SetScreenPower(false);
            // add mouse handler for reenabling
            HookManager.MouseDown += OnGlobalMouseHandler;
            HookManager.MouseMove += OnGlobalMouseHandler;
            StopService();
        }

        /// <summary>
        /// Ensures that the given lockscreen is shown on screen. (fullscreen prefered.) 
        /// </summary>
        /// <param name="lockScreen">The lockscreen form</param>
        public override void Show(LockForm lockScreen)
        {
            lockScreen.WindowState = FormWindowState.Maximized;
            lockScreen.FormBorderStyle = FormBorderStyle.None;
            lockScreen.TopMost = true;
            lockScreen.Visible = true;
            lockScreen.Show();
            lockScreen.Activate();
            Win32.SetWinFullScreen(lockScreen.Handle);
        }

        #region TouchScreen Service

        /// <summary>
        /// Starts the touchscreen input service.
        /// </summary>
        public static void StartService()
        {
            Logger.Debug("starting touchscreen service");
            ServiceController service = new ServiceController(TouchScreenServiceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(Timeout);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception e)
            {
                Logger.ErrorException("Failed to start touchscreen service", e);
            }
        }


        /// <summary>
        /// Stops the touchscreen input service.
        /// </summary>
        public static void StopService()
        {
            Logger.Debug("stopping touchscreen service");
            ServiceController service = new ServiceController(TouchScreenServiceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(Timeout);
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch (Exception e)
            {
                Logger.ErrorException("Failed to stop touchscreen service", e);
            }
        }

        #endregion

        #region Power

        /// <summary>
        /// Sets the screen power to active/inactive.
        /// </summary>
        /// <param name="active">whether to activate or disable the screen power</param>
        private static void SetScreenPower(bool active)
        {
#if !UIDEBUG
            Logger.Debug("changing screen power to {0}", active);
            int newValue = (active) ? Win32.SCREEN_TURN_ON : Win32.SCREEN_SHUT_OFF;
            // get mainform handle
            Form form = LockForm.Instance;
            if (form == null || !form.IsHandleCreated)
            {
                Logger.Error("no lockscreen found!");
                return;
            }


            Win32.SendMessage(form.Handle.ToInt32(), Win32.WM_SYSCOMMAND, Win32.SC_MONITORPOWER, newValue);
#endif
        }

        #endregion


    }
}
