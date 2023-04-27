namespace Lexicon.Data
{
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using System.Text.RegularExpressions;

    [TestClass]
    public class JsonFormatterTests
    {
        [TestMethod]
        [TestCategory("positive")]
        public async Task FormatAsync_SeveralRecords_Formatted()
        {
            var formatter = new JsonFormatter();

            var jsonString = await formatter.FormatAsync(WordSets.CzechMaleNames, default);

            Assert.AreEqual(WordSets.CzechMaleNames.Count(),
                Regex.Matches(jsonString, nameof(WordRecord.Word)).Count);
            Assert.AreEqual(WordSets.CzechMaleNames.Count(),
                Regex.Matches(jsonString, nameof(WordRecord.Metadata.Language)).Count);
            Assert.AreEqual(WordSets.CzechMaleNames.Count(),
                Regex.Matches(jsonString, nameof(WordRecord.Metadata.Class)).Count);
        }
    }
}