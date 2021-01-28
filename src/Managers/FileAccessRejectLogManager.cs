using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FileAccessControlAgent.Managers
{
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
        public string creationTime { get; set; }
        public string lastWriteTime { get; set; }
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

    class LogData
    {
        public FileAccessInfo fileAccessInfo { get; set; }
        public FileInfo fileInfo { get; set; }
        public ModuleInfo moduleInfo { get; set; }
    }

    class SendResponse : ITCPResponse
    {
        private string result;

        public string Result { get { return result; } set { result = value; } }
    }

    static class FileAccessRejectLogManager
    {
        public static void ReceiveFileAccessLog(object stateInfo)
        {
            while (true)
            {
                using (var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut))
                {
                    pipeServer.WaitForConnection();

                    LogData logData;

                    try
                    {
                        using (var streamReader = new StreamReader(pipeServer, Encoding.UTF8))
                        {
                            string jsonString = "";
                            char[] buffer = new char[512];
                            int nread = 0;

                            do
                            {
                                nread = streamReader.Read(buffer);
                                jsonString += new string(buffer, 0, nread);
                            }
                            while (nread == 512);

                            logData = JsonSerializer.Deserialize<LogData>(jsonString);

                            using (var binaryWriter = new BinaryWriter(pipeServer))
                            {
                                bool whitelistCheckResult = true;
                                binaryWriter.Write(whitelistCheckResult);
                                binaryWriter.Flush();

                                if (whitelistCheckResult)
                                {
                                    // Show pop-up message
                                    Task.Factory.StartNew(() =>
                                    {
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            var popup = new FileAccessRejectPopup();
                                            popup.ShowDialog();
                                        });
                                    });

                                    // Send the reject log to the server
                                    var response = jsonString.SendRequest<SendResponse>();
                                    MessageBox.Show(response.Result);
                                }
                            }
                        }
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        private static readonly string pipeName = "RejectLogPipe";
    }
}
