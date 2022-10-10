namespace Lexicon.TestReady
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Diagnostics;
    using Lexicon.EntityModel;

    public class HardCodedWordProvider : IWordProvider
    {
        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(filter);

            var words = WordRecordsFilter.Filter(WordSets.All, filter);

            return Task.FromResult(words);
        }
    }
}
