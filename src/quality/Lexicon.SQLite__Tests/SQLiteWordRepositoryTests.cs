namespace Lexicon.SQLite
{
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class SQLiteWordRepositoryTests
    {
        [TestMethod]
        [TestCategory("positive")]
        public async Task CountAsync_AllRecords_Match()
        {
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();

            var count = await repo.CountAsync();

            Assert.AreEqual(WordSets.All.Count(), count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task SaveAsync_SingleRecord_CountIsOne()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();
            var record1 = WordSets.CzechMaleNames.First();

            await repo.SaveAsync(record1);

            var count = await repo.CountAsync();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [TestCategory("negative")]
        [ExpectedException(typeof(FluentValidation.ValidationException))]
        public async Task SaveAsync_InvalidRecord_Exception()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();
            var record = new WordRecord();

            await repo.SaveAsync(record);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task SaveAsync_ConflictingRecord_Updated()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();
            var record1 = WordSets.CzechMaleNames.First();
            await repo.SaveAsync(record1);
            var record2 = record1 with { Metadata = new WordMetadata { Language = Language.Greek } };

            await repo.SaveAsync(record2);

            var count = await repo.CountAsync();
            Assert.AreEqual(1, count);
            var dbRecords = await repo.GetByFilterAsync(WordFilter.Empty);
            Assert.AreEqual(record2.Metadata.Language, dbRecords.First().Metadata.Language);
            Assert.AreEqual(record2.Metadata.Class, dbRecords.First().Metadata.Class);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task SaveAllAsync_MultipleRecords_CountIsMatching()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();

            await repo.SaveAllAsync(WordSets.CzechMaleNames);

            var count = await repo.CountAsync();
            Assert.AreEqual(WordSets.CzechMaleNames.Count(), count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task GetByFilterAsync_EmptyFilter_ReturnAll()
        {
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();

            var records = await repo.GetByFilterAsync(WordFilter.Empty);

            Assert.AreEqual(WordSets.All.Count(), records.Count());
            CollectionAssert.AreEquivalent(WordSets.All.ToArray(), records.ToArray());
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task GetByFilterAsync_FullCondition_ReturnFiltered()
        {
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();
            var filter = new WordFilter()
            {
                Language = Language.Czech,
                Class = WordClass.Noun,
                StartsWith = "J"
            };

            var records = (await repo.GetByFilterAsync(filter)).ToArray();

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
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();

            await repo.GetByFilterAsync(null);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task RemoveAsync_ExistingRecord_IsNotPresent()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();
            var record1 = WordSets.CzechMaleNames.First();
            await repo.SaveAsync(record1);

            await repo.RemoveAsync(record1.Word);

            var count = await repo.CountAsync();
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task ClearAsync_ExistingRecords_CountIsZero()
        {
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();

            await repo.ClearAsync();

            var count = await repo.CountAsync();
            Assert.AreEqual(0, count);
        }
    }
}