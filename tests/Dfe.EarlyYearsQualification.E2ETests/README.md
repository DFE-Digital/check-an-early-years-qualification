# Notes on running the tests locally

These tests are ran using [Playwright](https://playwright.dev/).

To run the tests, run the following commands:

- Navigate to the correct folder:
    - `cd tests/Dfe.EarlyYearsQualification.E2ETests`
- Tell Node Version Manager that you want the latest version with: `nvm use node --lts`
- Run the tests with: `npx playwright install` (install all playwright browsers)
- Run the tests with: `npx playwright test`
    - Optional arguments:
    - `--ui` will run the tests in the playwright UI
    - `--grep "@TAG"` will run only tests with the provided tag

| TestTag       | Content Type |
|---------------|--------------|
| `@e2e`        | Mock         |
| `@validation` | Live         |
| `@smoke`      | Live         |

If a test using mock content fails, check that the mock content contains the required content.

To run validation checks, you need to set up environment variables for the `WEBAPP_URL`, `DOMAIN` and `AUTH_SECRET`.

## Environment Variables Setup

Required environment variables:

- `WEBAPP_URL`: Target environment URL
- `DOMAIN`: Target domain (same as URL for staging/dev, or localhost)
- `AUTH_SECRET`: Challenge page secret

Replace 'XXX' with the values for the environment you wish to use.

### GitBash:

``WEBAPP_URL=XXX DOMAIN=XXX AUTH_SECRET=XXX npx playwright test``

### Powershell:

`$env:WEBAPP_URL = 'XXX'`

`$env:DOMAIN = 'XXX'`

`$env:AUTH_SECRET = 'XXX'`

`npx playwright test`
