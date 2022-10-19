namespace Lexicon
{
    using System;
    using System.Data.SQLite;
    using System.Threading.Tasks;
    using Lexicon.SQLite;
    using Lexicon.TestReady;

    public static class Helper
    {
        public static string TestDbFile => ".\\testDb.sqlite";

        public static string InMemoryDbConnectionString => "Data Source = InMemoryDb; Mode = Memory; Cache = Shared";
        public static string TestDbConnectionString => $"Data Source = {TestDbFile}";

        public static SQLiteOptions InMemoryDbOptions => new SQLiteOptions 
        { 
            ConnectionString = InMemoryDbConnectionString 
        };

        public static SQLiteOptions TestFileDbOptions => new SQLiteOptions
        {
            ConnectionString = TestDbConnectionString
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
            var deployer = new SQLiteDatabaseDeployer(InMemoryDbOptions);
            await deployer.CreateDatabaseAsync();
            await new SQLiteWordRepository(InMemoryDbOptions).SaveAllAsync(WordSets.All);

            return new SQLiteWordRepository(InMemoryDbOptions);
        }

        public static long GetNumberOfRecord(string connectionString, string table)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using var command = new SQLiteCommand(connection)
            {
                CommandText = $"Select count(*) FROM {table}"
            };
            var count = command.ExecuteScalar();

            return (long)count;
        }
    }
}
