describe('A spec used to check a new user is challenged to enter the secret', () => {

  it("should redirect the user to the challenge page", () => {

    cy.visit("/");

    cy.location().should((loc) => {
      expect(loc.pathname).to.eq('/Challenge');
      expect(loc.search).to.eq('?redirectAddress=%2F')
    })

    cy.get('#redirectAddress').should("have.value", '/');
    cy.get('#value').should("be.empty");
  })
})