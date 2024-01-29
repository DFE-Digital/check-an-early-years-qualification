describe("A dummy spec used to test Cypress setup", () => {

  beforeEach(() => {
    cy.visit("/");
  })

  it("opens on the example site", () => {
    cy.url().should("eq", "https://example.cypress.io/");
  })

  it("opens the utilities page", () => {
    cy.get("a[href='/utilities']").first().click();
    cy.url().should("contain", "/utilities");
  })
})