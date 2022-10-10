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
        public async Task<string> FormatAsync<T>(IEnumerable<T> items, CancellationToken ct = default)
        {
            var csv = new StringBuilder();

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
            };

            await using var textWriter = new StringWriter(csv);
            await using var csvWriter = new CsvWriter(textWriter, csvConfig);

            await csvWriter.WriteRecordsAsync(items, ct);

            // make sure all records are flushed to stream
            await csvWriter.FlushAsync();

            return csv.ToString();
        }
    }
}
