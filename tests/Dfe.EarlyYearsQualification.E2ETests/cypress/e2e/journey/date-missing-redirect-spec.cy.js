import { pagesThatRedirectIfDateMissing } from "../shared/urls-to-check";

describe('A spec used to check that if the user skips entering the date of the qual, then they are redirected back to the date selection page', () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    });
    
    pagesThatRedirectIfDateMissing.forEach((page) => {
        it(`navigating to ${page} should redirect the user to the date selection page`, () => {
            
            cy.visit(page);

            cy.location().should((loc) => {
                expect(loc.pathname).to.eq('/questions/when-was-the-qualification-started');
            })
            
        })
    })
})