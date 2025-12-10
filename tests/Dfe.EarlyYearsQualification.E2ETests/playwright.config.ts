import {defineConfig, devices} from '@playwright/test';

/**
 * Read environment variables from file.
 * https://github.com/motdotla/dotenv
 */
// import dotenv from 'dotenv';
// import path from 'path';
// dotenv.config({ path: path.resolve(__dirname, '.env') });

/**
 * See https://playwright.dev/docs/test-configuration.
 */
require('dotenv').config();

var chromeUse = process.env.CI ? {...devices['Desktop Chrome'], channel: 'chromium'} : {...devices['Desktop Chrome']};

export default defineConfig({
    snapshotPathTemplate: 'tests/e2e/snapshots/.snaps/{arg}{ext}',
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
    reporter: process.env.CI ? 'html' : 'line',
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
            use: {...devices['Desktop Firefox']},
        },
        {
            name: 'webkit',
            use: {...devices['Desktop Safari']},
        },
        /* Test against mobile viewports. */
        // {
        //   name: 'Mobile Chrome',
        //   use: { ...devices['Pixel 5'] },
        // },
        // {
        //   name: 'Mobile Safari',
        //   use: { ...devices['iPhone 12'] },
        // },

        /* Test against branded browsers. */
        // {
        //   name: 'Microsoft Edge',
        //   use: { ...devices['Desktop Edge'], channel: 'msedge' },
        // },
        // {
        //   name: 'Google Chrome',
        //   use: { ...devices['Desktop Chrome'], channel: 'chrome' },
        // },
    ],

    /* Run your local dev server before starting the tests */
    webServer: {
        command: buildCommand(),
        url: process.env.WEBAPP_URL,
        reuseExistingServer: true
    },
});

function buildCommand() {

    let command = `cd ../../src/Dfe.EarlyYearsQualification.Web && dotnet run `
        + `--environment ASPNETCORE_ENVIRONMENT="Development" `
        + `--urls "${process.env.WEBAPP_URL}" `
        + `--project ./Dfe.EarlyYearsQualification.Web.csproj `
        + `--UseMockContentful="${process.env.USE_MOCK_CONTENTFUL ?? true}" `
        + `--RunValidationTests="${process.env.RUN_VALIDATION_TESTS ?? false}" `
        + `--ServiceAccess:Keys:0="${process.env.AUTH_SECRET}" `
        + `--ContentfulOptions:UsePreviewApi="${process.env.USE_MOCK_CONTENTFUL ?? false}" `
        + `--UpgradeInsecureRequests="${process.env.UPGRADE_INSECURE_REQUESTS ?? true}" `
        + `--ServiceAccess:IsPublic="${process.env.IS_PUBLIC}" `;


    if (process.env.CONTENTFUL_DELIVERY_API_KEY !== undefined) {
        command += `--ContentfulOptions:DeliveryApiKey="${process.env.CONTENTFUL_DELIVERY_API_KEY}" `;
    }

    if (process.env.CONTENTFUL_SPACE_ID !== undefined) {
        command += `--ContentfulOptions:SpaceId="${process.env.CONTENTFUL_SPACE_ID}" `;
    }

    return command;
}