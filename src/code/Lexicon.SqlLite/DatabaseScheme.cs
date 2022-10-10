namespace Lexicon.SqlLite
{
    public static class DatabaseSqlScheme
    {
        public static string WordsTable => @"CREATE TABLE Words (
            word TEXT NOT NULL PRIMARY KEY,
            class TEXT NOT NULL,
            language TEXT NOT NULL)";
    }
}
