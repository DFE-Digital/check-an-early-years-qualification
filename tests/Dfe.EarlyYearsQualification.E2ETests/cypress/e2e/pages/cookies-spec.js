describe("A spec that tests the accessibility statement page", () => {
   
  // Mock details found in Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful. 
  it("Checks the content is present", () => {
      cy.visit("/cookies");


      cy.get("#cookies-heading").should("contain.text", "Test Cookies Heading");
      cy.get("#cookies-body").should("contain.text", "Test Cookies Body");

      cy.get("#test-option-value-1").should("exist");
      cy.get("#test-option-value-2").should("exist");
      
      cy.get("#cookies-button").should("contain.text", "Test Cookies Button");
  })
})