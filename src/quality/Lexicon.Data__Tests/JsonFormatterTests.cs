namespace Lexicon.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon.TestReady;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Text.RegularExpressions;
    using Lexicon.EntityModel;

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
