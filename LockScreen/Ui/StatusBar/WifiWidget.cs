using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using LockScreen.Properties;

namespace LockScreen.Ui.StatusBar
{
    public class WifiWidget : StatusBarWidget
    {
        private NetworkInterface _wifi;
        #region Overrides of StatusBarWidget

        protected override Image Icon
        {
            get { return WidgetResources.wifi; }
        }

        protected override string Label
        {
            get
            {
                if (_wifi == null)
                    return "Kein WLan Gerät gefunden";

                string ip = "Keine IP";
                foreach (UnicastIPAddressInformation addr in _wifi.GetIPProperties().UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = addr.Address.ToString();
                        break;
                    }
                }
                return ip;
            }
        }

        public WifiWidget()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface inter in interfaces)
            {
                if (inter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    _wifi = inter;
                    break;
                }
            }
        }

        #endregion
    }
}
