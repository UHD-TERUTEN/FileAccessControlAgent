using System.Windows;

namespace FileAccessControlAgent.Helpers
{
    static class MessageBoxHelper
    {
        public static MessageBoxResult Confirm(string message)
        {
            return MessageBox.Show(message, "확인", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }

        public static MessageBoxResult Warning(string message)
        {
            return MessageBox.Show(message, "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
