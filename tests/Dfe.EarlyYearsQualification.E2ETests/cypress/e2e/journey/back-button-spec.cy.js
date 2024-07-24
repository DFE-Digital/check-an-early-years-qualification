describe('A spec used to test the main back button route through the journey', () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
        cy.visit("/");
        cy.get('.govuk-button--start').should('exist');
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.
    it("back buttons should all navigate to the appropriate pages in the main journey", () => {
        // home page
        cy.get('.govuk-button--start').click();

        // where-was-the-qualification-awarded page - england selection moves us on
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
        })

        cy.get('#england').click();
        cy.get('button[id="question-submit"]').click();

        // when-was-the-qualification-started page - valid date moves us on
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/when-was-the-qualification-started');
        })

        cy.get('#date-started-month').type("6");
        cy.get('#date-started-year').type("2022");
        cy.get('button[id="question-submit"]').click();

        // what-level-is-the-qualification page - valid level moves us on
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/what-level-is-the-qualification');
        })
        cy.get('#3').click();
        cy.get('button[id="question-submit"]').click();

        // what-is-the-awarding-organisation page - valid awarding organisation moves us on
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/what-is-the-awarding-organisation');
        })

        cy.get('#awarding-organisation-select').select(1); // first no-default item in the list
        cy.get('button[id="question-submit"]').click();

        // qualifications page - click a qualification in the list to move us on
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
        cy.get('button[id="additional-requirement-button"]').click();

        // qualifications page
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/qualifications/qualification-details/EYQ-240');
        })
        
        /// Time to go back through the journey!
        cy.get('#back-button').click();

        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/confirm-qualification/eyq-240');
        })
        
        cy.get('#back-button').click();

        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/qualifications');
        })

        cy.get('#back-button').click();

        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/what-is-the-awarding-organisation');
        })
        
        cy.get('#back-button').click();

        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/what-level-is-the-qualification');
        })

        cy.get('#back-button').click();

        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/when-was-the-qualification-started');
        })

        cy.get('#back-button').click();

        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
        })

        cy.get('#back-button').click();

        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/');
        })
    })
})

describe('test that the back buttons work on all the secondary pages', () => {
    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
    })
    
    it('the back button on the accessibility statement page navigates back to the home page', () => {
        cy.visit("/accessibility-statement");
        cy.get('#back-button').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/');
        })
    });

    it('the back button on the cookies preference page navigates back to the home page', () => {
        cy.visit("/cookies");
        cy.get('#back-button').click();
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/');
        })
    });
});