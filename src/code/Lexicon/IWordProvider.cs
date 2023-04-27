namespace Lexicon
{
    using Lexicon.EntityModel;

    public interface IWordProvider
    {
        public Task<long> CountAsync(CancellationToken ct = default);

        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default);
    }

    public interface IWordRepository : IWordProvider
    {
        public Task SaveAsync(WordRecord record, CancellationToken ct = default);

        public Task SaveAllAsync(IEnumerable<WordRecord> record, CancellationToken ct = default);

        public Task RemoveAsync(string word, CancellationToken ct = default);

        public Task ClearAsync(CancellationToken ct = default);
    }
}