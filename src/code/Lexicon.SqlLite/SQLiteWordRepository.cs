namespace Lexicon.SqlLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;
    using Lexicon;
    using Lexicon.EntityModel;
    using Microsoft.Extensions.Logging;

    //https://zetcode.com/csharp/sqlite/
    //https://www.technical-recipes.com/2016/using-sqlite-in-c-net-environments/
    //https://stackoverflow.com/questions/15292880/create-sqlite-database-and-table

    //https://docs.microsoft.com/en-us/events/dotnetconf-focus-on-maui/csharp-and-linq-for-data-access-with-ef-core

    public class SQLiteWordRepository : IWordRepository
    {
        //private readonly ILogger<SQLiteWordRepository> _logger;

        private SQLiteOptions _options;

        public SQLiteWordRepository(SQLiteOptions options)
        {
            Guard.IsNotNull(options);
            Guard.IsNotNull(options.ConnectionString);
            //Guard.IsNotNull(logger);

            _options = options;
            //_logger = logger;
        }

        public async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection)
            {
                CommandText = "Select count(*) FROM Words"
            };
            var count = await command.ExecuteScalarAsync(cancellationToken)
                .ConfigureAwait(false);

            return (long)count;
        }

        public async Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(filter);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection)
            {
                //CommandText = "Select * FROM Words",
            };
            AppendFilterToQuery(command, filter);
            await command.PrepareAsync(cancellationToken)
                .ConfigureAwait(false);
            
            var wordRecords = await ReadWordRecordsAsync(command, cancellationToken)
                .ConfigureAwait(false);

            return wordRecords;
        }

        public async Task Save(WordRecord record, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(record);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection);
            SetInsertQuery(command, record);
            await command.PrepareAsync(cancellationToken)
                .ConfigureAwait(false);
            await command.ExecuteNonQueryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task SaveAll(IEnumerable<WordRecord> records, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(records);

            if (!records.Any())
                return;

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection);
            using var transaction = connection.BeginTransaction();

            foreach (var record in records)
            {
                SetInsertQuery(command, record);
                await command.PrepareAsync(cancellationToken)
                    .ConfigureAwait(false);
                await command.ExecuteNonQueryAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            await transaction.CommitAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task Remove(string word, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNullOrEmpty(word);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection)
            {
                CommandText = "DELETE FROM Words WHERE word = @word"
            };
            command.Parameters.AddWithValue("@word", word);
            await command.PrepareAsync(cancellationToken)
                .ConfigureAwait(false);

            await command.ExecuteNonQueryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task Clear(CancellationToken cancellationToken = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection)
            {
                CommandText = "DELETE FROM Words"
            };
            await command.ExecuteNonQueryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private static async Task<IEnumerable<WordRecord>> ReadWordRecordsAsync(SQLiteCommand command, CancellationToken cancellationToken = default)
        {
            var wordRecords = new List<WordRecord>();
            using var reader = await command.ExecuteReaderAsync(cancellationToken)
                .ConfigureAwait(false);

            while (await reader.ReadAsync(cancellationToken)
                .ConfigureAwait(false))
            {
                var record = ReadSingleRecord(reader);
                wordRecords.Add(record);
            }

            if (wordRecords is null)
                return Enumerable.Empty<WordRecord>();

            return wordRecords;
        }

        private static WordRecord? ReadSingleRecord(DbDataReader reader)
        {
            return new WordRecord()
            {
                Word = reader.GetString(0),
                Metadata = new()
                {
                    Language = Enum.Parse<Language>(reader.GetString(1)),
                    Class = Enum.Parse<WordClass>(reader.GetString(2)),
                }
            };
        }

        private static void SetInsertQuery(SQLiteCommand command, WordRecord record)
        {
            command.CommandText = "INSERT INTO Words values(@word, @language, @class)";
            command.Parameters.AddWithValue("@word", record.Word);
            command.Parameters.AddWithValue("@language", record.Metadata.Language);
            command.Parameters.AddWithValue("@class", record.Metadata.Class);
        }

        private static void AppendFilterToQuery(SQLiteCommand command, WordFilter? filter)
        {
            if (filter is null)
                return;

            //var condition = "values(";

            //if (filter.Language is not null)
            //    condition += "@language";

            //if (filter.Class is not null)
            //    condition += "@class";

            //command.CommandText += condition + ")";
            //command.CommandText = "SELECT * FROM Words WHERE language = @language AND class = @class";
            command.CommandText = "SELECT * FROM Words WHERE language = @language";

            command.Parameters.AddWithValue("@language", filter.Language);
            //command.Parameters.AddWithValue("@class", filter.Class);
            //command.Parameters.AddWithValue("@word", record.Word);
        }

        //verify record
        private static void VerifyRecord(WordRecord record)
        {

        }
    }
}
