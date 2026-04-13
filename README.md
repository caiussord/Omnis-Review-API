
# Omnis Review API

A modern ASP.NET Core REST API that integrates with **TMDB (The Movie Database)**, **Google Books**, and **RAWG Video Game Database** APIs, providing comprehensive entertainment data, book information, and video game database capabilities.

## Features

- **TMDB Integration**: Search and retrieve detailed information about movies and TV series
- **Google Books Integration**: Search books by title, author, ISBN, and publisher
- **RAWG Integration**: Complete video game database with search, filtering, and advanced queries
- **RESTful API**: Clean, well-documented endpoints following REST conventions
- **Full Test Coverage**: 70+ unit tests with 100% passing rate
- **Secure Configuration**: API keys managed through User Secrets
- **Structured Logging**: Comprehensive diagnostic tracing with ILogger
- **Dependency Injection**: Built-in .NET DI container for loose coupling
- **Authentication & Authorization**: JWT tokens with ASP.NET Core Identity
- **Database**: Entity Framework Core with SQL Server

## Technology Stack

- **.NET 10** with **C# 14.0**
- **ASP.NET Core** REST API framework
- **Entity Framework Core** for data persistence
- **SQL Server** database backend
- **NUnit 4.x** + **Moq** for comprehensive testing
- **Swagger/OpenAPI** for API documentation
- **System.Text.Json** for JSON serialization
- **ASP.NET Core Identity** for authentication

## Getting Started

### Prerequisites

