using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LockScreen.Config
{
    /// <summary>
    /// A class encapsulating the settings.
    /// </summary>
    [Serializable]
    public class LockScreenSettings 
    {
        private const string ConfigFile = "lock.cfg";

        /// <summary>
        /// Gets or sets the time till the screen will be relocked
        /// after screen inactivity.
        /// </summary>
        public int RelockTime { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to display a info balloon after init.
        /// </summary>
        /// <value><c>true</c> if aa info balloon is displayed after init; otherwise, <c>false</c>.</value>
        public bool DisplayInitInfo { get; set; }



        #region Singleton

        private static LockScreenSettings _current;
        /// <summary>
        /// Gets the current configuration instance.
        /// </summary>
        /// <remarks>
        /// Initialize:
        /// Try to load default config file
        /// a) If no config exists, create default config file
        /// b) On loading error, display error, use default
        /// </remarks>
        public static LockScreenSettings Current
        {
            get
            {
                if (_current == null)
                {
                    try
                    {
                        LoadSettings(DefaultConfigPath);
                    }
                    catch (FileNotFoundException)
                    {
                        _current = DefaultSettings;
                        SaveSettings(_current, DefaultConfigPath);
                    }
                    catch (Exception)
                    {
                        // TODO: Display error / create default
                        _current = DefaultSettings;
                    }
                }

                return _current;
            }
        }

        /// <summary>
        /// Gets a new instance of the default settings.
        /// </summary>
        public static LockScreenSettings DefaultSettings
        {
            get
            {
                LockScreenSettings def = new LockScreenSettings();
                def.RelockTime = 10;
                def.DisplayInitInfo = true;

                return def;
            }
        } 

        #endregion

        #region Loading/Saving
       
        /// <summary>
        /// Loads the settings from the given file.
        /// </summary>
        /// <param name="file">The file to load from</param>
        /// <returns>The loaded instance</returns>
        public static LockScreenSettings LoadSettings(string file)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LockScreenSettings));
            using (Stream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
            {
                return (LockScreenSettings)xmlSerializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// Saves the settings from the given file.
        /// </summary>
        /// <param name="current">The settings to save</param>
        /// <param name="path">The path to save into</param>
        private static void SaveSettings(LockScreenSettings current, string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LockScreenSettings));
            using (Stream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                xmlSerializer.Serialize(fs, current);
            }
        }

        /// <summary>
        /// Gets the path to the default config file;
        /// </summary>
        public static string DefaultConfigPath
        {
            get { return Path.Combine(Application.StartupPath, ConfigFile); }
        }
 
        #endregion
    }
}
