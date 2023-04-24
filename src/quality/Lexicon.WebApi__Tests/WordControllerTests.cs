namespace Lexicon.WebApi
{
    using FakeItEasy;
    using FluentAssertions;
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using Lexicon.WebApi.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Logging.Abstractions;
    using System.Collections.Generic;
    using Xunit;
    using System.Threading.Tasks;

    public class WordControllerTests
    {
        private readonly ILogger<WordsController> _logger = new NullLogger<WordsController>();
        private readonly IWordProvider _provider = A.Fake<IWordProvider>();

        private WordMultiSourceProvider _multiSourceProvider;
        private WordsController _controller;

        public WordControllerTests()
        {
            var providers = new List<(string, IWordProvider)>() { ("fake", _provider) };
            _multiSourceProvider = new WordMultiSourceProvider(new SourceProvider(providers));
            _controller = new WordsController(_multiSourceProvider, _logger);
        }

        [Fact()]
        [Trait("Category", "positive")]
        public async Task Get_EmptyFilter_FirstPage()
        {
            var filter = MultiSourceWordFilter.Empty;
            A.CallTo(() => _provider.GetByFilterAsync(filter.WordFilter, default))
                .Returns(Task.FromResult(WordSets.All));

            var result = await _controller.Get(0, 10, filter);

            Assert.NotNull(result);
            var dataPage = (DataPage<WordRecord>)((OkObjectResult)result.Result)?.Value;
            Assert.Equal(10, dataPage.Items.Count);
        }
    }
}