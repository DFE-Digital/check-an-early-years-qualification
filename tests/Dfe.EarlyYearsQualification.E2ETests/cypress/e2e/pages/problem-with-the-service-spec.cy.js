describe("A spec used to test the not found page", () => {
    
    it("Checks the page contains the relevant components", () => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
        
        cy.visit("/error");

        cy.get("#problem-with-service-heading").should("contain.text", "Sorry, there is a problem with the service");
        cy.get("#problem-with-service-body").should("contain.text", "Please try again later.");
        cy.get("#problem-with-service-body").should("contain.text", "In the meantime, you can download the early years qualifications list (EYQL) spreadsheet. This document is regularly updated by DfE to add new or updated qualifications as they become approved. You should refresh the page before downloading the EYQL spreadsheet to be sure that you are using the latest version.");

        cy.get("#problem-with-service-link").should("have.attr", "href", "https://www.gov.uk/government/publications/early-years-qualifications-achieved-in-england");
    });
});