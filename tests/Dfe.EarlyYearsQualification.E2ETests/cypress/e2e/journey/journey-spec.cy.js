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
    
    cy.get('#awarding-organisation-select').select(1);  // first no-default item in the list
    cy.get('button[id="question-submit"]').click();
    
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

      [
          {09, 2014},
          {06, 2017},
          {08, 2019},
      ].forEach((month, year) => {
        it(`should redirect when qualification is level 2 and startMonth is {month} and startYear is {year}`, () => {
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
    
          cy.get('#date-started-month').type(month);
          cy.get('#date-started-year').type(year);
          cy.get('button[id="question-submit"]').click();
    
          // what-level-is-the-qualification page
          cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/what-level-is-the-qualification');
          })
          cy.get('#2').click();
          cy.get('button[id="question-submit"]').click();
    
          // level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019 page
          cy.location().should((loc) => {
            expect(loc.pathname).to.eq('advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019');
          })
      })
  })
})