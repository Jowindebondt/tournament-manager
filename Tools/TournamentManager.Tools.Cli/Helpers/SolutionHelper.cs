
namespace TournamentManager.Tools.Cli;

public static class SolutionHelper
{
    private static DirectoryInfo _mainDirectory;

    public static DirectoryInfo GetMainDirectory()
    {
        if (_mainDirectory == null)
        {
            var currentDirectory = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory);
            while (!currentDirectory.GetFiles("TournamentManager.Tools.sln", SearchOption.TopDirectoryOnly).Any())
            {
                currentDirectory = currentDirectory.Parent;
            }
            _mainDirectory = currentDirectory.Parent;
        }

        return _mainDirectory;
    }
}
