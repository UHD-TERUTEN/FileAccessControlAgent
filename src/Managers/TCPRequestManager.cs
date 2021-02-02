using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace FileAccessControlAgent.Managers
{
    interface ITCPRequest { }   // json
    interface ITCPResponse      // json
    {
        public string Result { get; set; }
    }

    class EmptyResponse : ITCPResponse
    {
        private string result = "FAILED";
        public string Result { get { return result; } set { result = value; } }
    }

    static class TCPRequestManager
    {
        public static ReturnType SendRequest<ReturnType>(this ITCPRequest request) where ReturnType : ITCPResponse
        {
            return SendRequest<ReturnType>(JsonSerializer.Serialize((dynamic)request));
        }

        public static ReturnType SendRequest<ReturnType>(this string request) where ReturnType : ITCPResponse
        {
            var emptyResponse = new EmptyResponse();

            using (var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut))
            {
                if (!pipeClient.IsConnected)
                    pipeClient.Connect();

                try
                {
                    using (var binaryWriter = new BinaryWriter(pipeClient, Encoding.UTF8))
                    {
                        binaryWriter.Write(request);
                        binaryWriter.Flush();

                        using (var streamReader = new StreamReader(pipeClient, Encoding.UTF8))
                        {
                            var jsonString = streamReader.ReadLine();
                            if (jsonString == null)
                                throw new IOException("StreamReader.ReadLine() return null.");
                            return JsonSerializer.Deserialize<ReturnType>(jsonString);
                        }
                    }
                }
                catch (IOException e)
                {
                    emptyResponse.Result = e.Message;
                }
                return (ReturnType)Convert.ChangeType(emptyResponse, typeof(ReturnType));
            }
        }

        private static readonly string pipeName = "TcpPipe";
    }
}
