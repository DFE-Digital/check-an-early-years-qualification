describe("A spec used to test the home page", () => {

  beforeEach(() => {
    cy.visit("/");
  })

  // Mock details found in Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful. 
  it("Checks the page contains the relevant components", () => {
    cy.get(".govuk-heading-xl").should("contain.text", "Test Header");
    cy.get("#pre-cta-content").should("contain.text", "This is the pre cta content");
    cy.get(".govuk-button--start").should("contain.text", "Start Button Text");
    cy.get("#post-cta-content").should("contain.text", "This is the post cta content");
    cy.get("#subsection-title").should("contain.text", "Related content");
    cy.get("#right-hand-content").should("contain.text", "This is the right hand content");
  })

  it("Checks Crown copyright link text", () => {
    cy.get(".govuk-footer__copyright-logo").first().should("contain.text", "Crown copyright")
  })
})