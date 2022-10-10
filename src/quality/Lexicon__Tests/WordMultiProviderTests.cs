namespace Lexicon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WordMultiProviderTests
    {
        [TestMethod()]
        public async Task GetByFilterAsync_NoFilter_AllRecords()
        {
            var providers = new List<(string, IWordProvider)>() { ("coded", new HardCodedWordProvider()) };
            var sourceProvider = new SourceProvider(providers);
            var provider = new WordMultiSourceProvider(sourceProvider);

            var filter = MultiSourceWordFilter.Empty;
            var words = (await provider.GetByFilterAsync(filter)
                    .ConfigureAwait(false))
                .ToArray();

            Assert.IsNotNull(words);
            Assert.AreEqual(WordSets.All.Count(), words.Length);
        }
    }
}
