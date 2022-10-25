namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CachingProviderTests
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
            var provider = Helper.CreateCachingProviderWithCsvProvider();

            var count = await provider.CountAsync();

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task GetByFilterAsync_EmptyFilter_ReturnAll()
        {
            var provider = Helper.CreateCachingProviderWithCsvProvider();

            var records = await provider.GetByFilterAsync(WordFilter.Empty);

            Assert.AreEqual(2, records.Count());
        }

        [ClassCleanup]
        public void ClassCleanup()
        {
            File.Delete(Helper.TestCsvFile);
        }
    }
}
