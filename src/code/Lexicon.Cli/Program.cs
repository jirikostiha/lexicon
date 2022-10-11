using System.Collections.Generic;
using System;
using System.CommandLine;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Lexicon.Cli;

class Program
{
    static int Main(string[] args)
    {
        try
        {
            var rootCommand = new RootCommandFactory().Create();

            return rootCommand.InvokeAsync(args).Result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ExitCode.GeneralError;
        }
    }
}
