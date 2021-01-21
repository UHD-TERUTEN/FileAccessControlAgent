using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Samples;
using System.IO;
using System.Windows;

namespace FileAccessControlAgent
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DBManager.Init();
            DropTables();
            CreateTables();
            MenuSamples.InitSampleData();
        }

        private static void CreateTables()
        {
            _ = "create table if not exists RecentNotifications (Notification text)".Execute();
            _ = "create table if not exists WhitelistVersion (Version varchar(9))".Execute();
            _ = "create table if not exists LogList (DateTime text, ProgramName varchar(250), Preview varchar(50))".Execute();
        }

        private static void DropTables()
        {
            _ = "drop table if exists RecentNotifications".Execute();
            _ = "drop table if exists WhitelistVersion".Execute();
            _ = "drop table if exists LogList".Execute();
        }

    }
}
