namespace Lexicon.Cli
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Lexicon;
    using Lexicon.SqlLite;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Main command factory.
    /// </summary>
    public static class RootCommandFactory
    {
        public const string Title = "Lexicon Cli";

        public static RootCommand Create()
        {
            var configFileOption = new Option<FileInfo?>(
            name: "--config",
            description: "The file of an app configuration.");

            var sourceDataFileOption = new Option<FileInfo?>(
            name: "--sourceFile",
            description: "The file of a source data.");

            var rootCommand = new RootCommand(Title);
            rootCommand.AddOption(configFileOption);
            rootCommand.AddOption(sourceDataFileOption);

            var deployCommand = new Command("deploy", "Deploy a database.")
            {
                configFileOption,
                sourceDataFileOption
            };
            rootCommand.AddCommand(deployCommand);

            deployCommand.SetHandler(async (configFile, sourceFile) =>
            {
                var config = LoadConfiguration();
                var sectionName = SQLiteOptions.Name;
                var section = config.GetSection(sectionName);
                var options = section.Get<SQLiteOptions>();

            },
                configFileOption, sourceDataFileOption);

            return rootCommand;
        }

        public static async Task DeploySqlDatabase(SQLiteOptions options, FileInfo csvData)
        {

            var deployer = new SQLiteDatabaseDeployer(options, null);
            await deployer.CreateDatabaseAsync();
            await deployer.FillAsync();
        }

        public static IConfiguration LoadConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

            return config;
        }

    }
}
