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

  it("should redirect the user when they select qualification was awarded in Scotland", () => {
    // home page
    cy.get('.govuk-button--start').click();

    // where-was-the-qualification-awarded page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
    })

    cy.get('#scotland').click();
    cy.get('button[id="question-submit"]').click();

    // qualifications-achieved-in-scotland
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/advice/qualifications-achieved-in-scotland');
    })
  })

  it("should redirect the user when they select qualification was awarded in Wales", () => {
    // home page
    cy.get('.govuk-button--start').click();

    // where-was-the-qualification-awarded page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
    })

    cy.get('#wales').click();
    cy.get('button[id="question-submit"]').click();

    // qualifications-achieved-in-wales
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/advice/qualifications-achieved-in-wales');
    })
  })

  it("should redirect the user when they select qualification was awarded in Northern Ireland", () => {
    // home page
    cy.get('.govuk-button--start').click();

    // where-was-the-qualification-awarded page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
    })

    cy.get('#northern-ireland').click();
    cy.get('button[id="question-submit"]').click();

    // qualifications-achieved-in-scotland
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/advice/qualifications-achieved-in-northern-ireland');
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

    // qualifications page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })

    cy.get('a[href="/confirm-qualification/EYQ-240"]').click();

    // confirm qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/confirm-qualification/EYQ-240');
    })

    cy.get('#yes').click();
    cy.get('button[id="confirm-qualification-button"]').click();

    // check additional questions page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-240');
    })

    cy.get('#yes_0_0').click();
    cy.get('#yes_1_0').click();
    cy.get('button[id="additional-requirement-button"]').click();

    // qualification details page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/qualification-details/EYQ-240');
    })
  })

  it("Selecting the 'Qualification is not on the list' link on the qualification list page should navigate to the correct advice page", () => {
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

    // click not on the list link
    cy.get('a[href="/advice/qualification-not-on-the-list"]').click();

    // qualification not on the list page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/advice/qualification-not-on-the-list');
    })

    cy.get('#advice-page-heading').should("contain.text", "Qualification not on the list");

    // check back button goes back to the qualifications list page
    cy.get('#back-button').click();

    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })
  })

  it("Selecting qualification level 7 should navigate to the level 7 advice page", () => {
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
    cy.get('#7').click();
    cy.get('button[id="question-submit"]').click();

    // level 7 advice page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/advice/qualification-level-7');
    })

    // check back button goes back to the what level is the qualification page
    cy.get('#back-button').click();

    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/what-level-is-the-qualification');
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

  const testDates = [
    ['09', '2014'],
    ['06', '2017'],
    ['08', '2019'],
  ];

  testDates.forEach((date) => {
    const [month, year] = date;
    it(`should redirect when qualification is level 2 and startMonth is ${month} and startYear is ${year}`, () => {
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
        expect(loc.pathname).to.eq('/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019');
      })
    })
  })
})