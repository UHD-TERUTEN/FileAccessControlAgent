using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

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
            var emptyResponse = new EmptyResponse();

            using (var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut))
            {
                if (!pipeClient.IsConnected)
                    pipeClient.Connect();

                try
                {
                    using (var binaryWriter = new BinaryWriter(pipeClient, Encoding.UTF8))
                    {
                        binaryWriter.Write(JsonSerializer.SerializeToUtf8Bytes((dynamic)request));
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
    }
}
