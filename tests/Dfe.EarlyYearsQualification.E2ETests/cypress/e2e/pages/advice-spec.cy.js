describe("A spec that tests advice pages", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks the Qualifications achieved outside the United Kingdom details are on the page", () => {
        cy.visit("/advice/qualification-outside-the-united-kingdom");
        
        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved outside the United Kingdom");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })

    it("Checks the level 2 between 1 Sept 2014 and 31 Aug 2019 details are on the page", () => {
        cy.visit("/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        
        cy.get("#advice-page-heading").should("contain.text", "Level 2 qualifications started between 1 September 2014 and 31 August 2019");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })

    it("Checks the Qualifications achieved in Scotland details are on the page", () => {
        cy.visit("/advice/qualifications-achieved-in-scotland");

        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved in Scotland");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })

    it("Checks the Qualifications achieved in Wales details are on the page", () => {
        cy.visit("/advice/qualifications-achieved-in-wales");

        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved in Wales");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })

    it("Checks the Qualifications achieved in Northern Ireland details are on the page", () => {
        cy.visit("/advice/qualifications-achieved-in-northern-ireland");

        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved in Northern Ireland");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");
    })
})