export const getApiBaseUrl = (): string => {
  const hostname = window.location.hostname

  if (hostname.includes("localhost")) {
    return "https://localhost:7059/api"
  } else if (hostname.includes("staging")) { // assuming hostname https://code-test-ui-staging.com
    return "https://code-test-dao-lam-staging.com/api"
  } else { // assuming hostname https://code-test-ui.com
    return "https://code-test-dao-lam.com/api"
  }
};