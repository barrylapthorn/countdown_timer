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
using System.Media;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using Btl.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;


namespace Btl.ViewModels
{
    public class TimerViewModel : ViewModelBase
    {
        #region Members
        private int _completedCount = 0;
        private Color _statusColor;
        readonly TimerModel _timer = new TimerModel();
        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new timer view model.
        /// </summary>
        public TimerViewModel()
        {
            //  add event handlers
            AddEventHandlers();

            //  update all the settings, such as the timer duration and so on.
            UpdateMembersFromSettings();

            //  bind the commands to their respective actions
            BindCommands();

            //  Register against the Messenger singleton to receive any simple
            //  messages.  Specifically the one that says settings have changed.
            Messenger.Default.Register<SimpleMessage>(this, ConsumeMessage);

        }
        #endregion

        #region Commands

        public ICommand Settings { get; private set; }
        public ICommand StartTimer { get; private set; }
        public ICommand StopTimer { get; private set; }
        public ICommand ResetTimer { get; private set; }

        #region Settings Command

        private void SettingsExecute()
        {
            Messenger.Default.Send(new SimpleMessage { Type = SimpleMessage.MessageType.SwitchToSettingsView });
        }

        #endregion

        #region Start Command

        void StartTimerExecute()
        {
            _timer.Start();
        }

        bool CanStartTimerExecute()
        {
            return !_timer.Complete && _timer.Status != TimerModel.State.Running;
        }

        #endregion

        #region Stop Command

        void StopTimerExecute()
        {
            _timer.Stop();
        }

        bool CanStopTimerExecute()
        {
            return !_timer.Complete && _timer.Status == TimerModel.State.Running;
        }

        #endregion

        #region Reset Command

        void ResetTimerExecute()
        {
            _timer.Reset();
            UpdateTimerValues(_timer.Remaining);
        }

        bool CanResetTimerExecute()
        {
            return true;
        }

        private void BindCommands()
        {
            Settings = new RelayCommand(() => SettingsExecute());
            StartTimer = new RelayCommand(() => StartTimerExecute(), CanStartTimerExecute);
            StopTimer = new RelayCommand(() => StopTimerExecute(), CanStopTimerExecute);
            ResetTimer = new RelayCommand(() => ResetTimerExecute(), CanResetTimerExecute);
        }
        #endregion

        #endregion

        #region Properties
        string timerValue;

        /// <summary>
        /// The value of the timer as a string.
        /// </summary>
        public string TimerValue
        {
            get
            {
                return timerValue;
            }

            set
            {
                if (timerValue != value)
                {
                    timerValue = value;
                    RaisePropertyChanged("TimerValue");
                }
            }
        }

        private double percentElapsed;
        /// <summary>
        /// The percentage elapsed.
        /// </summary>
        public double PercentElapsed
        {
            get
            {
                return percentElapsed;
            }
            set
            {
                if (value != percentElapsed)
                {
                    percentElapsed = value;
                    RaisePropertyChanged("PercentElapsed");
                }
            }
        }

