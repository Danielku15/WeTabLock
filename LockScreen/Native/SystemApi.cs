using System;
using LockScreen.Native.WeTab;
using LockScreen.Native.Windows;
using NLog;

namespace LockScreen.Native
{
    /// <summary>
    /// The base class for accessing the system api.
    /// </summary>
    public abstract class SystemApi
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static SystemApi _instance;

        /// <summary>
        /// Gets the SystemAPI wrapper for the current system.
        /// </summary>
        public static SystemApi Instance
        {
            get
            {
                if(_instance == null)
                {
                    int platform = (int)Environment.OSVersion.Platform;
                    Logger.Debug("Initializing Using {0}", platform);
                    if ((platform == 4) || (platform == 6) || (platform == 128))
                    {
                        throw new ApplicationException("Unix/Mono is not yet supported");
                    }

                    try
                    {
                        // TODO: Better WeTab Detection
                        // Try using wetab sensor
                        Logger.Debug("Using WeTab API");
                        _instance = new WeTabWindowsApi();
                    }
                    catch (Exception e)
                    {
                        // TODO: Message fallback to user
                        Logger.DebugException("Error using WeTab API, fallback to default Windows API", e);
                        // fallback on default
                        _instance = new DefaultWindowsApi();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Activates and enables the touch screen.
        /// </summary>
        public abstract void EnableTouchScreen();

        /// <summary>
        /// Deactivates and disables the touchscreen.
        /// </summary>
        public abstract void DisableTouchScreen();

        /// <summary>
        /// Ensures that the given lockscreen is shown on screen. (fullscreen prefered.) 
        /// </summary>
        /// <param name="lockScreen">The lockscreen form</param>
        public abstract void Show(LockForm lockScreen);

        /// <summary>
        /// This event is thrown if the system should be locked.
        /// </summary>
        public event LockEventHandler LockRequested;

        /// <summary>
        /// Invokes the <see cref="LockRequested"/> event.
        /// </summary>
        protected virtual void OnLockRequested()
        {
            if (LockRequested != null)
                LockRequested();
        }

        /// <summary>
        /// This event is thrown if the system should be unlocked.
        /// </summary>
        public event LockEventHandler UnlockRequested;

        /// <summary>
        /// Invokes the <see cref="UnlockRequested"/> event.
        /// </summary>
        protected virtual void OnUnlockRequested()
        {
            if (UnlockRequested != null)
                UnlockRequested();
        }

    }

    /// <summary>
    /// A delegate used for lock/unlock events.
    /// </summary>
    public delegate void LockEventHandler();
}
