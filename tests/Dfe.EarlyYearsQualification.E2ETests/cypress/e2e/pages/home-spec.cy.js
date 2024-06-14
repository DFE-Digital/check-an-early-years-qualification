describe("A spec used to test the home page", () => {

  beforeEach(() => {
    cy.setCookie('auth-secret', Cypress.env('auth_secret'));
  })

  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 
  it("Checks the page contains the relevant components", () => {
    cy.visit("/");

    cy.get(".govuk-heading-xl").should("contain.text", "Test Header");
    cy.get("#pre-cta-content p").should("contain.text", "This is the pre cta content");
    cy.get(".govuk-button--start").should("contain.text", "Start Button Text");
    cy.get("#post-cta-content p").should("contain.text", "This is the post cta content");
    cy.get("#right-hand-content-header").should("contain.text", "Related content");
    cy.get("#right-hand-content p").should("contain.text", "This is the right hand content");
  })

  it("Checks Crown copyright link text", () => {
    cy.visit("/");

    cy.get(".govuk-footer__copyright-logo").first().should("contain.text", "Crown copyright")
  })
})