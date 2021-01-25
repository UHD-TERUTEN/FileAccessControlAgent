using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
//using System.Text.Json;
using System.Threading.Tasks;

namespace FileAccessControlAgent.Managers
{
    interface ITCPRequest { }   // json
    interface ITCPResponse { }  // json

    static class TCPRequestManager
    {
        public static ITCPResponse SendRequest(this ITCPRequest request)
        {
            using (var pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.Out))
            {
                try
                {
                    using (var streamWriter = new StreamWriter(pipeServer))
                    {
                        //JsonSerializer.SerializeToUtf8Bytes();
                        streamWriter.AutoFlush = true;
                        streamWriter.WriteLine();
                    }
                }
                catch (IOException e)
                {
                    ;
                }
            }
            throw new NotImplementedException();
        }
    }
}
