using FileAccessControlAgent.Samples;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileAccessControlAgent.Views
{
    public partial class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            InitializeComponent();

            // (recentNotifications.Content as TextBlock).Text update
            var notificationTextBlock = recentNotifications.Content as TextBlock;
            foreach (string notification in MenuSamples.RecentNotifications)
            {
                notificationTextBlock.Text += notification + Environment.NewLine;
            }

            // whitelistVersion update
            whitelistVersion.Text = MenuSamples.WhitelistVersion;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("화이트리스트를 업데이트했습니다.");
        }
    }
}
