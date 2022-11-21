namespace Lexicon
{
    public interface IJsonFormatter
    {
        Task<string> FormatAsync<T>(IEnumerable<T> items, CancellationToken ct = default);
    }
}
