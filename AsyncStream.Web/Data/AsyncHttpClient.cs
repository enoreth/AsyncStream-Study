using System.Net.Http;

namespace AsyncStream.Web.Data
{
    public class AsyncHttpClient 
    {
        public HttpClient Client { get; private set; }
        public AsyncHttpClient(HttpClient client) {
            Client = client;
        }

    }
}