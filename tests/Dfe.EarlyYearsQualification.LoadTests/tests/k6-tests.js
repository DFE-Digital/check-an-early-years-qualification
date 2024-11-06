import http from 'k6/http';
import { sleep } from 'k6';

export async function happyJourney(data) {

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

export function verifyStatusCodeTest(data) {
  const res = http.get(`${data.SETTINGS.baseUrl}/ServiceFilter?postcode=E1%202EN&adminarea=E09000030&latitude=51.517612&longitude=-0.056838&frompostcodesearch=True')`);

  check(res, {
    'status is 200': (r) => r.status === 200,
  });
}