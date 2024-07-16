describe("A spec used to test the not found page", () => {
    
    it("Checks the page contains the relevant components", () => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
        
        cy.visit("/error");

        cy.get("#problem-with-service-heading").should("contain.text", "Sorry, there is a problem with the service");
        cy.get("#problem-with-service-body").should("contain.text", "Try again later.");
        cy.get("#problem-with-service-body").should("contain.text", "We have not saved your answers.");
        cy.get("#problem-with-service-body").should("contain.text", "When the service is available, you will have to start again.You can download the early years qualifications list (EYQL) spreadsheet.");

        cy.get("#problem-with-service-link").should("have.attr", "href", "https://www.gov.uk/government/publications/early-years-qualifications-achieved-in-england");
    });
});