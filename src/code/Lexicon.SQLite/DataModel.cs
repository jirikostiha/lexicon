namespace Lexicon.SQLite
{
    public static class DataModel
    {
        public const string Sql = WordsTable.Sql;

        public static class WordsTable
        {
            public const string Name = "Words";
            public const string WordColumnName = "word";
            public const string LanguageColumnName = "language";
            public const string ClassColumnName = "class";


            public const string Sql =
                $@"CREATE TABLE {Name} (
                {WordColumnName} TEXT NOT NULL PRIMARY KEY,
                {LanguageColumnName} INT NOT NULL,
                {ClassColumnName} INT NOT NULL)";
        }
    }
}
