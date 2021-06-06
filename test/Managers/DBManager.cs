using System.IO;
using Xunit;
using System.Data.SQLite;
using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Views;

namespace test
{
    public class DBManagerTest
    {
#if DEBUG
        private static void InitializeDb()
        {
            SQLiteConnection.CreateFile(dbName);
            CreateTables();
        }

        private static void ClearDb()
        {
            DropTables();
            File.Delete(dbName);
        }

        [Fact]
        public void TestLogList()
        {
            InitializeDb();
            {
                var result = "select * from LogList".Read<LogInfo>(connString);

                Assert.Empty(result);

                InitSampleLogList();
                result = "select * from LogList".Read<LogInfo>(connString);

                Assert.NotEmpty(result);
                Assert.Equal("2021-01-18", result[0].DateTime);
                Assert.Equal("WINWORD.EXE", result[0].ProgramName);
                Assert.Equal("파일 접근 거부", result[0].Preview);
                Assert.Equal("2021-01-20", result[2].DateTime);
                Assert.Equal("EXCEL.EXE", result[2].ProgramName);
                Assert.Equal("파일 접근 거부", result[2].Preview);
            }
            ClearDb();
        }

        [Fact]
        public void TestRecentNotifications()
        {
            InitializeDb();
            {
                var result = "select * from RecentNotifications".Read<Notification>(connString);

                Assert.Empty(result);

                InitSampleRecentNotifications();
                result = "select * from RecentNotifications".Read<Notification>(connString);

                Assert.NotEmpty(result);
                Assert.Equal("화이트리스트 업데이트 v1.0.0", result[0].notification);
                Assert.Equal("화이트리스트 업데이트 v1.0.1", result[1].notification);
            }
            ClearDb();
        }

        [Fact]
        public void TestWhitelistVersion()
        {
            InitializeDb();
            {
                var result = "select * from WhitelistVersion".Read<WhitelistVersion>(connString);

                Assert.Empty(result);

                InitSampleWhitelistVersion();
                result = "select * from WhitelistVersion".Read<WhitelistVersion>(connString);

                Assert.NotEmpty(result);
                Assert.Equal("v1.0.12", result[0].Version);
            }
            ClearDb();
        }

        private static void CreateTables()
        {
            _ = "create table if not exists RecentNotifications (Notification text)".Execute(connString);
            _ = "create table if not exists WhitelistVersion (Version varchar(9))".Execute(connString);
            _ = "create table if not exists LogList (DateTime text, ProgramName varchar(250), Preview varchar(50))".Execute(connString);
        }

        private static void DropTables()
        {
            _ = "drop table if exists RecentNotifications".Execute(connString);
            _ = "drop table if exists WhitelistVersion".Execute(connString);
            _ = "drop table if exists LogList".Execute(connString);
        }

        private static void InitSampleRecentNotifications()
        {
            _ = "insert into RecentNotifications VALUES ('화이트리스트 업데이트 v1.0.0')".Execute(connString);
            _ = "insert into RecentNotifications VALUES ('화이트리스트 업데이트 v1.0.1')".Execute(connString);
        }

        private static void InitSampleWhitelistVersion()
        {
            _ = "insert into WhitelistVersion VALUES ('v1.0.12')".Execute(connString);
        }

        private static void InitSampleLogList()
        {
            _ = "insert into LogList VALUES ('2021-01-18', 'WINWORD.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-19', 'WINWORD.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
            _ = "insert into LogList VALUES ('2021-01-20', 'EXCEL.EXE', '파일 접근 거부')".Execute(connString);
        }


        private static readonly string dbName = "dbUnitTest.sqlite";

        private static readonly string connString = $"Data Source={dbName};Version=3";
#endif
    }
}
