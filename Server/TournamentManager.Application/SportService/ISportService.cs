using TournamentManager.Domain;

namespace TournamentManager.Application;

public interface ISportService
{
    void Generate(Tournament tournament);
}
