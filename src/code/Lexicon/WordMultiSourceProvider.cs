namespace Lexicon
{
    using CommunityToolkit.Diagnostics;
    using Lexicon.EntityModel;

    public class WordMultiSourceProvider
    {
        private readonly SourceProvider _sourceProvider;

        public WordMultiSourceProvider(SourceProvider sourceProvider)
        {
            Guard.IsNotNull(sourceProvider);

            _sourceProvider = sourceProvider;
        }

        public async Task<IEnumerable<WordRecord>> GetByFilterAsync(MultiSourceWordFilter filter, CancellationToken ct = default)
        {
            Guard.IsNotNull(filter);
            Guard.IsNotNull(filter.WordFilter);

            var sourceIds = filter.SourceIds is null || !filter.SourceIds.Any()
                ? _sourceProvider.GetAllKeys().ToList()
                : filter.SourceIds;

            var tasks = new List<Task<IEnumerable<WordRecord>>>(sourceIds.Count);
            for (int i = 0; i < sourceIds.Count; i++)
            {
                var provider = _sourceProvider.GetByKey(sourceIds[i]);
                if (provider is not null)
                    tasks.Add(provider.GetByFilterAsync(filter.WordFilter, ct));
            }

            var results = await Task.WhenAll(tasks)
                .ConfigureAwait(false);

            var all = results.SelectMany(x => x);

            return all;
        }
    }
}
