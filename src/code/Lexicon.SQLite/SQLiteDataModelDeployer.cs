namespace Lexicon.SQLite
{
    public sealed class SQLiteModelDeployer
    {
        private readonly SQLiteOptions _options;

        public SQLiteModelDeployer(string sqlModel, SQLiteOptions options)
        {
            Guard.IsNotNull(sqlModel);
            Guard.IsNotNull(options);

            SqlModel = sqlModel;
            _options = options;
        }

        public string SqlModel { get; }

        public string ConnectionString => _options.ConnectionString;

        public TimeSpan ConnectionTimeout => _options.ConnectionTimeout;

        public IList<string> TablesOrderToDrop { get; set; } = new List<string>();

        public async Task DeployAsync(CancellationToken ct = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);

            using var timeoutCts = new CancellationTokenSource(_options.ConnectionTimeout);
            using var ctsWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, ct);

            await connection.OpenAsync(ctsWithTimeout.Token)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection);
            // Drop tables one by one. Currently sqlite has no drop all alternative command.
            foreach (var table in TablesOrderToDrop)
            {
                command.CommandText = $"DROP TABLE IF EXISTS {table}";
                timeoutCts.TryReset();
                await command.ExecuteNonQueryAsync(ctsWithTimeout.Token)
                    .ConfigureAwait(false);
            }

            command.CommandText = SqlModel;
            timeoutCts.TryReset();
            await command.ExecuteNonQueryAsync(ctsWithTimeout.Token)
                .ConfigureAwait(false);
        }
    }
}