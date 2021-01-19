using FileAccessControlAgent.Views;
using System.Collections.Generic;

namespace FileAccessControlAgent.Samples
{
    public static class MenuSamples
    {
        public static List<LogInfo> LogListSample
        {
            get
            {
                List<LogInfo> sampleLogList = new List<LogInfo>();
                sampleLogList.Add(new LogInfo("2021-01-18", "WINWORD.EXE", "파일 접근 거부"));
                sampleLogList.Add(new LogInfo("2021-01-19", "WINWORD.EXE", "파일 접근 거부"));
                sampleLogList.Add(new LogInfo("2021-01-20", "EXCEL.EXE", "파일 접근 거부"));
                return sampleLogList;
            }
        }

        public static List<string> RecentNotifications
        {
            get
            {
                List<string> sampleRecentNotifications = new List<string>();
                sampleRecentNotifications.Add("화이트리스트 업데이트 v1.0.1");
                sampleRecentNotifications.Add("화이트리스트 업데이트 v1.0.0");
                return sampleRecentNotifications;
            }
        }

        public static string WhitelistVersion
        {
            get { return "v1.0.1"; }
        }
    }
}
