//using Xunit;
//using FileAccessControlAgent.Managers;
//using System.Net.Sockets;
//using System.Net;
//using FileAccessControlAgent.Views;
//using System.IO;
//using System.Text;
//using System.Text.Json;
//using System.Threading;

//namespace test
//{
//    public class TCPRequestManagerTest
//    {
//        [Fact]
//        public void TestGetWhitelistVersion()
//        {
//            HttpRequestManager.Server = server;
//            HttpRequestManager.Port = port;

//            ThreadPool.QueueUserWorkItem(RunTcpServer);

//            var res = new GetWhitelistVersion().SendRequest<WhitelistVersion>();

//            Assert.Equal("OK", res.Result);
//            Assert.Equal("v1.0.13", res.Version);
//        }

//        private static void RunTcpServer(object _)
//        {
//            var listener = new TcpListener(IPAddress.Parse(server), port);

//            listener.Start();
//            {
//                var client = listener.AcceptTcpClient();
//                var tcpStream = client.GetStream();

//                using (var streamReader = new StreamReader(tcpStream, Encoding.UTF8))
//                {
//                    var requestString = streamReader.ReadToEnd();
//                    string responseString = null;

//                    try
//                    {
//                        var request = JsonSerializer.Deserialize<GetWhitelistVersion>(requestString);
//                        responseString = getWhitelistVersionResponse;
//                    }
//                    catch
//                    {
//                        // try other request types...
//                    }
//                    finally
//                    {
//                        using (var streamWriter = new StreamWriter(tcpStream, Encoding.UTF8))
//                        {
//                            streamWriter.Write(responseString);
//                            streamWriter.Flush();
//                        }
//                    }
//                }
//            }
//            listener.Stop();
//        }

//        private static readonly string server = "127.0.0.1";

//        private static readonly int port = 12345;

//        private static readonly string getWhitelistVersionResponse = "{\"Result\":\"OK\",\"Version\":\"v1.0.13\"}\n";
//    }
//}
