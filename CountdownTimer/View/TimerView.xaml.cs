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
