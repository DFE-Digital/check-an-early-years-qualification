describe("A spec that tests the accessibility statement page", () => {
  beforeEach(() => {
    cy.setCookie('auth-secret', Cypress.env('auth_secret'));
  })

  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 
  it("Checks the heading and content are present", () => {
      cy.visit("/accessibility-statement");


      cy.get("#accessibility-statement-heading").should("contain.text", "Test Accessibility Statement Heading");
      cy.get("#accessibility-statement-body").should("contain.text", "Test Accessibility Statement Body");
  })
})