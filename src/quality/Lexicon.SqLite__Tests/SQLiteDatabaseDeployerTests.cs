namespace Lexicon.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon.SQLite;
    using Lexicon.TestReady;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SQLiteDatabaseDeployerTests
    {
        [TestMethod]
        public async Task CreateDatabaseAsync_InMemory_Empty()
        {
            var deployer = new SQLiteDatabaseDeployer(Helper.InMemoryDbOptions);

            await deployer.CreateDatabaseAsync();

            var count = Helper.GetNumberOfRecord(Helper.InMemoryDbOptions.ConnectionString, "Words");
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public async Task CreateDatabaseAsync_AsFile_Empty()
        {
            var deployer = new SQLiteDatabaseDeployer(Helper.TestFileDbOptions);

            await deployer.CreateDatabaseAsync();

            Assert.IsTrue(File.Exists(Helper.TestDbFile));
            var count = Helper.GetNumberOfRecord(Helper.TestFileDbOptions.ConnectionString, "Words");
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public async Task FillAsync_InMemoryFullSet_DbWithRecords()
        {
            var deployer = new SQLiteDatabaseDeployer(Helper.InMemoryDbOptions, () => WordSets.All);
            await deployer.CreateDatabaseAsync();

            await deployer.FillAsync();

            var count = Helper.GetNumberOfRecord(Helper.InMemoryDbOptions.ConnectionString, "Words");
            Assert.AreEqual(WordSets.All.Count(), count);
        }

        [TestMethod]
        public async Task FillAsync_AsFileFullSet_FileExistsWithRecords()
        {
            var deployer = new SQLiteDatabaseDeployer(Helper.TestFileDbOptions, () => WordSets.All);
            await deployer.CreateDatabaseAsync();

            await deployer.FillAsync();

            Assert.IsTrue(File.Exists(Helper.TestDbFile));
            var count = Helper.GetNumberOfRecord(Helper.TestFileDbOptions.ConnectionString, "Words");
            Assert.AreEqual(WordSets.All.Count(), count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(Helper.TestDbFile);
        }
    }
}
