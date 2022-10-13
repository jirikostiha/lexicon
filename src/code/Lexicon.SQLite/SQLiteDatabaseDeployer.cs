namespace Lexicon.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;
    using Lexicon.EntityModel;

    public class SQLiteDatabaseDeployer
    {
        private SQLiteOptions _options;
        private Func<IEnumerable<WordRecord>> _recordsProvider;

        public SQLiteDatabaseDeployer(SQLiteOptions options, Func<IEnumerable<WordRecord>>? recordsProvider = null)
        {
            Guard.IsNotNull(options);
            Guard.IsNotNull(options.ConnectionString);

            _options = options;
            _recordsProvider = recordsProvider ?? (() => Enumerable.Empty<WordRecord>());
        }

        public async Task CreateDatabaseAsync(CancellationToken ct = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);
            
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var dropCommand = new SQLiteCommand(connection)
            {
                CommandText = "DROP TABLE IF EXISTS Words"
            };
            await dropCommand.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);

            using var creteTableCommand = new SQLiteCommand(connection)
            {
                CommandText = DatabaseSqlScheme.WordsTable
            };
            await creteTableCommand.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task FillAsync(CancellationToken ct = default)
        {
            await new SQLiteWordRepository(_options)
                .SaveAllAsync(_recordsProvider(), ct)
                .ConfigureAwait(false);
        }
    }
}
