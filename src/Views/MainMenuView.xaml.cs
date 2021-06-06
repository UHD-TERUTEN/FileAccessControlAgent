using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Managers;
using System;
using System.Windows;
using System.Windows.Controls;
using Environment = System.Environment;

namespace FileAccessControlAgent.Views
{
    public class Notification
    {
        public string notification { get; private set; }
    }

    public class WhitelistVersion : IHttpResponse
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public DateTime? LastDistributed { get; set; }
    }

    public partial class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            InitializeComponent();
            UpdateData();
        }

        private async void Update(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            try
            {
                var latest = await "api/whitelist/latest".Get<WhitelistVersion>();
                if (latest.LastDistributed is null)
                    latest.LastDistributed = DateTime.Today;
                _ = $"insert into WhitelistVersion values({latest.Id},'{latest.Version}','{latest.LastDistributed?.ToShortDateString()}')".Execute();
                _ = $"insert into RecentNotifications values('화이트리스트 업데이트 {latest.Version}')".Execute();
                UpdateData();
                MessageBox.Show($"화이트리스트를 업데이트했습니다.\n[Version] {latest.Version}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"화이트리스트 업데이트에 실패했습니다.\n{exception.Message}");
            }
            progressRing.IsActive = false;
        }

        private void UpdateData()
        {
            // Load the recentNotifications
            var notificationTextBlock = recentNotifications.Content as TextBlock;
            notificationTextBlock.Text = "";

            var notifications = "select * from RecentNotifications".Read<Notification>();
            if (notifications.Count > 0)
            {
                foreach (var notification in notifications)
                    notificationTextBlock.Text += notification.notification + Environment.NewLine;
            }

            // Load the whitelistVersion
            var versions = "select * from WhitelistVersion".Read<WhitelistVersion>();
            if (versions.Count > 0)
                whitelistVersion.Text = versions[0].Version;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }
    }
}
