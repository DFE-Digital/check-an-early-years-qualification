describe("A spec that tests advice pages", () => {
   
    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks the qualification details are on the page", () => {
        cy.visit("/advice/qualification-outside-the-united-kingdom");


        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved outside the United Kingdom");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })
})