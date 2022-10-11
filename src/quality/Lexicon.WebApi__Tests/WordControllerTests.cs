namespace Lexicon.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FakeItEasy;
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using Lexicon.WebApi.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WordControllerTests
    {
        private readonly ILogger<WordsController> _logger = new NullLogger<WordsController>();
        private readonly IWordProvider _provider = A.Fake<IWordProvider>();
        
        private WordMultiSourceProvider _multiSourceProvider;
        private WordsController _controller;

        [TestInitialize]
        public void SetUp()
        {
            var providers = new List<(string, IWordProvider)>() { ("fake", _provider) };
            _multiSourceProvider = new WordMultiSourceProvider(new SourceProvider(providers));
            _controller = new WordsController(_multiSourceProvider, _logger);
        }

        [TestMethod]
        public async Task Get()
        {
            var filter = MultiSourceWordFilter.Empty;
            A.CallTo(() => _provider.GetByFilterAsync(filter.WordFilter, default))
                .Returns(Task.FromResult(WordSets.All));
            
            var result = await _controller.Get(0, 10, filter);

            Assert.IsNotNull(result);
            var dataPage = (DataPage<WordRecord>)((OkObjectResult)result.Result)?.Value;
            Assert.AreEqual(10, dataPage.Items.Count);
        }
    }
}
