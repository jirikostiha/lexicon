namespace Lexicon.Cli.Commands
{
    using System;
    using System.CommandLine;

    public sealed class ConfigurationNameOption : Option<string>
    {
        public ConfigurationNameOption() 
            : base(
                  name: "--configName", 
                  description: "Configuration name e.g. 'Production' or 'Development' or other.")
        {
            AddAlias("-cn");
        }
    }
}
