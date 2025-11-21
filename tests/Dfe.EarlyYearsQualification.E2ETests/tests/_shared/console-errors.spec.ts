import {test, expect} from '@playwright/test'

const logs: {
    message: string;
    type: string;
}[] = []

const errorMsg: {
    name: string,
    message: string
}[] = []

test.describe('error and console log check', {tag: ["@e2e", "@smoke"]}, () => {
    test.beforeEach(async ({page}) => {
        //get the logs
        page.on('console', (msg) => {
            if (msg.type() == 'error') {
                logs.push({message: msg.text(), type: msg.type()})
            }
        });

        //get the errors
        page.on('pageerror', (error) => {
            errorMsg.push({name: error.name, message: error.message})
        });
    })

    test('Check the exception and logs in console log', async ({page}, testInfo) => {

        await page.goto('/');

        const errorLogs = [...logs?.map(e => e.message)].join('\n');

        if (logs?.length > 0) {
            await testInfo.attach('console logs', {
                body: errorLogs.toString(),
                contentType: 'text/plain'
            });
        }

        expect.soft(logs.length).toBe(0);
        expect(errorMsg.length).toBe(0);
    })
})