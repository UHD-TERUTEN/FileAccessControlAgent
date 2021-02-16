using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Managers;
using FileAccessControlAgent.Samples;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FileAccessControlAgent
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Add exception handler
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            Current.DispatcherUnhandledException += DispatcherUnhandledException;
            Dispatcher.UnhandledException += DispatcherUnhandledException;

            // Initialize the database
            DBManager.Init();
            DropTables();
            CreateTables();
            MenuSamples.InitSampleData();

            // Run threads
            string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\";
            using (var streamReader = new StreamReader(parentDirectory + argumentsPath))
            {
                string[] arguments = streamReader.ReadLine()?.Split(' ');
                if (arguments != null)
                {
                    TCPRequestManager.Server = arguments[0];
                    TCPRequestManager.Port = int.Parse(arguments[1]);
                }
            }
            ThreadPool.QueueUserWorkItem(FileAccessRejectLogManager.ReceiveFileAccessLog);
        }

        private new void DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBoxHelper.Error(e.Exception);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBoxHelper.Error(e.ExceptionObject as Exception);
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

        private static readonly string argumentsPath = "arguments.txt";
    }
}
