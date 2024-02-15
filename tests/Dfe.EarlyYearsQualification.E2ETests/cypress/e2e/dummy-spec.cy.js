describe("A dummy spec used to test Cypress setup", () => {

  beforeEach(() => {
    cy.visit("/");
  })

  it("Checks Crown copyright link text", () => {
    cy.get(".govuk-footer__copyright-logo").first().should("contain.text", "Crown copyright")
  })
})