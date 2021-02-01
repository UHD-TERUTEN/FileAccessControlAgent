using FileAccessControlAgent.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileAccessControlAgent.Views
{
    public partial class NavigationBarView : UserControl
    {
        public NavigationBarView()
        {
            InitializeComponent();
            ColorButtons(mainMenuNavigateButton);
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
            ColorButtons(sender as Button);
        }

        private void ColorButtons(Button clicked)
        {
            foreach (Button item in stackPanel.Children)
                item.Background = Brushes.LightSkyBlue;
            clicked.Background = Brushes.White;
        }
    }
}
