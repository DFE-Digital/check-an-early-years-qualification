describe("A spec that tests advice pages", () => {
   
    // Mock details found in Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful. 
    it("Checks the qualification details are on the page", () => {
        cy.visit("/advice/qualification-outside-the-united-kingdom");


        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved outside the United Kingdom");
        cy.get("#outside-uk-body").should("contain.text", "This is the body of the page");
    })
})