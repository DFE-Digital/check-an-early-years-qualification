describe("A spec used to test the qualification details page", () => {

  beforeEach(() => {
    cy.setCookie('auth-secret', Cypress.env('auth_secret'));
  })

  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
  it("Checks the qualification details are on the page", () => {
    cy.visit("/qualifications/qualification-details/eyq-240");

    cy.get("#qualification-name-value").should("contain.text", "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)");
    cy.get("#awarding-qualification-value").should("contain.text", "NCFE");
    cy.get("#qualification-level-value").should("contain.text", "3");
    cy.get("#qualification-number-value").should("contain.text", "603/5829/4");
    cy.get("#from-which-year-value").should("contain.text", "2020");
    cy.get("#additional-requirements-value").should("contain.text", "Additional notes");
  })
})