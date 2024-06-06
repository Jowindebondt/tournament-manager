import { uuid } from '../support/app.po';

const apiPoules = `${Cypress.env('APIURL')}/poule`
const apiRounds = `${Cypress.env('APIURL')}/round`
const apiTournaments = `${Cypress.env('APIURL')}/tournament`

describe('Poule API', () => {
    it('validates CRUD', () => {
        const tournament = {
            name: `Tournament${uuid()}`
        };

        cy.request('POST', `${apiTournaments}/Create`, tournament).then((response) => {
            const tournamentId = response.body.id;
            const round = {
                tournamentId: tournamentId,
                name: `Round${uuid()}`
            };
        
            cy.request('POST', `${apiRounds}/Create`, round).then((response) => {
                const roundId = response.body.id;
                const id = uuid();
                const pouleName = `Poule${id}`;

                const pouleOrigin = {
                    roundId: roundId,
                    name: pouleName
                };

                // Check current list
                cy.request('GET', `${apiPoules}/GetList/${roundId}`).then((response) => {
                    expect(response.status).to.be.oneOf([200, 204]);
                    const pouleListOrigin = response.body ?? [];
                    expect(Cypress._.find(pouleListOrigin, {name: pouleName})).to.be.undefined;

                    // Create new
                    cy.request('POST', `${apiPoules}/Create`, pouleOrigin).then((response) => {
                        expect(response.status).to.eq(200);
                        expect(response.body).has.property('id');
                        expect(response.body).has.property('name', pouleName);
                        
                        const pouleCreated = response.body;
                        const pouleId = pouleCreated.id;

                        // Get list with newly created
                        cy.request('GET', `${apiPoules}/GetList/${roundId}`).then((response) => {
                            expect(response.status).to.eq(200);
                            expect(Cypress._.find(response.body, {name: pouleName})).to.be.not.null.and.to.be.not.undefined;

                            // Get newly created
                            cy.request('GET', `${apiPoules}/GetById/${pouleId}`).then((response) => {
                                expect(response.status).to.eq(200);
                                expect(response.body).has.property('name', pouleCreated.name);

                                const newId = uuid();
                                pouleCreated.name = `Poule${newId}`;

                                // Update newly created
                                cy.request('PUT', `${apiPoules}/Update/${pouleId}`, pouleCreated).then((response) => {
                                    expect(response.status).to.eq(200);
                                    expect(response.body).has.property('name', pouleCreated.name);

                                    // Remove newly created
                                    cy.request('DELETE', `${apiPoules}/Delete/${pouleId}`).then((response) => {
                                        expect(response.status).to.eq(200);

                                        // Get list without newly created
                                        cy.request('GET', `${apiPoules}/GetList/${roundId}`).then((response) => {
                                            expect(response.status).to.be.oneOf([200, 204]);
                                            const pouleListRemoved = response.body ?? [];
                                            expect(Cypress._.find(pouleListRemoved, {name: pouleCreated.name})).to.be.undefined;
                                            expect(Cypress._.find(pouleListRemoved, {name: pouleName})).to.be.undefined;
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