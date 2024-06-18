using TournamentManager.Domain;

namespace TournamentManager.Application;

public class TableTennisMatchGenerator : TableTennisBaseGenerator
{
    private readonly PouleTemplateResolver _templateResolver;
    private readonly IPouleService _pouleService;

    public TableTennisMatchGenerator(PouleTemplateResolver templateResolver, IPouleService pouleService)
    {
        _templateResolver = templateResolver;
        _pouleService = pouleService;
    }

    protected override bool CanHandle(Tournament tournament)
    {
        return tournament.Rounds.Any(u => u.Poules.Any(v => v.Matches.Count == 0));
    }

    protected override void Handle(Tournament tournament)
    {
        var poules = tournament.Rounds.SelectMany(u => u.Poules);
        foreach (var poule in poules)
        {
            if (poule.Matches.Count > 0)
            {
                continue;
            }
            
            var pouleTemplate = _templateResolver(poule.Players.Count).GetTemplate();
            var originalPlayers = poule.Players.ToList();
            poule.Players = [];
            for (var i = 0; i < pouleTemplate.Players.Count; i++)
            {
                var player = pouleTemplate.Players[i];
                player.Members = originalPlayers.ElementAt(i).Members;
                poule.Players.Add(player);
            }

            foreach (var match in pouleTemplate.Matches)
            {
                poule.Matches.Add(match);
            }

            _pouleService.Update(poule.Id.Value, poule);
        }
    }
}
