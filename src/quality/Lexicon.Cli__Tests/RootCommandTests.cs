namespace Lexicon.SQLite
{
    using Lexicon.Cli;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.CommandLine;
    using System.Threading.Tasks;

    [TestClass]
    public class RootCommandTests
    {
        [TestMethod]
        [TestCategory("positive")]
        public async Task InvokeAsync_CreateDb_Created()
        {
            var args = new[] { "createDb", "--sectionName", "SQLite-gr" };
            var command = RootCommandFactory.Create();

            await command.InvokeAsync(args);

            //db file created
        }
    }
}