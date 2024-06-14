﻿using TournamentManager.Domain;

namespace TournamentManager.Application;

public class Poule9PlayerTemplateService : BasePouleTemplateService
{
    public override Poule GetTemplate()
    {
        var players = GetPlayers(9);

        return new Poule {
            Name = "Default",
            Players = players.ToList(),
            Matches = [
                GetMatch(players, 2, 9),
                GetMatch(players, 3, 8),
                GetMatch(players, 4, 7),
                GetMatch(players, 5, 6),
                GetMatch(players, 1, 9),
                GetMatch(players, 2, 8),
                GetMatch(players, 3, 7),
                GetMatch(players, 4, 6),
                GetMatch(players, 1, 8),
                GetMatch(players, 2, 7),
                GetMatch(players, 3, 6),
                GetMatch(players, 4, 5),
                GetMatch(players, 1, 7),
                GetMatch(players, 2, 6),
                GetMatch(players, 3, 5),
                GetMatch(players, 8, 9),
                GetMatch(players, 1, 6),
                GetMatch(players, 2, 5),
                GetMatch(players, 4, 9),
                GetMatch(players, 7, 8),
                GetMatch(players, 1, 5),
                GetMatch(players, 3, 9),
                GetMatch(players, 4, 8),
                GetMatch(players, 6, 7),
                GetMatch(players, 5, 9),
                GetMatch(players, 6, 8),
                GetMatch(players, 1, 4),
                GetMatch(players, 2, 3),
                GetMatch(players, 5, 8),
                GetMatch(players, 7, 9),
                GetMatch(players, 1, 3),
                GetMatch(players, 2, 4),
                GetMatch(players, 6, 9),
                GetMatch(players, 5, 7),
                GetMatch(players, 3, 4),
                GetMatch(players, 1, 2),
            ]
        };
    }
}
