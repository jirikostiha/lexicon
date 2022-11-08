namespace Lexicon.Cli.Commands
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;
    using System.CommandLine.Invocation;

    public sealed class CreateDbCommand : Command
    {
        public CreateDbCommand()
            : base("createDb", "Create database.")
        {
            AddOption(new ConfigurationNameOption());
            AddOption(new SectionNameOption());
        }

        public class CommandHandler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                //var config = LoadConfiguration(configName);
                //var section = config.GetSection(sectionName);
                //var options = section.Get<SQLiteOptions>();

                //Console.WriteLine("Deploying model to target '{0}'", options.ConnectionString);

                //var modelDeployer = new SQLiteModelDeployer(DM.Sql, options)
                //{
                //    TablesOrderToDrop = DM.TableNames.ToArray()
                //};
                //await modelDeployer.DeployAsync(ct);
                return default;
            }

            public int Invoke(InvocationContext context) => throw new NotImplementedException();
        }
    }
}
