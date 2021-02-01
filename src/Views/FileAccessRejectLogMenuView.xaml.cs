using System;
using System.Windows;
using System.Windows.Controls;
using FileAccessControlAgent.Helpers;

namespace FileAccessControlAgent.Views
{
    public class LogInfo
    {
        public string DateTime { get; private set; }
        public string ProgramName { get; private set; }
        public string Preview { get; private set; }

        public LogInfo(string dateTime, string programName, string preview)
        {
            DateTime = dateTime;
            ProgramName = programName;
            Preview = preview;
        }
    }

    public partial class FileAccessRejectLogMenuView : UserControl
    {
        public FileAccessRejectLogMenuView(Action NavigateToInquiry)
        {
            InitializeComponent();

            foreach (var logInfo in "select * from LogList".Read<LogInfo>())
                logList.Items.Add(logInfo);

            logList.SelectionChanged += LogListSelectionChanged;

            navigateToInquiry = NavigateToInquiry;
        }

        private void LogListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var logInfo = logList.SelectedItem as LogInfo;

            (logDetails.Content as TextBlock).Text =
                $"DateTime :\t{logInfo.DateTime}" + Environment.NewLine +
                $"ProgramName :\t{logInfo.ProgramName}" + Environment.NewLine +
                $"Preview :\t{logInfo.Preview}";
        }

        private void Inquire(object sender, System.Windows.RoutedEventArgs e)
        {
            if (logList.SelectedItem == null)
                MessageBoxHelper.Warning("로그를 선택해주세요.");
            else
            {
                var logInfo = logList.SelectedItem as LogInfo;
                InquiryMenuView.LogParam = $"{logInfo.DateTime} [{logInfo.ProgramName}] {logInfo.Preview}";
                navigateToInquiry.Invoke();
            }
        }

        private Action navigateToInquiry;
    }
}
