namespace Lexicon
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Lexicon.Data;
    using Microsoft.Extensions.Caching.Memory;

    public static class Helper
    {
        public static string TestCsvFile => @"testCsv.csv";

        public static string TestCsvFileContent => 
            $"Word,Language,Class\n" +
            $"word1,0,0\n" +
            $"word2,1,1";

        public static CsvHelperWordProviderOptions CsvTestOptions => new()
        {
            File = TestCsvFile
        };

        public static void CreateCsvFile()
        {
            File.WriteAllText(TestCsvFile, TestCsvFileContent);
        }

        public static CsvHelperWordProvider CreateCsvProvider()
            => new CsvHelperWordProvider(CsvTestOptions);

        public static CachingProvider CreateCachingProviderWithCsvProvider()
        {
            //todo
            var memoryCache = new MemoryCache(new MemoryCacheOptions() { });
            var cachingProvider = new CachingProvider(Helper.CreateCsvProvider(), memoryCache);

            new CsvHelperWordProvider(CsvTestOptions);
            return default;
        }
    }
}
