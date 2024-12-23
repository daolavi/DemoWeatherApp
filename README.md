# DemoWeatherApp

## Introduction

DemoWeatherApp is a simple web application that displays the local time, temperature, and sunrise/sunset times for a specified city. It leverages a React frontend and backend integration with a weather API.

## Features

- **Weather Data Integration:**  
  Utilises the [WeatherAPI](https://www.weatherapi.com/) to fetch weather and astronomical data:  
  - **Current Weather:**  
    Endpoint: `http://api.weatherapi.com/v1/current.json`  
    Provides details such as city location, temperature, and local time.  
  - **Astronomy Data:**  
    Endpoint: `http://api.weatherapi.com/v1/astronomy.json`  
    Fetches sunrise and sunset times.  

- **Interactive User Interface:**  
  A React-based web page where users can enter a city name to view its weather details.

## Prerequisites

Ensure the following are installed on your system:

- **.NET 8 SDK & Runtime**  
- **Node.js**

## Quick Start

1. **Run the Backend**  
   Start the .NET backend by executing:  
   ```bash
   dotnet run
   ```
2. **Run the Frontend**
   Navigate to the React frontend directory and run the following commands:
   ```bash
   npm install
   npm start
   ```

## Author

- Dao Lam
