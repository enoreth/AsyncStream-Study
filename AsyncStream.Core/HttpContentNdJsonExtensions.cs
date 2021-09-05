using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace AsyncStream.Core
{
    public static class HttpContentNdJsonExtensions
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async IAsyncEnumerable<TValue> ReadFromNdjsonAsync<TValue>(this HttpContent content)
        {
            if(content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            string? mediaType = content.Headers.ContentType?.MediaType;

            if(mediaType is null || !mediaType.Equals("application/x-ndjson", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException();
            }

            Stream contentStream = await content.ReadAsStreamAsync()
                .ConfigureAwait(false);

            using (contentStream)
            {
                using (var reader = new StreamReader(contentStream))
                {
                    while (!reader.EndOfStream)
                    {
                        yield return JsonSerializer.Deserialize<TValue>(
                            await reader.ReadLineAsync().ConfigureAwait(false), _serializerOptions
                            );
                    }
                }
            }
        }
    }
}
