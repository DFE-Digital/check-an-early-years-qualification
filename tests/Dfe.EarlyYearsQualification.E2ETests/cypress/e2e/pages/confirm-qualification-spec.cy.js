describe("A spec that tests the confirm qualification page", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.

    it("Checks the static page content is on the page", () => {
        cy.visit("/confirm-qualification/eyq-240");

        cy.get("#heading").should("contain.text", "Test heading");
        cy.get("#qualification-name-row dt").should("contain.text", "Test qualification label");
        cy.get("#qualification-level-row dt").should("contain.text", "Test level label");
        cy.get("#qualification-org-row dt").should("contain.text", "Test awarding organisation label");
        cy.get("#qualification-date-added-row dt").should("contain.text", "Test date added label");
        cy.get("#radio-heading").should("contain.text", "Test radio heading");

        cy.get('input[value="yes"]').should("exist");
        cy.get('input[value="no"]').should("exist");
        
        cy.get('label[for="yes"]').should("contain.text", "yes");
        cy.get('label[for="no"]').should("contain.text", "no");
        
        cy.get("#confirm-qualification-button").should("contain.text", "Test button text");
        
        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#confirm-qualification-choice-error").should("not.exist");
    });
    
    it("Shows errors if user does not select an option", () => {
        cy.visit("/confirm-qualification/eyq-240");

        cy.get("#confirm-qualification-button").click();

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "Test error banner heading");
        cy.get("#error-banner-link").should("contain.text", "Test error banner link");
        
        cy.get("#confirm-qualification-choice-error").should("contain.text", "Test error text");
        cy.get(".govuk-form-group--error").should("be.visible");
    });
})