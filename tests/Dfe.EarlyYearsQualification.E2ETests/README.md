# Notes on running the tests locally

These tests are run using [Playwright](https://playwright.dev/).

Before running tests ensure you have Node and Playwright installed, see the following commands:

- Navigate to the correct folder:
    - `cd tests/Dfe.EarlyYearsQualification.E2ETests`
- Tell Node Version Manager that you want the latest version with: `nvm use node --lts`
- Run the tests with: `npx playwright install` (install all playwright browsers)

## Running Tests

There are a number of scripts created in the package.json file to make to easier to run e2e and snapshot tests.
These are:
- `npm run e2e` (Runs all the e2e tests across both Chrome and Firefox)
- `npm run e2e:chrome` (Runs all the e2e tests on Chrome)
- `npm run snapshot` (Runs the snapshot tests)
- `npm run snapshot:update` (Updates all the snapshots)

You can also run the tests using the method below which you will need to do for both smoke and validation tests as these require an access token.

- Run the tests with: `npx playwright test`
    - Optional arguments:
    - `--ui` will run the tests in the playwright UI
    - `--grep "@TAG"` will run only tests with the provided tag
    - `--project PROJECT` will run the tests only for the selected browser profile

| TestTag       | Content Type | Additional Params                                                            | Notes                                                                           |
|---------------|--------------|------------------------------------------------------------------------------|---------------------------------------------------------------------------------|
| `@e2e`        | Mock         |                                                                              |                                                                                 |
| `@validation` | Live         |                                                                              |                                                                                 |
| `@smoke`      | Live         |                                                                              |                                                                                 |
| `@snapshot`   | Mock         | `--update-snapshots`: if a snapshot test fails, it will replace the snapshot | Use chromium browser using `--project chromium`. Firefox is unstable with links |

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
