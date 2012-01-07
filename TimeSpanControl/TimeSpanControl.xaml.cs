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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Btl.Control
{
    /// <summary>
    /// A simple UserControl that wraps a TimeSpan object
    /// </summary>
    public partial class TimeSpanControl : UserControl
    {
        #region Constructors
        public TimeSpanControl()
        {
            InitializeComponent();
            Value = TimeSpan.FromMinutes(25.0);
        }
        #endregion

        public TimeSpan Value
        {
            get { return (TimeSpan) GetValue(timeSpanValue); }
            set { SetValue(timeSpanValue, value); }
        }

        public static readonly DependencyProperty timeSpanValue =
        DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimeSpanControl),
        new UIPropertyMetadata(DateTime.Now.TimeOfDay, OnValueChanged));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanControl control = obj as TimeSpanControl;
            control.Hours = ((TimeSpan)e.NewValue).Hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.Seconds = ((TimeSpan)e.NewValue).Seconds;
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(int), typeof(TimeSpanControl),
        new UIPropertyMetadata(0, OnTimeChanged));

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(int), typeof(TimeSpanControl),
        new UIPropertyMetadata(0, OnTimeChanged));

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public static readonly DependencyProperty SecondsProperty =
        DependencyProperty.Register("Seconds", typeof(int), typeof(TimeSpanControl),
        new UIPropertyMetadata(0, OnTimeChanged));


        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanControl control = obj as TimeSpanControl;
            control.Value = new TimeSpan(control.Hours, control.Minutes, control.Seconds);
        }

        private void Down(object sender, KeyEventArgs args)
        {
            switch (((Grid)sender).Name)
            {
                case "sec":
                    if (args.Key == Key.Up)
                        Seconds++;
                    if (args.Key == Key.Down)
                        Seconds--;
                    break;

                case "min":
                    if (args.Key == Key.Up)
                        Minutes++;
                    if (args.Key == Key.Down)
                        Minutes--;
                    break;

                case "hour":
                    if (args.Key == Key.Up)
                        Hours++;
                    if (args.Key == Key.Down)
                        Hours--;
                    break;
            }
        }
 
    }
}
