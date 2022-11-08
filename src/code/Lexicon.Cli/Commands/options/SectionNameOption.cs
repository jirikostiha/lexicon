namespace Lexicon.Cli.Commands
{
    using System;
    using System.CommandLine;

    public sealed class SectionNameOption : Option<string>
    {
        public SectionNameOption()
            : base(
                  name: "--sectionName", 
                  description: "Configuration section name in configuration file e.g. 'SQLite'.")
        {
            AddAlias("-s");
        }
    }
}
