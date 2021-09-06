using AsyncStream.Core;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncStream.Web.Data
{
    public class AsyncHttpClient 
    {
        public HttpClient Client { get; private set; }
        public AsyncHttpClient(HttpClient client) {
            Client = client;
        }

        public async IAsyncEnumerable<Author> RetrieveAsync()
        {

            using (HttpResponseMessage response = await Client.GetAsync("https://localhost:44331/api/Author/getndjsonasync", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                await foreach (var author in response.Content!.ReadFromNdjsonAsync<Author>()
                    .ConfigureAwait(false))
                {
                    yield return author;
                }
            }
        }

        public async IAsyncEnumerable<Author> RetriveAsync(CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await Client.GetAsync("https://localhost:44331/api/Author/getndjsonasync", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            await foreach (var author in response.Content!.ReadFromNdjsonAsync<Author>()
                .ConfigureAwait(false))
            {
                yield return author;
            }
        }
    }
}