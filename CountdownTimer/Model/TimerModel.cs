using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace Btl.Model
{
    internal class TimerModel
    {
        #region Members
        /// <summary>
        /// The underlying timer
        /// </summary>
        readonly DispatcherTimer timer = new DispatcherTimer();
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a timer with a default duration of 25 minutes.  This is a 
        /// 'pomodoro' time span.
        /// </summary>
        public TimerModel() : this(new TimeSpan(0, 25, 0))
        {
        }

        /// <summary>
        /// Create a timer of the specified duration.
        /// </summary>
        /// <param name="duration"></param>
        public TimerModel(TimeSpan duration)
        {
            //  use a coarse grained interval, as this timer isn't meant
            //  to be completely accurate.
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += (sender, e) => OnDispatcherTimerTick();

            Duration = duration;
            Reset();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Return the timer interval that we're using.
        /// </summary>
        public TimeSpan Interval { get { return timer.Interval; } }

        /// <summary>
        /// Return the time remaining.
        /// </summary>
        public TimeSpan Remaining { get; private set; }

        /// <summary>
        /// Has the time completed?
        /// </summary>
        public bool Complete
        {
            get
            {
                return Remaining <= TimeSpan.Zero;
            }
        }

        private TimeSpan duration;

        /// <summary>
        /// Set or get the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (duration != value)
                {
                    duration = value;
                    Reset();
                }
            }
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Start the countdown.
        /// </summary>
        public void Start()
        {
            timer.Start();
            OnStarted();
        }

        /// <summary>
        /// Stop the countdown.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            OnStopped();
        }

        /// <summary>
        /// Stop the current countdown and reset.
        /// </summary>
        public void Reset()
        {
            Stop();
            Remaining = Duration;
            OnReset();
        }

        #region Event Handlers
        /// <summary>
        /// Handle the ticking of the system timer.
        /// </summary>
        private void OnDispatcherTimerTick()
        {
            Remaining = Remaining - Interval;
            OnTick();
            if (Complete)
            {
                Stop();
                Remaining = TimeSpan.Zero;
                OnCompleted();
            }
        }
        #endregion
        #endregion

        #region Events

        public event EventHandler<TimerModelEventArgs> Tick;
        public event EventHandler<TimerModelEventArgs> Started;
        public event EventHandler<TimerModelEventArgs> Stopped;
        public event EventHandler<TimerModelEventArgs> TimerReset;
        public event EventHandler<TimerModelEventArgs> Completed;


        #region OnReset
        /// <summary>
        /// Triggers the TimerReset event
        /// </summary>
        private void OnReset()
        {
            if (TimerReset != null)
            {
                TimerReset(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Reset));
            }
        }
        #endregion

        #region OnCompleted
        /// <summary>
        /// Triggers the Completed event.
        /// </summary>
        private void OnCompleted()
        {
            if (Completed != null)
            {
                Completed(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Completed));
            }
        }
        #endregion

        #region OnStopped
        /// <summary>
        /// Triggers the Stopped event.
        /// </summary>
        private void OnStopped()
        {
            if (Stopped != null)
            {
                Stopped(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Stopped));
            }
        }
        #endregion

        #region OnStarted
        /// <summary>
        /// Triggers the Started event.
        /// </summary>
        private void OnStarted()
        {
            if (Started != null)
            {
                Started(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Started));
            }
        }
        #endregion

        #region OnTick
        /// <summary>
        /// Triggers the Tick event.
        /// </summary>
        private void OnTick()
        {
            if (Tick != null)
            {
                Tick(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Running));
            }
        }
        #endregion

        #endregion
    }
}
