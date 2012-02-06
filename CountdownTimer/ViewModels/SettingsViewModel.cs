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
using Btl.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;


namespace Btl.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        readonly SettingsModel _settings = new SettingsModel();

        public SettingsViewModel()
        {
            OK = new RelayCommand(() => OkExecute(), CanOkExecute);
            Cancel = new RelayCommand(() => CancelExecute());
        }

        #region Properties
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
        #endregion

        #region Commands

        public ICommand OK { get; private set; }
        public ICommand Cancel { get; private set; }

        void OkExecute()
        {
            _settings.Save();

            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SettingsChanged });

            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SwitchToTimerView });
        }

        bool CanOkExecute()
        {
            return _settings.Modified;
        }

        void CancelExecute()
        {
            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SwitchToTimerView });
        }

        #endregion
    }
}
