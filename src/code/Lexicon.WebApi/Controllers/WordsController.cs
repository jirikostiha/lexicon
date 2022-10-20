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
    using Microsoft.Extensions.Logging;
    using SerilogTimings;

    /// <summary>
    /// Words controller.
    /// </summary>
    [Route("api/words")]
    [ApiController]
    public sealed class WordsController : ControllerBase
    {
        private readonly ILogger<WordsController> _logger;
        private readonly WordMultiSourceProvider _multiSourceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="multiSourceProvider"> multi source provider of words </param>
        /// <param name="logger"> logger </param>
        public WordsController(WordMultiSourceProvider multiSourceProvider, ILogger<WordsController> logger)
        {
            _logger = logger;
            _multiSourceProvider = multiSourceProvider;
        }

        /// <summary>
        /// Get all word records.
        /// </summary>
        /// <param name="page"> page number starting from 0 </param>
        /// <param name="pageSize"> page size </param>
        /// <param name="ct"> Cancelation token </param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DataPage<WordRecord>>> GetAll(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            CancellationToken ct = default)
        {
            if (page < Pagination.PageNumberMin)
                return BadRequest($"Parameter '{nameof(page)}' is less than minimal value ({Pagination.PageNumberMin}).");
            if (page > Pagination.PageNumberMax)
                return BadRequest($"Parameter '{nameof(page)}' is greater than maximal value ({Pagination.PageNumberMax}).");
            if (pageSize < Pagination.PageSizeMin)
                return BadRequest($"Parameter '{nameof(pageSize)}' is less than minimal value ({Pagination.PageNumberMin}).");
            if (pageSize > Pagination.PageSizeMax)
                return BadRequest($"Parameter '{nameof(pageSize)}' is greater than maximal value ({Pagination.PageNumberMax}).");

            WordRecord[]? records = null;
            using (Operation.Time("Getting {0} records from sources.", nameof(WordRecord)))
            {
                records = (await _multiSourceProvider.GetByFilterAsync(MultiSourceWordFilter.Empty, ct)
                    .ConfigureAwait(false))
                .ToArray();
            }

            _logger.GotRecordsCount(records.Length);

            if (!records.Any())
                return NotFound();

            var dataPage = new DataPage<WordRecord>
            {
                PageNumber = page,
                ItemsPerPage = pageSize,
                Items = records.Skip(page * pageSize).Take(pageSize).ToArray(),
            };

            return Ok(dataPage);
        }

        /// <summary>
        /// Get filtered word records.
        /// </summary>
        /// <param name="page"> page number starting from 0 </param>
        /// <param name="pageSize"> page size </param>
        /// <param name="filter"> Data filter </param>
        /// <param name="ct"> Cancelation token </param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DataPage<WordRecord>>> Get(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromBody] MultiSourceWordFilter? filter = null,
            CancellationToken ct = default)
        {
            if (page < Pagination.PageNumberMin)
                return BadRequest($"Parameter '{nameof(page)}' is less than minimal value ({Pagination.PageNumberMin}).");
            if (page > Pagination.PageNumberMax)
                return BadRequest($"Parameter '{nameof(page)}' is greater than maximal value ({Pagination.PageNumberMax}).");
            if (pageSize < Pagination.PageSizeMin)
                return BadRequest($"Parameter '{nameof(pageSize)}' is less than minimal value ({Pagination.PageNumberMin}).");
            if (pageSize > Pagination.PageSizeMax)
                return BadRequest($"Parameter '{nameof(pageSize)}' is greater than maximal value ({Pagination.PageNumberMax}).");

            WordRecord[]? records = null;
            using (Operation.Time("Getting {0} records from sources.", nameof(WordRecord)))
            {
                records = (await _multiSourceProvider.GetByFilterAsync(filter ?? MultiSourceWordFilter.Empty, ct)
                    .ConfigureAwait(false))
                .ToArray();
            }

            _logger.GotRecordsCount(records.Length);

            if (!records.Any())
                return NotFound();

            var dataPage = new DataPage<WordRecord>
            {
                PageNumber = page,
                ItemsPerPage = pageSize,
                Items = records.Skip(page * pageSize).Take(pageSize).ToArray(),
            };

            return Ok(dataPage);
        }
    }
}
