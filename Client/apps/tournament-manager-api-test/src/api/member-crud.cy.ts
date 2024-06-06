import { uuid } from '../support/app.po';

const apiMembers = `${Cypress.env('APIURL')}/member`
const apiTournaments = `${Cypress.env('APIURL')}/tournament`

describe('Member API', () => {
    it('validates CRUD', () => {
        const tournament = {
            name: `Tournament${uuid()}`
        };

        cy.request('POST', `${apiTournaments}/Create`, tournament).then((response) => {
            const tournamentId = response.body.id;
            const id = uuid();
            const memberName = `Member${id}`;

            const memberOrigin = {
                tournamentId: tournamentId,
                name: memberName
            };

            // Check current list
            cy.request('GET', `${apiMembers}/GetList/${tournamentId}`).then((response) => {
                expect(response.status).to.be.oneOf([200, 204]);
                const memberListOrigin = response.body ?? [];
                expect(Cypress._.find(memberListOrigin, {name: memberName})).to.be.undefined;

                // Create new
                cy.request('POST', `${apiMembers}/Create`, memberOrigin).then((response) => {
                    expect(response.status).to.eq(200);
                    expect(response.body).has.property('id');
                    expect(response.body).has.property('name', memberName);
                    
                    const memberCreated = response.body;
                    const memberId = memberCreated.id;

                    // Get list with newly created
                    cy.request('GET', `${apiMembers}/GetList/${tournamentId}`).then((response) => {
                        expect(response.status).to.eq(200);
                        expect(Cypress._.find(response.body, {name: memberName})).to.be.not.null.and.to.be.not.undefined;

                        // Get newly created
                        cy.request('GET', `${apiMembers}/GetById/${memberId}`).then((response) => {
                            expect(response.status).to.eq(200);
                            expect(response.body).has.property('name', memberCreated.name);

                            const newId = uuid();
                            memberCreated.name = `Member${newId}`;

                            // Update newly created
                            cy.request('PUT', `${apiMembers}/Update/${memberId}`, memberCreated).then((response) => {
                                expect(response.status).to.eq(200);
                                expect(response.body).has.property('name', memberCreated.name);

                                // Remove newly created
                                cy.request('DELETE', `${apiMembers}/Delete/${memberId}`).then((response) => {
                                    expect(response.status).to.eq(200);

                                    // Get list without newly created
                                    cy.request('GET', `${apiMembers}/GetList/${tournamentId}`).then((response) => {
                                        expect(response.status).to.be.oneOf([200, 204]);
                                        const memberListRemoved = response.body ?? [];
                                        expect(Cypress._.find(memberListRemoved, {name: memberCreated.name})).to.be.undefined;
                                        expect(Cypress._.find(memberListRemoved, {name: memberName})).to.be.undefined;
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