using System;
using System.Drawing;
using System.Windows.Forms;
using LockScreen.Ui;

namespace LockScreen
{
    public class BatteryWidget : StatusBarWidget
    {
        protected override Image Icon
        {
            get { return WidgetResources.battery; }
        }

        protected override string Label
        {
            get
            {
                PowerStatus power = SystemInformation.PowerStatus;

                string s = string.Format("{0:P}", power.BatteryLifePercent);
                switch (power.PowerLineStatus)
                {
                    case PowerLineStatus.Online:
                        s += " (Laden...)";
                        break;
                    case PowerLineStatus.Offline:
                        TimeSpan t = TimeSpan.FromSeconds(power.BatteryLifeRemaining);
                        s += "(" + string.Format("{0:HH}std {0:mm}min", new DateTime(t.Ticks)) + ")";
                        break;
                    //case PowerLineStatus.Unknown:
                    //    s += "Unknown Status";
                }
                return s;
            }
        }
    }
}
