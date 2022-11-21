namespace Lexicon
{
    using CommunityToolkit.Diagnostics;
    using System.Collections.Concurrent;

    public class SourceProvider
    {
        private readonly ConcurrentDictionary<string, IWordProvider> _providers;

        public SourceProvider(IEnumerable<(string, IWordProvider)> providers)
        {
            _providers = new ConcurrentDictionary<string, IWordProvider>();
            foreach (var provider in providers)
                _providers.TryAdd(provider.Item1, provider.Item2);
        }

        public SourceProvider()
            : this(new ConcurrentDictionary<string, IWordProvider>())
        { }

        public SourceProvider(ConcurrentDictionary<string, IWordProvider> providersDic)
        {
            _providers = providersDic ?? new ConcurrentDictionary<string, IWordProvider>();
        }

        public IEnumerable<string> GetAllKeys()
        {
            return _providers.Keys;
        }

        public IWordProvider? GetByKey(string providerId)
        {
            Guard.IsNotNullOrEmpty(providerId);

            return _providers.TryGetValue(providerId, out var provider) 
                ? provider 
                : null;
        }
    }
}
