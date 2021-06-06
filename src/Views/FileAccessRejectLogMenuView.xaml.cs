using System;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Managers;

namespace FileAccessControlAgent.Views
{
    public class LogInfo
    {
        public string ProgramName { get; private set; }
        public string FileName { get; private set; }
        public string Operation { get; private set; }
        public string PlainText { get; private set; }
        public bool IsAccept { get; private set; }

        public LogInfo(string programName, string fileName, string operation, string plainText, bool isAccept)
        {
            ProgramName = programName;
            FileName = fileName;
            Operation = operation;
            PlainText = plainText;
            IsAccept = isAccept;
        }
    }

    public partial class FileAccessRejectLogMenuView : UserControl
    {
        public FileAccessRejectLogMenuView(Action NavigateToInquiry)
        {
            InitializeComponent();
            UpdateData();
            logList.SelectionChanged += LogListSelectionChanged;
            navigateToInquiry = NavigateToInquiry;
        }

        private void LogListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var logInfo = logList.SelectedItem as LogInfo;

            var obj = JsonSerializer.Deserialize<LogData>(logInfo.PlainText);
            var pretty = JsonSerializer.Serialize(obj, jsonOptions);
            (logDetails.Content as TextBlock).Text = pretty;
        }

        private void Inquire(object sender, System.Windows.RoutedEventArgs e)
        {
            if (logList.SelectedItem == null)
                MessageBoxHelper.Warning("로그를 선택해주세요.");
            else
            {
                var logInfo = logList.SelectedItem as LogInfo;
                InquiryMenuView.LogParam1 = $"[{logInfo.ProgramName}] {logInfo.Operation} [{logInfo.FileName}]";
                InquiryMenuView.LogParam2 = logInfo.PlainText;
                navigateToInquiry.Invoke();
            }
        }

        private Action navigateToInquiry;

        private void UpdateData()
        {
            var logs = "select * from LogList".Read<LogInfo>();
            if (logs.Count > 0)
            {
                foreach (var logInfo in logs)
                    logList.Items.Add(logInfo);
            }
        }

        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateData();
        }

        private static readonly JsonSerializerOptions jsonOptions =
            new JsonSerializerOptions()
            {
                WriteIndented = true
            };
    }
}
