namespace Lexicon
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;

    public interface IWordProvider
    {
        public Task<long> CountAsync(CancellationToken ct = default);

        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default);
    }

    public interface IWordRepository : IWordProvider
    {
        public Task Save(WordRecord record, CancellationToken ct = default);

        public Task SaveAll(IEnumerable<WordRecord> record, CancellationToken ct = default);

        public Task Remove(string word, CancellationToken ct = default);

        public Task Clear(CancellationToken ct = default);
    }
}
