describe("A spec that tests the cookies page", () => {
  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 

  beforeEach(() => {
    cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    cy.visit("/cookies");
  });

  it("Checks the content is present", () => {
    cy.get("#cookies-set-banner").should("not.exist");

    cy.get("#cookies-heading").should("contain.text", "Test Cookies Heading");
    cy.get("#cookies-body").should("contain.text", "Test Cookies Page Body");

    cy.get("#cookies-form-heading").should("contain.text", "Test Form Heading");
    
    cy.get("#test-option-value-1").should("exist");
    cy.get("#test-option-value-2").should("exist");

    cy.get("label[for='test-option-value-1']").should("contain.text", "Test Option Label 1");
    cy.get("label[for='test-option-value-2']").should("contain.text", "Test Option Label 2");

    cy.get("#cookies-choice-error")
      .should("not.be.visible")
      .and("contain.text", "Test Error Text");

    cy.get('button[id="cookies-button"]').should("contain.text", "Test Cookies Button");
  });

  describe("Check the functionality of the page", () => {
    it("Checks that the radio button validation is working", () => {
      cy.get('button[id="cookies-button"]').click();

      cy.get("#cookies-set-banner").should("not.exist");

      cy.get("#cookies-choice-error").should("be.visible");
    });

    ["test-option-value-1", "test-option-value-2"].forEach((option) => {
      it(`Checks that selecting ${option} reveals success banner`, () => {

        cy.get(`#${option}`).click();

        cy.get('button[id="cookies-button"]').click();

        cy.get("#cookies-set-banner").should("be.visible");

        // Seen as the success banner doesn't exist in the rendered HTML by default; we have to check the content once we expect the heading to be visible.
        cy.get("#cookies-set-banner-heading").should("contain.text", "Test Success Banner Heading");
        cy.get("#cookies-set-banner-content").should("contain.text", "Test Success Banner Content");

        cy.get("#cookies-choice-error").should("not.be.visible");
      });
    });
  });
});
