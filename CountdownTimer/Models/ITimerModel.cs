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

namespace Btl.Models
{
    public interface ITimerModel
    {
        /// <summary>
        /// Return the timer interval that we're using.
        /// </summary>
        TimeSpan Interval { get; }

        /// <summary>
        /// Return the time remaining.
        /// </summary>
        TimeSpan Remaining { get; }

        /// <summary>
        /// Has the time completed?
        /// </summary>
        bool Complete { get; }

        /// <summary>
        /// Set or get the duration.
        /// </summary>
        TimeSpan Duration { get; set; }


        /// <summary>
        /// The state of the model
        /// </summary>
        TimerState Status { get; set; }

        /// <summary>
        /// Start the countdown.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the countdown.
        /// </summary>
        void Stop();

        /// <summary>
        /// Stop the current countdown and reset.
        /// </summary>
        void Reset();

        /// <summary>
        /// Tick event
        /// </summary>
        event EventHandler<TimerModelEventArgs> Tick;

        /// <summary>
        /// Timer started event
        /// </summary>
        event EventHandler<TimerModelEventArgs> Started;

        /// <summary>
        /// Timer stopped event
        /// </summary>
        event EventHandler<TimerModelEventArgs> Stopped;

        /// <summary>
        /// Timer reset event
        /// </summary>
        event EventHandler<TimerModelEventArgs> TimerReset;

        /// <summary>
        /// Timer completed event
        /// </summary>
        event EventHandler<TimerModelEventArgs> Completed;
    }
}
