describe("A spec used to test the qualification details page", () => {

  beforeEach(() => {
    cy.setCookie('auth-secret', Cypress.env('auth_secret'));
  })

  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
  it("Checks the qualification details are on the page", () => {
    // Value is '{"WhereWasQualificationAwarded":"england","WhenWasQualificationAwarded":"7/2015","LevelOfQualification":"3","WhatIsTheAwardingOrganisation":"NCFE","SearchCriteria":"","AdditionalQuestionsAnswers":{"Test question":"yes","Test question 2":"yes"}}' encoded
    cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationAwarded%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D');
    cy.visit("/qualifications/qualification-details/eyq-240");

    cy.get("#page-header").should("contain.text", "Test Main Heading");
    cy.get("#qualification-name-value").should("contain.text", "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)");
    cy.get("#awarding-organisation-label").should("contain.text", "Awarding Org Label");
    cy.get("#awarding-organisation-value").should("contain.text", "NCFE");
    cy.get("#qualification-level-label").should("contain.text", "Test Level Label");
    cy.get("#qualification-level-value").should("contain.text", "3");
    cy.get("#qualification-number-label").should("contain.text", "Test Qualification Number Label");
    cy.get("#qualification-number-value").should("contain.text", "603/5829/4");
    cy.get("#from-which-year-label").should("contain.text", "Test Date Added Label");
    cy.get("#from-which-year-value").should("contain.text", "2020");
    cy.get("#date-of-check-label").should("contain.text", "Test Date Of Check Label");
    
    cy.get("#ratio-heading").should("contain.text", "Test ratio heading");
    cy.get("#ratio-heading + p[class='govuk-body']").should("contain.text", "This is the ratio text");
    
    cy.get("#requirements-heading").should("contain.text", "Test requirements heading");
    cy.get("#requirements-heading + p[class='govuk-body']").should("contain.text", "This is the requirements text");

    cy.get("#check-another-qualification-link").should("contain.text", "Check another qualification");
  })

  it("Checks the order of the ratios on the page", () => {
    // Value is '{"WhereWasQualificationAwarded":"england","WhenWasQualificationAwarded":"7/2015","LevelOfQualification":"3","WhatIsTheAwardingOrganisation":"NCFE","SearchCriteria":"","AdditionalQuestionsAnswers":{"Test question":"yes","Test question 2":"yes"}}' encoded
    cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationAwarded%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D');
    cy.visit("/qualifications/qualification-details/eyq-240");

    cy.get(".ratio-row").should('have.length', 4);
    cy.get(".ratio-heading").eq(0).should("contain.text", "Level 3");
    cy.get(".ratio-heading").eq(1).should("contain.text", "Level 2");
    cy.get(".ratio-heading").eq(2).should("contain.text", "Unqualified");
    cy.get(".ratio-heading").eq(3).should("contain.text", "Level 6");

    // Phase Banner uses govuk-tag also hence index starting at 1
    cy.get(".govuk-tag").eq(1).should("contain.text", "Approved");
    cy.get(".govuk-tag").eq(2).should("contain.text", "Approved");
    cy.get(".govuk-tag").eq(3).should("contain.text", "Approved");
    cy.get(".govuk-tag").eq(4).should("contain.text", "Not Approved");

    cy.get(".govuk-tag").eq(1).should("have.class", "govuk-tag--green");
    cy.get(".govuk-tag").eq(2).should("have.class", "govuk-tag--green");
    cy.get(".govuk-tag").eq(3).should("have.class", "govuk-tag--green");
    cy.get(".govuk-tag").eq(4).should("have.class", "govuk-tag--red");
  })

  it("Checks the order of the ratios on the page when a user incorrectly answers an additional requirement question", () => {
    // Value is '{"WhereWasQualificationAwarded":"england","WhenWasQualificationAwarded":"7/2015","LevelOfQualification":"3","WhatIsTheAwardingOrganisation":"NCFE","SearchCriteria":"","AdditionalQuestionsAnswers":{"Test question":"yes","Test question 2":"no"}}' encoded
    cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationAwarded%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D');
    cy.visit("/qualifications/qualification-details/eyq-240");

    cy.get(".ratio-row").should('have.length', 4);
    cy.get(".ratio-heading").eq(0).should("contain.text", "Unqualified");
    cy.get(".ratio-heading").eq(1).should("contain.text", "Level 6");
    cy.get(".ratio-heading").eq(2).should("contain.text", "Level 3");
    cy.get(".ratio-heading").eq(3).should("contain.text", "Level 2");

    // Phase Banner uses govuk-tag also hence index starting at 1
    cy.get(".govuk-tag").eq(1).should("contain.text", "Approved");
    cy.get(".govuk-tag").eq(2).should("contain.text", "Not Approved");
    cy.get(".govuk-tag").eq(3).should("contain.text", "Not Approved");
    cy.get(".govuk-tag").eq(4).should("contain.text", "Not Approved");

    cy.get(".govuk-tag").eq(1).should("have.class", "govuk-tag--green");
    cy.get(".govuk-tag").eq(2).should("have.class", "govuk-tag--red");
    cy.get(".govuk-tag").eq(3).should("have.class", "govuk-tag--red");
    cy.get(".govuk-tag").eq(4).should("have.class", "govuk-tag--red");
  })
  
  it("Clicking the print button bring up the print dialog", () => {
    cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationAwarded%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D');
    cy.visit("/qualifications/qualification-details/eyq-240");

    var printStub;
    
    cy.window().then(win => {
      printStub = cy.stub(win, 'print')
    })
    
    cy.get("#print-button").click();
    cy.window().then(win => {
      expect(printStub).to.be.calledOnce
    })
  });
});