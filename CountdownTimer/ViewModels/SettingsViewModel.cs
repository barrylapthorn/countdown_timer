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
using System.Windows.Input;
using System.Windows.Media;
using Btl.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;


namespace Btl.ViewModels
{
    /// <summary>
    /// The view-model for the Settings view.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        readonly ISettingsModel _settings = SettingsModelFactory.GetNewSettings();

        public SettingsViewModel()
        {
            OK = new RelayCommand(() => OkExecute(), CanOkExecute);
            Cancel = new RelayCommand(() => CancelExecute());
        }

        #region Properties
        /// <summary>
        /// The timer duration
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return _settings.Duration;
            }
            set
            {
                if (_settings.Duration == value)
                    return;
                _settings.Duration = value;
                RaisePropertyChanged("Duration");
            }
        }


        /// <summary>
        /// Is this the first time the application has been run?
        /// </summary>
        public bool FirstRun
        {
            get
            {
                return _settings.FirstRun;
            }
            set
            {
                if (_settings.FirstRun == value)
                    return;
                _settings.FirstRun = value;
                RaisePropertyChanged("FirstRun");
            }
        }

        /// <summary>
        /// Do we want our window to be top-most?
        /// </summary>
        public bool TopMost
        {
            get
            {
                return _settings.TopMost;
            }
            set
            {
                if (_settings.TopMost == value)
                    return;
                _settings.TopMost = value;
                RaisePropertyChanged("TopMost");
            }
        }

        /// <summary>
        /// The clock font size.
        /// </summary>
        public double FontSize
        {
            get
            {
                return _settings.FontSize;
            }
            set
            {
                if (_settings.FontSize == value)
                    return;
                _settings.FontSize = value;
                RaisePropertyChanged("FontSize");
            }
        }

        /// <summary>
        /// The clock font family
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _settings.FontFamily;
            }
            set
            {
                if (_settings.FontFamily == value)
                    return;
                _settings.FontFamily = value;
                RaisePropertyChanged("FontFamily");
            }
        }

        /// <summary>
        /// Play a beep when starting.
        /// </summary>
        public bool PlayBeep
        {
            get
            {
                return _settings.PlayBeep;
            }
            set
            {
                if (_settings.PlayBeep == value)
                    return;
                _settings.PlayBeep = value;
                RaisePropertyChanged("PlayBeep");
            }
        }

        /// <summary>
        /// Play an exclamation when complete.
        /// </summary>
        public bool PlayExclamation
        {
            get
            {
                return _settings.PlayExclamation;
            }
            set
            {
                if (_settings.PlayExclamation == value)
                    return;
                _settings.PlayExclamation = value;
                RaisePropertyChanged("PlayExclamation");
            }
        }
        #endregion

        #region Commands

        public ICommand OK { get; private set; }
        public ICommand Cancel { get; private set; }

        private void OkExecute()
        {
            _settings.Save();

            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SettingsChanged });
            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SwitchToTimerView });
        }

        private bool CanOkExecute()
        {
            return _settings.Modified;
        }

        private void CancelExecute()
        {
            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SwitchToTimerView });
        }

        #endregion
    }
}
