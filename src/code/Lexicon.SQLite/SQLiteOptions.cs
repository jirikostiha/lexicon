namespace Lexicon.SQLite
{
    public record SQLiteOptions
    {
        /// <summary> Base name </summary>
        public const string BaseName = "SQLite";

        public string ConnectionString { get; init; }
    }
}
