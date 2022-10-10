namespace Lexicon.SqlLite
{
    public static class DatabaseSqlScheme
    {
        public static string WordsTable => @"CREATE TABLE Words (
            word TEXT NOT NULL PRIMARY KEY,
            language INT NOT NULL,
            class INT NOT NULL)";
    }
}
