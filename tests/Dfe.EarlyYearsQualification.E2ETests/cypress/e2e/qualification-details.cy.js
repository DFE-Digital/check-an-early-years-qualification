describe("A spec used to test the qualification details page", () => {

    beforeEach(() => {
      cy.visit("/qualifications/qualification-details/eyq-240");
    })
  
    // Mock details found in Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful. 
    it("Checks the qualification title is on the page", () => {
      cy.get("#qualification-name-value").should("contain.text", "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)");
      cy.get("#awarding-qualification-value").should("contain.text", "NCFE");
      cy.get("#qualification-level-value").should("contain.text", "3");
      cy.get("#qualification-number-value").should("contain.text", "603/5829/4");
      cy.get("#from-which-year-value").should("contain.text", "2020");
      cy.get("#notes-value").should("contain.text", "The course must be assessed within the EYFS in an Early Years setting in England. Please note that the name of this qualification changed in February 2023. Qualifications achieved under either name are full and relevant provided that the start date for the qualification aligns with the date of the name change.");
      cy.get("#additional-requirements-value").should("contain.text", "Additional notes");
    })
  })