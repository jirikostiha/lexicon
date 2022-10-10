namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class JsonFormatter : IJsonFormatter
    {
        public Task<string> FormatAsync<T>(IEnumerable<T> records, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }       
    }
}
