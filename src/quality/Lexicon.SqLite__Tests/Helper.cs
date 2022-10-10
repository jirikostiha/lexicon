namespace Lexicon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon.SqlLite;
    using Lexicon.TestReady;

    public static class Helper
    {
        public static string InMemoryDbConnectionString => "Data Source = InMemoryDb; Mode = Memory; Cache = Shared";

        public static SQLiteOptions InMemoryDbOptions => new SQLiteOptions 
        { 
            ConnectionString = InMemoryDbConnectionString 
        };

        public static async Task PrepareInMemoryDbAsync()
        {
            var deployer = new SQLiteDatabaseDeployer(InMemoryDbOptions);
            await deployer.CreateDatabaseAsync();
        }

        public static async Task<SQLiteWordRepository> CreateRepoWithInMemoryDbAsync()
        {
            await PrepareInMemoryDbAsync();

            return new SQLiteWordRepository(InMemoryDbOptions);
        }

        public static async Task<SQLiteWordRepository> CreateRepoWithFilledInMemoryDbAsync()
        {
            var deployer = new SQLiteDatabaseDeployer(InMemoryDbOptions, () => WordSets.All);
            await deployer.CreateDatabaseAsync();
            await deployer.FillAsync();

            return new SQLiteWordRepository(InMemoryDbOptions);
        }
    }
}
