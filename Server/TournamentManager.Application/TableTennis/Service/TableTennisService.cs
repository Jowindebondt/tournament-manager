using TournamentManager.Domain;

namespace TournamentManager.Application;

public class TableTennisService : ISportService
{
    private ITableTennisGenerator _generator;

    public TableTennisService(TableTennisRoundGenerator roundGenerator, TableTennisPouleGenerator pouleGenerator, 
        TableTennisMatchGenerator matchGenerator, TableTennisGameGenerator gameGenerator)
    {
        _generator = roundGenerator
            .SetNext(pouleGenerator
                .SetNext(matchGenerator
                    .SetNext(gameGenerator
                )
            )
        );
    }

    public void Generate(Tournament tournament)
    {
        _generator.Generate(tournament);
    }
}
