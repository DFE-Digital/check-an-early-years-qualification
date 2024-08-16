describe("A spec used to test the not found page", () => {

    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    });
    
    it("Checks the page contains the relevant components", () => {
        cy.visit({
            url: "/error/404",
            failOnStatusCode: false,
        });

        cy.get("#page-not-found-heading").should("contain.text", "Page not found");
        cy.get("#page-not-found-statement-body").should("contain.text", "If you typed out the web address, check it is correct.");
        cy.get("#page-not-found-statement-body").should("contain.text", "If you pasted the web address, check you copied the entire address.");
        cy.get("#page-not-found-statement-body").should("contain.text", "If the web address is correct or you selected a link or button, contact the check an early years qualification team to report a fault with the service.");
        
        cy.get("#page-not-found-link").should("have.attr", "href", "#");
        
    });

    it("Check that visiting a URL that doesn't exist shows this page without altering the URL", () => {
        cy.visit({
            url: "/does-not-exist",
            failOnStatusCode: false,
        });
        
        cy.url().should("include", "/does-not-exist");

        cy.get("#page-not-found-heading").should("contain.text", "Page not found");
    });
});