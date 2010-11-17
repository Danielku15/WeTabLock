using System;
using System.Timers;
using LockScreen.Native.Windows;
using Microsoft.WindowsAPICodePack.Sensors;
using NLog;

namespace LockScreen.Native.WeTab
{
    public class WeTabWindowsApi : DefaultWindowsApi
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly WeTabSensor _sensor;
        private readonly Timer _timer;

        /// <summary>
        /// Initializes a new Instance of the <see cref="DefaultWindowsApi"/> class.
        /// </summary>
        public WeTabWindowsApi()
        {
            // initialize sensor for and deactivating 
            try
            {
                _sensor = SensorManager.GetSensorsByTypeId<WeTabSensor>()[0];
                _sensor.DataReportChanged += OnDataReportChanged;
                _timer = new Timer(750) {AutoReset = false, Enabled = false};
                _timer.Elapsed += LockTimer;
            }
            catch (IndexOutOfRangeException)
            {
                throw new ApplicationException("No WeTab Sensor found!");
            }
            catch (Exception)
            {
                throw new ApplicationException("Error initializing sensor!");
            }
        }

        private void LockTimer(object sender, ElapsedEventArgs e)
        {
            Logger.Debug("Sensortimer elapsed, locked={0}", LockForm.Instance.IsLocked);
            _timer.Stop();
            if (LockForm.Instance.IsLocked)
            {
                OnUnlockRequested();
            }
            else
            {
                OnLockRequested();
            }
        }

        void OnDataReportChanged(Sensor sender, EventArgs e)
        {
            // get sensor
            WeTabSensor sensor = sender as WeTabSensor;
            if (sensor != null)
            {
                Logger.Debug("Sensor reported");

                // get data object
                WeTabSensorData r = sensor.CurrentWeTabSensorData;
                switch (r.Message)
                {
                    case WeTabSensorMessage.SensorButtonDown:
                        Logger.Debug("Sensor button pressed");
                        _timer.Enabled = true;
                        Logger.Debug("Sensortimer started");
                        break;
                    case WeTabSensorMessage.SensorButtonUp:
                        Logger.Debug("Sensor button released");
                        _timer.Enabled = false;
                        Logger.Debug("Sensortimer interupted");
                        break;
                }
            }
        }


    }
}
