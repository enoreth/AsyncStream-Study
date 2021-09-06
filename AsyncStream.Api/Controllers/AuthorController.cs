using AsyncStream.Core;
using Microsoft.AspNetCore.Mvc;
using Ndjson.AsyncStreams.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncStream.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private string[] namepool = new string[] { "Hans", "Thomas", "Mark", "Lisa" };
        private string[] sirnamepool = new string[] { "Jahovic", "Hannesberg"};

        [HttpGet]
        public ActionResult<IEnumerable<Author>> Get()
        {
            return new List<Author>
            {
                new Author{ Id = 1, FirstName ="Hans", LastName = "Toman"}
            };
        }

        [HttpGet("getasync")]
        public async IAsyncEnumerable<Author> GetAsync()
        {
            await foreach(var author in GetAuthorsAsync())
            {
                yield return author;
            }            
        }

        private async IAsyncEnumerable<Author> GetAuthorsAsync()
        {
            Random r1 = new Random();
            Random r2 = new Random();

            for(var i = 0; i < 100000; i++)
            {
                var author = new Author
                {
                    Id = i,
                    FirstName = namepool[r1.Next(0, 3)],
                    LastName = sirnamepool[r2.Next(0, 1)]
                };
                await Task.Delay(100)
                    .ConfigureAwait(false);
                yield return author;
            }
        }

        [HttpGet("getndjsonasync")]
        public NdjsonAsyncEnumerableResult<Author> GetAuthorsStreamedAsync()
        {
            return new NdjsonAsyncEnumerableResult<Author>(GetAuthorsAsync());
        }

        [HttpGet("negotiate-stream")]
        public IAsyncEnumerable<Author> NegotiateStream()
        {
            return GetAuthorsAsync();
        }
    }
}
