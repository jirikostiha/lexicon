using System.Collections.Generic;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Lexicon.Cli;

class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            var rootCommand = RootCommandFactory.Create();

            return await rootCommand.InvokeAsync(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ExitCode.GeneralError;
        }
    }
}
