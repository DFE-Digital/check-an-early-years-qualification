import { pages } from "./urls-to-check";

describe("A spec that tests the phase banner is showing on all pages", () => {
   
  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 

  pages.forEach((option) => {
    it(`Checks that the phase banner is present at the URL: ${option}`, () => {

      cy.visit(option);
      
      cy.get(".govuk-phase-banner").should("be.visible");

      cy.get(".govuk-phase-banner__content__tag").should("contain.text", "Test phase banner name");
      cy.get(".govuk-phase-banner__text").should("contain.text", "Test phase banner content");
    });
  });
})