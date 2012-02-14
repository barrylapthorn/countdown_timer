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
    internal interface ISettingsModel
    {
        /// <summary>
        /// Get/set whether to highlight the clock using built-in colours.
        /// </summary>
        bool Colours { get; set; }
        /// <summary>
        /// Returns whether this is the first ever run of the application.
        /// </summary>
        bool FirstRun { get; set; }
        /// <summary>
        /// The font-family of the clock
        /// </summary>
        System.Windows.Media.FontFamily FontFamily { get; set; }
        /// <summary>
        /// Returns whether the settings have been modified in some way.
        /// </summary>
        bool Modified { get; }
        /// <summary>
        /// The (last) selected duration of our timer.
        /// </summary>
        TimeSpan Duration { get; set; }
        /// <summary>
        /// Play the system beep on start
        /// </summary>
        bool PlayBeep { get; set; }
        /// <summary>
        /// Play the system exclamation on stop
        /// </summary>
        bool PlayExclamation { get; set; }
        /// <summary>
        /// is the window top-most?
        /// </summary>
        bool TopMost { get; set; }
        /// <summary>
        /// the font size of the clock
        /// </summary>
        double FontSize { get; set; }
        /// <summary>
        /// Reload the settings from the application properties if necessary.
        /// </summary>
        void Reload();
        /// <summary>
        /// Save the settings if they have been modified, otherwise do nothing.
        /// </summary>
        void Save();
    }
}
