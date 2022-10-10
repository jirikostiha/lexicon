namespace Lexicon.SqlLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;
    using Lexicon;
    using Lexicon.EntityModel;
    using SqlKata;
    using SqlKata.Compilers;

    public class SQLiteWordRepository : IWordRepository
    {
        private SQLiteOptions _options;

        public SQLiteWordRepository(SQLiteOptions options)
        {
            Guard.IsNotNull(options);
            Guard.IsNotNull(options.ConnectionString);

            _options = options;
        }

        public async Task<long> CountAsync(CancellationToken ct = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection)
            {
                CommandText = "Select count(*) FROM Words"
            };
            var count = await command.ExecuteScalarAsync(ct)
                .ConfigureAwait(false);

            return (long)count;
        }

        public async Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default)
        {
            Guard.IsNotNull(filter);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            var query = new Query("Words");
            if (filter.Language is not null)
                query = query.Where("language", (int)filter.Language);
            if (filter.Class is not null)
                query = query.Where("class", (int)filter.Class);
            if (!string.IsNullOrEmpty(filter.StartsWith))
                query.WhereStarts("word", filter.StartsWith, false);

            var compiler = new SqliteCompiler();
            var sqlQuery = compiler.Compile(query).ToString();

            using var command = new SQLiteCommand(connection)
            {
                CommandText = sqlQuery,
            };

            var wordRecords = await ReadWordRecordsAsync(command, ct)
                .ConfigureAwait(false);

            return wordRecords;
        }

        public async Task Save(WordRecord record, CancellationToken ct = default)
        {
            Guard.IsNotNull(record);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            var query = new Query("Words")
                .AsInsert(new
                {
                    word = record.Word,
                    language = (long)record.Metadata.Language,
                    @class = (long)record.Metadata.Class,
                });

            var compiler = new SqliteCompiler();
            var sqlQuery = compiler.Compile(query).ToString();
            using var command = new SQLiteCommand(connection)
            {
                CommandText = sqlQuery
            };

            await command.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task SaveAll(IEnumerable<WordRecord> records, CancellationToken ct = default)
        {
            Guard.IsNotNull(records);

            if (!records.Any())
                return;

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection);
            using var transaction = connection.BeginTransaction();

            foreach (var record in records)
            {
                var query = new Query("Words")
                .AsInsert(new
                {
                    word = record.Word,
                    language = (long)record.Metadata.Language,
                    @class = (long)record.Metadata.Class,
                });

                var compiler = new SqliteCompiler();
                var sqlQuery = compiler.Compile(query).ToString();
                command.CommandText = sqlQuery;

                await command.ExecuteNonQueryAsync(ct)
                    .ConfigureAwait(false);
            }

            await transaction.CommitAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task Remove(string word, CancellationToken ct = default)
        {
            Guard.IsNotNullOrEmpty(word);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection)
            {
                CommandText = "DELETE FROM Words WHERE word = @word"
            };
            command.Parameters.AddWithValue("@word", word);
            await command.PrepareAsync(ct)
                .ConfigureAwait(false);

            await command.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task Clear(CancellationToken ct = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection)
            {
                CommandText = "DELETE FROM Words"
            };
            await command.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }

        private static async Task<IEnumerable<WordRecord>> ReadWordRecordsAsync(SQLiteCommand command, CancellationToken ct = default)
        {
            var wordRecords = new List<WordRecord>();
            using var reader = await command.ExecuteReaderAsync(ct)
                .ConfigureAwait(false);

            while (await reader.ReadAsync(ct)
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
                    Language = (Language)reader.GetInt32(1),
                    Class = (WordClass)reader.GetInt32(2),
                }
            };
        }
    }
}
