using System;
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

        public static MessageBoxResult Error(Exception e)
        {
            if (e.GetType() == typeof(NotImplementedException))
                return MessageBox.Show("미구현 항목입니다.");
            else
                return MessageBox.Show(e.Message);
        }
    }
}
