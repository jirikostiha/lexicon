using System.Collections;
using System.Collections.Generic;

namespace Lexicon.SQLite
{
    /// <summary>
    /// Data model
    /// </summary>
    public static class DM
    {
        public const string Sql = TWords.Sql;

        public static HashSet<string> TableNames => new() { TWords.Name };

        /// <summary> Table </summary>
        public static class TWords
        {
            public const string Name = "Words";
            public const string CWord = "word";
            public const string CLanguage = "language";
            public const string CClass = "class";

            public const string Sql =
                $@"CREATE TABLE {Name} (
                {CWord} TEXT NOT NULL PRIMARY KEY,
                {CLanguage} INT NOT NULL,
                {CClass} INT NOT NULL)";
        }
    }
}
