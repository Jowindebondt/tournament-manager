import { uuid } from '../support/app.po';

const apiTournaments = `${Cypress.env('APIURL')}/tournament`

describe('Tournament API', () => {
    it('validates CRUD', () => {
        const id = uuid();
        const tournamentName = `Tournament${id}`;

        const tournamentOrigin = {
            name: tournamentName
        };

        // Check current list
        cy.request('GET', `${apiTournaments}/GetList`).then((response) => {
            expect(response.status).to.be.oneOf([200, 204]);
            const tournamentListOrigin = response.body ?? [];
            expect(Cypress._.find(tournamentListOrigin, {name: tournamentName})).to.be.undefined;

            // Create new
            cy.request('POST', `${apiTournaments}/Create`, tournamentOrigin).then((response) => {
                expect(response.status).to.eq(200);
                expect(response.body).has.property('id');
                expect(response.body).has.property('name', tournamentName);
                
                const tournamentCreated = response.body;
                const tournamentId = tournamentCreated.id;

                // Get list with newly created
                cy.request('GET', `${apiTournaments}/GetList`).then((response) => {
                    expect(response.status).to.eq(200);
                    expect(Cypress._.find(response.body, {name: tournamentName})).to.be.not.null.and.to.be.not.undefined;

                    // Get newly created
                    cy.request('GET', `${apiTournaments}/GetById/${tournamentId}`).then((response) => {
                        expect(response.status).to.eq(200);
                        expect(response.body).has.property('name', tournamentCreated.name);

                        const newId = uuid();
                        tournamentCreated.name = `Tournament${newId}`;

                        // Update newly created
                        cy.request('PUT', `${apiTournaments}/Update/${tournamentId}`, tournamentCreated).then((response) => {
                            expect(response.status).to.eq(200);
                            expect(response.body).has.property('name', tournamentCreated.name);

                            // Remove newly created
                            cy.request('DELETE', `${apiTournaments}/Delete/${tournamentId}`).then((response) => {
                                expect(response.status).to.eq(200);

                                // Get list without newly created
                                cy.request('GET', `${apiTournaments}/GetList`).then((response) => {
                                    expect(response.status).to.be.oneOf([200, 204]);
                                    const tournamentListRemoved = response.body ?? [];
                                    expect(Cypress._.find(tournamentListRemoved, {name: tournamentCreated.name})).to.be.undefined;
                                    expect(Cypress._.find(tournamentListRemoved, {name: tournamentName})).to.be.undefined;
                                });
                            }); 
                        });
                    });
                });
            });
        });
    });
});