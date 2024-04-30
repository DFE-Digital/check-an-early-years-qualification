describe("A spec that tests the accessibility statement page", () => {
   
  // Mock details found in Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful. 
  it("Checks the heading and content are present", () => {
      cy.visit("/accessibility-statement");


      cy.get("#accessibility-statement-heading").should("contain.text", "Test Accessibility Statement Heading");
      cy.get("#accessibility-statement-body").should("contain.text", "Test Accessibility Statement Body");
  })
})