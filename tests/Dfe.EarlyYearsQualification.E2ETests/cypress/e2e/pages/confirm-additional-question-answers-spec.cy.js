describe("A spec used to test the check additional requirements answer page", () => {

    beforeEach(() => {
        cy.setCookie('auth-secret', Cypress.env('auth_secret'));
        // cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');
        
    })

    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 
    it("Checks the page contains the relevant components", () => {
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D')

        cy.visit("/qualifications/check-additional-questions/EYQ-240/confirm-answers");

        // Full cookie set from running through the entire journey up to this page.
        
        cy.get(".govuk-heading-xl").should("contain.text", "Test page heading");
        
        cy.get("#question-1-question").should("contain.text", "Test question");
        cy.get("#question-1-answer").should("contain.text", "Yes");
        cy.get("#question-1-change").should("contain.text", "Test change answer text");
        cy.get("#question-2-question").should("contain.text", "Test question");
        cy.get("#question-2-answer").should("contain.text", "Yes");
        cy.get("#question-2-change").should("contain.text", "Test change answer text");
        
        cy.get(".govuk-warning-text__text").should("contain.text", "Test answer disclaimer text");
        
        cy.get("#confirm-answers").should("contain.text", "Test button text");
    })
    
    it("Navigates to the correct question page if the user clicks to change an answer", () => {
        cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D')

        cy.visit("/qualifications/check-additional-questions/EYQ-240/confirm-answers");

        cy.get("#question-1-change a").click();

        cy.url().should("include", "/qualifications/check-additional-questions/EYQ-240/1");

        cy.visit("/qualifications/check-additional-questions/EYQ-240/confirm-answers");

        cy.get("#question-2-change a").click();

        cy.url().should("include", "/qualifications/check-additional-questions/EYQ-240/2");
    })
    
    it("Navigates back to the ")
})