using FileAccessControlAgent.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileAccessControlAgent.Views
{
    public class Notification
    {
        public string notification { get; private set; }
    }

    public class WhitelistVersion
    {
        public string version { get; private set; }
    }

    public partial class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            InitializeComponent();

            // Load the recentNotifications
            var notificationTextBlock = recentNotifications.Content as TextBlock;

            foreach (var notification in "select * from RecentNotifications".Read<Notification>())
            {
                notificationTextBlock.Text += notification.notification + Environment.NewLine;
            }

            // Load the whitelistVersion
            whitelistVersion.Text = "select * from WhitelistVersion".Read<WhitelistVersion>()[0].version;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("화이트리스트를 업데이트했습니다.");
        }
    }
}
