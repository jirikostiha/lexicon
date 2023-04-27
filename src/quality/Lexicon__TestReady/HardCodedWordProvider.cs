namespace Lexicon.TestReady
{
    using CommunityToolkit.Diagnostics;
    using Lexicon.EntityModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class HardCodedWordProvider : IWordProvider
    {
        private Func<IEnumerable<WordRecord>> _recordsProvider;

        public HardCodedWordProvider(Func<IEnumerable<WordRecord>> recordsProvider = null)
        {
            _recordsProvider = recordsProvider ?? (() => Enumerable.Empty<WordRecord>());
        }

        public Task<long> CountAsync(CancellationToken ct = default)
            => Task.FromResult(_recordsProvider().LongCount());

        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default)
        {
            Guard.IsNotNull(filter);

            var words = WordFilter.Filter(_recordsProvider(), filter);

            return Task.FromResult(words);
        }
    }
}