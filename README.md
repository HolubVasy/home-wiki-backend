# home-wiki-backend

A .NET 8 Web API powering a personal home management knowledge base — articles, tips, and how-tos organized by categories and tags.

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 8 ASP.NET Core |
| ORM | Entity Framework Core 8 + Npgsql |
| Database | PostgreSQL 16 |
| Logging | Serilog |
| API Docs | Swagger / OpenAPI |

---

## Architecture

The solution follows **Clean Architecture** with a strict one-way dependency rule:

```
WebApi → BL → DAL → DAL.Common → Shared
```

| Project | Responsibility |
|---|---|
| `WebApi` | HTTP layer — controllers, DI wiring, middleware, Swagger, seeder bootstrap |
| `BL.Common` | Service contracts (interfaces) and request/response DTOs |
| `BL` | Business logic — `ArticleService`, `CategoryService`, `TagService` |
| `DAL.Common` | Domain entities, `DbWikiContext`, `IGenericRepository<T>`, `ISpecification<T>` |
| `DAL` | `GenericRepository<T>`, specification classes, EF Core migrations |
| `Shared` | Cross-cutting primitives — `ResultModel<T>`, `PagedList<T>`, `ErrorCode` enum, LINQ helpers |

### Generic Repository

`IGenericRepository<T>` abstracts all data access behind a consistent interface: `AddAsync`, `GetAsync`, `FirstOrDefaultAsync`, `ListAsync`, `GetPagedAsync`, `ExistsAsync`, `RemoveAsync`, `GetQueryable`. Each entity gets its own registered `GenericRepository<T>` — no hand-rolled SQL, no duplicate query logic.

### Specification Pattern

Query logic lives in dedicated specification classes that extend `SpecificationBase<T>`. A `SpecificationEvaluator` applies them to `IQueryable` inside the repository, keeping business rules out of the data layer.

Key specifications:
- `ArticlesWithCategoryAndTagsSpecification` — eager-loads related entities
- `ArticleForFilterSpecification` — applies name search, category/tag filtering, sorting, and pagination in one composable object
- `ArticlesByCategorySpecification` / `ArticlesByTagSpecification` — single-purpose filters

### ResultModel Pattern

Every service method returns `ResultModel<T>` — no exceptions thrown to the controller:

```csharp
class ResultModel<T> {
    bool    Success
    int     Code      // maps to HTTP status
    string  Message
    T?      Data
    ErrorResultModel? Error
}
```

Controllers simply check `result.Success` and return the matching HTTP response. This keeps error handling explicit and testable.

### Tag Sync on Article Update

When an article is updated, the service computes a diff rather than replacing tags wholesale:
1. Load the existing article with `.Include(a => a.Tags)` via EF
2. Determine tags to remove (in DB, not in request) and detach them from the collection
3. Determine tags to add (in request, not in DB) and attach the loaded `Tag` entities
4. `SaveChangesAsync` — EF handles the `article_tag` junction table automatically

---

## API Endpoints

### Articles — `POST /api/Article`

| Method | Path | Description |
|---|---|---|
| GET | `/api/Article` | All articles with category and tags |
| GET | `/api/Article/{id}` | Single article by ID |
| GET | `/api/Article/paged` | Paginated list (`?pageNumber=1&pageSize=10`) |
| GET | `/api/Article/search` | Search by name, paginated (`?name=X&pageNumber=1&pageSize=10`) |
| GET | `/api/Article/category/{id}` | Articles filtered by category |
| GET | `/api/Article/tag/{id}` | Articles filtered by tag |
| POST | `/api/Article` | Create article |
| POST | `/api/Article/filter` | Advanced filter (body: `ArticleFilterRequestDto`) |
| PUT | `/api/Article` | Update article (syncs tag diff) |
| DELETE | `/api/Article/{id}` | Delete article |

### Categories — `/api/Category` &nbsp;|&nbsp; Tags — `/api/Tag`

Standard CRUD: `GET` all, `GET` by id, `POST`, `PUT`, `DELETE`.

---

## Database Schema

```
category    (pk_category PK, Name varchar(150), CreatedBy, CreatedAt, ModifiedBy?, ModifiedAt?)
tag         (pk_tag PK,      Name varchar(150), CreatedBy, CreatedAt, ModifiedBy?, ModifiedAt?)
article     (pk_article PK,  Name varchar(150), Description varchar(5000), CategoryId FK, audit fields)
article_tag (ArticleId FK,   TagId FK)  -- composite PK, cascade delete on both sides
```

Unique indexes: `IX_category_Name`, `IX_tag_Name`, `IX_article_Name`.

> **PostgreSQL note:** column names are quoted in migrations and are case-sensitive in raw SQL.
> Use `SELECT "Name" FROM category;` — not `SELECT name`.

---

## Running Locally

```bash
cd src
dotnet run --project home-wiki-backend.WebApi
```

Swagger UI is available at `/swagger` in Development mode.

Set the connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=wiki;Username=wiki;Password=yourpassword"
  }
}
```

Migrations and seed data are applied automatically on startup via `app.ApplyMigrationsAndSeed()`.

---

## Docker / VPS Deployment

`docker-compose.yml` in the root of this repo orchestrates all three services: `wiki-db`, `wiki-backend`, and `wiki-frontend`.

### Prerequisites

- Docker + Docker Compose on the server
- Both repos cloned into the same parent directory:

```
/opt/home-wiki/
├── home-wiki-backend/    ← this repo (contains docker-compose.yml)
└── home-wiki-frontend/
```

- A `.env` file alongside `docker-compose.yml` (not committed):

```env
DB_PASSWORD=your_secure_password
```

### Deployment Commands

```bash
# First-time startup
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d

# Deploy a new backend version
git -C /opt/home-wiki/home-wiki-backend pull origin main
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml build wiki-backend
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d

# View logs
docker logs wiki-backend --tail 50
docker logs wiki-db --tail 20

# Full clean restart — drops and recreates the database
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml down -v
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d
```

### Healthcheck

`wiki-db` declares a `pg_isready` healthcheck. `wiki-backend` uses `condition: service_healthy` in `depends_on`, so the API only starts after PostgreSQL is fully ready — no race condition on cold start.

---

## Seed Data

On first startup, `InitialSeeder` populates the database with:
- **6 categories** — Уход за домом, Бытовая техника, Огород и растения, Ремонт и инструменты, Отдых и уют, Развлечение
- **17 tags** — Советы, Безопасность, Энергосбережение, DIY, Юмор, and more
- **19 articles** in Russian with full descriptions

The seeder is idempotent — it checks existing records before inserting and is safe to run on every restart.
