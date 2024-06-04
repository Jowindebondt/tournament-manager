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
            cy.log(`############## GetList body: ${response.body}`);
            
            // Create new
            cy.request('POST', `${apiTournaments}/Create`, tournamentOrigin).then((response) => {
                expect(response.status).to.eq(200);
                cy.log(`############## Create body: ${response.body}`);
                expect(response.body).has.property('id');
                expect(response.body).has.property('name', tournamentName);
                
                const tournamentCreated = response.body;
                const tournamentId = tournamentCreated.id;

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
                        });
                    });
                });
            });
        });
    });
});