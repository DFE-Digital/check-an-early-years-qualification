describe('A spec used to test the various routes through the journey', () => {
  beforeEach(() => {
    cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    cy.visit("/");
    cy.get('.govuk-button--start').should('exist');
  })

  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 
  it("should redirect the user when they select qualification was awarded outside the UK", () => {
    // home page
    cy.get('.govuk-button--start').click();

    // where-was-the-qualification-awarded page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
    })

    cy.get('#outside-uk').click();
    cy.get('button[id="question-submit"]').click();

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
    cy.get('button[id="question-submit"]').click();

    // when-was-the-qualification-started page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/when-was-the-qualification-started');
    })

    cy.get('#date-started-month').type("6");
    cy.get('#date-started-year').type("2022");
    cy.get('button[id="question-submit"]').click();

    // what-level-is-the-qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/what-level-is-the-qualification');
    })
    cy.get('#3').click();
    cy.get('button[id="question-submit"]').click();
    
    // what-is-the-awarding-organisation page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/what-is-the-awarding-organisation');
    })
    
    cy.get('#awarding-organisation-select').select('A awarding organisation');
    cy.get('button[id="question-submit"]').click();
    
    // qualifications page (This is only a temporary page)
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })

    cy.get('a[href="/confirm-qualification/eyq-240"]').click();

    // confirm qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/confirm-qualification/eyq-240');
    })

    cy.get('#yes').click();
    cy.get('button[id="confirm-qualification-button"]').click();
    
    // qualification details page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/qualification-details/EYQ-240');
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