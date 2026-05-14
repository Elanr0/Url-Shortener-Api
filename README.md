# URL Shortener API

A simple URL shortener API built with ASP.NET Core Minimal API.

## About the Project

This project is a beginner-friendly backend API that shortens long URLs and redirects users to the original URL using a generated short code.

The main purpose of this project is to understand the basics of backend development, API endpoints, service structure, dependency injection, and Swagger testing.

## Technologies Used

- C#
- ASP.NET Core Minimal API
- Swagger / Swashbuckle
- .NET
- Visual Studio Code

## Project Structure

```txt
UrlShortener
├── Models
│   ├── ShortUrl.cs
│   └── CreateShortUrlRequest.cs
├── Services
│   └── UrlShortenerService.cs
├── Program.cs
├── appsettings.json
└── UrlShortener.csproj
```

## Features

- Create a short URL from a long URL
- Redirect to the original URL using a short code
- List all shortened URLs
- Test endpoints with Swagger UI

## Endpoints

### GET /

Checks if the API is running.

```txt
GET http://localhost:5000/
```

---

### POST /shorten

Creates a shortened URL.

Request body:

```json
{
  "originalUrl": "https://google.com"
}
```

Example response:

```json
{
  "originalUrl": "https://google.com",
  "shortCode": "5711db",
  "shortUrl": "http://localhost:5000/5711db"
}
```

---

### GET /{shortCode}

Redirects the user to the original URL.

Example:

```txt
GET http://localhost:5000/5711db
```

---

### GET /urls/all

Returns all shortened URLs stored in memory.

```txt
GET http://localhost:5000/urls/all
```

## How to Run

Clone the project:

```bash
git clone https://github.com/your-username/url-shortener-api.git
```

Go to the project folder:

```bash
cd url-shortener-api
```

Run the project:

```bash
dotnet run
```

Open Swagger UI:

```txt
http://localhost:5000/swagger
```

## Current Limitations

- Data is stored in memory, so records are deleted when the application stops.
- There is no database yet.
- Short code collision control is not fully handled.
- There is no user authentication.
- There is no click tracking yet.

## Planned Improvements

- Add database support with SQLite and Entity Framework Core
- Add click count tracking
- Add custom short codes
- Add expiration date for short links
- Improve error responses
- Add a simple frontend interface

## Project Status

This project is currently in development.
