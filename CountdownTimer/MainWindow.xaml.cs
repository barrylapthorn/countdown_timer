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
using GalaSoft.MvvmLight.Messaging;

namespace Btl
{
    /// <summary>
    /// Most of the cruft in this class is to manage the taskbar item preview
    /// and the window title.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //  Reposition the window to where it last was.
            LoadWindowPosition();

            //  Consume any TaskbarItemMessages that are being dispatched around
            //  the mvvm framework.
            Messenger.Default.Register<TaskbarItemMessage>(this, ConsumeTaskbarItemMessage);
            Messenger.Default.Register<SimpleMessage>(this, ConsumeSimpleMessage);

            WindowTitle = Title;
        }

        /// <summary>
        /// Update the TaskbarItemInfo values with whatever is specified in the
        /// message.
        /// </summary>
        /// <param name="message"></param>
        void ConsumeTaskbarItemMessage(TaskbarItemMessage message)
        {
            if (message == null)
                return;

            TaskbarItemInfo.ProgressState = message.State;

            //  if the taskbar item message carried a (percentage) value,
            //  update the taskbar progressvalue with it.
            if (message.HasValue)
                TaskbarItemInfo.ProgressValue = message.Value;
        }

        /// <summary>
        /// If the user has changed the settings, the only thing that affects
        /// the MainWindow state is its TopMost value.
        /// </summary>
        private void OnSettingsChanged()
        {
            var settings = SettingsModelFactory.GetNewSettings();
            Topmost = settings.TopMost;
        }

        /// <summary>
        /// Only respond to settings that directly affect this main window,
        /// and do not handle the rest.
        /// </summary>
        /// <param name="message"></param>
        private void ConsumeSimpleMessage(SimpleMessage message)
        {
            switch (message.Type)
            {
                case SimpleMessage.MessageType.TimerTick:
                    //  this happens the most so put it first
                    Title = message.Message;
                    break;
                case SimpleMessage.MessageType.SettingsChanged:
                    OnSettingsChanged();
                    break;
                case SimpleMessage.MessageType.TimerStop:
                    Title = WindowTitle;
                    break;
                case SimpleMessage.MessageType.TimerReset:
                    //  restore window title if we reset.
                    Title = WindowTitle;
                    break;
            }
        }

        /// <summary>
        /// Window closing event.  Save the window position.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //  Persist the window position.
            SaveWindowPosition();
        }

        /// <summary>
        /// Save the window position into the application settings.
        /// </summary>
        private void SaveWindowPosition()
        {
            var settings = SettingsModelFactory.GetNewSettings();

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
            var settings = SettingsModelFactory.GetNewSettings();

            if (settings.WindowWidth > 0)
            {
                this.Top = settings.WindowTop;
                this.Left = settings.WindowLeft;
                this.Height = settings.WindowHeight;
                this.Width = settings.WindowWidth;
            }

            this.Topmost = settings.TopMost;
        }

        /// <summary>
        /// Store the window title away as we change it later.
        /// </summary>
        private string WindowTitle { get; set; }

    }
}
