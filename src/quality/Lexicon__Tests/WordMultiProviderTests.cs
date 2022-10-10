namespace Lexicon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WordMultiProviderTests
    {
        [TestMethod()]
        public async Task GetByFilterAsync_NoFilter_RecordsFromAllSources()
        {
            var providers = new List<(string, IWordProvider)>() 
            { 
                ("source1", new HardCodedWordProvider(() => WordSets.CzechMaleNames)),
                ("source2", new HardCodedWordProvider(() => WordSets.EnglishMaleNames)),
                ("source3", new NullWordProvider())
            };
            var sourceProvider = new SourceProvider(providers);
            var provider = new WordMultiSourceProvider(sourceProvider);

            var words = (await provider.GetByFilterAsync(MultiSourceWordFilter.Empty)
                    .ConfigureAwait(false))
                .ToArray();

            Assert.IsNotNull(words);
            Assert.AreEqual(WordSets.CzechMaleNames.Union(WordSets.EnglishMaleNames).Count(), words.Length);
        }

        [TestMethod()]
        public async Task GetByFilterAsync_OnlyFromOneSource_RecordsFromThatSource()
        {
            var providers = new List<(string, IWordProvider)>()
            {
                ("source1", new HardCodedWordProvider(() => WordSets.CzechMaleNames)),
                ("source2", new HardCodedWordProvider(() => WordSets.EnglishMaleNames)),
                ("source3", new NullWordProvider())
            };
            var sourceProvider = new SourceProvider(providers);
            var provider = new WordMultiSourceProvider(sourceProvider);

            var words = (await provider.GetByFilterAsync(new MultiSourceWordFilter { SourceIds = new() { "source1" } })
                    .ConfigureAwait(false))
                .ToArray();

            Assert.IsNotNull(words);
            Assert.AreEqual(WordSets.CzechMaleNames.Count(), words.Length);
        }
    }
}
