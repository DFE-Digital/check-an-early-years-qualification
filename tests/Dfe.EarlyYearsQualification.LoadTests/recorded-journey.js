// Adapted from recordings created by Grafana k6 Browser Recorder 1.0.4

import { sleep, group, check } from 'k6'
import http from 'k6/http'

export const options = {
  stages: [
    { duration: '10m', target: 100 },
    { duration: '5m', target: 100 },
    { duration: '10m', target: 300 },
    { duration: '5m', target: 300 },
    { duration: '5m', target: 20 },
    { duration: '3m', target: 0 }
  ]
}

const ENVIRONMENT = {
  password: __ENV.CHALLENGE_PASSWORD, // secret value, passed in from the CLI
  customDomain: __ENV.CUSTOM_DOMAIN // custom domain, the address of the service to test, passed in from the CLI
};

export default function main() {
  let response

  const cookieJar = http.cookieJar();
  const address = 'https://' + ENVIRONMENT.customDomain;

  cookieJar.set(address, 'auth-secret', ENVIRONMENT.password);

  let antiForgeryToken;
  let requestVerificationToken;

  group(
    `page_1 - ${address}/`,
    function () {
      response = http.get(`${address}/`, {
        headers: {
          'upgrade-insecure-requests': '1',
          'user-agent':
            'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
          'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
          'sec-ch-ua-mobile': '?0',
          'sec-ch-ua-platform': '"Windows"',
        },
      })

      const cookies = cookieJar.cookiesForURL(address);

      antiForgeryToken = cookies['.AspNetCore.Antiforgery'][0];

      requestVerificationToken = getRequestVerificationTokenValue(response);

      check(response, {
        "has cookie 'auth-secret'": (r) => cookies['auth-secret'].length > 0,
        "'auth-secret' cookie has expected value": (r) => cookies['auth-secret'][0] === ENVIRONMENT.password,
        'anti-forgery token has value': (r) => antiForgeryToken && antiForgeryToken.length > 0
      });

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              '${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              '${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              '${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/images/favicon.svg`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
    }
  )

  group(
    `page_2 - ${address}/questions/start-new`,
    function () {
      response = http.get(
        `${address}/questions/start-new`,
        {
          headers: {
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      check(response, {
        'request-verification token has value': (r) => requestVerificationToken && requestVerificationToken.length > 0
      })

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/favicon.svg`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.post(
        `${address}/questions/where-was-the-qualification-awarded`,
        {
          Option: 'england',
          __RequestVerificationToken: requestVerificationToken,
        },
        {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
            origin: 'null',
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.post(
        `${address}/questions/when-was-the-qualification-started`,
        {
          SelectedMonth: '9',
          SelectedYear: '2014',
          __RequestVerificationToken: requestVerificationToken,
        },
        {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
            origin: 'null',
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.post(
        `${address}/questions/what-level-is-the-qualification`,
        {
          Option: '3',
          __RequestVerificationToken: requestVerificationToken,
        },
        {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
            origin: 'null',
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.post(
        `${address}/questions/what-is-the-awarding-organisation`,
        {
          SelectedValue: 'NCFE',
          __RequestVerificationToken: requestVerificationToken,
          NotInTheList: 'false',
        },
        {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
            origin: 'null',
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
    }
  )

  group(
    `page_3 - ${address}/confirm-qualification/EYQ-224`,
    function () {
      response = http.get(
        `${address}/confirm-qualification/EYQ-224`,
        {
          headers: {
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
    }
  )

  group(
    `page_4 - ${address}/confirm-qualification`,
    function () {
      response = http.post(
        `${address}/confirm-qualification`,
        {
          qualificationId: 'EYQ-224',
          ConfirmQualificationAnswer: 'yes',
          __RequestVerificationToken: requestVerificationToken,
        },
        {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
            origin: 'null',
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.post(
        `${address}/qualifications/check-additional-questions/EYQ-224/1`,
        {
          qualificationId: 'EYQ-224',
          questionIndex: '1',
          question: "Does the qualification name include 'Early Years Educator'?",
          Answer: 'yes',
          __RequestVerificationToken: requestVerificationToken,
        },
        {
          headers: {
            'content-type': 'application/x-www-form-urlencoded',
            origin: 'null',
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
    }
  )

  group(
    `page_5 - ${address}/qualifications/qualification-details/EYQ-224`,
    function () {
      response = http.get(
        `${address}/qualifications/qualification-details/EYQ-224`,
        {
          headers: {
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )

      requestVerificationToken = getRequestVerificationTokenValue(response);

      response = http.get(
        `${address}/lib/bootstrap/dist/css/bootstrap.min.css`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/css/site.css?v=igoQaqnM1Mg8CnrQJH6phEDWpG58m_dbMUgZxK0EsYY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/lib/bootstrap/dist/js/bootstrap.bundle.min.js`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/js/site.js?v=PVhEInckNOQLnX1CbKZYN9JbKFv7pI2XJKwv1s_zsGY`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.js?v=qWAac5Qu0FixAEn44iyug6gG9I_RILPlCYO7ZpK9z_0`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
          headers: {
            origin: address,
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/govuk/all.min.js`,
        {
          headers: {
            origin: address,
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/images/icon-print.png`,
        {
          headers: {
            referer: '',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
      response = http.get(
        `${address}/assets/images/govuk-crest.png`,
        {
          headers: {
            referer:
              `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
            'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
          },
        }
      )
    }
  )

  sleep(1)
}

function getRequestVerificationTokenValue(response) {

  const requestVerificationTokenInput = response.html().find('input[name=__RequestVerificationToken]');

  const requestVerificationToken = requestVerificationTokenInput.attr('value');

  return requestVerificationToken;
}