        public Color StatusColor
        {
            get
            {
                return _statusColor;
            }
            set
            {
                if (value != _statusColor)
                {
                    _statusColor = value;
                    RaisePropertyChanged("StatusColor");
                    StatusBrush = new SolidColorBrush(_statusColor);
                }
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return _timer.Duration;
            }

            set
            {
                if (_timer.Duration == value)
                    return;
                _timer.Duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        public int CompletedCount
        {
            get
            {
                return _completedCount;
            }
            private set
            {
                if (_completedCount == value)
                    return;
                _completedCount = value;
                RaisePropertyChanged("CompletedCount");
            }
        }

        Brush statusBrush = new SolidColorBrush();
        public Brush StatusBrush
        {
            get
            {
                return statusBrush;
            }

            private set
            {
                if (value != statusBrush)
                {
                    statusBrush = value;
                    RaisePropertyChanged("StatusBrush");
                }
            }
        }

        private TaskbarItemInfo taskbarItemInfo = null;

        /// <summary>
        /// Get or set a reference to the TaskbarItemInfo of the application
        /// so that the view model can update the progress bar in the taskbar
        /// application icon.
        /// </summary>
        public TaskbarItemInfo TaskbarItemInfo
        {
            get
            {
                return taskbarItemInfo;
            }
            set
            {
                if (taskbarItemInfo == value)
                    return;
                taskbarItemInfo = value;
            }
        }

        public string ClockFontFamily
        {
            get
            {
                return Properties.Settings.Default.ClockFontFamily;
            }
            set
            {
                if (Properties.Settings.Default.ClockFontFamily == value)
                    return;
                Properties.Settings.Default.ClockFontFamily = value;
                RaisePropertyChanged("ClockFontFamily");
            }
        }

        public double ClockFontSize
        {
            get
            {
                return Properties.Settings.Default.ClockFontSize;
            }
            set
            {
                if (Properties.Settings.Default.ClockFontSize == value)
                    return;
                Properties.Settings.Default.ClockFontSize = value;
                RaisePropertyChanged("ClockFontSize");
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Update the TimerViewModel values from the (user defined) settings.
        /// </summary>
        private void UpdateMembersFromSettings()
        {
            SettingsModel settings = new SettingsModel();

            Duration = settings.Duration;

            UpdateTimerValues(settings.Duration);
        }

        /// <summary>
        /// Consume any messages that are passed between models
        /// </summary>
        /// <param name="message"></param>
        void ConsumeMessage(SimpleMessage message)
        {
            switch (message.Type)
            {
                case SimpleMessage.MessageType.SwitchToTimerView:
                    //  ignored
                    break;
                case SimpleMessage.MessageType.SwitchToSettingsView:
                    //  ignored
                    break;
                case SimpleMessage.MessageType.SettingsChanged:
                    StopTimerExecute();
                    UpdateMembersFromSettings();
                    break;
            }
        }
        /// <summary>
        /// Update the timer view model properties based on the time span passed in.
        /// </summary>
        /// <param name="t"></param>
        private void UpdateTimer(TimeSpan t, TimerModelEventArgs e)
        {
            UpdateTimerValues(t);
            UpdateTimerStatusColor(e);
        }

        /// <summary>
        /// Set the solid colour based on the timer status.
        /// </summary>
        /// <param name="e"></param>
        private void UpdateTimerStatusColor(TimerModelEventArgs e)
        {
            switch (e.State)
            {
                case TimerModelEventArgs.Status.NotSpecified:
                    StatusColor = Colors.Beige;
                    break;
                case TimerModelEventArgs.Status.Stopped:
                    StatusColor = Colors.Beige;
                    break;
                case TimerModelEventArgs.Status.Started:
                    StatusColor = Colors.LightSalmon;
                    break;
                case TimerModelEventArgs.Status.Running:
                    StatusColor = Colors.LightSalmon;
                    break;
                case TimerModelEventArgs.Status.Completed:
                    StatusColor = Colors.LightGreen;
                    break;
                case TimerModelEventArgs.Status.Reset:
                    StatusColor = Colors.Beige;
                    break;
            }
        }

        /// <summary>
        /// Update the timer view model properties based on the time span passed in.
        /// </summary>
        /// <param name="t"></param>
        private void UpdateTimerValues(TimeSpan t)
        {
            TimerValue = string.Format("{0}:{1}:{2}", t.Hours.ToString("D2"),
                t.Minutes.ToString("D2"), t.Seconds.ToString("D2"));

            PercentElapsed = 100.0 - (100.0 * _timer.Remaining.TotalSeconds / _timer.Duration.TotalSeconds);
        }

        /// <summary>
        /// Add the event handlers.
        /// </summary>
        private void AddEventHandlers()
        {
            _timer.Tick += (sender, e) => OnTick(sender, e);
            _timer.Completed += (sender, e) => OnCompleted(sender, e);
            _timer.Started += (sender, e) => OnStarted(sender, e);
            _timer.Stopped += (sender, e) => OnStopped(sender, e);
            _timer.TimerReset += (sender, e) => OnReset(sender, e);
        }
        #endregion

        #region Event handlers
        private void OnCompleted(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(_timer.Remaining, e);

            CompletedCount++;

            SystemSounds.Exclamation.Play();

            Messenger.Default.Send(new TaskbarItemMessage { State = TaskbarItemProgressState.Normal, Value = 1.0 });
        }

        private void OnTick(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(_timer.Remaining, e);

            Messenger.Default.Send(new TaskbarItemMessage { State = TaskbarItemProgressState.Normal, Value = PercentElapsed / 100.0 });
        }

        void OnStarted(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(_timer.Remaining, e);

            SystemSounds.Beep.Play();

            Messenger.Default.Send(new TaskbarItemMessage { State = TaskbarItemProgressState.Normal });
        }

        private void OnStopped(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(_timer.Remaining, e);

            Messenger.Default.Send(new TaskbarItemMessage { State = TaskbarItemProgressState.Paused });
        }

        private void OnReset(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(_timer.Remaining, e);

            Messenger.Default.Send(new TaskbarItemMessage { State = TaskbarItemProgressState.None, Value = 0.0 });
        }
        #endregion

        //  to do:  
        //  settings to change timer value
        //  

    }
}
