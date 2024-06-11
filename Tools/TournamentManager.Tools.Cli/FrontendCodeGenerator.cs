using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace TournamentManager.Tools.Cli;

public class FrontendCodeGenerator
{
    public Command GetCommand()
    {
        var command = new Command("frontend");
        command.SetHandler(Execute);
        return command;
    }

    private void Execute(InvocationContext context)
    {
        BuildTournamentManagerSolution();
        Generate();
    }

    private void BuildTournamentManagerSolution()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo("dotnet", "build")
            {
                WorkingDirectory = @$"{SolutionHelper.GetMainDirectory().FullName}\Server",
            }
        };

        process.Start();
        process.WaitForExit();
    }

    private void Generate()
    {
        var domainDll = Assembly.LoadFile(@$"{SolutionHelper.GetMainDirectory().FullName}\Server\TournamentManager.Domain\bin\Debug\net8.0\TournamentManager.Domain.dll");
        var generatedModels = new List<string>();
        var generatedEnums = new List<string>();
        foreach (var type in domainDll.GetExportedTypes())
        {
            if (type.IsEnum)
            {
                var enumType = new EnumType(type);
                enumType.Load();
                enumType.Generate();
                generatedEnums.Add(type.Name.PascalToKebabCase());
            }
            else
            {
                var modelType = new ModelType(type);
                modelType.Load();
                modelType.Generate();
                generatedModels.Add(type.Name.PascalToKebabCase());
            }
        }

        GenerateModelExportFile(generatedModels);
        GenerateEnumExportFile(generatedEnums);
    }

    private static void GenerateEnumExportFile(List<string> generatedEnums)
    {
        var sb = new StringBuilder();
        sb.AddGeneratorInformation();
        foreach (var generated in generatedEnums)
        {
            sb.AppendLine($"export * from './generated/{generated}'");
        }
        File.WriteAllText(@$"{EnumType.OutputFolder}\generated-enums.ts", sb.ToString());
    }

    private static void GenerateModelExportFile(List<string> generatedModels)
    {
        var sb = new StringBuilder();
        sb.AddGeneratorInformation();
        foreach (var generated in generatedModels)
        {
            sb.AppendLine($"export * from './generated/{generated}'");
        }
        File.WriteAllText(@$"{ModelType.OutputFolder}\generated-models.ts", sb.ToString());
    }
}
