using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client
{
    public class ClientHttpConnection
    {
        private readonly HttpClient _client;
        private readonly string _uri;

        public static string BuildHttpUri(string host, int port, params string[] path)
        {
            var postfix = string.Join("/", path);
            postfix = postfix.Length > 0 ? postfix + "/" : "";
            return $"http://{host}:{port}/{postfix}";
        }

        public ClientHttpConnection(string uri)
        {
            _client = new HttpClient();
            _uri = uri;
        }

        public async Task PostAsync(object data, Action<string> resultCallback)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(data));
            HttpResponseMessage response = await _client.PostAsync(_uri, requestContent);
            HttpContent content = response.Content;
            var result = await content.ReadAsStringAsync();
            resultCallback(result);
        }
    }
}
