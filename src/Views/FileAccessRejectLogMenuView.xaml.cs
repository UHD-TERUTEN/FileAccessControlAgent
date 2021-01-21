using System;
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
        public FileAccessRejectLogMenuView()
        {
            InitializeComponent();

            var logListView = logList.Content as ListView;

            foreach (var logInfo in "select * from LogList".Read<LogInfo>())
                logListView.Items.Add(logInfo);

            logListView.SelectionChanged += LogListSelectionChanged;
        }

        private void LogListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var logInfo = (logList.Content as ListView).SelectedItem as LogInfo;

            (logDetails.Content as TextBlock).Text =
                $"DateTime :\t{logInfo.DateTime}" + Environment.NewLine +
                $"ProgramName :\t{logInfo.ProgramName}" + Environment.NewLine +
                $"Preview :\t{logInfo.Preview}";
        }
    }
}
