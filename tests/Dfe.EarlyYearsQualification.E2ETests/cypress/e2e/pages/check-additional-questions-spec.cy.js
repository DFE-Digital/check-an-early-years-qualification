describe("A spec that tests the check additional questions page", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks the check additional questions details are on the first question page", () => {
        cy.visit("/qualifications/check-additional-questions/eyq-240/1");
        
        cy.get("#back-button").should("have.attr", "href").and("include", "/qualifications");
        
        cy.get("#question").should("contain.text", "Test question");
        cy.get("#hint").should("contain.text", "This is the hint text");
        cy.get(".govuk-details__summary-text").should("contain.text", "This is the details heading");
        cy.get(".govuk-details__text").should("contain.text", "This is the details content");
        cy.get("Label[for='yes']").should("contain.text", "Yes");
        cy.get("Label[for='no']").should("contain.text", "No");
        
        cy.get("#additional-requirement-button").should("contain.text", "Get result");
    })

    it("Checks the check additional questions details are on the second question page", () => {
        cy.visit("/qualifications/check-additional-questions/eyq-240/2");

        cy.get("#back-button").should("have.attr", "href").and("include", "/previous");
        
        cy.get("#question").should("contain.text", "Test question 2");
        cy.get("#hint").should("contain.text", "This is the hint text");
        cy.get(".govuk-details__summary-text").should("contain.text", "This is the details heading");
        cy.get(".govuk-details__text").should("contain.text", "This is the details content");
        cy.get("Label[for='yes']").should("contain.text", "Yes");
        cy.get("Label[for='no']").should("contain.text", "No");
    })

    it("Shows errors if user does not select an option", () => {
        cy.visit("/qualifications/check-additional-questions/eyq-240/1");

        cy.get("#additional-requirement-button").click();

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There was a problem");
        cy.get("#error-banner-link").should("contain.text", "This is a test error message");

        cy.get("#option-error").should("be.visible");
        cy.get("#option-error").should("contain.text", "This is a test error message");
        cy.get(".govuk-form-group--error").should("be.visible");
    });
})