namespace Lexicon.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;
    using Lexicon.Cli.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
