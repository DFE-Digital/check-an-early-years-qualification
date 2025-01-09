﻿import {test} from '@playwright/test';
import {
    checkUrl,
    checkValue
} from '../shared/processLogic';

test.describe('A spec used to check a new user is challenged to enter the secret', () => {
    test("should redirect the user to the challenge page", async ({page}) => {
        await page.goto("/");
        await checkUrl(page, "/challenge?redirectAddress=%2F");
        await checkValue(page, "#redirectAddress", "/");
        await checkValue(page, "#PasswordValue", '');
    });
});