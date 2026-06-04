# URL Shortener API

A URL shortening API built with ASP.NET Core, Entity Framework Core, SQLite, and JWT Authentication.

## Features

- User Registration
- User Login
- JWT Authentication
- Password Hashing (SHA256)
- Create Short URLs
- Custom Short Codes
- URL Redirection
- Click Count Tracking
- User-Specific URL Management
- URL History
- SQLite Database
- Entity Framework Core Migrations
- Swagger UI Documentation

## Technologies

- ASP.NET Core
- C#
- Entity Framework Core
- SQLite
- JWT Authentication
- Swagger / OpenAPI

## Project Structure

```text
UrlShortener
│
├── Data
│   └── AppDbContext.cs
│
├── Models
│   ├── User.cs
│   └── ShortUrl.cs
│
├── Requests
│   ├── RegisterRequest.cs
│   ├── LoginRequest.cs
│   └── CreateShortUrlRequest.cs
│
├── Services
│   ├── AuthServices.cs
│   ├── TokenService.cs
│   └── UrlShortenerService.cs
│
└── Program.cs
```

## Authentication

Users can register and log in using:

- Email Address
- Phone Number

After a successful login, the API generates a JWT token.

Protected endpoints require authentication using:

```text
Bearer <your_token>
```

## API Endpoints

### Authentication

```http
POST /auth/register
POST /auth/login
```

### URL Management

```http
POST /shorten
GET /users/{userId}/urls
GET /{shortCode}
```

## Database

SQLite is used as the database provider.

Main tables:

### Users

```text
Id
Email
PhoneNumber
PasswordHash
CreatedAt
```

### ShortUrls

```text
Id
OriginalUrl
ShortCode
CreatedAt
ClickCount
UserId
```

## Security

- JWT Authentication
- Password Hashing
- User-Based Authorization
- Protected Endpoints

## Future Improvements

- Refresh Tokens
- URL Update
- URL Delete
- Admin Panel
- Analytics Dashboard
- Frontend Application

## Author

Elanur Karaca
