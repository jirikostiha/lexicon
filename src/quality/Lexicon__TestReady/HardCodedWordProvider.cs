﻿namespace Lexicon.TestReady
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;
    using Lexicon.EntityModel;
    using Lexicon.SqlLite;

    public class HardCodedWordProvider : IWordProvider
    {
        private Func<IEnumerable<WordRecord>> _recordsProvider;

        public HardCodedWordProvider(Func<IEnumerable<WordRecord>>? recordsProvider = null)
        {
            _recordsProvider = recordsProvider ?? (() => Enumerable.Empty<WordRecord>());
        }

        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default)
        {
            Guard.IsNotNull(filter);

            var words = WordRecordsFilter.Filter(_recordsProvider(), filter);

            return Task.FromResult(words);
        }
    }
}
