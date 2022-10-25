namespace Lexicon.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CsvHelperWordProviderTests
    {
        [ClassInitialize]
        public void ClassSetUp()
        {
            Helper.CreateCsvFile();
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task CountAsync_AllRecords_Match()
        {
            var provider = Helper.CreateCsvProvider();

            var count = await provider.CountAsync();

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task GetByFilterAsync_EmptyFilter_ReturnAll()
        {
            var provider = Helper.CreateCsvProvider();

            var records = await provider.GetByFilterAsync(WordFilter.Empty);

            Assert.AreEqual(2, records.Count());
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task GetByFilterAsync_FullCondition_ReturnFiltered()
        {
            var provider = Helper.CreateCsvProvider();
            var filter = new WordFilter()
            {
                Language = Language.Czech,
                Class = WordClass.Noun,
                StartsWith = "J"
            };

            var records = (await provider.GetByFilterAsync(filter)).ToArray();

            var inputFiltered = WordSets.All.Where(x =>
                x.Metadata.Language == Language.Czech
                && x.Metadata.Class == WordClass.Noun
                && x.Word.StartsWith("J")
            ).ToArray();
            Assert.AreEqual(inputFiltered.Length, records.Length);
            CollectionAssert.AreEquivalent(inputFiltered, records);
        }

        [TestMethod]
        [TestCategory("negative")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByFilterAsync_NullFilter_Exception()
        {
            var repo = new CsvHelperWordProvider(new CsvHelperWordProviderOptions());

            await repo.GetByFilterAsync(null);
        }

        [ClassCleanup]
        public void ClassCleanup()
        {
            File.Delete(Helper.TestCsvFile);
        }
    }
}
