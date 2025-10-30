# URL Shortener API

A simple and efficient URL shortener service built with ASP.NET Core 9.0 and SQLite. This service allows you to create short URLs, manage them, and track access statistics.

## Features

- **Shorten URLs**: Convert long URLs into short, manageable links
- **Custom Short Codes**: Generate unique 6-character alphanumeric codes
- **Access Tracking**: Monitor how many times each shortened URL is accessed
- **CRUD Operations**: Create, read, update, and delete shortened URLs
- **Statistics**: Get detailed statistics for each shortened URL
- **RESTful API**: Clean and intuitive API endpoints
- **Swagger Documentation**: Interactive API documentation
- **SQLite Database**: Lightweight, file-based database

## Tech Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQLite with Entity Framework Core
- **Language**: C# 13
- **Documentation**: Swagger/OpenAPI
- **ORM**: Entity Framework Core 9.0

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git (for cloning the repository)

## Installation & Setup

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd URL-Shortener/HARDWORK
```

### 2. Restore Dependencies
```bash
dotnet restore API
```

### 3. Apply Database Migrations
```bash
dotnet ef database update --project API
```

### 4. Run the Application
```bash
dotnet run --project API
```

The API will be available at:
- **HTTP**: `http://localhost:5002`
- **Swagger UI**: `http://localhost:5002/swagger` (in development mode)

## API Documentation

### Base URL
```
http://localhost:5002
```

### Endpoints

#### 1. Create Short URL
**POST** `/api/shorten`

Creates a new shortened URL.

**Request Body:**
```json
{
  "url": "https://www.example.com/very/long/url/that/needs/shortening"
}
```

**Response:**
```json
{
  "id": 1,
  "url": "https://www.example.com/very/long/url/that/needs/shortening",
  "shortCode": "aBc123",
  "createdAt": "2025-10-30T10:00:00Z",
  "updatedAt": "2025-10-30T10:00:00Z"
}
```

#### 2. Redirect to Original URL
**GET** `/api/shorten/{shortCode}`

Redirects to the original URL and increments the access count.

**Example:**
```
GET /api/shorten/aBc123
```

**Response:** HTTP 302 Redirect to the original URL

#### 3. Get URL Information
**GET** `/api/shorten/{shortCode}/info`

Returns information about the shortened URL without redirecting.

**Response:**
```json
{
  "id": 1,
  "url": "https://www.example.com/very/long/url/that/needs/shortening",
  "shortCode": "aBc123",
  "createdAt": "2025-10-30T10:00:00Z",
  "updatedAt": "2025-10-30T10:00:00Z"
}
```

#### 4. Update Short URL
**PUT** `/api/shorten/{shortCode}`

Updates the target URL for an existing short code.

**Request Body:**
```json
{
  "newUrl": "https://www.updated-example.com/new/url"
}
```

**Response:**
```json
{
  "id": 1,
  "url": "https://www.updated-example.com/new/url",
  "shortCode": "aBc123",
  "createdAt": "2025-10-30T10:00:00Z",
  "updatedAt": "2025-10-30T10:15:00Z"
}
```

#### 5. Delete Short URL
**DELETE** `/api/shorten/{shortCode}`

Deletes a shortened URL.

**Response:** HTTP 200 OK (if successful) or HTTP 404 Not Found

#### 6. Get URL Statistics
**GET** `/api/shorten/{shortCode}/stats`

Returns detailed statistics for a shortened URL.

**Response:**
```json
{
  "id": 1,
  "url": "https://www.example.com/very/long/url/that/needs/shortening",
  "shortCode": "aBc123",
  "createdAt": "2025-10-30T10:00:00Z",
  "updatedAt": "2025-10-30T10:15:00Z",
  "accessCount": 25
}
```

#### 7. Get All URLs
**GET** `/api/shorten/all`

Returns a list of all shortened URLs.

**Response:**
```json
[
  {
    "id": 1,
    "url": "https://www.example.com/url1",
    "shortCode": "aBc123",
    "createdAt": "2025-10-30T10:00:00Z",
    "updatedAt": "2025-10-30T10:00:00Z",
    "accessCount": 25
  },
  {
    "id": 2,
    "url": "https://www.example.com/url2",
    "shortCode": "XyZ789",
    "createdAt": "2025-10-30T11:00:00Z",
    "updatedAt": "2025-10-30T11:00:00Z",
    "accessCount": 10
  }
]
```

### Status Codes

| Code | Description |
|------|-------------|
| 200 | Success |
| 201 | Created |
| 302 | Redirect (for short URL access) |
| 400 | Bad Request (invalid input) |
| 404 | Not Found (short code doesn't exist) |
| 500 | Internal Server Error |

### Error Response Format

```json
{
  "error": "Error description",
  "statusCode": 400
}
```

## Database Schema

### URLMaps Table

| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key (auto-increment) |
| Url | TEXT | Original URL (max 2048 characters) |
| ShortCode | TEXT | Generated short code (max 10 characters, unique) |
| CreatedAt | TEXT | Timestamp when URL was created |
| UpdatedAt | TEXT | Timestamp when URL was last updated |
| AccessCount | INTEGER | Number of times the short URL was accessed |

## Configuration

### Connection String
The application uses SQLite by default. The connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=urlshortener.db"
  }
}
```

### CORS Configuration
CORS is configured to allow all origins, methods, and headers for development. For production, configure specific allowed origins.

## Testing the API

### Using cURL

**Create a short URL:**
```bash
curl -X POST "http://localhost:5002/api/shorten" \
  -H "Content-Type: application/json" \
  -d '{"url": "https://www.google.com"}'
```

**Access a short URL:**
```bash
curl -X GET "http://localhost:5002/api/shorten/aBc123"
```

**Get URL statistics:**
```bash
curl -X GET "http://localhost:5002/api/shorten/aBc123/stats"
```

### Using Swagger UI
1. Start the application
2. Navigate to `http://localhost:5002/swagger`
3. Use the interactive interface to test all endpoints

## Development

### Project Structure
```
API/
├── Controllers/
│   └── ShortenController.cs    # API endpoints
├── Data/
│   └── URLShortenerContext.cs  # Database context
├── DTOs/
│   └── UrlDto.cs              # Data transfer objects
├── Models/
│   └── URLMap.cs              # Database entities
├── Services/
│   ├── IShortenService.cs     # Service interface
│   └── ShortenService.cs      # Business logic
├── Migrations/                # EF Core migrations
├── Properties/
│   └── launchSettings.json    # Launch configuration
├── Program.cs                 # Application entry point
└── API.csproj                # Project file
```

### Running in Development
```bash
# Watch for file changes and auto-reload
dotnet watch run --project API

# Run with specific environment
ASPNETCORE_ENVIRONMENT=Development dotnet run --project API
```

### Database Migrations
```bash
# Add a new migration
dotnet ef migrations add MigrationName --project API

# Update database
dotnet ef database update --project API

# Remove last migration
dotnet ef migrations remove --project API
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## Support

If you encounter any issues or have questions, please open an issue in the GitHub repository.

---

**Happy URL Shortening!**
