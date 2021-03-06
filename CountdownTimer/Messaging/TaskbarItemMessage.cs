﻿// Copyright 2012 lapthorn.net.
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


using System.Windows.Shell;

namespace Btl.Messaging
{
    /// <summary>
    /// Trivial container for a message for the taskbaritem progress info.
    /// </summary>
    public class TaskbarItemMessage
    {
        public TaskbarItemMessage()
        {
            State = TaskbarItemProgressState.None;
            Value = -1.0;
        }
        public TaskbarItemProgressState State { get; set; }

        public double Value { get; set; }

        public bool HasValue { get { return ! (Value < 0.0); } }
    }
}
