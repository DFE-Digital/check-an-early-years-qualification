
export function getRequestVerificationTokenValue(response) {

  const requestVerificationTokenInput = response.html().find('input[name=__RequestVerificationToken]');

  const requestVerificationToken = requestVerificationTokenInput.attr('value');

  return requestVerificationToken;
}