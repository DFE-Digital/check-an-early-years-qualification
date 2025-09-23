import {test} from '@playwright/test';
import {pages} from "../../_shared/urls-to-check";
import { authorise, checkText, isVisible, setCookie, journeyCookieName } from '../../_shared/playwrightWrapper';

test.describe('A spec that tests the phase banner is showing on all pages', {tag: "@e2e"}, () => {

    test.beforeEach(async ({context}) => {
        await authorise(context);
        await setCookie(context, '%7B%22SelectedQualificationName%22%3A%22%22%2C%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Afalse%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%7D%2C%22QualificationWasSelectedFromList%22%3A1%2C%22HasSubmittedEmailAddressInFeedbackFormQuestion%22%3Afalse%2C%22HasUserGotEverythingTheyNeededToday%22%3A%22%22%2C%22HelpFormEnquiry%22%3A%7B%22ReasonForEnquiring%22%3A%22%22%2C%22QualificationName%22%3A%22NCFE%20CACHE%20Level%203%20Diploma%20in%20Holistic%20Baby%20and%20Child%20Care%20%28Early%20Years%20Educator%29%22%2C%22QualificationStartDate%22%3A%22%22%2C%22QualificationAwardedDate%22%3A%22%22%2C%22AwardingOrganisation%22%3A%22NCFE%22%2C%22AdditionalInformation%22%3A%22%22%7D%2C%22IsUserCheckingTheirOwnQualification%22%3A%22yes%22%7D', journeyCookieName);
    });

    pages.forEach((url) => {
        test(`Checks that the phase banner is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);

            await isVisible(page, ".govuk-phase-banner");
            await checkText(page, ".govuk-phase-banner__content__tag", "Test phase banner name");
            await checkText(page, ".govuk-phase-banner__text", "Some TextLink Text");
        })
    });
});