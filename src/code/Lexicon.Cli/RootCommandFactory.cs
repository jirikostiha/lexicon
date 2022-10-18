namespace Lexicon.Cli
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using CsvHelper;
    using Lexicon.EntityModel;
    using Lexicon.SQLite;
    using Microsoft.Extensions.Configuration;
    using System.Diagnostics;

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

            //var configurationFileOption = new Option<FileInfo?>(
            //name: "--configFile",
            //description: "Configuration file.");
            //configurationFileOption.AddAlias("-cf");

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
                await CreateDatabase(configName, sectionName);
            }, 
                configurationNameOption, sectionNameOption);
            rootCommand.AddCommand(createDbCommand);

            var importCommand = new Command("import", "Import data to database.")
            {
                configurationNameOption, sectionNameOption, sourceDataFileOption
            };
            importCommand.SetHandler(async (configName, sectionName, sourceFile) =>
            {
                await ImportDataToDatabase(configName, sectionName, sourceFile);
            },
                configurationNameOption, sectionNameOption, sourceDataFileOption);
            rootCommand.AddCommand(importCommand);

            return rootCommand;
        }

        public static async Task CreateDatabase(string? configName, string? sectionName, CancellationToken ct = default)
        {
            var config = LoadConfiguration(configName);
            var section = config.GetSection(sectionName);
            var options = section.Get<SQLiteOptions>();

            Console.WriteLine("creating db: '{0}'", options.ConnectionString);

            var deployer = new SQLiteDatabaseDeployer(options);
            await deployer.CreateDatabaseAsync(ct);

            //Console.WriteLine("db file: '{0}'", dbFile);
        }

        public static async Task ImportDataToDatabase(string? configName, string? sectionName, FileInfo sourceFile, CancellationToken ct = default)
        {
            var config = LoadConfiguration(configName);
            var section = config.GetSection(sectionName);
            var options = section.Get<SQLiteOptions>();

            await ImportDataToDatabase(options, sourceFile, ct);
        }

        public static async Task ImportDataToDatabase(SQLiteOptions options, FileInfo csvDataFile, CancellationToken ct = default)
        {
            Console.WriteLine("Importing data from '{0}'", csvDataFile.FullName);
            
            var records = new List<WordRecord>();
            using (var reader = new StreamReader(csvDataFile.FullName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = await csv.GetRecordsAsync<WordRecord>(ct)
                    .ToListAsync(ct)
                    .ConfigureAwait(false);
            }

            var deployer = new SQLiteDataModelDeployer(options.ConnectionString);
            await deployer.DeployAsync(ct);
            var repo = new SQLiteWordRepository(options);
            await repo.SaveAllAsync(records, ct);
            Console.WriteLine("Imported to: '{0}'", options.ConnectionString);
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
