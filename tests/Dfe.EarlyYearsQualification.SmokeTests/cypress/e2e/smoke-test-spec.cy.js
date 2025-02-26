describe('A spec used to smoke test the environment once a deployment has happened', () => {
    beforeEach(() => {
      cy.setCookie('auth-secret', Cypress.env('auth_secret'));
      cy.visit("/");
      cy.get('.govuk-button--start').should('exist');
    })
  
    it("should return search results", () => {
        // home page
        cy.get('.govuk-button--start').click();

        // where-was-the-qualification-awarded page
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/where-was-the-qualification-awarded');
        })

        cy.get('#england').click();
        cy.get('button[id="question-submit"]').click();

        // when-was-the-qualification-started-and-awarded page
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/when-was-the-qualification-started-and-awarded');
        })

        cy.get('#StartedQuestion.SelectedMonth').type("7");
        cy.get('#StartedQuestion.SelectedYear').type("2015");
        cy.get('button[id="question-submit"]').click();

        // what-level-is-the-qualification page
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/what-level-is-the-qualification');
        })
        cy.get('#0').click(); // Select not sure
        cy.get('button[id="question-submit"]').click();

        // what-is-the-awarding-organisation page
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/questions/what-is-the-awarding-organisation');
        })

        cy.get('#awarding-organisation-not-in-list').click();  // Tick the not on the list
        cy.get('button[id="question-submit"]').click();

        // qualifications page
        cy.location().should((loc) => {
            expect(loc.pathname).to.eq('/qualifications');
        })

        // If this shows then no qualifications are getting returned indicating possible issue
        cy.get('#no-result-content').should('not.exist'); 
    })
  })