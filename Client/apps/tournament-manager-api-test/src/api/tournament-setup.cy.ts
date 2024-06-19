import { uuid } from '../support/app.po';
import { Match, Member, Poule, Round, Sport, TableTennisHandicap, TableTennisRoundSettings, TableTennisSettings, TableTennisTournamentType, Tournament } from '@tournament-manager/tournament-manager-domain';

const apiTournaments = `${Cypress.env('APIURL')}/tournament`
const apiRounds = `${Cypress.env('APIURL')}/round`
const apiPoules = `${Cypress.env('APIURL')}/poule`
const apiMembers = `${Cypress.env('APIURL')}/member`
const apiMatches = `${Cypress.env('APIURL')}/match`
const apiGames = `${Cypress.env('APIURL')}/game`
const apiTableTennis = `${Cypress.env('APIURL')}/tabletennis`

describe('Dutch tabletennis tournament', () => {
    it('Sets up the round, poule, matches and games for a tournament', () => {
        // We create a tournament
        // We set the tournament settings:
        // - Dutch tabletennis (NTTB)
        // - No handicap
        // - 1-vs-1 matches (single)
        // Add 4 members to the tournament
        // Generate all matches and games according to NTTB format

        let tournament: Tournament = {
            name: `Tournament${uuid()}`,
            sport: Sport.TableTennis
        };

        cy.request('POST', `${apiTournaments}/Create`, tournament).then((response) => {
            tournament = response.body;

            let tournamentSettings: TableTennisSettings = {
                tournamentId: <number>tournament.id,
                handicap: TableTennisHandicap.None,
                tournamentType: TableTennisTournamentType.Single,
            };

            cy.request('POST', `${apiTableTennis}/SetTournamentSettings`, tournamentSettings).then(() => {
                let member1: Member = {
                    tournamentId: <number>tournament.id,
                    name: `Member${uuid()}`,
                    class: 0,
                    rating: 0
                };

                cy.request('POST', `${apiMembers}/Create`, member1).then((response) => {
                    member1 = response.body;

                    let member2: Member = {
                        tournamentId: <number>tournament.id,
                        name: `Member${uuid()}`,
                        class: 0,
                        rating: 0
                    };

                    cy.request('POST', `${apiMembers}/Create`, member2).then((response) => {
                        member2 = response.body;

                        let member3: Member = {
                            tournamentId: <number>tournament.id,
                            name: `Member${uuid()}`,
                            class: 0,
                            rating: 0
                        };
    
                        cy.request('POST', `${apiMembers}/Create`, member3).then((response) => {
                            member3 = response.body;

                            let member4: Member = {
                                tournamentId: <number>tournament.id,
                                name: `Member${uuid()}`,
                                class: 0,
                                rating: 0
                            };
        
                            cy.request('POST', `${apiMembers}/Create`, member4).then((response) => {
                                member4 = response.body;

                                cy.request('POST', `${apiTournaments}/Generate/${tournament.id}`).then(() => {
                                    cy.request('GET', `${apiRounds}/GetList/${tournament.id}`).then((response) => {
                                        const rounds = <Round[]>response.body;
                                        // check amount of rounds
                                        expect(rounds.length).to.eq(1);

                                        cy.request('GET', `${apiPoules}/GetList/${rounds[0].id}`).then((response) => {
                                            const poules = <Poule[]>response.body;
                                            // check amount of poules
                                            expect(poules.length).to.eq(1);

                                            cy.request('GET', `${apiMatches}/GetList/${poules[0].id}`).then((response) => {
                                                const matches = <Match[]>response.body;
                                                // check amount of matches
                                                expect(matches.length).to.eq(6);

                                                matches.forEach(match => {
                                                    cy.request('GET', `${apiGames}/GetList/${match.id}`).then((response) => {
                                                        // check amount of games
                                                        expect(response.body.length).to.eq(5);
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    });
    
    it('Sets up the poule, matches and games for a tournament', () => {
        // We create a tournament
        // We set the tournament settings:
        // - Dutch tabletennis (NTTB)
        // - No handicap
        // - 1-vs-1 matches (single)
        // Add 1 round to the tournament
        // We set the round settings:
        // - Best of 5 games (first 3 wins the match)
        // Add 4 members to the tournament
        // Generate all matches and games according to NTTB format

        let tournament: Tournament = {
            name: `Tournament${uuid()}`,
            sport: Sport.TableTennis
        };

        cy.request('POST', `${apiTournaments}/Create`, tournament).then((response) => {
            tournament = response.body;

            let tournamentSettings: TableTennisSettings = {
                tournamentId: <number>tournament.id,
                handicap: TableTennisHandicap.None,
                tournamentType: TableTennisTournamentType.Single,
            };

            cy.request('POST', `${apiTableTennis}/SetTournamentSettings`, tournamentSettings).then(() => {
                let round: Round = {
                    tournamentId: <number>tournament.id,
                    name: `Round${uuid()}`
                };

                cy.request('POST', `${apiRounds}/Create`, round).then((response) => {
                    round = response.body;

                    let roundSettings: TableTennisRoundSettings = {
                        roundId: <number>round.id,
                        bestOf: 5
                    };

                    cy.request('POST', `${apiTableTennis}/SetRoundSettings`, roundSettings).then(() => {

                        let member1: Member = {
                            tournamentId: <number>tournament.id,
                            name: `Member${uuid()}`,
                            class: 0,
                            rating: 0
                        };

                        cy.request('POST', `${apiMembers}/Create`, member1).then((response) => {
                            member1 = response.body;

                            let member2: Member = {
                                tournamentId: <number>tournament.id,
                                name: `Member${uuid()}`,
                                class: 0,
                                rating: 0
                            };

                            cy.request('POST', `${apiMembers}/Create`, member2).then((response) => {
                                member2 = response.body;

                                let member3: Member = {
                                    tournamentId: <number>tournament.id,
                                    name: `Member${uuid()}`,
                                    class: 0,
                                    rating: 0
                                };
            
                                cy.request('POST', `${apiMembers}/Create`, member3).then((response) => {
                                    member3 = response.body;
        
                                    let member4: Member = {
                                        tournamentId: <number>tournament.id,
                                        name: `Member${uuid()}`,
                                        class: 0,
                                        rating: 0
                                    };
                
                                    cy.request('POST', `${apiMembers}/Create`, member4).then((response) => {
                                        member4 = response.body;

                                        cy.request('POST', `${apiTournaments}/Generate/${tournament.id}`).then(() => {
                                            cy.request('GET', `${apiPoules}/GetList/${round.id}`).then((response) => {
                                                const poules = <Poule[]>response.body;
                                                // check amount of poules
                                                expect(poules.length).to.eq(1);

                                                cy.request('GET', `${apiMatches}/GetList/${poules[0].id}`).then((response) => {
                                                    const matches = <Match[]>response.body;
                                                    // check amount of matches
                                                    expect(matches.length).to.eq(6);

                                                    matches.forEach(match => {
                                                        cy.request('GET', `${apiGames}/GetList/${match.id}`).then((response) => {
                                                            // check amount of games
                                                            expect(response.body.length).to.eq(5);
                                                        });
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    });

    it('Sets up the matches and games for a tournament', () => {
        // We create a tournament
        // We set the tournament settings:
        // - Dutch tabletennis (NTTB)
        // - No handicap
        // - 1-vs-1 matches (single)
        // Add 1 round to the tournament
        // We set the round settings:
        // - Best of 5 games (first 3 wins the match)
        // Add 1 poule to the round
        // Add 4 members to the tournament
        // Assign 4 players to the poule
        // Generate all matches and games according to NTTB format

        let tournament: Tournament = {
            name: `Tournament${uuid()}`,
            sport: Sport.TableTennis
        };

        cy.request('POST', `${apiTournaments}/Create`, tournament).then((response) => {
            tournament = response.body;

            let tournamentSettings: TableTennisSettings = {
                tournamentId: <number>tournament.id,
                handicap: TableTennisHandicap.None,
                tournamentType: TableTennisTournamentType.Single,
            };

            cy.request('POST', `${apiTableTennis}/SetTournamentSettings`, tournamentSettings).then(() => {
                let round: Round = {
                    tournamentId: <number>tournament.id,
                    name: `Round${uuid()}`
                };

                cy.request('POST', `${apiRounds}/Create`, round).then((response) => {
                    round = response.body;

                    let roundSettings: TableTennisRoundSettings = {
                        roundId: <number>round.id,
                        bestOf: 5 
                    };

                    cy.request('POST', `${apiTableTennis}/SetRoundSettings`, roundSettings).then(() => {
                        let poule: Poule = {
                            roundId: <number>round.id,
                            name: `Poule${uuid()}`
                        };

                        cy.request('POST', `${apiPoules}/Create`, poule).then((response) => {
                            poule = response.body;

                            let member1: Member = {
                                tournamentId: <number>tournament.id,
                                name: `Member${uuid()}`,
                                class: 0,
                                rating: 0
                            };

                            cy.request('POST', `${apiMembers}/Create`, member1).then((response) => {
                                member1 = response.body;

                                let member2: Member = {
                                    tournamentId: <number>tournament.id,
                                    name: `Member${uuid()}`,
                                    class: 0,
                                    rating: 0
                                };

                                cy.request('POST', `${apiMembers}/Create`, member2).then((response) => {
                                    member2 = response.body;

                                    let member3: Member = {
                                        tournamentId: <number>tournament.id,
                                        name: `Member${uuid()}`,
                                        class: 0,
                                        rating: 0
                                    };
                
                                    cy.request('POST', `${apiMembers}/Create`, member3).then((response) => {
                                        member3 = response.body;
            
                                        let member4: Member = {
                                            tournamentId: <number>tournament.id,
                                            name: `Member${uuid()}`,
                                            class: 0,
                                            rating: 0
                                        };
                    
                                        cy.request('POST', `${apiMembers}/Create`, member4).then((response) => {
                                            member4 = response.body;

                                            cy.request('POST', `${apiPoules}/AddMembers/${poule.id}`, [member1.id, member2.id, member3.id, member4.id]).then(() => {
                                                cy.request('POST', `${apiTournaments}/Generate/${tournament.id}`).then(() => {
                                                    cy.request('GET', `${apiMatches}/GetList/${poule.id}`).then((response) => {
                                                        const matches = <Match[]>response.body;
                                                        // check amount of matches
                                                        expect(matches.length).to.eq(6);

                                                        matches.forEach(match => {
                                                            cy.request('GET', `${apiGames}/GetList/${match.id}`).then((response) => {
                                                                // check amount of games
                                                                expect(response.body.length).to.eq(5);
                                                            });
                                                        });
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    });
});