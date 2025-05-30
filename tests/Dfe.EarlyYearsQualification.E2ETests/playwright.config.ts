import { defineConfig, devices } from '@playwright/test';

/**
 * See https://playwright.dev/docs/test-configuration.
 */
require('dotenv').config();

var chromeUse = process.env.CI ? { ...devices['Desktop Chrome'], channel: 'chromium' } : { ...devices['Desktop Chrome'] };

export default defineConfig({
    testDir: './tests',
    /* Run tests in files in parallel */
    fullyParallel: true,
    /* Fail the build on CI if you accidentally left test.only in the source code. */
    forbidOnly: !!process.env.CI,
    /* Retry on CI only */
    retries: process.env.CI ? 2 : 0,
    /* Opt out of parallel tests on CI. */
    workers: process.env.CI ? 1 : undefined,
    /* Reporter to use. See https://playwright.dev/docs/test-reporters */
    reporter: 'html',
    /* Shared settings for all the projects below. See https://playwright.dev/docs/api/class-testoptions. */
    use: {
        /* Base URL to use in actions like `await page.goto('/')`. */
        baseURL: process.env.WEBAPP_URL,

        /* Collect trace when retrying the failed test. See https://playwright.dev/docs/trace-viewer */
        trace: 'on-first-retry',

        ignoreHTTPSErrors: true,
    },
    /* Configure projects for major browsers */
    projects: [
        {
            name: 'chromium',
            use: chromeUse,
        },
        {
            name: 'firefox',
            use: { ...devices['Desktop Firefox'] },
        },
    ],

    /* Run your local dev server before starting the tests */
    webServer: {
        command: `cd ../../src/Dfe.EarlyYearsQualification.Web && dotnet run --urls "${process.env.WEBAPP_URL}" --project ./Dfe.EarlyYearsQualification.Web.csproj --UseMockContentful=true --ServiceAccess:Keys:0="${process.env.AUTH_SECRET}"`,
        url: process.env.WEBAPP_URL,
        reuseExistingServer: !process.env.CI,
    },
});
