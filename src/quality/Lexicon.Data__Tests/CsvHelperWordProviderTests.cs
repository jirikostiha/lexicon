namespace Lexicon.Data
{
    using Lexicon.EntityModel;
    using System.IO;

    [TestClass]
    public class CsvHelperWordProviderTests
    {
        [TestInitialize]
        public void SetUp()
        {
            Helper.CreateCsvFile();
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task CountAsync_AllRecords_Match()
        {
            var provider = Helper.CreateProvider();

            var count = await provider.CountAsync();

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task GetByFilterAsync_EmptyFilter_ReturnAll()
        {
            var provider = Helper.CreateProvider();

            var records = await provider.GetByFilterAsync(WordFilter.Empty);

            Assert.AreEqual(2, records.Count());
        }

        [TestMethod]
        [TestCategory("negative")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByFilterAsync_NullFilter_Exception()
        {
            var repo = new CsvHelperWordProvider(new CsvHelperWordProviderOptions());

            await repo.GetByFilterAsync(null);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(Helper.TestCsvFile);
        }
    }
}