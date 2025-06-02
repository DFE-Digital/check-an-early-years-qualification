import { browser } from 'k6/browser';
import { sleep } from 'k6';

export const options = {
  scenarios: {
    ui: {
      executor: 'shared-iterations',
      options: {
        browser: {
          type: 'chromium'
        }
      }
    }
  },
  thresholds: {
    checks: ['rate==1.0']
  }
};

const ENVIRONMENT = {
  challenge: __ENV.CHALLENGE_PASSWORD, // secret value, passed in from the CLI
  customDomain: __ENV.CUSTOM_DOMAIN // custom domain, the address of the service to test, passed in from the CLI
}

// The function that defines VU logic.
//
// See https://grafana.com/docs/k6/latest/examples/get-started-with-k6/ to learn more
// about authoring k6 scripts.
//
export default async function () {

  const address = 'https://' + ENVIRONMENT.customDomain + '/';

  const context = await browser.newContext();
  const page = await context.newPage();

  try {

    await context.addCookies([
      { name: 'auth-secret', value: ENVIRONMENT.challenge, sameSite: 'Strict', url: address }
    ]);

    await page.goto(address,);

    await page.screenshot({ path: 'screenshots/screenshot.png' });

    sleep(1);
  }
  finally {

    await page.close();

  }
}
