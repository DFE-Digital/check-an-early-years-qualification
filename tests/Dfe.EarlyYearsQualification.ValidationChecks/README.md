# Notes on running the Validation Checks locally

The check are ran using [Playwright](https://playwright.dev/).

You need to pass in a couple of environment variables:

- WEBAPP_URL: This is the target environment
- DOMAIN: This is usually the same as the target url when pointing at environments like staging or dev, otherwise it would just be localhost
- AUTH_SECRET: This is the Challenge page secret.

To run the tests, run the following commands:
- Navigate to the correct folder, be it:
    - ``cd tests/Dfe.EarlyYearsQualification.ValidationChecks`` for the E2E tests.
- If not already installed, install all the playwright browsers: ``npx playwright install``
- Run the tests with: ``WEBAPP_URL=XXX DOMAIN=XXX AUTH_SECRET=XXX npx playwright test`` (add --ui to run the tests in playwrights UI)
Don't forget to replace the XXX's with the correct environment values.