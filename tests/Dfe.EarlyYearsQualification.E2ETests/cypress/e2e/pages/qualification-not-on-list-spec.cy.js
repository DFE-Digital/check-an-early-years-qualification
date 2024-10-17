describe("A spec that tests the qualification not on list page", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks content renders for the level 3 specific qualification not on the list page", () => {
        // Encoded cookie value is: {"WhereWasQualificationAwarded":"england","WhenWasQualificationStarted":"7/2015","LevelOfQualification":"3"}
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%7D');
        cy.visit("/advice/qualification-not-on-the-list");

        cy.get("#advice-page-heading").should("contain.text", "This is the level 3 page");
        cy.get("#advice-page-body").should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback banner heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "Banner body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback banner heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "Banner body text");
    })

    it("Checks content renders for the level 4 specific qualification not on the list page", () => {
        // Encoded cookie value is: {"WhereWasQualificationAwarded":"england","WhenWasQualificationStarted":"7/2015","LevelOfQualification":"4"}
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%224%22%7D');
        cy.visit("/advice/qualification-not-on-the-list");

        cy.get("#advice-page-heading").should("contain.text", "This is the level 4 page");
        cy.get("#advice-page-body").should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback banner heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "Banner body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback banner heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "Banner body text");
    })

    it("Checks default content renders when no specific qualification not on the list page exists", () => {
        // Encoded cookie value is: {"WhereWasQualificationAwarded":"england","WhenWasQualificationStarted":"7/2015","LevelOfQualification":"5"}
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%225%22%7D');
        cy.visit("/advice/qualification-not-on-the-list");

        cy.get("#advice-page-heading").should("contain.text", "Qualification not on the list");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "This is the body text");
    })
})