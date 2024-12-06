import React from "react"
import {render, screen, fireEvent, waitFor} from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import {CityInformationDetails} from "./cityInformationDetails"

global.fetch = jest.fn()

describe("CityInformationDetails", () => {
  beforeEach(() => {
    jest.clearAllMocks()
  })

  test("renders the search bar and input elements", () => {
    render(<CityInformationDetails/>)
    expect(screen.getByPlaceholderText("Enter city name")).toBeInTheDocument()
    expect(screen.getByRole("button", {name: /Get Information/i})).toBeInTheDocument()
  })

  test("handles user input correctly", async () => {
    render(<CityInformationDetails/>)
    const input = screen.getByPlaceholderText("Enter city name")

    await userEvent.type(input, "London")

    expect((input as HTMLInputElement).value).toBe("London")
  })

  test("displays loading state when fetching data", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: true,
      json: async () => ({
        city: "London",
        region: "City of London, Greater London",
        country: "United Kingdom",
        localTime: "2024-12-06 06:06",
        temperature: 7.1,
        sunrise: "07:51 AM",
        sunset: "03:52 PM",
      }),
    })

    render(<CityInformationDetails/>)
    const input = screen.getByPlaceholderText("Enter city name")
    const button = screen.getByRole("button", {name: /Get Information/i})

    await userEvent.type(input, "London")
    fireEvent.click(button)

    expect(button).toHaveTextContent("Loading...")
    await waitFor(() => expect(button).toHaveTextContent("Get Information"))
  })

  test("fetches and displays city details correctly", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: true,
      json: async () => ({
        city: "London",
        region: "City of London, Greater London",
        country: "United Kingdom",
        localTime: "2024-12-06 06:06",
        temperature: 7.1,
        sunrise: "07:51 AM",
        sunset: "03:52 PM",
      }),
    })

    render(<CityInformationDetails/>)
    const input = screen.getByPlaceholderText("Enter city name")
    const button = screen.getByRole("button", {name: /Get Information/i})

    await userEvent.type(input, "London")
    fireEvent.click(button)

    await waitFor(() => expect(screen.getByText("City Information")).toBeInTheDocument())
    expect(screen.getByTestId("city").textContent).toBe("City: London")
    expect(screen.getByTestId("region").textContent).toBe("Region: City of London, Greater London")
    expect(screen.getByTestId("country").textContent).toBe("Country: United Kingdom")
    expect(screen.getByTestId("localTime").textContent).toBe("Local Time: 2024-12-06 06:06")
    expect(screen.getByTestId("temperature").textContent).toBe("Temperature: 7.1°C")
    expect(screen.getByTestId("sunrise").textContent).toBe("Sunrise: 07:51 AM")
    expect(screen.getByTestId("sunset").textContent).toBe("Sunset: 03:52 PM")
  })

  test("handles API errors correctly", async () => {
    (fetch as jest.Mock).mockRejectedValueOnce(new Error("Failed to fetch city details"))

    render(<CityInformationDetails/>)
    const input = screen.getByPlaceholderText("Enter city name")
    const button = screen.getByRole("button", {name: /Get Information/i})

    await userEvent.type(input, "Invalid City")
    fireEvent.click(button)

    await waitFor(() => expect(screen.getByText(/Error: Failed to fetch city details/i)).toBeInTheDocument())
  })

  test("displays a message when no details are available", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: false,
      json: async () => ({"status": 404, "detail": "Invalid city provided"}),
    })

    render(<CityInformationDetails/>)
    const input = screen.getByPlaceholderText("Enter city name")
    const button = screen.getByRole("button", {name: /Get Information/i})

    await userEvent.type(input, "Unknown City")
    fireEvent.click(button)

    await waitFor(() => expect(screen.getByText(/Error: Invalid city provided/i)).toBeInTheDocument())
  })
})