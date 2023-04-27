namespace Lexicon.SQLite
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class SQLiteDataModelDeployerTests
    {
        [TestMethod]
        [TestCategory("positive")]
        public async Task DeployAsync_InMemory_Empty()
        {
            var deployer = new SQLiteModelDeployer(DM.Sql, Helper.InMemoryDbOptions)
            {
                TablesOrderToDrop = DM.TableNames.ToArray()
            };

            await deployer.DeployAsync();

            var count = Helper.GetNumberOfRecord(Helper.InMemoryDbOptions.ConnectionString, DM.TWords.Name);
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        [TestCategory("positive")]
        public async Task DeployAsync_AsFile_Empty()
        {
            var deployer = new SQLiteModelDeployer(DM.Sql, Helper.TestFileDbOptions)
            {
                TablesOrderToDrop = DM.TableNames.ToArray()
            };

            await deployer.DeployAsync();

            Assert.IsTrue(File.Exists(Helper.TestDbFile));
            var count = Helper.GetNumberOfRecord(Helper.TestFileDbOptions.ConnectionString, DM.TWords.Name);
            Assert.AreEqual(0, count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(Helper.TestDbFile);
        }
    }
}