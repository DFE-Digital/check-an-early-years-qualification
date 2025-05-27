import { sleep } from 'k6';

import { browser } from 'k6/browser';

export default async function ncfeSearchJourney(ENVIRONMENT, DATA) {

  const address = 'https://' + ENVIRONMENT.customDomain;

  if (browser.context() == null) {
    await browser.newContext();
  }

  const page = await browser.newPage();

  const context = page.context();

  // set up password from environment
  await context.addCookies([
    { name: "auth-secret", value: ENVIRONMENT.password, sameSite: "Strict", domain: ENVIRONMENT.customDomain, path: "/", httpOnly: true, secure: true },
  ]);

  try {

    await page.goto(address, { waitUntil: "networkidle" });

    let submitButton = page.locator(".govuk-button--start");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    await page.locator("input[value='england']").check();

    submitButton = page.locator("button[id='question-submit']");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    await page.locator("input[name='StartedQuestion.SelectedMonth']").type("9");
    await page.locator("input[name='StartedQuestion.SelectedYear']").type("2016");
    await page.locator("input[name='AwardedQuestion.SelectedMonth']").type("6");
    await page.locator("input[name='AwardedQuestion.SelectedYear']").type("2018");

    submitButton = page.locator("button[id='question-submit']");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    await page.locator("input[value='3']").check();

    submitButton = page.locator("button[id='question-submit']");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    const awardingOrgOptions = page.locator('#awarding-organisation-select');

    await awardingOrgOptions.selectOption("NCFE");

    submitButton = page.locator("button[id='question-submit']");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    submitButton = page.locator("#cta-button");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    const selectedOrg = page.locator("//a[@href='/confirm-qualification/EYQ-224']");

    await Promise.all([page.waitForNavigation(), selectedOrg.click()]);

    await page.locator("input[value='yes']").check();

    submitButton = page.locator("#confirm-qualification-button");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    await page.locator("input[value='yes']").check();

    submitButton = page.locator("#additional-requirement-button");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

    submitButton = page.locator("#confirm-answers");

    await Promise.all([page.waitForNavigation(), submitButton.click()]);

  } finally {

    await page.close();
  }

  sleep(1)
}