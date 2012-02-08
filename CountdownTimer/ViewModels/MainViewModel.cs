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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Btl.Models;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace Btl.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel = null;

        private readonly static TimerViewModel _timerViewModel;
        private readonly static SettingsViewModel _settingsViewModel;
        private readonly static AboutViewModel _aboutViewModel;


        /// <summary>
        /// Use a static constructor as it is the easiest way to handle any
        /// exceptions that might be thrown when creating the view models.
        /// </summary>
        static MainViewModel()
        {
            try
            {
                _timerViewModel = new TimerViewModel();
                _settingsViewModel = new SettingsViewModel();
                _aboutViewModel = new AboutViewModel();
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(string.Format("An exception was thrown trying to construct the ViewModels:  {0}", exception.Message), 
                    "Oh Dear!", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            CurrentViewModel = _timerViewModel;
            TimerViewCommand = new RelayCommand(() => ExecuteViewTimerCommand());
            SettingsViewCommand = new RelayCommand(() => ExecuteViewSettingsCommand());
            PlayCommand = new RelayCommand(() => ExecutePlayCommand());
            PauseCommand = new RelayCommand(() => ExecutePauseCommand());

            Messenger.Default.Register<SimpleMessage>(this, ConsumeMessage);
        }

        void ConsumeMessage(SimpleMessage message)
        {
            switch (message.Type)
            {
                case SimpleMessage.MessageType.SwitchToTimerView:
                    ExecuteViewTimerCommand();
                    break;
                case SimpleMessage.MessageType.SwitchToSettingsView:
                    ExecuteViewSettingsCommand();
                    break;
                case SimpleMessage.MessageType.SwitchToAboutView:
                    ExecuteViewAboutCommand();
                    break;
                case SimpleMessage.MessageType.SettingsChanged:
                    //  ignored
                    break;
            }  
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public ICommand TimerViewCommand { get; private set; }
        public ICommand SettingsViewCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        
        private void ExecuteViewTimerCommand()
        {
            CurrentViewModel = _timerViewModel;
        }

        private void ExecuteViewSettingsCommand()
        {
            CurrentViewModel = _settingsViewModel;
        }

        private void ExecuteViewAboutCommand()
        {
            CurrentViewModel = _aboutViewModel;
        }

        private void ExecutePlayCommand()
        {
            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.StartTimer });
        }

        private void ExecutePauseCommand()
        {
            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.StopTimer });
        }


    }
}