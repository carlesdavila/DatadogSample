// src/app/csr/page.tsx
"use client";

import { useEffect, useState } from 'react';
import { Weather } from '@/types/Weather';

export default function CSRPage() {
  const [weatherData, setWeatherData] = useState<Weather[]>([]);

  useEffect(() => {
    async function fetchWeather() {
      const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/weather/weather`);
      const data: Weather[] = await res.json();
      setWeatherData(data);
    }

    fetchWeather();
  }, []);

  return (
    <div>
      <h1>CSR: Client-Side Rendering</h1>
      {weatherData.map((weather: Weather, index: number) => (
        <div key={index}>
          <h2>{weather.date}</h2>
          <p>Temperature: {weather.temperatureC}°C</p>
          <p>Summary: {weather.summary}</p>
        </div>
      ))}
    </div>
  );
}