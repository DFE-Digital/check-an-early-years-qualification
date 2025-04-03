# Notes on running the E2E tests locally

End to end tests are ran using [Playwright](https://playwright.dev/).

To run the tests, run the following commands:
- Navigate to the correct folder, be it:
    - ``cd tests/Dfe.EarlyYearsQualification.E2ETests`` for the E2E tests.
    - ``cd tests/Dfe.EarlyYearsQualification.SmokeTests`` for the smoke tests.
- Tell Node Version Manager that you want the latest version with: ``nvm use node --lts``
- Run the tests with: ``npx playwright install`` (install all playwright browsers)
- Run the tests with: ``npx playwright test`` (add --ui to run the tests in playwrights UI)

The e2e tests are set up to use mock content, the smoke tests are set up to use contentful. If smoke tests fail, check that the mock content contains required content