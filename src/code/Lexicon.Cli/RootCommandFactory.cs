namespace Lexicon.Cli
{
    using CsvHelper;
    using Lexicon.EntityModel;
    using Lexicon.SQLite;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Root command factory.
    /// </summary>
    public static class RootCommandFactory
    {
        public const string Title = "Lexicon Cli";

        public static RootCommand Create()
        {
            var configurationNameOption = new Option<string?>(
            name: "--configName",
            description: "Configuration name e.g. 'Production' or 'Development' or other.")
            {
                IsRequired = false,
            };
            configurationNameOption.AddAlias("-cn");

            var sectionNameOption = new Option<string?>(
            name: "--sectionName",
            description: "Configuration section name in configuration file e.g. 'SQLite'.")
            {
                IsRequired = false
            };
            configurationNameOption.AddAlias("-s");

            var sourceDataFileOption = new Option<FileInfo>(
            name: "--dataFile",
            description: "The file of a source data.")
            {
                IsRequired = true
            };
            sourceDataFileOption.AddAlias("-df");

            var rootCommand = new RootCommand(Title);

            var createDbCommand = new Command("createDb", "Create database.")
            {
                configurationNameOption, sectionNameOption
            };
            createDbCommand.SetHandler(async (configName, sectionName) =>
            {
                await DeployModel(configName!, sectionName!)
                    .ConfigureAwait(false);
            },
                configurationNameOption, sectionNameOption);
            rootCommand.AddCommand(createDbCommand);

            var importCommand = new Command("import", "Import data to database.")
            {
                configurationNameOption, sectionNameOption, sourceDataFileOption
            };
            importCommand.SetHandler(async (configName, sectionName, sourceFile) =>
            {
                await ImportDataToDatabase(configName!, sectionName!, sourceFile)
                    .ConfigureAwait(false);
            },
                configurationNameOption, sectionNameOption, sourceDataFileOption);
            rootCommand.AddCommand(importCommand);

            return rootCommand;
        }

        public static async Task DeployModel(string configName, string sectionName, CancellationToken ct = default)
        {
            var config = LoadConfiguration(configName);
            var section = config.GetRequiredSection(sectionName);
            var options = section.Get<SQLiteOptions>();

            Guard.IsNotNull(options);

            Console.WriteLine("Deploying model to target '{0}'", options.ConnectionString);

            var modelDeployer = new SQLiteModelDeployer(DM.Sql, options)
            {
                TablesOrderToDrop = DM.TableNames.ToArray()
            };
            await modelDeployer.DeployAsync(ct);
        }

        public static async Task ImportDataToDatabase(string configName, string sectionName, FileInfo sourceFile, CancellationToken ct = default)
        {
            var config = LoadConfiguration(configName);
            var section = config.GetSection(sectionName);
            var options = section.Get<SQLiteOptions>();

            Guard.IsNotNull(options);

            await ImportDataToDatabase(options, sourceFile, ct);
        }

        public static async Task ImportDataToDatabase(SQLiteOptions options, FileInfo csvDataFile, CancellationToken ct = default)
        {
            Console.WriteLine("Importing data from '{0}' to target '{1}'", csvDataFile.FullName, options.ConnectionString);

            var records = new List<WordRecord>();
            using (var reader = new StreamReader(csvDataFile.FullName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = await csv.GetRecordsAsync<WordRecord>(ct)
                    .ToListAsync(ct)
                    .ConfigureAwait(false);
            }

            var repo = new SQLiteWordRepository(options);
            await repo.SaveAllAsync(records, ct);
        }

        public static IConfiguration LoadConfiguration(string? configurationName)
        {
            var configName = configurationName
                ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Production";

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{configName}.json", optional: true)
                .Build();

            return config;
        }
    }
}