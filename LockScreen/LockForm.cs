using System;
using System.Drawing;
using System.Windows.Forms;
using LockScreen.Config;
using LockScreen.Native;
using Timer = System.Timers.Timer;

namespace LockScreen
{
    /// <summary>
    /// The main lock form
    /// </summary>
    public partial class LockForm : Form
    {
        private static LockForm _instance;
        private bool _canClose;

        // Hack: MouseMove event is sent even if no mouse is moved
        private Point _lastMousePosition;

        // relock 
        private readonly object _relockLock = new object();
        private int _relockCountDown;
        private readonly Timer _relockTimer;

        /// <summary>
        /// Gets or sets whether the lockscreen is currently locked
        /// </summary>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Gets the global instance of the lockform
        /// </summary>
        public static LockForm Instance
        {
            get { return _instance ?? (_instance = new LockForm()); }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LockForm"/> class.
        /// </summary>
        private LockForm()
        {
            InitializeComponent();

            // initialize relock timer
            _relockTimer = new Timer(1000);
            _relockCountDown = LockScreenSettings.Current.RelockTime;
            _relockTimer.AutoReset = true;
            _relockTimer.Elapsed += OnRelockTimerElapsed;

            // Hide and create window
            Visible = false;
            RecreateHandle();

            // Display initializing baloon
            if (LockScreenSettings.Current.DisplayInitInfo)
            {
                icoTray.ShowBalloonTip(3000, "LockScreen loaded", "Tap and Hold the Sensor Button for 1 Second to lock and unlock the screen.", ToolTipIcon.Info);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!_canClose)
                e.Cancel = true;
        }

        /// <summary>
        /// Called when a mouse event occures.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void OnGlobalMouseHandler(object sender, MouseEventArgs e)
        {
            // if we are relocking restart relocker
            // Hack: MouseMove event is sent even if no mouse is moved
            if (_lastMousePosition != e.Location)
            {
                _lastMousePosition = e.Location;
                if (_relockTimer.Enabled)
                {
                    RestartRelock();
                }
            }
        }

        /// <summary>
        /// Called when a keyboard event occures.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void OnGlobalKeyDown(object sender, KeyEventArgs e)
        {
            // if we are relocking restart relocker
            if (_relockTimer.Enabled)
            {
                RestartRelock();
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SystemApi.Instance.LockRequested += Lock;
            SystemApi.Instance.UnlockRequested += Restore;
        }

        #region Relock

        /// <summary>
        /// Starts the relock timer.
        /// </summary>
        private void StartRelock()
        {
            RestartRelock();
        }

        /// <summary>
        /// Resets and starts the relock timer.
        /// </summary>
        private void RestartRelock()
        {
            lock (_relockLock)
            {
                _relockCountDown = LockScreenSettings.Current.RelockTime;
                BeginInvoke(new MethodInvoker(() => UpdateTimerLabel(_relockCountDown)));
            }
            _relockTimer.Stop();
            _relockTimer.Start();
        }

        /// <summary>
        /// Called when the relock timer elapsed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void OnRelockTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_relockLock)
            {
                _relockCountDown--;

                BeginInvoke(new Action<int>(UpdateTimerLabel), _relockCountDown);
                if (_relockCountDown <= 0)
                {
                    BeginInvoke(new MethodInvoker(Lock));
                    StopRelock();
                }
            }
        }

        /// <summary>
        /// Stops the relock.
        /// </summary>
        private void StopRelock()
        {
            _relockTimer.Stop();
        }

        /// <summary>
        /// Updates the timer label.
        /// </summary>
        /// <param name="countdown">The countdown.</param>
        private void UpdateTimerLabel(int countdown)
        {
            lblRelock.Text = string.Format("Time till autolock: {0}s", countdown);
        }

        #endregion

        #region Locking

        /// <summary>
        /// Starts the restore process.
        /// </summary>
        private void Restore()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(Restore));
                return;
            }

            if (IsLocked)
            {
                IsLocked = false;
                SystemApi.Instance.Show(this);
                Refresh();
                SystemApi.Instance.EnableTouchScreen();
                Refresh();
                // start relock timer on unlock
                StartRelock();
            }
        }

        /// <summary>
        /// Starts the locking process.
        /// </summary>
        private void Lock()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(Lock));
                return;
            }
            // disable relock on lock performing
            _relockTimer.Stop();
            //Visible = false;
            IsLocked = true;
            SystemApi.Instance.DisableTouchScreen();
        }
        #endregion

        /// <summary>
        /// Called when the unlock button was clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnUnlockClick(object sender, EventArgs e)
        {
            Visible = false;
            IsLocked = false;
            // quit relock on complete unlock
            _relockTimer.Stop();
        }

        /// <summary>
        /// Called when the close menu entry was clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCloseClick(object sender, EventArgs e)
        {
            // quit relock on shutdown
            _relockTimer.Stop();
            _canClose = true;
            Application.Exit();
        }


    }
}
