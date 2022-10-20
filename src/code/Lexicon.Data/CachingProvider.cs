namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;
    using Microsoft.Extensions.Caching.Memory;

    public class CachingProvider : IWordProvider
    {
        private readonly IWordProvider _underlyingProvider;
        private readonly IMemoryCache _cache;

        public CachingProvider(IWordProvider underlyingProvider, IMemoryCache cache)
        {
            _underlyingProvider = underlyingProvider;
            _cache = cache;
        }

        public async Task<long> CountAsync(CancellationToken ct = default)
        {
            return await _cache.GetOrCreateAsync(
                nameof(CountAsync),
                async _ => await _underlyingProvider.CountAsync(ct)
                    .ConfigureAwait(false));
        }

        public async Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default)
        {
            return await _cache.GetOrCreateAsync(
                filter.ToString(), 
                async _ => await _underlyingProvider.GetByFilterAsync(filter, ct)
                    .ConfigureAwait(false));
        }
    }
}
