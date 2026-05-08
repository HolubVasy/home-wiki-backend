# home-wiki-backend

.NET 8 Web API backend for a personal home management knowledge base.

## Tech Stack

| | |
|---|---|
| Runtime | .NET 8 ASP.NET Core |
| ORM | Entity Framework Core + Npgsql |
| Database | PostgreSQL 16 |
| Logging | Serilog |
| Docs | Swagger / OpenAPI |

## Architecture

Clean Architecture across 6 projects:

```
WebApi → BL → DAL → DAL.Common → Shared
```

| Project | Contains |
|---|---|
| `WebApi` | Controllers, DI, middleware, Program.cs, seeder |
| `BL.Common` | Service interfaces + DTOs |
| `BL` | ArticleService, CategoryService, TagService |
| `DAL.Common` | Entities, DbContext, IGenericRepository, ISpecification |
| `DAL` | GenericRepository, Specifications, EF migrations |
| `Shared` | ResultModel\<T\>, PagedList\<T\>, ErrorCode enum |

Key patterns: Generic Repository, Specification, ResultModel (services never throw to controllers).

## Running Locally

```bash
cd src
dotnet run --project home-wiki-backend.WebApi
```

Swagger is available at `/swagger` in Development mode.

Connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=wiki;Username=wiki;Password=..."
}
```

Migrations and seeding are applied automatically on startup via `app.ApplyMigrationsAndSeed()` in `Program.cs`.

## Docker / VPS Deployment

`docker-compose.yml` is in the root of this repository and manages three services: `wiki-db`, `wiki-backend`, `wiki-frontend`.

### Server Requirements

- Docker + Docker Compose
- Both repos cloned into the same parent directory:

```
/opt/home-wiki/
├── home-wiki-backend/    ← this repo
└── home-wiki-frontend/
```

- `.env` file next to `docker-compose.yml`:

```
DB_PASSWORD=<password>
```

### Commands

```bash
# Start all services
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d

# Deploy a new backend version
git -C /opt/home-wiki/home-wiki-backend pull origin main
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml build wiki-backend
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d

# View logs
docker logs wiki-backend --tail 50
docker logs wiki-db --tail 20

# Clean restart (destroys DB data)
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml down -v
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d
```

### Healthcheck

`wiki-db` declares a healthcheck (`pg_isready`). `wiki-backend` starts only after `condition: service_healthy`, eliminating the race condition on startup.

## API Endpoints

### Articles `/api/Article`

| Method | Path | Description |
|---|---|---|
| GET | `/` | All articles with category and tags |
| GET | `/{id}` | Article by ID |
| GET | `/paged` | Paginated (`?pageNumber=1&pageSize=10`) |
| GET | `/search` | Search by name (`?name=X&pageNumber=1&pageSize=10`) |
| GET | `/category/{id}` | Articles by category |
| GET | `/tag/{id}` | Articles by tag |
| POST | `/` | Create article |
| POST | `/filter` | Advanced filter (body: `ArticleFilterRequestDto`) |
| PUT | `/` | Update article (syncs tag diff) |
| DELETE | `/{id}` | Delete article |

### Categories `/api/Category` and Tags `/api/Tag`

Standard CRUD: GET all, GET by id, POST, PUT, DELETE.

## Database Schema

```
category     (pk_category, Name, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
tag          (pk_tag, Name, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
article      (pk_article, Name, Description, CategoryId FK, audit fields)
article_tag  (ArticleId FK, TagId FK)
```

> **PostgreSQL note:** column names are case-sensitive. Use quotes in raw SQL: `SELECT "Name" FROM category;`

## Seed Data

On first startup `InitialSeeder` populates the database: 6 categories, 17 tags, 19 articles (in Russian).

The seeder is idempotent — safe to run on restart.

Categories: Уход за домом, Бытовая техника, Огород и растения, Ремонт и инструменты, Отдых и уют, Развлечение.
