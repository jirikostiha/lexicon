namespace Lexicon.SQLite
{
    public static class Db
    {
        public const string Definition = WordsTable.Definition;

        public static class WordsTable
        {
            public const string Name = "Words";
            public const string WordColumnName = "word";
            public const string LanguageColumnName = "language";
            public const string ClassColumnName = "class";


            public const string Definition = 
                $@"CREATE TABLE {Name} (
                {WordColumnName} TEXT NOT NULL PRIMARY KEY,
                {LanguageColumnName} INT NOT NULL,
                {ClassColumnName} INT NOT NULL)";
        }
    }
}
