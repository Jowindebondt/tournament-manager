using TournamentManager.Domain;

namespace TournamentManager.Application;

public interface ITableTennisGenerator
{
    ITableTennisGenerator SetNext(ITableTennisGenerator nextGenerator);
    void Generate(Tournament tournament);
}
