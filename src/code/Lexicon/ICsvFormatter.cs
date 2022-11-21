namespace Lexicon
{
    public interface ICsvFormatter
    {
        Task<string> FormatAsync<T>(IEnumerable<T> items, CancellationToken ct = default);
    }
}
