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

//separate export, import
//get all filter options

//ODapa web api - filtering
//paging example - rest.schneids.net - paginng example
//fluent validation
//mediatR - ?

    /// <summary>
    /// Words quering controller.
    /// </summary>
    [Route("api/words")]  //wo action?
    [ApiController]
    public class WordsController : ControllerBase
    {
        private const int PageSizeMin = 4;
        private const int PageSizeMax = 100_000;
        private const int PageNumberMin = 0;
        private const int PageNumberMax = 10_000;

        private WordMultiSourceProvider _multiSourceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="multiSourceProvider"> multi source provider of words </param>
        public WordsController(WordMultiSourceProvider multiSourceProvider)
        {
            _multiSourceProvider = multiSourceProvider;
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
            [FromBody] MultiSourceWordFilter filter,
            CancellationToken ct = default)
        {
            if (page < PageNumberMin)
                return BadRequest($"Parameter '{nameof(page)}' is less than minimal value ({PageNumberMin}).");
            if (page > PageNumberMax)
                return BadRequest($"Parameter '{nameof(page)}' is greater than maximal value ({PageNumberMax}).");
            if (pageSize < PageSizeMin)
                return BadRequest($"Parameter '{nameof(pageSize)}' is less than minimal value ({PageNumberMin}).");
            if (pageSize > PageSizeMax)
                return BadRequest($"Parameter '{nameof(pageSize)}' is greater than maximal value ({PageNumberMax}).");

            WordRecord[]? records = null;
            using (Operation.Time("Getting {0} records from sources.", nameof(WordRecord)))
            {
                records = (await _multiSourceProvider.GetByFilterAsync(filter, ct)
                    .ConfigureAwait(false))
                .ToArray();
            }

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
