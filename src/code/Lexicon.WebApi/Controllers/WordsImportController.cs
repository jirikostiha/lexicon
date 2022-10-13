namespace Lexicon.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;
    using Lexicon.SqlLite;
    using Lexicon.WebApi;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SerilogTimings;

    /// <summary>
    /// Words quering controller.
    /// </summary>
    [Route("api/words/import")]
    [ApiController]
    public class WordsImportController : ControllerBase
    {
        private readonly ILogger<WordsImportController> _logger;
        private readonly IWordRepository _repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"> word repository </param>
        /// <param name="logger"> logger </param>
        public WordsImportController(IWordRepository repository, ILogger<WordsImportController> logger)
        {
            _logger = logger;
            _repository = repository;
        }
    }
}
