# home-wiki-backend

.NET 8 Web API — backend для личной базы знаний по домашнему хозяйству.

## Tech Stack

| | |
|---|---|
| Runtime | .NET 8 ASP.NET Core |
| ORM | Entity Framework Core + Npgsql |
| Database | PostgreSQL 16 |
| Logging | Serilog |
| Docs | Swagger / OpenAPI |

## Architecture

Clean Architecture, 6 проектов:

```
WebApi → BL → DAL → DAL.Common → Shared
```

| Проект | Содержит |
|---|---|
| `WebApi` | Controllers, DI, middleware, Program.cs, seeder |
| `BL.Common` | Service interfaces + DTOs |
| `BL` | ArticleService, CategoryService, TagService |
| `DAL.Common` | Entities, DbContext, IGenericRepository, ISpecification |
| `DAL` | GenericRepository, Specifications, EF migrations |
| `Shared` | ResultModel\<T\>, PagedList\<T\>, ErrorCode enum |

Паттерны: Generic Repository, Specification, ResultModel (сервисы не бросают исключения в контроллер).

## Running Locally

```bash
cd src
dotnet run --project home-wiki-backend.WebApi
```

Swagger доступен по `/swagger` в режиме Development.

Строка подключения в `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=wiki;Username=wiki;Password=..."
}
```

Миграции применяются автоматически при старте через `app.ApplyMigrationsAndSeed()` в `Program.cs`.

## Docker / VPS Deployment

`docker-compose.yml` находится в корне этого репозитория и управляет тремя сервисами: `wiki-db`, `wiki-backend`, `wiki-frontend`.

### Требования на сервере

- Docker + Docker Compose
- Git clone обоих репо в одну директорию:

```
/opt/home-wiki/
├── home-wiki-backend/    ← этот репо
└── home-wiki-frontend/
```

- Файл `.env` рядом с `docker-compose.yml`:

```
DB_PASSWORD=<пароль>
```

### Команды

```bash
# Запуск
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d

# Деплой новой версии backend
git -C /opt/home-wiki/home-wiki-backend pull origin main
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml build wiki-backend
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d

# Логи
docker logs wiki-backend --tail 50
docker logs wiki-db --tail 20

# Чистый перезапуск (удаляет данные БД)
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml down -v
docker compose -f /opt/home-wiki/home-wiki-backend/docker-compose.yml up -d
```

### Healthcheck

`wiki-db` объявляет healthcheck (`pg_isready`). `wiki-backend` стартует только после `condition: service_healthy` — race condition исключён.

## API Endpoints

### Articles `/api/Article`

| Method | Path | Description |
|---|---|---|
| GET | `/` | Все статьи с категорией и тегами |
| GET | `/{id}` | Статья по ID |
| GET | `/paged` | Пагинация (`?pageNumber=1&pageSize=10`) |
| GET | `/search` | Поиск по названию (`?name=X&pageNumber=1&pageSize=10`) |
| GET | `/category/{id}` | Статьи по категории |
| GET | `/tag/{id}` | Статьи по тегу |
| POST | `/` | Создать статью |
| POST | `/filter` | Расширенный фильтр (body: `ArticleFilterRequestDto`) |
| PUT | `/` | Обновить статью (синхронизирует теги) |
| DELETE | `/{id}` | Удалить статью |

### Categories `/api/Category` и Tags `/api/Tag`

Стандартный CRUD: GET all, GET by id, POST, PUT, DELETE.

## Database Schema

```
category     (pk_category, Name, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
tag          (pk_tag, Name, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
article      (pk_article, Name, Description, CategoryId FK, audit fields)
article_tag  (ArticleId FK, TagId FK)
```

> **PostgreSQL note:** имена колонок case-sensitive. В raw SQL используй кавычки: `SELECT "Name" FROM category;`

## Seed Data

При первом запуске `InitialSeeder` заполняет БД: 6 категорий, 17 тегов, 19 статей на русском.

Сидер идемпотентный — безопасен при повторных запусках.

Категории: Уход за домом, Бытовая техника, Огород и растения, Ремонт и инструменты, Отдых и уют, Развлечение.
