describe("A spec that tests question pages", () => {
   
    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 
    it("Checks the qualification details are on the page", () => {
        cy.visit("/questions/where-was-the-qualification-awarded");

        cy.get("#question").should("contain.text", "Where was the qualification awarded?");
        cy.get("#england").should("exist");
        cy.get("#outside-uk").should("exist");
    })

    it("shows an error message when a user doesnt select an option", () => {
        cy.visit("/questions/where-was-the-qualification-awarded");

        cy.get("#option-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
          expect(loc.pathname).to.eq("/questions/where-was-the-qualification-awarded");
        })

        cy.get('#option-error').should("exist");
        cy.get('#option-error').should("contain.text", "Test error message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
    })
})