using System.CommandLine;
using TournamentManager.Tools.Cli;

namespace TournamentManager.Tools.Generator;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Welcome to the commandline interface of the TournamentManager");

        var generateCommand = new Command("generate");
        generateCommand.AddCommand(new FrontendCodeGenerator().GetCommand());

        rootCommand.AddCommand(generateCommand);
        // return await rootCommand.InvokeAsync(args);
        return await rootCommand.InvokeAsync(new[]{"generate","frontend"});
    }
}