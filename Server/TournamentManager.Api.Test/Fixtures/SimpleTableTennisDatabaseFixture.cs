using TournamentManager.Domain;
using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimpleTableTennisDatabaseFixture : DatabaseFixture
{
    public SimpleTableTennisDatabaseFixture() : base(nameof(SimpleTableTennisDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.RoundSettings.RemoveRange(DbContext.RoundSettings);
        DbContext.Rounds.RemoveRange(DbContext.Rounds);
        DbContext.TableTennisSettings.RemoveRange(DbContext.TableTennisSettings);
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
    }

    protected override void FillContext()
    {
        for (var i = 0; i < 10; i++)
        {
            var tournament = TournamentBuilder.GetSingleTournament(i + 1);
            DbContext.Tournaments.Add(tournament);
            var settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>(i + 1, i + 1);
            DbContext.TableTennisSettings.Add(settings);

            var round = RoundBuilder.GetSingleRound(i + 1, i + 1);
            DbContext.Rounds.Add(round);
            var roundSettings = RoundSettingsBuilder.GetSingleRoundSettings<TableTennisRoundSettings>(i + 1, i + 1);
            DbContext.RoundSettings.Add(roundSettings);
        }
    }
}
