using FileAccessControlAgent.Helpers;
using FileAccessControlAgent.Managers;
using FileAccessControlAgent.Samples;
using System;
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
            if (DBManager.Init())
            {
                DropTables();
                CreateTables();
                //MenuSamples.InitSampleData();
            }

            // Run threads
            string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\";
            using (var streamReader = new StreamReader(parentDirectory + argumentsPath))
            {
                string[] arguments = streamReader.ReadLine()?.Split(' ');
                if (arguments != null)
                {
                    HttpRequestManager.Server = arguments[0];
                    HttpRequestManager.Port = int.Parse(arguments[1]);
                }
            }
            ThreadPool.QueueUserWorkItem(FileAccessRejectNotifier.ReceiveNotification);
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
            _ = "create table if not exists WhitelistVersion (Id integer, Version varchar(10), LastDistributed varchar(30))".Execute();
            _ = "create table if not exists LogList (ProgramName varchar(250), FileName varchar(250), Operation varchar(20), PlainText text unique, IsAccept boolean)".Execute();
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
