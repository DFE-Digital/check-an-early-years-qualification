describe("A spec that tests question pages", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 

    /// Where was the qualification awarded page tests
    it("Checks the content on where-was-the-qualification-awarded page", () => {
        cy.visit("/questions/where-was-the-qualification-awarded");

        cy.get("#question").should("contain.text", "Where was the qualification awarded?");
        cy.get("#england").should("exist");
        cy.get("#outside-uk").should("exist");
    })

    it("shows an error message when a user doesnt select an option on the where-was-the-qualification-awarded page", () => {
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

    /// When was the qualification awarded page tests
    it("Checks the content on when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get("#question").should("contain.text", "Test Date Question");

        cy.get("#date-started-month").should("exist");
        cy.get("#date-started-year").should("exist");
    })

    it("shows an error message when a user doesnt type a date on the when-was-the-qualification-started page", () => {
      cy.visit("/questions/when-was-the-qualification-started");

      cy.get("#date-error").should("not.exist");
      cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

      cy.get('button[id="question-submit"]').click();
      cy.location().should((loc) => {
        expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
      })

      cy.get('#date-error').should("exist");
      cy.get('#date-error').should("contain.text", "Test Error Message");
      cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
  })

    /// What level is the qualification page
    it("Checks the content on what-level-is-the-qualification page", () => {
        cy.visit("/questions/what-level-is-the-qualification");

        cy.get("#question").should("contain.text", "What level is the qualification?");
        cy.get("#2").should("exist");
        cy.get("#3").should("exist");
    })

    it("shows an error message when a user doesnt select an option on the what-level-is-the-qualification page", () => {
        cy.visit("/questions/what-level-is-the-qualification");

        cy.get("#option-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/what-level-is-the-qualification");
        })

        cy.get('#option-error').should("exist");
        cy.get('#option-error').should("contain.text", "Test error message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
    })
    
    /// What is the awarding organisation page
    it("Checks the content on what-is-the-awarding-organisation page", () => {
        cy.visit("/questions/what-is-the-awarding-organisation");

        cy.get("#question").should("contain.text", "Test Dropdown Question");
        cy.get("#awarding-organisation-select").should("exist");
        cy.get("#awarding-organisation-not-in-list").should("exist");
        cy.get('button[id="question-submit"]').should("exist");
    })

    it("shows an error message when a user doesnt select an option from the dropdown list" +
        "and also does not check 'not in the list' on the what-is-the-awarding-organisation", () => {
        cy.visit("/questions/what-is-the-awarding-organisation");

        cy.get("#dropdown-error").should("not.exist");
        cy.get("#awarding-organisation-select").should("not.have.class", "govuk-select--error");

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/what-is-the-awarding-organisation");
        })

        cy.get('#dropdown-error').should("exist");
        cy.get('#dropdown-error').should("contain.text", "Test Error Message");
        cy.get("#awarding-organisation-select").should("have.class", "govuk-select--error");
    })
})