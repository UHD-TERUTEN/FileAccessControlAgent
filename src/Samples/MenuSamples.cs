using FileAccessControlAgent.Helpers;

namespace FileAccessControlAgent.Samples
{
    public static class MenuSamples
    {
        private static void InitSampleRecentNotifications()
        {
            _ = "insert into RecentNotifications VALUES ('화이트리스트 업데이트 v1.0.0')".Execute();
            _ = "insert into RecentNotifications VALUES ('화이트리스트 업데이트 v1.0.1')".Execute();
        }

        private static void InitSampleWhitelistVersion()
        {
            _ = "insert into WhitelistVersion VALUES ('v1.0.12')".Execute();
        }

        private static void InitSampleLogList()
        {
            _ = "insert into LogList VALUES ('2021-01-18', 'WINWORD.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-19', 'WINWORD.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute();
        }

        public static void InitSampleData()
        {
            InitSampleRecentNotifications();
            InitSampleWhitelistVersion();
            InitSampleLogList();
        }
    }
}
