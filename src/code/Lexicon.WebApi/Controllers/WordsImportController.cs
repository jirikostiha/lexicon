namespace Lexicon.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SerilogTimings;

    /// <summary>
    /// Words import controller.
    /// </summary>
    [Route("api/[words]/[import]")]
    [ApiController]
    public class WordsImportController : ControllerBase
    {
        private const int PageSizeMin = 4;
        private const int PageSizeMax = 100_000;
        private const int PageNumberMin = 0;
        private const int PageNumberMax = 10_000;

        private WordMultiSourceProvider _multiSourceProvider;

//repo

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="multiSourceProvider"> multi source provider of words </param>
        public WordsImportController(WordMultiSourceProvider multiSourceProvider)
        {
            _multiSourceProvider = multiSourceProvider;
        }

        /// <summary>
        /// Import word records to writable source.
        /// </summary>
        /// <param name="ct"> Cancelation token </param>
        [HttpPost]
        public async Task<IActionResult> Import(
            [FromBody] MultiSourceWordFilter filter,
            CancellationToken ct = default)
        {
            
        }
    }
}
