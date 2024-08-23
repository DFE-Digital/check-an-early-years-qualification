describe("A spec used to test the qualification list page", () => {

    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks the details are showing on the page", () => {
        // Value is '{"WhereWasQualificationAwarded":"england","WhenWasQualificationStarted":"6/2022","LevelOfQualification":"3","WhatIsTheAwardingOrganisation":"NCFE"}' encoded
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%7D');

        cy.visit("/qualifications");

        cy.get("#your-search-header").should("contain.text", "Your search");
        cy.get("#filter-country").should("contain.text", "England");
        cy.get("#filter-start-date").should("contain.text", "June 2022");
        cy.get("#filter-level").should("contain.text", "Level 3");
        cy.get("#filter-org").should("contain.text", "NCFE");

        cy.get("#heading").should("contain.text", "Test Header");
        cy.get("#found-heading").should("contain.text", "3 qualifications found");

        cy.get("#pre-search-content").should("contain.text", "Pre search box content");
        cy.get("#post-list-content").should("contain.text", "Link to not on list advice page");
        cy.get("#post-filter-content").should("contain.text", "Post search criteria content");

        cy.get(".level").first().should("contain.text", "Level");
        cy.get(".awarding-org").first().should("contain.text", "Awarding organisation");

        cy.get("#clear-search").should("contain.text", "Clear search");
        cy.get("#no-result-content").should("not.exist");
    })

    it("Shows the default headings when any level and no awarding organisation selected", () => {
        // Value is '{"WhereWasQualificationAwarded":"england","WhenWasQualificationStarted":"6/2022","LevelOfQualification":"0","WhatIsTheAwardingOrganisation":""}' encoded
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D');
        cy.visit("/qualifications");

        cy.get("#filter-level").should("contain.text", "Any level");
        cy.get("#filter-org").should("contain.text", "Various awarding organisations");
    })

    it("Shows the correct no results content when there are no results in the search", () => {
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D');

        cy.visit("/qualifications");

        cy.get("#found-heading").should("contain.text", "No qualifications found");
        cy.get("#no-result-content").should("contain.text", "Test no qualifications text");
    })
})