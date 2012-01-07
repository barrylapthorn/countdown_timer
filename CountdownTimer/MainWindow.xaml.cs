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
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Btl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadWindowPosition();

            SettingsWindow s = new SettingsWindow();
            s.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Window closing event.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveWindowPosition();
        }

        /// <summary>
        /// Save the window position into the application settings.
        /// </summary>
        private void SaveWindowPosition()
        {
            Properties.Settings.Default.Top = this.Top;
            Properties.Settings.Default.Left = this.Left;
            Properties.Settings.Default.Height = this.Height;
            Properties.Settings.Default.Width = this.Width;

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Load the window position from the application settings.
        /// </summary>
        private void LoadWindowPosition()
        {
            UpgradeSettings();

            if (Properties.Settings.Default.Width > 0)
            {
                this.Top = Properties.Settings.Default.Top;
                this.Left = Properties.Settings.Default.Left;
                this.Height = Properties.Settings.Default.Height;
                this.Width = Properties.Settings.Default.Width;
            }
        }

        /// <summary>
        /// After defining 'UpgradeRequired' in our settings.settings and default
        /// it to 'true', we check here to see if we need to import settings from
        /// a previous application/assembly version.  In other words, a newer
        /// assembly has 'UpgradeRequired=True' and triggers the upgrade.
        /// </summary>
        private static void UpgradeSettings()
        {
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
