namespace Lexicon.SqlLite
{
    public record SQLiteOptions
    {
        //base name
        public const string Name = "SQLite";

        public string? ConnectionString { get; set; }
    }
}
