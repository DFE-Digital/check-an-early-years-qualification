describe("A spec that tests question pages", () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 

    /// Where was the qualification awarded page tests
    it("Checks the content on where-was-the-qualification-awarded page", () => {
        cy.visit("/questions/where-was-the-qualification-awarded");

        cy.get("#question").should("contain.text", "Where was the qualification awarded?");
        cy.get("#england").should("exist");
        cy.get("#scotland").should("exist");
        cy.get("#wales").should("exist");
        cy.get("#northern-ireland").should("exist");
        cy.get(".govuk-radios__divider").should("contain.text", "or");
        cy.get("#outside-uk").should("exist");
    })

    it("Checks additional information on the where-was-the-qualification-awarded page", () => {
        cy.visit("/questions/where-was-the-qualification-awarded");

        cy.get(".govuk-details").should("not.have.attr", "open");
        cy.get(".govuk-details__summary-text").should("contain.text", "This is the additional information header");
        cy.get(".govuk-details__text").should("contain.text", "This is the additional information body");

        cy.get(".govuk-details__summary-text").click();
        cy.get(".govuk-details").should("have.attr", "open");
    })

    it("shows an error message when a user doesnt select an option on the where-was-the-qualification-awarded page", () => {
        cy.visit("/questions/where-was-the-qualification-awarded");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#option-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/where-was-the-qualification-awarded");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        cy.get("#error-banner-link").should("contain.text", "Test error banner link text");

        cy.get('#option-error').should("exist");
        cy.get('#option-error').should("contain.text", "Test error message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
    })

    /// When was the qualification awarded page tests
    it("Checks the content on when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get("#question").should("contain.text", "Test Date Question");

        cy.get("#date-started-month").should("exist");
        cy.get("#date-started-year").should("exist");
    })

    it("Checks additional information on the when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-details").should("not.have.attr", "open");
        cy.get(".govuk-details__summary-text").should("contain.text", "This is the additional information header");
        cy.get(".govuk-details__text").should("contain.text", "This is the additional information body");

        cy.get(".govuk-details__summary-text").click();
        cy.get(".govuk-details").should("have.attr", "open");
    })

    it("shows an error message when a user doesnt type a date on the when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        cy.get("#error-banner-link").should("contain.text", "Test error banner link text");

        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Test Error Message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
    })

    it("shows an error message when a user puts a date in the future on when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('#date-started-month').type(10);
        cy.get('#date-started-year').type(3000);

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        cy.get("#error-banner-link").should("contain.text", "Future date error message banner link");

        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Future date error message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
    })

    it("shows an error message when a user puts an invalid format in on when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('#date-started-month').type(10);
        cy.get('#date-started-year').type(24);

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        let year = new Date().getFullYear();
        cy.get("#error-banner-link").should("contain.text", `Enter a month between 1 and 12 and a year between 1900 and ${year}`);

        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Incorrect format error message banner link");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
    })

    /// What level is the qualification page
    it("Checks the content on what-level-is-the-qualification page", () => {
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        cy.visit("/questions/what-level-is-the-qualification");

        cy.get("#question").should("contain.text", "What level is the qualification?");
        cy.get("#2").should("exist");
        cy.get("#3").should("exist");
        cy.get("#6").should("exist");
        cy.get("#6_hint").should("exist");
        cy.get("#6_hint").should("contain.text", "Some hint text");
        cy.get("#7").should("exist");
    })

    it("Checks additional information on the what-level-is-the-qualification page", () => {
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        cy.visit("/questions/what-level-is-the-qualification");

        cy.get(".govuk-details").should("not.have.attr", "open");
        cy.get(".govuk-details__summary-text").should("contain.text", "This is the additional information header");
        cy.get(".govuk-details__text").should("contain.text", "This is the additional information body");

        cy.get(".govuk-details__summary-text").click();
        cy.get(".govuk-details").should("have.attr", "open");
    })

    it("shows an error message when a user doesnt select an option on the what-level-is-the-qualification page", () => {
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        cy.visit("/questions/what-level-is-the-qualification");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#option-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/what-level-is-the-qualification");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        cy.get("#error-banner-link").should("contain.text", "Test error banner link text");

        cy.get('#option-error').should("exist");
        cy.get('#option-error').should("contain.text", "Test error message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
    })

    /// What is the awarding organisation page
    it("Checks the content on what-is-the-awarding-organisation page", () => {
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        cy.visit("/questions/what-is-the-awarding-organisation");

        cy.get("#question").should("contain.text", "Test Dropdown Question");
        cy.get("#awarding-organisation-select").should("exist");
        cy.get("#awarding-organisation-not-in-list").should("exist");
        cy.get('button[id="question-submit"]').should("exist");
    })

    it("Checks additional information on the what-is-the-awarding-organisation page", () => {
        cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        cy.visit("/questions/what-is-the-awarding-organisation");

        cy.get(".govuk-details").should("not.have.attr", "open");
        cy.get(".govuk-details__summary-text").should("contain.text", "This is the additional information header");
        cy.get(".govuk-details__text").should("contain.text", "This is the additional information body");

        cy.get(".govuk-details__summary-text").click();
        cy.get(".govuk-details").should("have.attr", "open");
    })

    it("shows an error message when a user doesnt select an option from the dropdown list" +
        "and also does not check 'not in the list' on the what-is-the-awarding-organisation", () => {
            cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
            cy.visit("/questions/what-is-the-awarding-organisation");

            cy.get(".govuk-error-summary").should("not.exist");
            cy.get("#dropdown-error").should("not.exist");
            cy.get("#awarding-organisation-select").should("not.have.class", "govuk-select--error");

            cy.get('button[id="question-submit"]').click();
            cy.location().should((loc) => {
                expect(loc.pathname).to.eq("/questions/what-is-the-awarding-organisation");
            })

            cy.get(".govuk-error-summary").should("be.visible");
            cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
            cy.get("#error-banner-link").should("contain.text", "Test error banner link text");

            cy.get('#dropdown-error').should("exist");
            cy.get('#dropdown-error').should("contain.text", "Test Error Message");
            cy.get("#awarding-organisation-select").should("have.class", "govuk-select--error");
        })
})