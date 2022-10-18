﻿namespace Lexicon.SQLite
{
    using System;
    using System.Data.SQLite;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;

    //[next] db deployer
    //  -model deployer
    //  -repo
    //  -migrator

    public class SQLiteDataModelDeployer
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
