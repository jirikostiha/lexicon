namespace Lexicon.Cli
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using Spectre.Console;

    /// <summary>
    /// Main command factory.
    /// </summary>
    public class RootCommandFactory
    {
        public const string Title = "Lexicon Cli";

        public RootCommand Create()
        {
            var rootCommand = new RootCommand();
            rootCommand.Name = "lexicon";
            rootCommand.Description = "Lexicon Cli.";

            return rootCommand;
        }
    }
}
