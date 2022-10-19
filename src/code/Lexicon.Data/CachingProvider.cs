namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;

    public class CachingProvider : IWordProvider
    {
        private readonly IWordProvider _underlyingProvider;

        public CachingProvider(IWordProvider underlyingProvider)
        {
            _underlyingProvider = underlyingProvider;
        }

        public Task<long> CountAsync(CancellationToken ct = default)
            =>  _underlyingProvider.CountAsync(ct);

        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
