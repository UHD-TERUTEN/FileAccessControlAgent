using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Managers;
using System;
using System.Windows;
using System.Windows.Controls;
using Environment = System.Environment;

namespace FileAccessControlAgent.Views
{
    public class Inquiry : IHttpRequest
    {
        public string ProgramName { get; set; }
        public string FileName { get; set; }
        public string Operation { get; set; }
        public string PlainText { get; set; }
        public bool IsAccept { get; set; }
    }

    public partial class InquiryMenuView : UserControl
    {
        public InquiryMenuView()
        {
            InitializeComponent();
        }

        private async void SendInquiry(object sender, RoutedEventArgs e)
        {
            if (titleTextBox.Text.Length == 0
                || logTextBox.Text.Length == 0
                || contentTextBox.Text.Length == 0)
            {
                MessageBoxHelper.Warning("일부 항목이 비어있습니다.\n모든 항목을 채운 뒤 보내기 버튼을 눌러주세요.");
                return;
            }

            string confirmMessage =
                $"제목 :\t{titleTextBox.Text}" + Environment.NewLine +
                $"로그 :\t{logTextBox.Text}" + Environment.NewLine +
                $"내용 :\t{contentTextBox.Text}";

            var a = logTextBox.Text.IndexOf(']');
            var b = logTextBox.Text.LastIndexOf('[');

            var inquiry = new Inquiry()
            {
                ProgramName = logTextBox.Text.Substring(1, a - 1),
                FileName = logTextBox.Text.Substring(a + 2, b - a - 3),
                Operation = logTextBox.Text.Substring(b + 1, logTextBox.Text.Length - b - 3),
                PlainText = contentTextBox.Text,
                IsAccept = false,
            };

            try
            {
                await "api/file-access-reject-log".Post(inquiry);
                if (MessageBoxHelper.Confirm(confirmMessage) == MessageBoxResult.Yes)
                    MessageBox.Show("문의사항을 보냈습니다.");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"문의사항 전송에 실패했습니다.\n{exception.Message}");
            }
        }

        private void OnDataContextChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            logTextBox.Text = LogParam1 ?? "";
            contentTextBox.Text = LogParam2 ?? "";
        }

        private void titleTextBoxOnTextChange(object sender, TextChangedEventArgs e)
        {
            title = (sender as TextBox).Text;
        }

        private void logTextBoxOnTextChange(object sender, TextChangedEventArgs e)
        {
            LogParam1 = (sender as TextBox).Text;
        }

        private void contentTextBoxOnTextChange(object sender, TextChangedEventArgs e)
        {
            content = (sender as TextBox).Text;
        }

        public static string LogParam1 { get; set; }

        public static string LogParam2 { get; set; }

        private string title;

        private string content;
    }
}
