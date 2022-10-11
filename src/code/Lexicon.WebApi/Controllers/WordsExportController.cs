﻿namespace Lexicon.WebApi.Controllers
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
    /// Words export controller.
    /// </summary>
    [Route("api/words/export")]
    [ApiController]
    public class WordsExportController : ControllerBase
    {
        private WordMultiSourceProvider _multiSourceProvider;

        private readonly ICsvFormatter _csvFormatter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="multiSourceProvider"> multi source provider of words </param>
        /// <param name="csvFormatter"> csv word formatter </param>
        public WordsExportController(WordMultiSourceProvider multiSourceProvider, ICsvFormatter csvFormatter)
        {
            _multiSourceProvider = multiSourceProvider;
            _csvFormatter = csvFormatter;
        }

        /// <summary>
        /// Export filtered word records.
        /// </summary>
        /// <param name="filter"> Data filter </param>
        /// <param name="ct"> Cancelation token </param>
        [HttpPost]
        public async Task<IActionResult> ExportToCsv(
            [FromBody] MultiSourceWordFilter filter,
            CancellationToken ct = default)
        {
            WordRecord[]? records = null;
            using (Operation.Time("Getting {0} records from sources.", nameof(WordRecord)))
            {
                records = (await _multiSourceProvider.GetByFilterAsync(filter, ct)
                    .ConfigureAwait(false))
                .ToArray();
            }

            byte[]? bytes = null;
            using (Operation.Time("Exporting {0} records to csv.", nameof(WordRecord)))
            {
                var content = await _csvFormatter.FormatAsync(records, ct)
                    .ConfigureAwait(false);
                 bytes = Encoding.ASCII.GetBytes(content);
            }

            return File(bytes, "application/octet-stream", "Words.csv");
        }
    }
}