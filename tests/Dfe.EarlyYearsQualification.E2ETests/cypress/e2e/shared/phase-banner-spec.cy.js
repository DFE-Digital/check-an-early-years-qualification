describe("A spec that tests the phase banner is showing on all pages", () => {
   
  // Mock details found in Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful.

  var pages = [
    "/",
    "/accessibility-statement",
    "/advice/qualification-outside-the-united-kingdom",
    "/cookies",
    "/qualifications/qualification-details/eyq-240",
    "/questions/where-was-the-qualification-awarded"
  ]

  pages.forEach((option) => {
    it(`Checks that the phase banner is present at the URL: ${option}`, () => {

      cy.visit(option);
      
      cy.get(".govuk-phase-banner").should("be.visible");

      cy.get(".govuk-phase-banner__content__tag").should("contain.text", "Test phase banner name");
      cy.get(".govuk-phase-banner__text").should("contain.text", "Test phase banner content");
    });
  });
})