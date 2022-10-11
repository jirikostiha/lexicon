namespace Lexicon.Cli
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using Lexicon;
    using Lexicon.SqlLite;
    using Spectre.Console;

    /// <summary>
    /// Main command factory.
    /// </summary>
    public class RootCommandFactory
    {
        public const string Title = "Lexicon Cli";

        public RootCommand Create()
        {
            var fileOption = new Option<FileInfo?>(
            name: "--optionsFile",
            description: "The file of an app options.");

            var rootCommand = new RootCommand(Title);
            rootCommand.AddOption(fileOption);

            var deployCommand = new Command("deploy", "Deploy a database.")
            {
                fileOption,
            };
            rootCommand.AddCommand(deployCommand);

            deployCommand.SetHandler(async (file) =>
            {
                var deployer = new SQLiteDatabaseDeployer(null, null);
                await deployer.CreateDatabaseAsync();
                await deployer.FillAsync();
            },
                fileOption);

            return rootCommand;
        }
    }
}
