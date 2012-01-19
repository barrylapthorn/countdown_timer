#  A WPF/MVVM Countdown Timer

![](btl_countdowntimer/CountdownTimer.png)

#  Introduction

In this article, we describe the construction of a countdown timer application written in WPF, and 

##  Choosing the underlying timer

We use the WPF [DispatchTimer](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.aspx) to perform a count. We are not making any guarantees about the accuracy of the timer.

In fact neither does the documentation:

> Timers are not guaranteed to execute exactly when the time interval occurs, but they are guaranteed to not execute before the time interval occurs. This is because DispatcherTimer operations are placed on the Dispatcher queue like other operations. When the DispatcherTimer operation executes is dependent on the other jobs in the queue and their priorities.
Performing the countdown

Again, leveraging the underlying framework, we use a `TimeSpan` that allows us to increment, and importantly, decrement by a specified amount. We then simply decrement our starting value every time the `DispatchTimer` ticks, until we get a negative `TimeSpan`, and then we stop.

##  MVVM style

### Persisting settings

To persist the application's settings, we take advantage of the class System.Configuration.ApplicationSettingsBase. This is subclassed for the WPF application when you create it, so you can then just address the application settings as, for example:

                this.Top = Properties.Settings.Default.Top;
                this.Left = Properties.Settings.Default.Left;
                this.Height = Properties.Settings.Default.Height;
                this.Width = Properties.Settings.Default.Width;
To cater for updates to the application we can use the following method: define UpgradeRequired in our settings, and set to True by default. Then use:

            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }
To force the upgrade of the application settings only when the UpgradeRequired flag is true. For newly versioned assemblies, all settings take their default values, this code is triggered, and the settings are copied from a previous application version to the new one.