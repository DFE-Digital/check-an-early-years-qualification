import {test} from '@playwright/test';
import {checkUrl, checkValue} from '../../_shared/playwrightWrapper';

test.describe('A spec used to check a new user is challenged to enter the secret', {tag: "@e2e"}, () => {
    test("should redirect the user to the challenge page", async ({page}) => {
        await page.goto("/");
        await checkUrl(page, "/challenge?redirectAddress=%2F");
        await checkValue(page, "#redirectAddress", "/");
        await checkValue(page, "#PasswordValue", '');
    });
});