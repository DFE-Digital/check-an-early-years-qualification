describe('A spec that tests the challenge page', () => {
    beforeEach(() => {
        cy.visit("/challenge");
    });
    
    it("should show the missing password error when the user doesn't enter a password", () => {

        cy.get('#error-banner').should("not.exist");
        cy.get('#error-message').should("not.exist");
        
        cy.get('#question-submit').click();

        cy.get('#error-banner').should("exist");
        cy.get('#error-banner-link').should("contain.text", "Test Missing Password Text");
        cy.get('#error-message').should("contain.text", "Test Missing Password Text");
    });

    it("should show the incorrect password error when the user enters an incorrect password", () => {

        cy.get('#error-banner').should("not.exist");
        cy.get('#error-message').should("not.exist");

        cy.get('#PasswordValue').type("Some incorrect password");
        cy.get('#question-submit').click();

        cy.get('#error-banner').should("exist");
        cy.get('#error-banner-link').should("contain.text", "Test Incorrect Password Text");
        cy.get('#error-message').should("contain.text", "Test Incorrect Password Text");
    });
    
    it("clicking the show password button changes the password input to text, clicking it again turns it back", () => {
        cy.get("#PasswordValue").should('have.attr', 'type', 'password');
        
        cy.get("#togglePassword").click();

        cy.get("#PasswordValue").should('have.attr', 'type', 'text');

        cy.get("#togglePassword").click();

        cy.get("#PasswordValue").should('have.attr', 'type', 'password');
    });
})