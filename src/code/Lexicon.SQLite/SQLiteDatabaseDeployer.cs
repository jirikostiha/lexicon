namespace Lexicon.SQLite
{
    using System;
    using System.Data.SQLite;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;

    //[next] deployer
    //  -model creator
    //  -repo
    //  -migrator

    public class SQLiteDatabaseDeployer
    {
        private readonly SQLiteOptions _options;

        public SQLiteDatabaseDeployer(SQLiteOptions options)
        {
            Guard.IsNotNull(options);
            Guard.IsNotNull(options.ConnectionString);

            _options = options;
        }

        public async Task CreateDatabaseAsync(CancellationToken ct = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);
            
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var dropCommand = new SQLiteCommand(connection)
            {
                CommandText = $"DROP TABLE IF EXISTS {DataModel.WordsTable.Name}"
            };
            await dropCommand.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);

            using var createDbCommand = new SQLiteCommand(connection)
            {
                CommandText = DataModel.Sql
            };
            await createDbCommand.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }
    }
}
