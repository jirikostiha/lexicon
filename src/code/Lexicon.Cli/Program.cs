using System.Collections.Generic;
using System;
using System.CommandLine;
using System.Threading.Tasks;
using Lexicon.Cli.Commands;
using System.CommandLine.Builder;

namespace Lexicon.Cli;

sealed class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            var rootCommand = CreateRootCommand();

            return await rootCommand.InvokeAsync(args);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled.");
            return ExitCode.Canceled;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ExitCode.GeneralError;
        }
    }

    private static CommandLineBuilder CommandLineBuilder => new(CreateRootCommand());

    private static RootCommand CreateRootCommand()
    {
        var rootCommand = new RootCommand("Lexicon Cli")
        {
            new CreateDbCommand(),
        };
        
        return rootCommand;
    }
}