- .NET 10 SDK or later
- SQL Server (local or remote instance)
- API Keys:
  - [TMDB API Key](https://www.themoviedb.org/settings/api)
  - [Google Books API Key](https://console.cloud.google.com/)
  - [RAWG API Key](https://rawg.io/api-console)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/caiussord/Omnis-Review-API.git
   cd Omnis-Review-API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure User Secrets** (development only)
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "Tmdb:ApiKey" "YOUR_TMDB_API_KEY_HERE"
   dotnet user-secrets set "GoogleBooks:ApiKey" "YOUR_GOOGLE_BOOKS_API_KEY_HERE"
   dotnet user-secrets set "Rawg:ApiKey" "YOUR_RAWG_API_KEY_HERE"
   ```

4. **Update database connection**
   - Edit `appsettings.json` with your SQL Server connection string
   - Run Entity Framework migrations:
     ```bash
     dotnet ef database update
     ```

5. **Run the API**
   ```bash
   dotnet run
   ```

   The API will be available at:
   - **HTTP**: `http://localhost:5168`
   - **HTTPS**: `https://localhost:7079`
   - **Swagger UI**: `http://localhost:5168/swagger`

---

## TMDB API Integration

### Overview

The TMDB (The Movie Database) integration provides access to comprehensive movie and TV series data, including search, details, cast information, and videos.

**Base URL**: `https://api.themoviedb.org/3`

### 🇧🇷 Language Support

By default, all TMDB endpoints return results in **Portuguese (Brazilian - pt-BR)**. You can override the language for individual requests by passing the `language` query parameter:

```bash
# Portuguese (default)
curl "http://localhost:5168/api/tmdb/movies/search?query=Matrix&page=1"

# English
curl "http://localhost:5168/api/tmdb/movies/search?query=Matrix&page=1&language=en-US"

# Other languages (ISO 639-1 code)
curl "http://localhost:5168/api/tmdb/movies/search?query=Matrix&page=1&language=es-ES"
```

Supported language codes: `en-US`, `pt-BR`, `es-ES`, `fr-FR`, `de-DE`, `it-IT`, `ja-JP`, `zh-CN`, and others as supported by TMDB API.

### Key Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tmdb/movies/search?query={query}&page={page}` | Search movies by title |
| GET | `/api/tmdb/series/search?query={query}&page={page}` | Search TV series by title |
| GET | `/api/tmdb/movies/{id}` | Get movie details |
| GET | `/api/tmdb/series/{id}` | Get series details |
| GET | `/api/tmdb/movies/{id}/cast` | Get movie cast information |
| GET | `/api/tmdb/series/{id}/cast` | Get series cast information |
| GET | `/api/tmdb/movies/{id}/videos` | Get movie videos (trailers, clips) |
| GET | `/api/tmdb/series/{id}/videos` | Get series videos |
| GET | `/api/tmdb/movies/popular?page={page}` | Get popular movies |
| GET | `/api/tmdb/series/popular?page={page}` | Get popular TV series |
| GET | `/api/tmdb/movies/top-rated?page={page}` | Get top-rated movies |
| GET | `/api/tmdb/series/top-rated?page={page}` | Get top-rated TV series |

### Example Requests

**Search Movies:**
```bash
curl "http://localhost:5168/api/tmdb/movies/search?query=Fight%20Club&page=1"
```

**Get Movie Details:**
```bash
curl "http://localhost:5168/api/tmdb/movies/550"
```

**Get Movie Cast:**
```bash
curl "http://localhost:5168/api/tmdb/movies/550/cast"
```

### Example Response (Movie Search)
```json
{
  "results": [
    {
      "id": 550,
      "title": "Fight Club",
      "overview": "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into much more.",
      "poster_path": "/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJA.jpg",
      "release_date": "1999-10-15"
    }
  ],
  "total_results": 42,
  "total_pages": 3
}
```

---

## Google Books API Integration

### Overview

The Google Books integration provides comprehensive book search and discovery capabilities, with support for multiple search methods including general search, title, author, ISBN, and publisher queries.

**Base URL**: `https://www.googleapis.com/books/v1`

### Key Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/googlebooks/search?query={query}&startIndex={idx}&maxResults={max}` | General book search |
| GET | `/api/googlebooks/search/title?title={title}` | Search by book title |
| GET | `/api/googlebooks/search/author?author={author}` | Search by author name |
| GET | `/api/googlebooks/search/isbn?isbn={isbn}` | Search by ISBN (exact match) |
| GET | `/api/googlebooks/search/publisher?publisher={publisher}` | Search by publisher |
| GET | `/api/googlebooks/{bookId}` | Get book details by ID |

### Response Structure

All responses include:
- `kind`: Response type identifier
- `totalItems`: Total matches found
- `items`: Array of book results with full metadata

**Book Data Includes:**
- Title, authors, description
- Categories and ratings (average rating & count)
- Image links (6 sizes: thumbnail to extra-large)
- ISBN, publication info, page count
- Self links for further API calls

### Example Requests

**General Search:**
```bash
curl "http://localhost:5168/api/googlebooks/search?query=Harry%20Potter&maxResults=5"
```

**Search by Title:**
```bash
curl "http://localhost:5168/api/googlebooks/search/title?title=The%20Great%20Gatsby"
```

**Search by Author:**
```bash
curl "http://localhost:5168/api/googlebooks/search/author?author=J.K.%20Rowling&maxResults=10"
```

**Search by ISBN:**
```bash
curl "http://localhost:5168/api/googlebooks/search/isbn?isbn=9780747532699"
```

**Get Book by ID:**
```bash
curl "http://localhost:5168/api/googlebooks/WQW-JQAACAAJ"
```

### Example Response (Book Search)
```json
{
  "kind": "books#volumes",
  "totalItems": 8456,
  "items": [
    {
      "id": "WQW-JQAACAAJ",
      "volumeInfo": {
        "title": "Harry Potter and the Philosopher's Stone",
        "authors": ["J.K. Rowling"],
        "description": "The first book in the Harry Potter series...",
        "categories": ["Juvenile Fiction", "Fantasy"],
        "averageRating": 4.7,
        "ratingsCount": 4500,
        "imageLinks": {
          "smallThumbnail": "http://...",
          "thumbnail": "http://...",
          "large": "http://..."
        }
      }
    }
  ]
}
```

### Constraints

- **Max Results Cap**: Paginated endpoints capped at 40 results to prevent abuse
- **Pagination**: Use `startIndex` parameter for offset-based pagination
- **ISBN Search**: Returns only exact matches (no pagination)
- **Required Parameters**: Query parameters are required for their respective endpoints

---

## RAWG Video Game Database Integration

### Overview

The RAWG integration provides access to the world's largest video game database with comprehensive game data, including search, filtering by genres, platforms, and sorting capabilities.

**Base URL**: `https://api.rawg.io/api`

### Key Features

- **7 Search Methods**: Multiple ways to discover games
- **Advanced Filtering**: Filter by genre, platform, release date, and more
- **Detailed Game Info**: Ratings, platforms, stores, screenshots, tags
- **Pagination Support**: Configurable page size (up to 40 results per page)
- **Performance**: Optimized for fast responses with comprehensive logging

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/rawg/search?query={query}&page={page}&pageSize={size}` | General game search |
| GET | `/api/rawg/search/genre?genre={genre}&page={page}&pageSize={size}` | Search by genre |
| GET | `/api/rawg/search/platform?platform={platform}&page={page}&pageSize={size}` | Search by platform |
| GET | `/api/rawg/{gameId}` | Get game details by ID |
| GET | `/api/rawg/popular?page={page}&pageSize={size}` | Get most popular games |
| GET | `/api/rawg/upcoming?page={page}&pageSize={size}` | Get upcoming releases |
| GET | `/api/rawg/sort/{sortBy}?page={page}&pageSize={size}` | Get games sorted by criteria |

### Supported Genres

Action, Adventure, Casual, Educational, Fighting, Indie, Massively Multiplayer, Platform, Puzzle, Racing, Role-playing, Shooter, Simulation, Sports, Strategy

### Supported Platforms

PC, macOS, PlayStation 3, PlayStation 4, PlayStation 5, Xbox 360, Xbox One, Xbox Series X/S, Nintendo Switch, iOS, Android, Web, Wii, Wii U

### Example Requests

**Search Games:**
```bash
curl "http://localhost:5168/api/rawg/search?query=Call%20of%20Duty&page=1&pageSize=20"
```

**Search by Genre:**
```bash
curl "http://localhost:5168/api/rawg/search/genre?genre=action&page=1&pageSize=20"
```

**Search by Platform:**
```bash
curl "http://localhost:5168/api/rawg/search/platform?platform=pc&page=1&pageSize=20"
```

**Get Popular Games:**
```bash
curl "http://localhost:5168/api/rawg/popular?page=1&pageSize=20"
```

**Get Upcoming Games:**
```bash
curl "http://localhost:5168/api/rawg/upcoming?page=1&pageSize=20"
```

**Get Game by ID:**
```bash
curl "http://localhost:5168/api/rawg/3498"
```

**Get Games Sorted by Rating:**
```bash
curl "http://localhost:5168/api/rawg/sort/rating?page=1&pageSize=20"
```

### Example Response (Game Search)

```json
{
  "count": 58231,
  "next": "https://api.rawg.io/api/games?search=call+of+duty&page=2",
  "previous": null,
  "results": [
    {
      "id": 3,
      "slug": "call-of-duty",
      "name": "Call of Duty",
      "playtime": 6,
      "platforms": [
        {
          "platform": {
            "id": 4,
            "name": "PC",
            "slug": "pc"
          }
        },
        {
          "platform": {
            "id": 14,
            "name": "Xbox 360",
            "slug": "xbox360"
          }
        }
      ],
      "stores": [
        {
          "store": {
            "id": 1,
            "name": "Steam",
            "slug": "steam",
            "domain": "steampowered.com"
          }
        }
      ],
      "released": "2003-10-29",
      "tba": false,
      "background_image": "https://media.rawg.io/media/games/...",
      "rating": 8.5,
      "rating_top": 5,
      "ratings": [
        {
          "id": 5,
          "title": "exceptional",
          "count": 450,
          "percent": 65.2
        },
        {
          "id": 4,
          "title": "recommended",
          "count": 200,
          "percent": 28.9
        }
      ],
      "added": 987654,
      "updated": "2024-01-15T10:30:00Z",
      "genres": [
        {
          "id": 12,
          "name": "Shooter",
          "slug": "shooter"
        }
      ],
      "tags": [
        {
          "id": 40836,
          "name": "Full controller support",
          "slug": "full-controller-support"
        }
      ]
    }
  ]
}
```

### Response Fields Explanation

| Field | Type | Description |
|-------|------|-------------|
| `id` | int | Unique game identifier |
| `name` | string | Game title |
| `slug` | string | URL-friendly game name |
| `released` | string | Release date (ISO format) |
| `playtime` | int | Average playtime in hours |
| `rating` | float | Average user rating (0-5) |
| `ratings` | array | Detailed rating distribution |
| `platforms` | array | Available platforms |
| `stores` | array | Where to buy (Steam, Epic, etc.) |
| `genres` | array | Game genres/categories |
| `tags` | array | Additional game tags |
| `added` | int | Number of library additions |
| `updated` | string | Last update timestamp |

### Pagination

All endpoints support pagination with:
- `page`: Page number (default: 1)
- `pageSize`: Results per page (default: 20, max: 40)

Example:
```bash
# Get page 2 with 30 results per page
curl "http://localhost:5168/api/rawg/search?query=zelda&page=2&pageSize=30"
```

### Constraints

- **Max Page Size**: 40 results per request (prevents API overload)
- **Rate Limiting**: RAWG API enforces rate limits (handled automatically)
- **Query Parameters**: Required for search endpoints
- **Game ID**: Use numeric ID from API responses

---

## 🧪 Testing

### Running Tests

Execute all tests:
```bash
dotnet test
```

Run with verbose output:
```bash
dotnet test --verbosity detailed
```

Run specific test class:
```bash
dotnet test --filter "ClassName=RawgServiceTests"
```

### Test Coverage

- **Total Tests**: 70+
- **Pass Rate**: 100% 
- **Test Categories**:
  - RAWG Service (11 tests)
  - Google Books Service (8 tests)
  - TMDB Service (27+ tests)
  - Authentication (25+ tests)

### Testing Approaches

**1. Swagger UI** (Interactive)
- Navigate to `http://localhost:5168/swagger`
- Click "Try it out" on any endpoint
- Enter parameters and execute

**2. cURL** (Command Line)
```bash
curl -X GET "http://localhost:5168/api/googlebooks/search?query=Harry%20Potter" \
  -H "Content-Type: application/json"
```

**3. Automated Tests**
```bash
dotnet test --logger "console;verbosity=detailed"
```

---

## ⚙️ Configuration

### User Secrets Setup (Development)

```bash
# Initialize secrets (one-time)
dotnet user-secrets init

# Set API Keys
dotnet user-secrets set "Tmdb:ApiKey" "your_tmdb_key_here"
dotnet user-secrets set "GoogleBooks:ApiKey" "your_google_books_key_here"

# List all secrets
dotnet user-secrets list

# Clear secrets
dotnet user-secrets clear
```

### Configuration Precedence

Configuration values are applied in this order (later overrides earlier):
1. `appsettings.json` (base configuration)
2. `appsettings.{Environment}.json` (environment-specific)
3. User Secrets (development only)
4. Environment variables (system-level)

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=OmnisReviewDb;Trusted_Connection=true;"
  },
  "Tmdb": {
    "ApiKey": ""
  },
  "GoogleBooks": {
    "ApiKey": ""
  },
  "Rawg": {
    "ApiKey": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## 🔍 Troubleshooting

### API Key Not Configured

**Error**: `InvalidOperationException: Google Books API key not configured`

**Solution**:
```bash
# Verify secrets are set
dotnet user-secrets list

# Re-set the secret (case-sensitive key name)
dotnet user-secrets set "GoogleBooks:ApiKey" "your_key_here"
```

### TMDB Network Error

**Error**: `NetworkError when attempting to fetch resource`

**Solution**:
1. Ensure API is running: `dotnet run`
2. Verify internet connection
3. Check TMDB API key validity at https://www.themoviedb.org/settings/api

### RAWG JSON Deserialization Error

**Error**: `System.Text.Json.JsonException: The JSON value could not be converted...`

**Solution**:
1. Ensure RAWG API key is correctly configured in User Secrets
2. Check that the API key is valid at https://rawg.io/api-console
3. Verify JSON response structure matches DTO definitions
4. Check API rate limiting (RAWG has per-second request limits)

### RAWG No Results Returned

**Issue**: Search endpoint returns empty results or HTTP 204

**Debugging Steps**:
1. Check Debug logs in Visual Studio Output window
2. Verify API key is set: `dotnet user-secrets list | findstr Rawg`
3. Test API connectivity manually:
   ```bash
   curl "https://api.rawg.io/api/games?search=mario&key=YOUR_API_KEY"
   ```
4. Verify search query has results at https://rawg.io

### Google Books API Returns 403 Forbidden

**Cause**: Public IP not whitelisted in Google Cloud Console

**Solution**:
1. Find your IP: https://whatismyipaddress.com
2. Go to [Google Cloud Console](https://console.cloud.google.com)
3. Navigate to APIs & Services → Credentials
4. Edit API key and add your IP to allowed IPs
5. Wait 5-10 minutes for propagation

### Port Already in Use

**Error**: `System.IO.IOException: The handle is invalid`

**Solution**:
```bash
# Windows PowerShell - Kill dotnet processes
Get-Process | Where-Object {$_.ProcessName -like "*dotnet*"} | Stop-Process -Force

# Or find specific process
netstat -ano | findstr :5168
taskkill /PID <PID> /F
```

---

## 📋 API Response Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request successful |
| 201 | Created - Resource created |
| 400 | Bad Request - Invalid parameters |
| 401 | Unauthorized - Authentication required |
| 403 | Forbidden - Access denied |
| 404 | Not Found - Resource not found |
| 500 | Server Error - Unexpected error |

---

## Security

- **Never commit API keys** to version control
- Use **User Secrets** for local development
- Use **environment variables** for production
- Configure **IP whitelisting** in TMDB and Google Cloud consoles
- Always use **HTTPS** in production
- JWT tokens for API authentication

---

## Documentation

Full API documentation via **Swagger/OpenAPI**:
- **Interactive UI**: `http://localhost:5168/swagger`
- **OpenAPI JSON**: `http://localhost:5168/swagger/v1/swagger.json`

---

## Contributing

Contributions welcome! Please:
1. Create feature branch: `git checkout -b feature/your-feature`
2. Commit changes: `git commit -am 'Add feature'`
3. Push to branch: `git push origin feature/your-feature`
4. Submit a Pull Request

---

## License

MIT License - see LICENSE file for details

---

**Stack**: .NET 10 | C# 14 | ASP.NET Core | SQL Server | EF Core | NUnit + Moq  
**API Version**: v1 | **Status**: Production Ready | **Tests**: 60+ (100% passing)
# 5. Iniciar API
dotnet run
```

A API estará disponível em `https://localhost:7001`

## Testes

O projeto inclui **41 testes unitários** cobrindo todas as camadas:

| Camada | Qtd | Descrição |
|--------|-----|-----------|
| Services | 13 | Lógica de negócio |
| Repositories | 14 | Acesso a dados |
| Controllers | 14 | Endpoints HTTP |

### Executar Testes

```bash
# Todos os testes
dotnet test

# Com verbosidade
dotnet test --verbosity normal

# Testes específicos
dotnet test --filter "AuthServiceTests"

# Com cobertura de código
dotnet test /p:CollectCoverage=true
```

**Status:** 41/41 passando (1.2s)

## Documentação

- [**Testes**](./Omnis-Review-API.Tests/README.md) - Padrões e cobertura de testes
- [**Git Conventions**](./GIT_CONVENTIONS.md) - Padrões de branches e commits
- [**Instruções NUnit**](./copilot-instructions-tests.md) - Guia de testes
- [**Configurações**](./CONFIGURACAO.md) - Setup e variáveis de ambiente
- [**Swagger**](https://localhost:7001/swagger) - API interativa (quando rodando)

## Estrutura

```
Omnis-Review-API/
├── Controllers/              # Endpoints HTTP
├── Services/                 # Lógica de negócio
├── Repositorys/              # Acesso a dados
├── Models/                   # DTOs e entidades
├── Data/                     # DbContext
├── Migrations/               # EF Core migrations
├── Omnis-Review-API.Tests/   # Testes unitários
├── Program.cs                # Configuração da API
└── appsettings.json          # Configurações

Omnis-Review-API.Tests/
├── Services/                 # Testes de serviços
├── Repositorys/              # Testes de repositórios
├── Controllers/              # Testes de controllers
└── Helpers/                  # Builders de dados
```

## Padrões

### Testes
- **AAA Pattern:** Arrange-Act-Assert
- **Mocking:** Moq com isolamento de dependências
- **Builders:** Fluent builders para dados de teste
- **Cobertura:** Happy Path + Exceções + Edge Cases

### Git
- **Branches:** `feat/`, `fix/`, `hotfix/`, `refactor/`, `test/`, `docs/`, `chore/`
- **Commits:** `type(scope): subject`
- Exemplo: `feat(auth): implement jwt token generation`

## Como Contribuir

1. Criar branch seguindo padrão: `feat/descricao-funcionalidade`
2. Implementar com testes
3. Commits com padrão convencional
4. Pull request com descrição clara
5. Verificar que todos os 41 testes passam

## Variáveis de Ambiente

```json
{
  "Jwt": {
    "SecretKey": "sua-chave-secreta-aqui",
    "ExpirationMinutes": 60
  },
  "Database": {
    "DefaultConnection": "Server=.;Database=OmnisReviewDb;Integrated Security=true;"
  }
}
```

Veja [appsettings.example.json](./appsettings.example.json) para template completo.

## Licença

Projeto pessoal - Omnis Review API
