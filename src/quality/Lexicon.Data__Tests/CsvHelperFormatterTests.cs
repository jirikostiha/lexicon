namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using CsvHelper.Configuration;
    using Lexicon.TestReady;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CsvHelperFormatterTests
    {
        [TestMethod()]
        public async Task FormatAsync_SeveralRecords_Formatted()
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };
            var formatter = new CsvHelperFormatter(csvConfig);

            var csvString = await formatter.FormatAsync(WordSets.CzechMaleNames, default);

            var lines = csvString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(WordSets.CzechMaleNames.Count() + 1, lines.Length);
        }
    }
}
