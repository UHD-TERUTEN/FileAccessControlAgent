using FileAccessControlAgent.Helpers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FileAccessControlAgent.Managers
{
    class TimeInfo
    {
        public bool isWorkingTime { get; set; }
    }

    class FileAccessInfo
    {
        public string functionName { get; set; }
        public int returnValue { get; set; }
        public uint errorCode { get; set; }
    }

    class FileInfo
    {
        public string fileName { get; set; }
        public long fileSize { get; set; }
        public long creationTime { get; set; }
        public long lastWriteTime { get; set; }
        public bool isHidden { get; set; }
    }

    class ModuleInfo
    {
        public string companyName { get; set; }
        public string fileDescription { get; set; }
        public string fileVersion { get; set; }
        public string internalName { get; set; }
        public string legalCopyright { get; set; }
        public string originalFilename { get; set; }
        public string productName { get; set; }
        public string productVersion { get; set; }
    }

    class Subject
    {
        public FileInfo fileInfo { get; set; }
        public ModuleInfo moduleInfo { get; set; }
    }

    class Object
    {
        public FileInfo fileInfo { get; set; }
        public ModuleInfo moduleInfo { get; set; }
    }

    class Operation
    {
        public FileAccessInfo fileAccessInfo { get; set; }
    }

    class Environment
    {
        public TimeInfo timeInfo { get; set; }
    }

    class LogData
    {
        public Subject subject { get; set; }
        public Object Object { get; set; }
        public Operation operation { get; set; }
        public Environment environment { get; set; }
    }

    class SendResponse : IHttpResponse
    {
        private string result;

        public string Result { get { return result; } set { result = value; } }
    }

    static class FileAccessRejectNotifier
    {
        public static void ReceiveNotification(object stateInfo)
        {
            string prevProcess = null;
            while (true)
            {
                using var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In);
                pipeServer.WaitForConnection();

                LogData? logData;

                try
                {
                    using var streamReader = new StreamReader(pipeServer, Encoding.UTF8);
                    string jsonString = "";
                    char[] buffer = new char[4096];
                    int nread = 0;

                    do
                    {
                        nread = streamReader.Read(buffer);
                        jsonString += new string(buffer, 0, nread);
                    }
                    while (nread == 4096);

                    logData = JsonSerializer.Deserialize<LogData>(jsonString, jsonOption);
                    if (logData is null)
                        continue;

                    // Update database
                    _ = @"INSERT OR IGNORE INTO LogList(ProgramName,FileName,Operation,PlainText,IsAccept)
                            VALUES(@ProgramName,@FileName,@Operation,@PlainText,false)"
                        .Execute(new Dictionary<string, string>()
                        {
                            { "@ProgramName", logData.subject.fileInfo.fileName },
                            { "@FileName", logData.Object.fileInfo.fileName },
                            { "@Operation", logData.operation.fileAccessInfo.functionName },
                            { "@PlainText", $"{jsonString}" },
                        });

                    // Show pop-up message
                    if (prevProcess != logData.subject.fileInfo.fileName)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var popup = new FileAccessRejectPopup();
                                popup.ShowDialog();
                            });
                        });
                    }
                    prevProcess = logData.subject.fileInfo.fileName;
                }
                catch (IOException e)
                {
                    MessageBoxHelper.Warning(e.Message);
                }
            }
        }

        private static readonly string pipeName = "RejectLogPipe";

        private static readonly JsonSerializerOptions jsonOption = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}