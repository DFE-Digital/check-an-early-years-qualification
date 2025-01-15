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

    it("shows the month and year missing error message when a user doesnt type a date on the when-was-the-qualification-started page", () => {
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
        cy.get("#date-started-month").should("have.class", "govuk-input--error");
        cy.get("#date-started-year").should("have.class", "govuk-input--error");
    })

    it("shows the month missing error message when a user doesnt type a month on the when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('#date-started-year').type(2024);

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        cy.get("#error-banner-link").should("contain.text", "Missing Month Banner Link Text");

        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Missing Month Error Message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
        cy.get("#date-started-month").should("have.class", "govuk-input--error");
        cy.get("#date-started-year").should("not.have.class", "govuk-input--error");
    })

    it("shows the year missing error message when a user doesnt type a month on the when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('#date-started-month').type(10);
        
        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        cy.get("#error-banner-link").should("contain.text", "Missing Year Banner Link Text");

        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Missing Year Error Message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
        cy.get("#date-started-month").should("not.have.class", "govuk-input--error");
        cy.get("#date-started-year").should("have.class", "govuk-input--error");
    })
    
    describe("When the month selected on the when-was-the-qualification-started page ", () => {
        
        const invalidMonthsToTest = [
            0,
            -1,
            13,
            99
        ]
        
        invalidMonthsToTest.forEach((value) => {
            it(`is ${value} then it shows the month out of bounds error message`, () => {
                cy.visit("/questions/when-was-the-qualification-started");

                cy.get(".govuk-error-summary").should("not.exist");
                cy.get("#date-error").should("not.exist");
                cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

                cy.get('#date-started-month').type(value);
                cy.get('#date-started-year').type(2024);
                
                cy.get('button[id="question-submit"]').click();
                cy.location().should((loc) => {
                    expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
                })

                cy.get(".govuk-error-summary").should("be.visible");
                cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
                cy.get("#error-banner-link").should("contain.text", "Month Out Of Bounds Error Link Text");

                cy.get('#date-error').should("exist");
                cy.get('#date-error').should("contain.text", "Month Out Of Bounds Error Message");
                cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
                cy.get("#date-started-month").should("have.class", "govuk-input--error");
                cy.get("#date-started-year").should("not.have.class", "govuk-input--error");
            })
        })
    })

    describe("When the year selected on the when-was-the-qualification-started page ", () => {

        var invalidYearsToTest = [
            0,
            1899,
            3000
        ]

        invalidYearsToTest.forEach((value) => {
            it(`is ${value} then it shows the incorrect year format error message`, () => {
                cy.visit("/questions/when-was-the-qualification-started");

                cy.get(".govuk-error-summary").should("not.exist");
                cy.get("#date-error").should("not.exist");
                cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

                cy.get('#date-started-month').type(1);
                cy.get('#date-started-year').type(value);

                cy.get('button[id="question-submit"]').click();
                cy.location().should((loc) => {
                    expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
                })

                cy.get(".govuk-error-summary").should("be.visible");
                cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
                cy.get("#error-banner-link").should("contain.text", "Year Out Of Bounds Error Link Text");

                cy.get('#date-error').should("exist");
                cy.get('#date-error').should("contain.text", "Year Out Of Bounds Error Message");
                cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
                cy.get("#date-started-month").should("not.have.class", "govuk-input--error");
                cy.get("#date-started-year").should("have.class", "govuk-input--error");
            })
        })
    })

    it("shows the month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('#date-started-month').type(0);
        cy.get('#date-started-year').type(20);
        
        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");
        
        cy.get("#error-banner-link").should("contain.text", "Month Out Of Bounds Error Link TextYear Out Of Bounds Error Link Text");
        
        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Month Out Of Bounds Error MessageYear Out Of Bounds Error Message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
        cy.get("#date-started-month").should("have.class", "govuk-input--error");
        cy.get("#date-started-year").should("have.class", "govuk-input--error");
    })

    it("shows the month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('#date-started-month').type(0);

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");

        cy.get("#error-banner-link").should("contain.text", "Month Out Of Bounds Error Link TextMissing Year Banner Link Text");

        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Month Out Of Bounds Error MessageMissing Year Error Message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
        cy.get("#date-started-month").should("have.class", "govuk-input--error");
        cy.get("#date-started-year").should("have.class", "govuk-input--error");
    })

    it("shows the month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the when-was-the-qualification-started page", () => {
        cy.visit("/questions/when-was-the-qualification-started");

        cy.get(".govuk-error-summary").should("not.exist");
        cy.get("#date-error").should("not.exist");
        cy.get(".govuk-form-group").should("not.have.class", "govuk-form-group--error");

        cy.get('#date-started-year').type(20);

        cy.get('button[id="question-submit"]').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq("/questions/when-was-the-qualification-started");
        })

        cy.get(".govuk-error-summary").should("be.visible");
        cy.get(".govuk-error-summary__title").should("contain.text", "There is a problem");

        cy.get("#error-banner-link").should("contain.text", "Missing Month Banner Link TextYear Out Of Bounds Error Link Text");

        cy.get('#date-error').should("exist");
        cy.get('#date-error').should("contain.text", "Missing Month Error MessageYear Out Of Bounds Error Message");
        cy.get(".govuk-form-group").should("have.class", "govuk-form-group--error");
        cy.get("#date-started-month").should("have.class", "govuk-input--error");
        cy.get("#date-started-year").should("have.class", "govuk-input--error");
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