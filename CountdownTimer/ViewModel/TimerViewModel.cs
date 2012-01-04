using Btl.Model;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows.Input;
using System.Windows.Media;


namespace Btl.ViewModel
{
    class TimerViewModel : ObservableObject
    {
        #region Members
        private Color statusColor;
        readonly TimerModel timer = new TimerModel(new TimeSpan(0, 0, 10));
        #endregion

        #region Constructors
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
        }

        private void OnTick(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);
        }

        void OnStarted(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);

            SystemSounds.Beep.Play();
        }

        private void OnStopped(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);
        }

        private void OnReset(object sender, TimerModelEventArgs e)
        {
            UpdateTimer(timer.Remaining, e);
        }
        #endregion

        //  to do:  
        //  settings to change timer value
        //  

    }
}
