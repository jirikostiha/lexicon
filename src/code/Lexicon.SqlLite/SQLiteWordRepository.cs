namespace Lexicon.SqlLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;
    using FluentValidation;
    using Lexicon;
    using Lexicon.EntityModel;
    using SqlKata;
    using SqlKata.Compilers;

    public class SQLiteWordRepository : IWordRepository
    {
        private readonly SQLiteOptions _options;

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

            var query = new Query("Words").AsCount();
            var sqlQuery = new SqliteCompiler().Compile(query).ToString();
            using var command = new SQLiteCommand(connection)
            {
                CommandText = sqlQuery
            };
            var count = await command.ExecuteScalarAsync(ct)
                .ConfigureAwait(false);

            return Convert.ToInt64(count, CultureInfo.InvariantCulture);
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
            
            var sqlQuery = new SqliteCompiler().Compile(query).ToString();
            using var command = new SQLiteCommand(connection)
            {
                CommandText = sqlQuery,
            };
            var wordRecords = await ReadWordRecordsAsync(command, ct)
                .ConfigureAwait(false);

            return wordRecords;
        }

        public async Task SaveAsync(WordRecord record, CancellationToken ct = default)
        {
            Guard.IsNotNull(record);

            var validator = new WordRecordValidator();
            validator.ValidateAndThrow(record);

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
            var sqlQuery = new SqliteCompiler().Compile(query).ToString();
            using var command = new SQLiteCommand(connection)
            {
                CommandText = sqlQuery
            };
            await command.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task SaveAllAsync(IEnumerable<WordRecord> records, CancellationToken ct = default)
        {
            Guard.IsNotNull(records);

            if (!records.Any())
                return;

            var validator = new WordRecordValidator();
            // How to validate and throw errors of all items at once?
            foreach (var record in records)
                validator.ValidateAndThrow(record);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            using var command = new SQLiteCommand(connection);
            using var transaction = connection.BeginTransaction();
            var compiler = new SqliteCompiler();
            foreach (var record in records)
            {
                var query = new Query("Words")
                .AsInsert(new
                {
                    word = record.Word,
                    language = (long)record.Metadata.Language,
                    @class = (long)record.Metadata.Class,
                });
                command.CommandText = compiler.Compile(query).ToString();
                await command.ExecuteNonQueryAsync(ct)
                    .ConfigureAwait(false);
            }

            await transaction.CommitAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task RemoveAsync(string word, CancellationToken ct = default)
        {
            Guard.IsNotNullOrEmpty(word);

            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            var query = new Query("Words").AsDelete().Where("word", word);
            var sqlQuery = new SqliteCompiler().Compile(query).ToString();
            using var command = new SQLiteCommand(connection)
            {
                CommandText = sqlQuery
            };
            //command.Parameters.AddWithValue("@word", word);
            //await command.PrepareAsync(ct)
            //    .ConfigureAwait(false);
            await command.ExecuteNonQueryAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task ClearAsync(CancellationToken ct = default)
        {
            using var connection = new SQLiteConnection(_options.ConnectionString);
            await connection.OpenAsync(ct)
                .ConfigureAwait(false);

            var query = new Query("Words").AsDelete();
            var sqlQuery = new SqliteCompiler().Compile(query).ToString();
            using var command = new SQLiteCommand(connection)
            {
                CommandText = sqlQuery
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
                if (record is not null)
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
