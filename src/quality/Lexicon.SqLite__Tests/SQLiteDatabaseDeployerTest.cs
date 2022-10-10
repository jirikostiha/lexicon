namespace Lexicon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon.SqlLite;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SQLiteDatabaseDeployerTest
    {
        [TestMethod]
        public async Task CreateDatabaseAsync_NoError()
        {
            var deployer = new SQLiteDatabaseDeployer(Helper.InMemoryDbOptions);

            await deployer.CreateDatabaseAsync();
        }

        [TestMethod]
        public async Task FillAsync_FullSet_NoError()
        {
            var deployer = new SQLiteDatabaseDeployer(Helper.InMemoryDbOptions);
            await deployer.CreateDatabaseAsync();

            await deployer.FillAsync();
        }
    }
}
