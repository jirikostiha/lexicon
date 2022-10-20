namespace Lexicon.SQLite
{
    using System;
    using System.Data.SQLite;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;

    public sealed class SQLiteDataModelDeployer
    {
        private readonly string _connectionString;

        public SQLiteDataModelDeployer(string connectionString)
        {
            Guard.IsNotNull(connectionString);

            _connectionString = connectionString;
        }

        public async Task DeployAsync(CancellationToken ct = default)
        {
            using var connection = new SQLiteConnection(_connectionString);
            
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var dropCommand = new SQLiteCommand(connection)
            {
                CommandText = $"DROP TABLE IF EXISTS {DM.TWords.Name}"
            };
            await dropCommand.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);

            using var createDbCommand = new SQLiteCommand(connection)
            {
                CommandText = DM.Sql
            };
            await createDbCommand.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }
    }
}
