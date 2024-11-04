import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {

  // A number specifying the number of VUs to run concurrently.
  vus: 10,

  // A string specifying the total duration of the test run.
  duration: '10s',

  challengeKey: __ENV.CHALLENGE_PASSWORD, // secret value, passed in from the CLI
  customDomain: __ENV.CUSTOM_DOMAIN // custom domain, the address of the service to test, passed in from the CLI

  // The following section contains configuration options for execution of this
  // test script in Grafana Cloud.
  //
  // See https://grafana.com/docs/grafana-cloud/k6/get-started/run-cloud-tests-from-the-cli/
  // to learn about authoring and running k6 test scripts in Grafana k6 Cloud.
  //
  // cloud: {
  //   // The ID of the project to which the test is assigned in the k6 Cloud UI.
  //   // By default tests are executed in default project.
  //   projectID: "",
  //   // The name of the test in the k6 Cloud UI.
  //   // Test runs with the same name will be grouped.
  //   name: "main.js"
  // },

  // Uncomment this section to enable the use of Browser API in your tests.
  //
  // See https://grafana.com/docs/k6/latest/using-k6-browser/running-browser-tests/ to learn more
  // about using Browser API in your test scripts.
  //
  // scenarios: {
  //   // The scenario name appears in the result summary, tags, and so on.
  //   // You can give the scenario any name, as long as each name in the script is unique.
  //   ui: {
  //     // Executor is a mandatory parameter for browser-based tests.
  //     // Shared iterations in this case tells k6 to reuse VUs to execute iterations.
  //     //
  //     // See https://grafana.com/docs/k6/latest/using-k6/scenarios/executors/ for other executor types.
  //     executor: 'shared-iterations',
  //     options: {
  //       browser: {
  //         // This is a mandatory parameter that instructs k6 to launch and
  //         // connect to a chromium-based browser, and use it to run UI-based
  //         // tests.
  //         type: 'chromium',
  //       },
  //     },
  //   },
  // }
};

// The function that defines VU logic.
//
// See https://grafana.com/docs/k6/latest/examples/get-started-with-k6/ to learn more
// about authoring k6 scripts.
//
export default function () {

  const address = 'https://' + options.customDomain + '/';

  const cookieJar = http.cookieJar();

  cookieJar.set(address, 'auth-secret', options.challengeKey);

  const resp = http.get(address);

  const cookies = cookieJar.cookiesForURL(address);

  check(resp, {
    "has cookie 'auth-secret'": (r) => cookies['auth-secret'].length > 0,
    'cookie has expected value': (r) => cookies['auth-secret'][0] === options.challengeKey
  })

  sleep(1);
}
