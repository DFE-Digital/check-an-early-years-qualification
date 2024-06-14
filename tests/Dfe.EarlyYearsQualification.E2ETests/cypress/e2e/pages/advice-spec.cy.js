describe("A spec that tests advice pages", () => {
   
    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks the qualification details are on the page", () => {
        cy.visit("/advice/qualification-outside-the-united-kingdom");
        
        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved outside the United Kingdom");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })

    it("Checks the level 2 between 1 Sept 2014 and 31 Aug 2019 are on the page", () => {
        cy.visit("/advice/level-2-qualifications-started-between-1-sept-2014-&-31-aug-2019");
        
        cy.get("#advice-page-heading").should("contain.text", "Level 2 qualifications started between 1 September 2014 and 31 August 2019");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })
})