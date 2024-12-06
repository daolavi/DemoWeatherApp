import { getApiBaseUrl } from "./getApiBaseUrl";

describe("getApiBaseUrl", () => {
  afterEach(() => {
    jest.resetAllMocks()
  })

  test("returns localhost API URL when hostname contains 'localhost'", () => {
    jest.spyOn(window, "location", "get").mockReturnValue({
      ...window.location,
      hostname: "localhost",
    })

    expect(getApiBaseUrl()).toBe("https://localhost:7059/api");
  })

  test("returns staging API URL when hostname contains 'staging'", () => {
    jest.spyOn(window, "location", "get").mockReturnValue({
      ...window.location,
      hostname: "code-test-ui-staging.com",
    });

    expect(getApiBaseUrl()).toBe("https://code-test-dao-lam-staging.com/api");
  });

  test("returns production API URL for other hostnames", () => {
    jest.spyOn(window, "location", "get").mockReturnValue({
      ...window.location,
      hostname: "code-test-ui.com",
    })

    expect(getApiBaseUrl()).toBe("https://code-test-dao-lam.com/api")
  })
})