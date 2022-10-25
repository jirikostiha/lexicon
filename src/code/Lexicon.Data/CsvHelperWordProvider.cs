namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
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
            var templateData = ParseTemplate(_options.File)
                .ToArray();

            var content = File.ReadAllText(_options.File);
            using var textReader = new StringReader(content);
            using var csvReader = new CsvReader(textReader, _options.CsvConfiguration);

            Language? templateLanguage = (Language)templateData.FirstOrDefault(x => x.Name == nameof(WordMetadata.Language)).Value;
            WordClass? templateClass = (WordClass)templateData.FirstOrDefault(x => x.Name == nameof(WordMetadata.Class)).Value;

            var templateRecord = new WordRecord 
            { 
                Metadata = new() 
                { 
                    Language = templateLanguage.Value,
                    Class = templateClass.Value
                } 
            };
            var records = await csvReader.GetRecordsAsync<WordRecord>(templateRecord, ct).Select(x =>
            {
                if (templateLanguage is not null)
                {

                }
                return x;
            })
                .ToArrayAsync(ct)
                .ConfigureAwait(false);
            
            return records;
        }

        private static IEnumerable<(string Name, object Value)> ParseTemplate(string rawTemplateString)
        {
            var templateString = Regex.Match(rawTemplateString, @".*\[.*\]", RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1)).Value;
            
            if (string.IsNullOrEmpty(templateString))
                yield break;

            var splittedValues = templateString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var valueStr in splittedValues)
            {
                if (Enum.TryParse<Language>(valueStr, out var language))
                {
                    yield return (nameof(WordMetadata.Language), language);
                    continue;
                }
                if (Enum.TryParse<WordClass>(valueStr, out var wordClass))
                {
                    yield return (nameof(WordMetadata.Class), wordClass);
                    continue;
                }
            }
        }
    }
}
