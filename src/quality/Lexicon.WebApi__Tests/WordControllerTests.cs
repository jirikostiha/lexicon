namespace Lexicon.WebApi
{
    using FakeItEasy;
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using Lexicon.WebApi.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;

    [TestClass]
    public class WordControllerTests
    {
        private readonly ILogger<WordsController> _logger = new NullLogger<WordsController>();
        private readonly IWordProvider _provider = A.Fake<IWordProvider>();

        private readonly WordMultiSourceProvider _multiSourceProvider;
        private readonly WordsController _controller;

        [TestInitialize]
        public void SetUp()
        {
            var providers = new List<(string, IWordProvider)>() { ("fake", _provider) };
            _multiSourceProvider = new WordMultiSourceProvider(new SourceProvider(providers));
            _controller = new WordsController(_multiSourceProvider, _logger);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task Get_EmptyFilter_FirstPage()
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