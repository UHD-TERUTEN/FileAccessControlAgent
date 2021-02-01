using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Managers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileAccessControlAgent.Views
{
    public class Notification
    {
        public string notification { get; private set; }
    }

    public class WhitelistVersion : ITCPResponse
    {
        private string result;

        public string Result { get { return result; } set { result = value; } }
        public string Version { get; set; }
    }

    public class GetWhitelistVersion : ITCPRequest
    {
        public string Method { get { return "Get"; } }
        public string Target { get { return "WhitelistVersion"; } }
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
            whitelistVersion.Text = "select * from WhitelistVersion".Read<WhitelistVersion>()[0].Version;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            var res = (new GetWhitelistVersion()).SendRequest<WhitelistVersion>();
            MessageBox.Show($"[{res.Result}] 화이트리스트를 업데이트했습니다.\n{res.Version}");
            progressRing.IsActive = false;
        }
    }
}
