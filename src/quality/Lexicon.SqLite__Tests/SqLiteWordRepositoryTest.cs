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
    public class SQLiteWordRepositoryTest
    {
        [TestMethod]
        public async Task Save_SingleRecord_CountIsOne()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();
            var record1 = WordSets.CzechMaleNames.First();

            await repo.Save(record1);

            var count = await repo.CountAsync();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task SaveAll_MultipleRecords_CountIsMatching()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();

            await repo.SaveAll(WordSets.CzechMaleNames);
            
            var count = await repo.CountAsync();
            Assert.AreEqual(WordSets.CzechMaleNames.Count(), count);
        }

        [TestMethod]
        public async Task GetByFilter_EmptyFilter_ReturnAll()
        {
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();

            var records = await repo.GetByFilterAsync(WordFilter.Empty);

            Assert.AreEqual(WordSets.All.Count(), records.Count());
            CollectionAssert.AreEquivalent(WordSets.All.ToArray(), records.ToArray());
        }

        [TestMethod]
        public async Task GetByFilter_FullCondition_ReturnFiltered()
        {
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();
            var filter = new WordFilter() { 
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
        public async Task Remove_ExistingRecord_IsNotPresent()
        {
            var repo = await Helper.CreateRepoWithInMemoryDbAsync();
            var record1 = WordSets.CzechMaleNames.First();
            await repo.Save(record1);

            await repo.Remove(record1.Word);

            var count = await repo.CountAsync();
            Assert.AreEqual(0, count);
        }
        
        [TestMethod]
        public async Task Clear_ExistingRecords_CountIsZero()
        {
            var repo = await Helper.CreateRepoWithFilledInMemoryDbAsync();

            await repo.Clear();

            var count = await repo.CountAsync();
            Assert.AreEqual(0, count);
        }
    }
}
