using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

[assembly: InternalsVisibleTo("test")]

namespace FileAccessControlAgent.Managers
{
    interface IHttpRequest { }  // json
    interface IHttpResponse { }  // json

    class EmptyResponse : IHttpResponse
    {
        public string Message { get; set; }
    }

    static class HttpRequestManager
    {
        public static string Server { get; set; }

        public static int Port { get; set; }

        public static async Task<ReturnType> Get<ReturnType>(this string uri) where ReturnType : IHttpResponse
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"http://{Server}:{Port}/{uri}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ReturnType>(json, jsonOptions);
        }

        public static async Task Post<ContentType>(this string uri, ContentType content) where ContentType : IHttpRequest
        {
            using var client = new HttpClient();
            var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(content));
            var byteContent = new ByteArrayContent(buffer)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };
            var response = await client.PostAsync($"http://{Server}:{Port}/{uri}", byteContent);
            response.EnsureSuccessStatusCode();
        }

        private static readonly JsonSerializerOptions jsonOptions =
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
    }
}
