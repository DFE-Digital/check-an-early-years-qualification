describe('A spec used to test the various routes through the journey', () => {
  beforeEach(() => {
    cy.visit("/");
    cy.get('.govuk-button--start').should('exist');
  })

  // Mock details found in Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful. 
  it("should redirect the user when they select qualification was awarded outside the UK", () => {
    // home page
    cy.get('.govuk-button--start').click();

    // where-was-the-qualification-awarded page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
    })

    cy.get('#outside-uk').click();
    cy.get('button[type="submit"]').click();

    // qualification-outside-the-united-kingdom page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/advice/qualification-outside-the-united-kingdom');
    })
  })

  it("should redirect the user when they select qualification was awarded in England", () => {
    // home page
    cy.get('.govuk-button--start').click();

    // where-was-the-qualification-awarded page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
    })

    cy.get('#england').click();
    cy.get('button[type="submit"]').click();

    // qualifications page (This is only a temporary page)
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })

    cy.get('a[href="/qualifications/qualification-details/eyq-240"]').click();

    // qualifications page (This is only a temporary page)
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/qualification-details/eyq-240');
    })
  })

  it("should move the user back to the previous page when they click on the back button", () => {
    // home page
    cy.get('.govuk-button--start').click();

    // where-was-the-qualification-awarded page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
    })

    cy.get('.govuk-back-link').click();

    // home page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/');
    })
  })
})