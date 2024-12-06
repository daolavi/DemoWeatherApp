import React, {useState, useCallback} from "react"
import "../App.css"
import {getApiBaseUrl} from "../helpers/getApiBaseUrl"

interface CityInformationData {
  city: string
  region: string
  country: string
  localTime: string
  temperature: string
  sunrise: string
  sunset: string
}

export const CityInformationDetails = () => {
  const [inputCity, setInputCity] = useState<string>("")
  const [details, setDetails] = useState<CityInformationData | null>(null)
  const [loading, setLoading] = useState<boolean>(false)
  const [error, setError] = useState<string | null>(null)

  const fetchCityDetails = useCallback(async (city: string) => {
    setLoading(true)
    setError(null)

    try {
      const baseUrl = getApiBaseUrl()
      const res = await fetch(`${baseUrl}/CityInformation/${city}`)
      if (res.ok) {
        const data: CityInformationData = await res.json()
        setDetails(data)
      } else {
        const error = await res.json()
        setError(error.detail)
        setDetails(null)
      }
    } catch (err: any) {
      setError(err.message || "An unexpected error occurred")
      setDetails(null)
    } finally {
      setLoading(false)
    }
  }, [])

  const handleInputChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setInputCity(e.target.value)
  }, [])

  const handleSubmit = useCallback(() => {
    if (inputCity.trim()) {
      void fetchCityDetails(inputCity)
    }
  }, [inputCity, fetchCityDetails])

  return (
    <div className="city-info-container">
      <div className="search-bar">
        <input
          type="text"
          value={inputCity}
          onChange={handleInputChange}
          placeholder="Enter city name"
        />
        <button type="button" onClick={handleSubmit} disabled={loading}>
          {loading ? "Loading..." : "Get Information"}
        </button>
      </div>

      {error && <div className="error-message">Error: {error}</div>}

      {details && (
        <div className="details-container">
          <h2 className="details-heading">City Information</h2>
          <div className="details-item" data-testid="city">
            <span className="details-label">City:</span> {details.city}
          </div>
          <div className="details-item" data-testid="region">
            <span className="details-label">Region:</span> {details.region}
          </div>
          <div className="details-item" data-testid="country">
            <span className="details-label">Country:</span> {details.country}
          </div>
          <div className="details-item" data-testid="localTime">
            <span className="details-label">Local Time:</span> {details.localTime}
          </div>
          <div className="details-item" data-testid="temperature">
            <span className="details-label">Temperature:</span> {details.temperature}&deg;C
          </div>
          <div className="details-item" data-testid="sunrise">
            <span className="details-label">Sunrise:</span> {details.sunrise}
          </div>
          <div className="details-item" data-testid="sunset">
            <span className="details-label">Sunset:</span> {details.sunset}
          </div>
        </div>
      )}
    </div>
  )
}