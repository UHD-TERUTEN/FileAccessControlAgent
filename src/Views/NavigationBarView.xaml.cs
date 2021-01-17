using FileAccessControlAgent.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileAccessControlAgent.Views
{
    public partial class NavigationBarView : UserControl
    {
        public NavigationBarView()
        {
            InitializeComponent();
            DataContext = new NavigationBarViewModel();
        }

        public static readonly DependencyProperty actionProperty
            = DependencyProperty.Register("action", typeof(Action<object>), typeof(NavigationBarView), new UIPropertyMetadata());

        public Action<object> action
        {
            get { return (Action<object>)GetValue(actionProperty); }
            set { SetValue(actionProperty, value); }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            action.Invoke((sender as Button).Tag);
        }
    }
}
