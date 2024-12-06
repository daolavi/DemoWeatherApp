import { render } from "@testing-library/react";
import App from "./App";

test("renders CityInformationDetails component", () => {
  const { container } = render(<App />);
  const cityInfoComponent = container.getElementsByClassName("city-info-container")
  expect(cityInfoComponent).not.toBeNull()
});
