using System.Collections.Generic;
using System;
using System.CommandLine;

namespace Lexicon.Cli;

class Program
{
    static int Main(string[] args)
    {
        var rootCommand = new RootCommandFactory().Create();

        return rootCommand.InvokeAsync(args).Result;
    }
}
