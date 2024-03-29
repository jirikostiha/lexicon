﻿namespace Lexicon.Data
{
    using System.IO;
    using System.Text;
    using System.Text.Json;

    public class JsonFormatter : IJsonFormatter
    {
        private readonly JsonSerializerOptions _options;

        public JsonFormatter(JsonSerializerOptions? options = null)
        {
            _options = options ?? new JsonSerializerOptions();
        }

        public async Task<string> FormatAsync<T>(IEnumerable<T> items, CancellationToken ct = default)
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, items, _options, ct)
                .ConfigureAwait(false);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}