namespace Lexicon.Cli
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using CsvHelper;
    using Lexicon;
    using Lexicon.EntityModel;
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
            name: "--configFile",
            description: "The file of an app configuration.");
            configFileOption.AddAlias("-cf");

            var sourceDataFileOption = new Option<FileInfo?>(
            name: "--dataFile",
            description: "The file of a source data.");
            sourceDataFileOption.AddAlias("-df");

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

                await DeploySqlDatabase(options, sourceFile ?? new FileInfo("data.csv"), default);

            },
                configFileOption, sourceDataFileOption);

            return rootCommand;
        }

        public static async Task DeploySqlDatabase(SQLiteOptions options, FileInfo csvDataFile, CancellationToken ct)
        {
            var records = new List<WordRecord>();
            using (var reader = new StreamReader(csvDataFile.FullName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = await csv.GetRecordsAsync<WordRecord>(ct)
                    .ToListAsync(ct)
                    .ConfigureAwait(false);
            }

            var deployer = new SQLiteDatabaseDeployer(options, () => records);
            await deployer.CreateDatabaseAsync(ct);
            await deployer.FillAsync(ct);
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
