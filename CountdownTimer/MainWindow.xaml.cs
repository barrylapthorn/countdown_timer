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

using System.Windows;
using Btl.Models;

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
            var settings = new SettingsModel();

            settings.WindowTop = this.Top;
            settings.WindowLeft = this.Left;
            settings.WindowHeight = this.Height;
            settings.WindowWidth = this.Width;

            settings.Save();
        }

        /// <summary>
        /// Load the window position from the application settings.
        /// </summary>
        private void LoadWindowPosition()
        {
            var settings = new SettingsModel();

            if (settings.WindowWidth > 0)
            {
                this.Top = settings.WindowTop;
                this.Left = settings.WindowLeft;
                this.Height = settings.WindowHeight;
                this.Width = settings.WindowWidth;
            }

            this.Topmost = settings.TopMost;
        }
    }
}
