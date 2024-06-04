export const getGreeting = () => cy.get('h1');
export const uuid = () => Cypress._.random(0, 1e6);
