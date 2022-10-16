namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Lexicon.EntityModel;

    /// <summary>
    /// Naive Csv Word Provider implementation.
    /// </summary>
    public class CsvHelperWordProvider : IWordProvider
    {
        private readonly CsvHelperWordProviderOptions _options;

        public CsvHelperWordProvider(CsvHelperWordProviderOptions options)
        {
            _options = options;
        }

        public async Task<long> CountAsync(CancellationToken ct = default)
        {
            var records = await GetRecordsAsync(ct)
                .ConfigureAwait(false);
            
            return records.Count();
        }

        public async Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default)
        {
            var records = await GetRecordsAsync(ct)
                .ConfigureAwait(false);
            
            return WordFilter.Filter(records, filter);
        }

        private async Task<IEnumerable<WordRecord>> GetRecordsAsync(CancellationToken ct = default)
        {
            var content = File.ReadAllText(_options.File);
            using var textReader = new StringReader(content);
            using var csvReader = new CsvReader(textReader, _options.CsvConfiguration);

            var records = await csvReader.GetRecordsAsync<WordRecord>(ct)
                .ToArrayAsync(ct)
                .ConfigureAwait(false);
            
            return records;
        }
    }
}
