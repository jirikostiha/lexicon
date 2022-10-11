namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CsvHelper;
    using CsvHelper.Configuration;

    public class CsvHelperFormatter : ICsvFormatter
    {
        private readonly CsvConfiguration _csvConfiguration;

        public CsvHelperFormatter(CsvConfiguration? csvConfiguration = null)
        {
            _csvConfiguration = csvConfiguration ?? new CsvConfiguration(CultureInfo.InvariantCulture);
        }

        public async Task<string> FormatAsync<T>(IEnumerable<T> items, CancellationToken ct = default)
        {
            var csv = new StringBuilder();
            await using var textWriter = new StringWriter(csv);
            await using var csvWriter = new CsvWriter(textWriter, _csvConfiguration);

            await csvWriter.WriteRecordsAsync(items, ct)
                .ConfigureAwait(false);

            // make sure all records are flushed to stream
            await csvWriter.FlushAsync()
                .ConfigureAwait(false);

            return csv.ToString();
        }
    }
}
