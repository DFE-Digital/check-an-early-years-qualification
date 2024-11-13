describe("A spec that tests advice pages", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.  
    it("Checks the Qualifications achieved outside the United Kingdom details are on the page", () => {
        cy.visit("/advice/qualification-outside-the-united-kingdom");

        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved outside the United Kingdom");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "This is the body text");
    })

    it("Checks the level 2 between 1 Sept 2014 and 31 Aug 2019 details are on the page", () => {
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        cy.visit("/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019");

        cy.get("#advice-page-heading").should("contain.text", "Level 2 qualifications started between 1 September 2014 and 31 August 2019");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "This is the body text");
    })

    it("Checks the Qualifications achieved in Scotland details are on the page", () => {
        cy.visit("/advice/qualifications-achieved-in-scotland");

        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved in Scotland");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "This is the body text");
    })

    it("Checks the Qualifications achieved in Wales details are on the page", () => {
        cy.visit("/advice/qualifications-achieved-in-wales");

        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved in Wales");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "This is the body text");
    })

    it("Checks the Qualifications achieved in Northern Ireland details are on the page", () => {
        cy.visit("/advice/qualifications-achieved-in-northern-ireland");

        cy.get("#advice-page-heading").should("contain.text", "Qualifications achieved in Northern Ireland");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "This is the body text");
    })
    
    it("Checks the Level 7 qualification post 2014 details are on the page", () => {
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        cy.visit("/advice/level-7-qualification-post-2014");

        cy.get("#advice-page-heading").should("contain.text", "Level 7 qualification post 2014");
        cy.get("#advice-page-body").should("contain.text", "Test Advice Page Body");

        cy.get(".govuk-notification-banner__title").eq(0).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(0).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(0).should("contain.text", "This is the body text");

        cy.get(".govuk-notification-banner__title").eq(1).should("contain.text", "Test banner title");
        cy.get(".govuk-notification-banner__heading").eq(1).should("contain.text", "Feedback heading");
        cy.get(".govuk-notification-banner__content").eq(1).should("contain.text", "This is the body text");
    })
})