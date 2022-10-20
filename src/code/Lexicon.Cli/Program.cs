using System.Collections.Generic;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Lexicon.Cli;

sealed class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            var rootCommand = RootCommandFactory.Create();

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
}
