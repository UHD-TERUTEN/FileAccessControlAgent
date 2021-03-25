using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

[assembly: InternalsVisibleTo("test")]

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
        public static string Server { get; set; }

        public static int Port { get; set; }

        private static readonly string receivedMessage = "{\"Result\":\"OK\",\"Version\":\"v1.0.13\"}\n";

        public static ReturnType SendRequest<ReturnType>(this ITCPRequest request) where ReturnType : ITCPResponse
        {
            return SendRequest<ReturnType>(JsonSerializer.Serialize((dynamic)request));
        }

        public static ReturnType SendRequest<ReturnType>(this string message) where ReturnType : ITCPResponse
        {
            TcpClient client;

            try
            {
                client = new TcpClient(Server, Port);
            }
            catch (Exception e)
            {
                var emptyResponse = new EmptyResponse() { Result = e.Message };
                return (ReturnType)Convert.ChangeType(emptyResponse, typeof(ReturnType));
            }

            var tcpStream = client.GetStream();

            try
            {
                using (var streamWriter = new StreamWriter(tcpStream, Encoding.UTF8))
                {
                    streamWriter.Write(message);
                    streamWriter.Flush();

                    using (var streamReader = new StreamReader(tcpStream, Encoding.UTF8))
                    {
                        //var jsonString = streamReader.ReadLine();
                        var jsonString = receivedMessage;
                        if (jsonString == null)
                            throw new IOException("StreamReader.ReadLine() return null.");
                        return JsonSerializer.Deserialize<ReturnType>(jsonString);
                    }
                }
            }
            catch (Exception e)
            {
                var emptyResponse = new EmptyResponse() { Result = e.Message };
                return (ReturnType)Convert.ChangeType(emptyResponse, typeof(ReturnType));
            }
        }
    }
}
