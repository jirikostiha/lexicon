namespace Lexicon
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IJsonFormatter
    {
        Task<string> FormatAsync<T>(IEnumerable<T> items, CancellationToken ct = default);
    }
}
