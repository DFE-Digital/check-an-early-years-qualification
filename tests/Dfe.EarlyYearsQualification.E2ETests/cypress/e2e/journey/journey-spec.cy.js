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

    // check additional questions first page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-240/1');
    })

    cy.get('#yes').click();
    cy.get('button[id="additional-requirement-button"]').click();

    // check additional questions seconf page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-240/2');
    })

    cy.get('#yes').click();
    cy.get('button[id="additional-requirement-button"]').click();
    
    // confirm answers page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-240/confirm-answers');
    })

    cy.get("#confirm-answers").click();
    
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

    cy.get('#advice-page-heading').should("contain.text", "This is the level 3 page");

    // check back button goes back to the qualifications list page
    cy.get('#back-button').click();

    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })
  })
  
  it("Selecting qualification level 7 started after 1 Sept 2014 should navigate to the level 7 post 2014 advice page", () => {
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

    cy.get('#date-started-month').type("8");
    cy.get('#date-started-year').type("2015");
    cy.get('button[id="question-submit"]').click();

    // what-level-is-the-qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/what-level-is-the-qualification');
    })
    cy.get('#7').click();
    cy.get('button[id="question-submit"]').click();

    // level 7 post 2014 advice page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/advice/level-7-qualification-post-2014');
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

  it("Should remove the search criteria when a user goes to the awarding organisation page and back again", () => {
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

    // Select a qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })
  
    cy.get('#refineSearch').type('test');
    cy.get('#refineSearchButton').click();

    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })
    
    cy.get('#back-button').click();
    // what-is-the-awarding-organisation page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/questions/what-is-the-awarding-organisation');
    })

    cy.get('#awarding-organisation-select').select(1);  // first no-default item in the list
    cy.get('button[id="question-submit"]').click();

    // Select a qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications');
    })

    cy.get('#refineSearch').should('have.value', '');
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

  it("should bypass remaining additional requirement question when answering yes to the Qts question", () => {
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
    cy.get('#6').click();
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

    cy.get('a[href="/confirm-qualification/EYQ-108"]').click();

    // confirm qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/confirm-qualification/EYQ-108');
    })

    cy.get('#yes').click();
    cy.get('button[id="confirm-qualification-button"]').click();

    // check additional questions first page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-108/1');
    })

    cy.get('#yes').click();
    cy.get('button[id="additional-requirement-button"]').click();

    // confirm answers page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-108/confirm-answers');
    })

    cy.get("#confirm-answers").click();

    // qualification details page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/qualification-details/EYQ-108');
    })
  })

  it("should not bypass remaining additional requirement question when answering no to the Qts question", () => {
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
    cy.get('#6').click();
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

    cy.get('a[href="/confirm-qualification/EYQ-108"]').click();

    // confirm qualification page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/confirm-qualification/EYQ-108');
    })

    cy.get('#yes').click();
    cy.get('button[id="confirm-qualification-button"]').click();

    // check additional questions first page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-108/1');
    })

    cy.get('#no').click();
    cy.get('button[id="additional-requirement-button"]').click();

    // check second additional question
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-108/2');
    })

    cy.get('#yes').click();
    cy.get('button[id="additional-requirement-button"]').click();

    // confirm answers page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/check-additional-questions/EYQ-108/confirm-answers');
    })

    cy.get("#confirm-answers").click();

    // qualification details page
    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/qualifications/qualification-details/EYQ-108');
    })
  })
})