namespace Lexicon.Data
{
    using System.Globalization;
    using CsvHelper.Configuration;
    using Lexicon.TestReady;

    [TestClass]
    public class CsvHelperFormatterTests
    {
        [TestMethod]
        [TestCategory("positive")]
        public async Task FormatAsync_SeveralRecords_Formatted()
        {
            var options = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };
            var formatter = new CsvHelperFormatter(options);

            var csvString = await formatter.FormatAsync(WordSets.CzechMaleNames, default);

            var lines = csvString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(WordSets.CzechMaleNames.Count() + 1, lines.Length);
        }
    }
}
