describe("A spec that tests the check additional questions page", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks the check additional questions details are on the page", () => {
        cy.visit("/qualifications/check-additional-questions/eyq-240");

        cy.get("#heading").should("contain.text", "Check the additional requirements");
        cy.get("#qualification-name-label").should("contain.text", "Qualification");
        cy.get("#qualification-name-value").should("contain.text", "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)");
        cy.get("#qualification-level-label").should("contain.text", "Qualification level");
        cy.get("#awarding-organisation-label").should("contain.text", "Awarding organisation");
        cy.get("#awarding-organisation-value").should("contain.text", "NCFE");
        cy.get("#question_0").should("contain.text", "Test question");
        cy.get("#question_0_hint").should("contain.text", "This is the hint text");
        cy.get("#question_0_details_heading").should("contain.text", "This is the details heading");
        cy.get("#question_0_details_content").should("contain.text", "This is the details content");
        cy.get("Label[for='yes_0_0']").should("contain.text", "Yes");
        cy.get("Label[for='no_0_1']").should("contain.text", "No");
        cy.get("#additional-requirement-warning").should("contain.text", "Your result is dependent on the accuracy of the answers you have provided");
        cy.get("#additional-requirement-button").should("contain.text", "Get result");
    })

    it("Shows errors if user does not select an option", () => {
        cy.visit("/qualifications/check-additional-questions/eyq-240");

        cy.get("#additional-requirement-button").click();

        cy.get("#question-choice-error").should("be.visible");
        cy.get("#question-choice-error").should("contain.text", "This is a test error message");

        cy.get(".govuk-form-group--error").should("be.visible");
    });
})