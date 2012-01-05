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


using Btl.Model;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;


namespace Btl.ViewModel
{
    class TimerViewModel : ObservableObject
    {
        #region Members
        private Color statusColor;
        readonly TimerModel timer = new TimerModel();
        #endregion

        #region Constructors
        
        /// <summary>
        /// Construct a new timer view model.
        /// </summary>
        public TimerViewModel()
        {
            //  add event handlers
            timer.Tick += (sender, e) => OnTick(sender, e);
            timer.Completed += (sender, e) => OnCompleted(sender, e);
            timer.Started += (sender, e) => OnStarted(sender, e);
            timer.Stopped += (sender, e) => OnStopped(sender, e);
            timer.TimerReset += (sender, e) => OnReset(sender, e);
        }
        #endregion

        #region Commands

        #region Start Command
        void StartTimerExecute()
        {
            timer.Start();
        }

        bool CanStartTimerExecute()
        {
            return !timer.Complete;
        }

        public ICommand StartTimer { get { return new RelayCommand(StartTimerExecute, CanStartTimerExecute); } }

        #endregion

        #region Stop Command

        void StopTimerExecute()
        {
            timer.Stop();
        }

        bool CanStopTimerExecute()
        {
            return !timer.Complete;
        }

        public ICommand StopTimer { get { return new RelayCommand(StopTimerExecute, CanStopTimerExecute); } }
        #endregion

        #region Reset Command

        void ResetTimerExecute()
        {
            timer.Reset();
            UpdateTimerValues(timer.Remaining);
        }

        bool CanResetTimerExecute()
        {
            return true;
        }

        public ICommand ResetTimer { get { return new RelayCommand(ResetTimerExecute, CanResetTimerExecute); } }
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
                return statusColor;
            }
            set
            {
                if (value != statusColor)
                {
                    statusColor = value;
                    RaisePropertyChanged("StatusColor");
                    StatusBrush = new SolidColorBrush(statusColor);
                }
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
        #endregion

        #region Methods
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
                    StatusColor = Colors.Orange;
                    break;
                case TimerModelEventArgs.Status.Running:
                    StatusColor = Colors.Orange;
                    break;
                case TimerModelEventArgs.Status.Completed:
                    StatusColor = Colors.ForestGreen;
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

            PercentElapsed = 100.0 - (100.0 * timer.Remaining.TotalSeconds / timer.Duration.TotalSeconds);
        }
        #endregion

        #region Event handlers
        private void OnCompleted(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);

            SystemSounds.Exclamation.Play();


            if (taskbarItemInfo != null)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                taskbarItemInfo.ProgressValue = 1.0;
            }
        }

        private void OnTick(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);

            if (taskbarItemInfo != null)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                taskbarItemInfo.ProgressValue = PercentElapsed / 100.0;
            }
        }

        void OnStarted(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);

            SystemSounds.Beep.Play();

            if (taskbarItemInfo != null)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            }
        }

        private void OnStopped(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);

            if (taskbarItemInfo != null)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
            }
        }

        private void OnReset(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);

            if (taskbarItemInfo != null)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                taskbarItemInfo.ProgressValue = 0.0;
            }
        }
        #endregion

        //  to do:  
        //  settings to change timer value
        //  

    }
}
