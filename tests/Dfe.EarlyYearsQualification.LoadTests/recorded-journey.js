// Adapted from recording created by Grafana k6 Browser Recorder 1.0.4

import { sleep, group, check } from 'k6'
import http from 'k6/http'

export const options = {
  stages: [
    { duration: '3m', target: 10 },
    { duration: '5m', target: 10 },
    { duration: '10m', target: 50 },
    { duration: '3m', target: 0 }
  ],
  challenge: __ENV.CHALLENGE_PASSWORD, // secret value, passed in from the CLI
  customDomain: __ENV.CUSTOM_DOMAIN // custom domain, the address of the service to test, passed in from the CLI


}

export default function main() {
  let response

  const cookieJar = http.cookieJar();
  const address = 'https://' + options.customDomain + '/';

  cookieJar.set(address, 'auth-secret', options.challenge);

  // Page 1: start
  response = http.get(address);

  const cookies = cookieJar.cookiesForURL(address);

  check(response, {
    "has cookie 'auth-secret'": (r) => cookies['auth-secret'].length > 0,
    'cookie has expected value': (r) => cookies['auth-secret'][0] === options.challenge
  })

  group(
    `page_2 - ${address}questions/start-new`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
          __RequestVerificationToken:
            'CfDJ8AK9tTBzYtFDpZlgo1wVLPwmPItT5fmSdmtSvIXn2AMCppjGdkuTSebPG3ioheos3EyvalJv88CV10gfdVuEvvco40yDkS1O7Ki_jnlwFPo9K8H_bYgIdTrMuTVXaDZzGkAjMIer78sBc8alLMM5Ids',
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
          __RequestVerificationToken:
            'CfDJ8AK9tTBzYtFDpZlgo1wVLPy_sGrFFGMITAgLReUpVjsAllHsg1WmvST2YIWra8VLR-R8zK9tLwifWUxTVBrnTHFpGZ2n-t1pdc5yGewMYE6V1x-rqKPqV_bqX_Mm-eEK64nwAIB4TRRaVQUazKTKios',
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
          __RequestVerificationToken:
            'CfDJ8AK9tTBzYtFDpZlgo1wVLPzTrY2tAkeWXoNbDMRGW0vqrzc8DOVIt69TZP6q_QiLuObScGkAdwc61oARY0slSa55hT2VRPIMVIvqwhR4Uqzz-jmJHDqI7OeDFzvQenjkNzVNF8SpQfzlJZ9A5aAS-jo',
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
          __RequestVerificationToken:
            'CfDJ8AK9tTBzYtFDpZlgo1wVLPx28l7L8gyTAdWeKgeabgYp6ME-sSXKHuC_dZ1tXaUXVn1LNx-IXX2mWM4LaNcmsP62meeZrJnkqE1fnJduP5WKN-ASSJexlSuZXJOaWyaJFHJ3RYx8CKxl8JPynrgnh50',
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
          __RequestVerificationToken:
            'CfDJ8AK9tTBzYtFDpZlgo1wVLPxg4rfJucdN41aAUvN1qkPPojHKgT0ofJxkRm4IEjDTHW2GnMfpXPvKwDB5qjcyWdTgneyDGKkBNO9_g3EyYwXbbn_STS-MvDMR_Me1EDFaWK37Qypt2SEknuBaw0q3gg4',
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
          __RequestVerificationToken:
            'CfDJ8AK9tTBzYtFDpZlgo1wVLPwXnL4MQo-VI3NFL5e6yey5VtcIlyoI-Gq88rOMCB57Kmuj8qAfkj7jvmj4qtH1aJrvPGCn4e3lqTwTLGzYw4thkF2pNgXmYqcxyKBf6dnBOyFSzb6RCfpcSOeEo7qY0OI',
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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
            origin: `${address}`,
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

  // Automatically added sleep
  sleep(1)
}
