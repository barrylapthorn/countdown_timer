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

using System.Windows.Input;
using Btl.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Reflection;

namespace Btl.ViewModels
{
    /// <summary>
    /// The view model for the about window.  It doesn't do anything apart 
    /// from accept the ok/dismiss command, which sends a message to switch
    /// back to the timer view.
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        private readonly string _homepage = "http://www.lapthorn.net";

        public AboutViewModel()
        {
            OK = new RelayCommand(() => OkExecute());
            HomePage = new RelayCommand(() => HomePageExecute());
        }

        public ICommand OK { get; private set; }
        public ICommand HomePage { get; private set; }

        void OkExecute()
        {
            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SwitchToTimerView });
        }

        private void HomePageExecute()
        {
            try
            {
                System.Diagnostics.Process.Start(_homepage);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        public string Version
        {
            get
            {
                return string.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }
    }
}
