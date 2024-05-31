
const apiTournaments = `${Cypress.env('apiUrl')}/tournament`

describe('Tournament API', () => {
    context('GET /GetList', () => {
        it('gets a list of tournaments', () => {
            cy.request('GET', `${apiTournaments}/GetList`).then((response) => {
                expect(response.status).to.eq(200);
                expect(response.body.results.length).to.eq(0);
            });
        });
    });
});