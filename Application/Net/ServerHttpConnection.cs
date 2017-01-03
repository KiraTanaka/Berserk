using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Net
{
    public class ServerHttpConnection
    {
        private const int BufferSize = 1024;
        private bool _listening = true;

        public static string BuildHttpUri(string host, int port, params string[] path)
        {
            var postfix = string.Join("/", path);
            postfix = postfix.Length > 0 ? postfix + "/" : "";
            return $"http://{host}:{port}/{postfix}";
        }

        public void StopListening()
        {
            _listening = false;
        }

        /// <summary>
        /// Listens the specified uri. 
        /// Passes user's post data to the specified callback.
        /// Returns to the user a result of the specified callback.
        /// </summary>
        public async Task Listen(string uri, Func<string, string> userRequestCallback)
        {
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(uri);
                listener.Start();
                if (listener.IsListening)
                    foreach (var prefix in listener.Prefixes)
                        Console.WriteLine($"Listening {prefix}");

                while (_listening)
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    Console.WriteLine($"Connected to {context.Request.RemoteEndPoint}");

                    var buffer = new byte[BufferSize];
                    var read = context.Request.InputStream.Read(buffer, 0, BufferSize);
                    var userRequestData = Encoding.Default.GetString(buffer, 0, read);

                    var userResponce = userRequestCallback(userRequestData);

                    buffer = Encoding.Default.GetBytes(userResponce);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
