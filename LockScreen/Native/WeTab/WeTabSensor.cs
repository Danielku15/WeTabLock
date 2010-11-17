using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Sensors;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace LockScreen.Native.WeTab
{
    /// <summary>
    /// Represents the Sensor built into the WeTab.
    /// </summary>
    [SensorDescription("0c51f854-d63f-42cf-ad25-31c6b5224c05")]
    public class WeTabSensor : Sensor
    {
        /// <summary>
        /// Gets the current sensor state. 
        /// </summary>
        public WeTabSensorData CurrentWeTabSensorData
        {
            get
            {
                return new WeTabSensorData(this, DataReport);
            }
        }
    }


    /// <summary>
    /// Represents a sensor state.
    /// </summary>
    public class WeTabSensorData
    {
        private static readonly PropertyKey GetMessage = new PropertyKey(new Guid(0x72b26341, 0x1cea, 0x4473, 130, 0xc1, 0xcc, 0xee, 0xb9, 0x74, 0x87, 90), 2);
        private static readonly PropertyKey GetAcceleratorXRotation = new PropertyKey(new Guid(0x72b26341, 0x1cea, 0x4473, 130, 0xc1, 0xcc, 0xee, 0xb9, 0x74, 0x87, 90), 3);
        private static readonly PropertyKey GetAcceleratorYRotation = new PropertyKey(new Guid(0x72b26341, 0x1cea, 0x4473, 130, 0xc1, 0xcc, 0xee, 0xb9, 0x74, 0x87, 90), 4);
        private static readonly PropertyKey GetAcceleratorZRotation = new PropertyKey(new Guid(0x72b26341, 0x1cea, 0x4473, 130, 0xc1, 0xcc, 0xee, 0xb9, 0x74, 0x87, 90), 5);

        /// <summary>
        /// Gets the type of the last event.
        /// </summary>
        public WeTabSensorMessage Message { get; private set; }

        /// <summary>
        /// Gets the current x axis rotation.
        /// </summary>
        public int XRotation { get; private set; }
        /// <summary>
        /// Gets the current y axis rotation.
        /// </summary>
        public int YRotation { get; private set; }
        /// <summary>
        /// Gets the current z axis rotation.
        /// </summary>
        public int ZRotation { get; private set; }

        /// <summary>
        /// Gets the current tablet orientation, based upon the current rotation.
        /// </summary>
        public Orientation Orientation { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="WeTabSensorData"/> class.
        /// </summary>
        /// <param name="sensor">The sensor.</param>
        /// <param name="report">The report object.</param>
        public WeTabSensorData(Sensor sensor, SensorReport report)
        {
            // Get the message type
            int message = Convert.ToInt32(report.Values[GetMessage.FormatId][0]);
            switch (message)
            {
                case 0xea:
                    Message = WeTabSensorMessage.RotationChanged;
                    break;
                case 0x94:
                    Message = WeTabSensorMessage.SensorButtonDown;
                    break;
                case 0x95:
                    Message = WeTabSensorMessage.SensorButtonUp;
                    break;
                default:
                    Message = WeTabSensorMessage.Unknown;
                    break;
            }

            // To read the accelerometer rotation we need to update 
            // the x,y,z properties
            DataFieldInfo[] info = {
                                       new DataFieldInfo(GetAcceleratorXRotation, "XLRX"),
                                       new DataFieldInfo(GetAcceleratorYRotation, "XLRY"),
                                       new DataFieldInfo(GetAcceleratorZRotation, "XLRZ")
                                   };
            IDictionary<PropertyKey, object> result = sensor.SetProperties(info);
            XRotation = Convert.ToInt32(result[GetAcceleratorXRotation]);
            YRotation = Convert.ToInt32(result[GetAcceleratorYRotation]);
            ZRotation = Convert.ToInt32(result[GetAcceleratorZRotation]);
            Orientation = CalculateOrientation(XRotation, YRotation, ZRotation);
        }

        /// <summary>
        /// Calculates the rotation based upon the given rotation value. 
        /// </summary>
        /// <param name="x">The x axis rotation</param>
        /// <param name="y">The y axis rotation</param>
        /// <param name="z">The z axis rotation</param>
        /// <returns>The tablet orientation</returns>
        private static Orientation CalculateOrientation(int x, int y, int z)
        {
            string xs = Convert.ToString(x, 0x10).ToUpper().PadLeft(4, '0');
            string ys = Convert.ToString(y, 0x10).ToUpper().PadLeft(4, '0');
            string zs = Convert.ToString(z, 0x10).ToUpper().PadLeft(4, '0');

            int thirdX = Convert.ToInt16(Convert.ToChar(xs.Substring(2, 1)));
            int thirdY = Convert.ToInt16(Convert.ToChar(ys.Substring(2, 1)));
            int thirdZ = Convert.ToInt16(Convert.ToChar(zs.Substring(2, 1)));


            // 0x00E0 <= z <= 0x00FF
            if (zs.StartsWith("00") && ((thirdZ >= 0x45) && (thirdZ <= 70)))
            {
                return Orientation.Unknown2;
            }
            // 0x0100 <= z <= 0x01FF
            if (zs.StartsWith("01"))
            {
                return Orientation.Unknown2;
            }

            // 0xFF00 <= x
            if (xs.StartsWith("FF"))
            {
                // 0x
                if (ys.StartsWith("FF"))
                {
                    if (((thirdX >= 0x37) && (thirdX <= 70)) && ((thirdY >= 0x31) && (thirdY <= 0x39)))
                    {
                        return 0;
                    }
                    if (((thirdX >= 0x45) && (thirdX <= 70)) && ((thirdY >= 0x41) && (thirdY <= 0x43)))
                    {
                        return 0;
                    }
                    if ((thirdX >= 0x31) && (thirdX <= 0x42))
                    {
                        if ((thirdY >= 0x39) && (thirdY <= 70))
                        {
                            return Orientation.Portrait270;
                        }
                        if (thirdY == 0x30)
                        {
                            return Orientation.Portrait270;
                        }
                    }
                    if ((thirdX == 0x30) && ((thirdY >= 0x43) && (thirdY <= 0x45)))
                    {
                        return Orientation.Portrait270;
                    }
                    if ((thirdX == 0x30) && ((thirdY >= 0x31) && (thirdY <= 50)))
                    {
                        return Orientation.LandscapeNormal;
                    }
                    if (((thirdX >= 0x44) && (thirdX <= 70)) && (thirdY == 0x30))
                    {
                        return Orientation.LandscapeNormal;
                    }
                    if ((thirdX == 0x31) && (thirdY == 0x31))
                    {
                        return Orientation.LandscapeNormal;
                    }
                }
                else if (ys.StartsWith("FE"))
                {
                    if (((thirdX >= 0x44) && (thirdX <= 70)) && ((thirdY >= 0x45) && (thirdY <= 70)))
                    {
                        return Orientation.LandscapeNormal;
                    }
                    if ((thirdX == 0x45) && (thirdY == 0x30))
                    {
                        return Orientation.LandscapeNormal;
                    }
                }
                else
                {
                    if ((thirdX >= 0x38) && (thirdX <= 70))
                    {
                        if ((thirdY >= 0x34) && (thirdY <= 0x44))
                        {
                            return Orientation.LandscapeFlipped;
                        }
                    }
                    else if ((thirdX == 0x30) && ((thirdY >= 0x34) && (thirdY <= 0x43)))
                    {
                        return Orientation.LandscapeFlipped;
                    }
                    if (((thirdX >= 0x31) && (thirdX <= 0x41)) && ((thirdY >= 0x30) && (thirdY <= 0x35)))
                    {
                        return Orientation.Portrait270;
                    }
                }
            }
            else if (xs.StartsWith("FE"))
            {
                if (ys.StartsWith("FF"))
                {
                    if ((thirdX >= 0x45) && (thirdX <= 70))
                    {
                        if ((thirdY >= 0x44) && (thirdY <= 70))
                        {
                            return Orientation.Portrait270;
                        }
                    }
                    else if ((thirdX == 0x30) && ((thirdY >= 0x45) && (thirdY <= 70)))
                    {
                        return Orientation.Portrait270;
                    }
                    if ((thirdX == 0x45) && (thirdY == 0x42))
                    {
                        return Orientation.Portrait270;
                    }
                }
                else if (ys.StartsWith("00"))
                {
                    return Orientation.Portrait270;
                }
            }
            else if (ys.StartsWith("FF"))
            {
                if (((thirdX >= 0x33) && (thirdX <= 0x44)) && ((thirdY >= 0x38) && (thirdY <= 70)))
                {
                    return Orientation.Portrait90;
                }
                if ((thirdX == 0x41) && (thirdY == 0x30))
                {
                    return Orientation.Portrait90;
                }
                if (thirdX == 0x45)
                {
                    if ((thirdY >= 0x45) && (thirdY <= 70))
                    {
                        return Orientation.Portrait90;
                    }
                    if ((thirdY >= 0x42) && (thirdY <= 0x44))
                    {
                        return Orientation.Portrait90;
                    }
                }
                if (((thirdX >= 0x30) && (thirdX <= 0x36)) && ((thirdY >= 0x31) && (thirdY <= 0x39)))
                {
                    return Orientation.LandscapeNormal;
                }
                if ((thirdX == 70) && (thirdY == 0x30))
                {
                    return Orientation.LandscapeNormal;
                }
                if (((thirdX >= 0x30) && (thirdX <= 0x31)) && (thirdY == 0x30))
                {
                    return Orientation.LandscapeNormal;
                }
            }
            else if (ys.StartsWith("FE"))
            {
                if ((thirdX == 50) && (thirdY == 0x44))
                {
                    return Orientation.LandscapeNormal;
                }
                if ((thirdX == 0x30) && (thirdY == 70))
                {
                    return Orientation.LandscapeNormal;
                }
                if ((thirdX == 0x31) && ((thirdY >= 0x45) && (thirdY <= 70)))
                {
                    return Orientation.LandscapeNormal;
                }
            }
            else
            {
                if (((thirdX >= 0x30) && (thirdX <= 0x37)) && ((thirdY >= 0x34) && (thirdY <= 0x45)))
                {
                    return Orientation.LandscapeFlipped;
                }
                if ((thirdX >= 0x33) && (thirdX <= 0x44))
                {
                    if ((thirdY >= 0x30) && (thirdY <= 0x34))
                    {
                        return Orientation.Portrait90;
                    }
                    if (thirdY == 70)
                    {
                        return Orientation.Portrait90;
                    }
                }
            }
            return Orientation.Unknown1;

        }


    }

    /// <summary>
    /// Lists all messages send by the sensor.
    /// </summary>
    public enum WeTabSensorMessage
    {
        /// <summary>
        /// Unknown message
        /// </summary>
        Unknown,
        /// <summary>
        /// The screen rotation/orientation changed
        /// </summary>
        RotationChanged,
        /// <summary>
        /// Sensor button pushed down
        /// </summary>
        SensorButtonDown,
        /// <summary>
        /// Sensor button released
        /// </summary>
        SensorButtonUp
    }

    /// <summary>
    /// Lists all screen rotations.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Normal landscape
        /// </summary>
        LandscapeNormal,
        /// <summary>
        /// Flipped/inverse landscape
        /// </summary>
        LandscapeFlipped,
        /// <summary>
        /// Usb ports on top
        /// </summary>
        Portrait90,
        /// <summary>
        /// Usb ports on bottom
        /// </summary>
        Portrait270,
        /// <summary>
        /// Unknown state
        /// </summary>
        Unknown1,
        /// <summary>
        /// Unknown state caused by z rotation
        /// </summary>
        Unknown2,
    }
}
