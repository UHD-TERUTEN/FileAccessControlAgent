using System;
using System.Windows;
using System.Windows.Controls;

namespace FileAccessControlAgent.Views
{
    public partial class InquiryMenuView : UserControl
    {
        public InquiryMenuView()
        {
            InitializeComponent();
        }

        private void SendInquiry(object sender, RoutedEventArgs e)
        {
            if (titleTextBox.Text.Length == 0
                || logTextBox.Text.Length == 0
                || contentTextBox.Text.Length == 0)
            {
                MessageBox.Show("일부 항목이 비어있습니다.\n모든 항목을 채운 뒤 보내기 버튼을 눌러주세요.");
                return;
            }

            string confirmMessage =
                $"제목 :\t{titleTextBox.Text}" + Environment.NewLine +
                $"로그 :\t{logTextBox.Text}" + Environment.NewLine +
                $"내용 :\t{contentTextBox.Text}";

            if (MessageBox.Show(confirmMessage,
                                "문의사항 보내기",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                MessageBox.Show("문의사항을 보냈습니다.");
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            logTextBox.Text = LogParam ?? "";
        }

        private void titleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            title = (sender as TextBox).Text;
        }

        private void logTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogParam = (sender as TextBox).Text;
        }

        private void contentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            content = (sender as TextBox).Text;
        }

        public static string LogParam { get; set; }

        private string title;

        private string content;
    }
}
