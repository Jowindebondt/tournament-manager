using TournamentManager.Domain;

namespace TournamentManager.Application;

public class TableTennisPouleGenerator : TableTennisBaseGenerator
{
    private readonly PouleTemplateResolver _templateResolver;
    private readonly IPouleService _pouleService;

    public TableTennisPouleGenerator(PouleTemplateResolver templateResolver, IPouleService pouleService)
    {
        _templateResolver = templateResolver;
        _pouleService = pouleService;
    }

    protected override bool CanHandle(Tournament tournament)
    {
        return tournament.Rounds.Count == 1 && tournament.Rounds.First().Poules.Count == 0;
    }

    protected override void Handle(Tournament tournament)
    {
        var tournamentSettings = tournament.Settings as TableTennisSettings;
        
        var totalPlayers = tournamentSettings.TournamentType == TableTennisTournamentType.Single
            ? tournament.Members.Count
            : tournament.Members.Count / 2;
        
        var pouleTemplate = _templateResolver(totalPlayers).GetTemplate();
        pouleTemplate.RoundId = tournament.Rounds.First().Id.Value;

        if (tournamentSettings.TournamentType == TableTennisTournamentType.Single)
        {
            SetMembersForSingle(tournament, pouleTemplate);
        }
        else {
            SetMembersForDouble(tournament, pouleTemplate);
        }

        _pouleService.Insert(pouleTemplate);
    }

    private void SetMembersForSingle(Tournament tournament, Poule pouleTemplate)
    {
        var members = tournament.Members.OrderBy(u => u.Rating);
        for (var i = 0; i < pouleTemplate.Players.Count; i++)
        {
            pouleTemplate.Players[i].Members = [members.ElementAt(i)];
        }
    }

    private void SetMembersForDouble(Tournament tournament, Poule pouleTemplate)
    {
        var members = tournament.Members.OrderBy(u => u.Rating);
        var topMembers = members.SkipLast(members.Count() / 2).ToList();
        var bottomMembers = members.Skip(members.Count() / 2).ToList();

        var rand = new Random((int)DateTime.Now.Ticks);
        for (var i = 0; i < pouleTemplate.Players.Count; i++)
        {
            var topMember = topMembers.ElementAt(rand.Next(topMembers.Count));
            var bottomMember = bottomMembers.ElementAt(rand.Next(bottomMembers.Count));

            pouleTemplate.Players[i].Members = [
                topMember,
                bottomMember
            ];

            topMembers.Remove(topMember);
            bottomMembers.Remove(bottomMember);
        }
    }
}
