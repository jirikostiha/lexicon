﻿namespace Lexicon.WebApi.Controllers
{
    using Lexicon.EntityModel;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SerilogTimings;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Words export controller.
    /// </summary>
    [Route("api/words/export")]
    [ApiController]
    public sealed class WordsExportController : ControllerBase
    {
        private readonly ILogger<WordsExportController> _logger;
        private readonly WordMultiSourceProvider _multiSourceProvider;
        private readonly ICsvFormatter _csvFormatter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="multiSourceProvider"> multi source provider of words </param>
        /// <param name="csvFormatter"> csv word formatter </param>
        /// <param name="logger"> logger </param>
        public WordsExportController(WordMultiSourceProvider multiSourceProvider, ICsvFormatter csvFormatter, ILogger<WordsExportController> logger)
        {
            _logger = logger;
            _multiSourceProvider = multiSourceProvider;
            _csvFormatter = csvFormatter;
        }

        /// <summary>
        /// Export filtered word records.
        /// </summary>
        /// <param name="filter"> Data filter </param>
        /// <param name="ct"> Cancellation token </param>
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

            _logger.GotRecordsCount(records.Length);

            byte[]? bytes = null;
            using (Operation.Time("Exporting {0} records to csv.", nameof(WordRecord)))
            {
                var content = await _csvFormatter.FormatAsync(records, ct)
                    .ConfigureAwait(false);
                bytes = Encoding.ASCII.GetBytes(content);
            }

            _logger.ExportedSize(bytes.Length);

            return File(bytes, MediaTypeNames.Application.Octet, "Words.csv");
        }
    }
}