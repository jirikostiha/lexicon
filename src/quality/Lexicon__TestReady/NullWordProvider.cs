namespace Lexicon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;

    public class NullWordProvider : IWordProvider
    {
        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default) 
            => Task.FromResult(Enumerable.Empty<WordRecord>());
    }
}
