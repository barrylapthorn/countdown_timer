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

using Btl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;

namespace Btl.View
{
    /// <summary>
    /// Interaction logic for TimerView.xaml
    /// </summary>
    public partial class TimerView : UserControl
    {
        public TimerView()
        {
            InitializeComponent();
        }
        
        private void SetTaskbarItemInfo()
        {
            Window parent = Window.GetWindow(this);
            if (parent != null && parent.TaskbarItemInfo != null)
            {
                viewModel.TaskbarItemInfo = parent.TaskbarItemInfo;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetTaskbarItemInfo();
        }
    }
}
