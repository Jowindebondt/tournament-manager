import { uuid } from '../support/app.po';

const apiRounds = `${Cypress.env('APIURL')}/round`
const apiTournaments = `${Cypress.env('APIURL')}/tournament`

describe('Round API', () => {
    it('validates CRUD', () => {
        const tournament = {
            name: `Tournament${uuid()}`
        };

        cy.request('POST', `${apiTournaments}/Create`, tournament).then((response) => {
            const tournamentId = response.body.id;
            const id = uuid();
            const roundName = `Round${id}`;

            const roundOrigin = {
                tournamentId: tournamentId,
                name: roundName
            };

            // Check current list
            cy.request('GET', `${apiRounds}/GetList/${tournamentId}`).then((response) => {
                expect(response.status).to.be.oneOf([200, 204]);
                const roundListOrigin = response.body ?? [];
                expect(Cypress._.find(roundListOrigin, {name: roundName})).to.be.undefined;

                // Create new
                cy.request('POST', `${apiRounds}/Create`, roundOrigin).then((response) => {
                    expect(response.status).to.eq(200);
                    expect(response.body).has.property('id');
                    expect(response.body).has.property('name', roundName);
                    
                    const roundCreated = response.body;
                    const roundId = roundCreated.id;

                    // Get list with newly created
                    cy.request('GET', `${apiRounds}/GetList/${tournamentId}`).then((response) => {
                        expect(response.status).to.eq(200);
                        expect(Cypress._.find(response.body, {name: roundName})).to.be.not.null.and.to.be.not.undefined;

                        // Get newly created
                        cy.request('GET', `${apiRounds}/GetById/${roundId}`).then((response) => {
                            expect(response.status).to.eq(200);
                            expect(response.body).has.property('name', roundCreated.name);

                            const newId = uuid();
                            roundCreated.name = `Round${newId}`;

                            // Update newly created
                            cy.request('PUT', `${apiRounds}/Update/${roundId}`, roundCreated).then((response) => {
                                expect(response.status).to.eq(200);
                                expect(response.body).has.property('name', roundCreated.name);

                                // Remove newly created
                                cy.request('DELETE', `${apiRounds}/Delete/${roundId}`).then((response) => {
                                    expect(response.status).to.eq(200);

                                    // Get list without newly created
                                    cy.request('GET', `${apiRounds}/GetList/${tournamentId}`).then((response) => {
                                        expect(response.status).to.be.oneOf([200, 204]);
                                        const roundListRemoved = response.body ?? [];
                                        expect(Cypress._.find(roundListRemoved, {name: roundCreated.name})).to.be.undefined;
                                        expect(Cypress._.find(roundListRemoved, {name: roundName})).to.be.undefined;
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