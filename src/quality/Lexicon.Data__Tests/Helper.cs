namespace Lexicon
{
    using System.IO;

    public static class Helper
    {
        public static string TestCsvFile => @"testCsv.csv";

        public static string TestCsvFileContent =>
            $"Word,Language,Class\n" +
            $"word1,0,0\n" +
            $"word2,1,1";

        public static CsvHelperWordProviderOptions TestOptions => new()
        {
            File = TestCsvFile
        };

        public static void CreateCsvFile()
        {
            File.WriteAllText(TestCsvFile, TestCsvFileContent);
        }

        public static CsvHelperWordProvider CreateProvider()
            => new CsvHelperWordProvider(TestOptions);
    }
}
