namespace Lexicon.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SQLiteDataModelDeployerTests
    {
        [TestMethod]
        [TestCategory("positive")]
        public async Task DeployAsync_InMemory_Empty()
        {
            var deployer = new SQLiteDataModelDeployer(Helper.InMemoryDbConnectionString);

            await deployer.DeployAsync();

            var count = Helper.GetNumberOfRecord(Helper.InMemoryDbOptions.ConnectionString, DataModel.WordsTable.Name);
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task DeployAsync_AsFile_Empty()
        {
            var deployer = new SQLiteDataModelDeployer(Helper.TestDbConnectionString);

            await deployer.DeployAsync();

            Assert.IsTrue(File.Exists(Helper.TestDbFile));
            var count = Helper.GetNumberOfRecord(Helper.TestFileDbOptions.ConnectionString, DataModel.WordsTable.Name);
            Assert.AreEqual(0, count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(Helper.TestDbFile);
        }
    }
}
