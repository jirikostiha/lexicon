namespace Lexicon
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Lexicon.EntityModel;

    public interface IWordProvider
    {
        public Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken cancellationToken = default);
    }

    public interface IWordRepository : IWordProvider
    {
        public Task Save(WordRecord record, CancellationToken cancellationToken = default);

        public Task SaveAll(IEnumerable<WordRecord> record, CancellationToken cancellationToken = default);

        public Task Remove(string word, CancellationToken cancellationToken = default);

        public Task Clear(CancellationToken cancellationToken = default);
    }
}
