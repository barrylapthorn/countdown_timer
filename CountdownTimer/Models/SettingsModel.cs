// Copyright 2012 lapthorn.net.
//
// This software is provided "as is" without a warranty of any kind. All
// express or implied conditions, representations and warranties, including
// any implied warranty of merchantability, fitness for a particular purpose
// or non-infringement, are hereby excluded. lapthorn.net and its licensors
// shall not be liable for any damages suffered by licensee as a result of
// using the software. In no event will lapthorn.net be liable for any
// lost revenue, profit or data, or for direct, indirect, special,
// consequential, incidental or punitive damages, however caused and regardless
// of the theory of liability, arising out of the use of or inability to use
// software, even if lapthorn.net has been advised of the possibility of
// such damages.
//
// You are free to fork this via github:  https://github.com/barrylapthorn/countdown_timer

using System;

namespace Btl.Models
{
    /// <summary>
    /// A class that just encapsulates the application settings.
    /// </summary>
    internal class SettingsModel
    {
        #region Constructors

        /// <summary>
        /// default constructor.
        /// </summary>
        public SettingsModel()
        {
            //  upgrade settings from a previous application version, if any
            UpgradeSettings();

            //  load the settings into this class.
            LoadSettings();

            //  reset modified state to false
            Modified = false;
        }
        #endregion

        #region Private members

        private TimeSpan _duration;

        private double _windowTop;
        private double _windowLeft;
        private double _windowHeight;
        private double _windowWidth;

        private bool _topMost;

        #endregion

        #region Properties

        /// <summary>
        /// Returns whether the settings have been modified in some way.
        /// </summary>
        public bool Modified { get; private set; }

        /// <summary>
        /// The (last) selected duration of our timer.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                if (_duration == value)
                    return;

                _duration = value;
                Modified = true;

            }
        }

        public bool TopMost
        {
            get
            {
                return _topMost;
            }
            set
            {
                if (_topMost == value)
                    return;

                _topMost = value;
                Modified = true;
            }
        }
        /// <summary>
        /// The main window left position.
        /// </summary>
        public double WindowLeft
        {
            get
            {
                return _windowLeft;
            }
            set
            {
                if (_windowLeft == value)
                    return;

                _windowLeft = value;
                Modified = true;
            }
        }

        /// <summary>
        /// The main window height.
        /// </summary>
        public double WindowHeight
        {
            get
            {
                return _windowHeight;
            }
            set
            {
                if (_windowHeight == value)
                    return;

                _windowHeight = value;
                Modified = true;
            }
        }

        /// <summary>
        /// The main window top position.
        /// </summary>
        public double WindowTop
        {
            get
            {
                return _windowTop;
            }
            set
            {
                if (_windowTop == value)
                    return;

                _windowTop = value;
                Modified = true;
            }
        }

        /// <summary>
        /// The Main window width
        /// </summary>
        public double WindowWidth
        {
            get
            {
                return _windowWidth;
            }
            set
            {
                if (_windowWidth == value)
                    return;

                _windowWidth = value;
                Modified = true;
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Actually update and save/persist the application settings.
        /// </summary>
        private void SaveSettings()
        {
            //  save the selected duration
            Properties.Settings.Default.Duration = Duration;

            //  save the window properties.
            Properties.Settings.Default.Top = WindowTop;
            Properties.Settings.Default.Left = WindowLeft;
            Properties.Settings.Default.Height = WindowHeight;
            Properties.Settings.Default.Width = WindowWidth;
            Properties.Settings.Default.TopMost = TopMost;

            //  persist the settings.
            Properties.Settings.Default.Save();

            Modified = false;
        }

        /// <summary>
        /// Load the application settings into the class.
        /// </summary>
        private void LoadSettings()
        {
            //  Now 'load' existing properties.
            Duration = Properties.Settings.Default.Duration;
            WindowTop = Properties.Settings.Default.Top;
            WindowLeft = Properties.Settings.Default.Left;
            WindowHeight = Properties.Settings.Default.Height;
            WindowWidth = Properties.Settings.Default.Width;
            TopMost = Properties.Settings.Default.TopMost;
        }

        /// <summary>
        /// Reload the settings from the application properties if necessary.
        /// </summary>
        public void Reload()
        {
            LoadSettings();
        }

        /// <summary>
        /// Save the settings if they have been modified, otherwise do nothing.
        /// </summary>
        public void Save()
        {
            if (!Modified)
                return;

            SaveSettings();
        }

        private static void UpgradeSettings()
        {
            //  This is simple 'trick' to persist settings over from a previous
            //  application to a newer version.
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

    }
}
