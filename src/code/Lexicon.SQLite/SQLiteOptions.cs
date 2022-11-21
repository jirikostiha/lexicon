namespace Lexicon.SQLite
{
    public record SQLiteOptions
    {
        /// <summary> Base name </summary>
        public const string BaseName = "SQLite";

        public string ConnectionString { get; init; }

        /// <summary> Single connection timeout. </summary>
        public TimeSpan ConnectionTimeout { get; init; } = TimeSpan.FromSeconds(5);
    }
}
