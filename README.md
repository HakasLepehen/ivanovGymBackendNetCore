# ivanovGymBackendNetCore

RESTful API проект с .NET Core, луковой архитектурой (Onion Architecture) и PostgreSQL базой данных.

## Структура проекта

```
ivanovGymBackendNetCore/
├── src/
│   ├── ivanovGymBackendNetCore.API/          # Presentation Layer (API)
│   │   ├── Controllers/
│   │   ├── Program.cs
│   │   └── appsettings*.json
│   ├── ivanovGymBackendNetCore.Application/  # Application Layer (бизнес-логика)
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Profiles/
│   │   ├── Services/
│   │   └── ApplicationServiceExtensions.cs
│   ├── ivanovGymBackendNetCore.Domain/       # Domain Layer (ядро)
│   │   ├── Entities/
│   │   └── Interfaces/
│   └── ivanovGymBackendNetCore.Infrastructure/ # Infrastructure Layer
│       ├── Data/
│       │   ├── AppDbContext.cs
│       │   └── Configurations/
│       └── Repositories/
├── .vscode/
├── docker-compose.yml     # только PostgreSQL для локальной разработки
├── Dockerfile             # production-образ API (собирается в GitHub Actions)
└── ivanovGymBackendNetCore.slnx
```

## Требования

- .NET 10.0
- Docker (для запуска PostgreSQL)

## Локальная разработка

`docker-compose.yml` поднимает **только** контейнер с PostgreSQL. Пароль по умолчанию
совпадает с `appsettings.Development.json`; при желании его можно переопределить через
`.env`-файл (`POSTGRES_DB`, `POSTGRES_USER`, `POSTGRES_PASSWORD`).

```bash
docker compose up -d   # старт PostgreSQL на порту 5432
docker compose down    # остановка (данные сохраняются в томе postgres_data)
```

### Ключ подписи JWT

Ключ подписи не хранится в репозитории и обязателен: без него приложение не стартует
(и `dotnet ef` тоже, так как использует конфигурацию API-проекта). Локально ключ хранится
в user-secrets (файл вне репозитория):

```bash
dotnet user-secrets --project src/ivanovGymBackendNetCore.API set "JwtSettings:Key"=<ключ-не-короче-32-символов>
dotnet user-secrets --project src/ivanovGymBackendNetCore.API list
```

Генерация ключа: `openssl rand -base64 48 | tr -d '/+=' | cut -c1-48`

### Запуск API

API запускается вне Docker, как обычное .NET-приложение:

```bash
dotnet run --project src/ivanovGymBackendNetCore.API
# или с автопересборкой:
dotnet watch run --project src/ivanovGymBackendNetCore.API
```

API доступно по адресу http://localhost:5000, Swagger — http://localhost:5000/swagger.

## Работа с миграциями Entity Framework Core

Миграции позволяют синхронизировать модель данных с базой данных PostgreSQL.

### Создание новой миграции

После изменения сущностей или конфигураций создайте миграцию:

```bash
dotnet ef migrations add MigrationName --project src/ivanovGymBackendNetCore.Infrastructure --startup-project src/ivanovGymBackendNetCore.API
```

**Пример:**
```bash
dotnet ef migrations add AddTrainingExercise --project src/ivanovGymBackendNetCore.Infrastructure --startup-project src/ivanovGymBackendNetCore.API
```

### Применение миграций к базе данных

```bash
dotnet ef database update --project src/ivanovGymBackendNetCore.Infrastructure --startup-project src/ivanovGymBackendNetCore.API
```

### Удаление последней миграции

Если миграция создана с ошибкой:

```bash
dotnet ef migrations remove --project src/ivanovGymBackendNetCore.Infrastructure --startup-project src/ivanovGymBackendNetCore.API
```

### Просмотр состояния миграций

```bash
dotnet ef migrations list --project src/ivanovGymBackendNetCore.Infrastructure --startup-project src/ivanovGymBackendNetCore.API
```

### Установка инструментов EF Core (если команды не работают)

```bash
dotnet tool install --global dotnet-ef
```

## Запуск проекта

### Сборка решения

```bash
dotnet build
```

### Запуск API

```bash
dotnet run --project src/ivanovGymBackendNetCore.API
```

API будет доступно по адресу: http://localhost:5000

### Swagger UI

После запуска откройте браузер по адресу: http://localhost:5000/swagger

## Production (VPS)

Образ API собирается в GitHub Actions (`.github/workflows/docker.yml`) при пуше в `master`
и публикуется в Docker Hub с тегами `latest` и коротким SHA коммита.

На VPS используется собственный `docker-compose.yml`, который берёт готовый образ из
Docker Hub, — сам репозиторий его не содержит. В GitHub репозитории необходимо настроить
секреты:

- `DOCKERHUB_USERNAME` — логин Docker Hub (используется и в имени образа);
- `DOCKERHUB_TOKEN` — токен доступа Docker Hub (не пароль).

Имя образа: `docker.io/<DOCKERHUB_USERNAME>/ivanov-gym-api`. Для деплоя на VPS достаточно
указать в его compose `image: <DOCKERHUB_USERNAME>/ivanov-gym-api:latest` (или конкретный
SHA-тег для воспроизводимости).

Контейнер API ожидает пароль БД либо в Docker secret `/run/secrets/postgres_password`,
либо в строке подключения `ConnectionStrings:DefaultConnection` (см. `entrypoint.sh` и
`Program.cs`).

Ключ подписи JWT также обязателен и в образ не входит: `entrypoint.sh` читает его из
`/run/secrets/jwt_key` либо из переменной окружения `JwtSettings__Key` и завершает запуск
с ошибкой, если ключа нет или он короче 32 символов.

## Роли и доступ

Три роли: `admin`, `trainer`, `user`. Источник ролей — колонка `users."Roles"` (`text[]`),
именно она попадает в JWT клеймом `ClaimTypes.Role`; таблицы Identity (`AspNetRoles`,
`AspNetUserRoles`) в приложении не используются. Константы ролей и email администратора —
`src/ivanovGymBackendNetCore.Domain/Enums/UserRole.cs`, имена политик —
`src/ivanovGymBackendNetCore.Domain/AuthorizationPolicies.cs`.

**Роль определяется только на сервере.** Поле `role` в теле `signup` принимается, только если
запрос сделан аутентифицированным администратором; иначе возвращается 403. Публичная
регистрация всегда создаёт роль `user`.

| Возможность | admin | trainer | user |
|---|---|---|---|
| Зарегистрировать учётную запись ролью `user` | ✅ | ❌ | ✅ (публично) |
| Зарегистрировать учётную запись ролью `trainer` | ✅ | ❌ | ❌ |
| Стать администратором | только зарегистрировав `UserRole.AdminEmail`, пока админа нет | ❌ | ❌ |
| Сбросить пароль чужой учётной записи | ✅ любая | ✅ только роль `user` | ❌ |
| Сменить собственный пароль (со знанием текущего) | ✅ | ✅ | ✅ |
| Прочие эндпоинты API | по существующим `[Authorize]` | | |

Администратор — суперпользователь. Email администратора задан константой
`UserRole.AdminEmail` и **должен быть заменён на реальный перед деплоем**: в исходном коде
он открыт, поэтому повторная выдача роли `admin` заблокирована — если администратор уже
существует, регистрация с этим email отклоняется.

Все эндпоинты по умолчанию требуют аутентификацию (`FallbackPolicy`), открытые помечены
`[AllowAnonymous]` (`signup`, `login`, `POST /api/consultationRequests`, `GET /api`).
Побочный эффект fallback-политики: несуществующий маршрут без токена возвращает 401, а не 404.

### Роль на фронтенде

Источник роли для клиента — ответы `POST /api/auth/login` и `GET /api/auth/me`
(`roles: string[]`). Разбирать JWT на клиенте не нужно: клеймы уходят полными URI-именами
(`http://schemas.microsoft.com/ws/2008/06/identity/claims/role`), а роль в токене
не обновляется до истечения срока (`JwtSettings:ExpiryMinutes`), то есть после смены роли
нужен новый вход.

## API Endpoints

### Auth (`/api/auth`)

| Method | Endpoint | Доступ | Description |
|--------|----------|--------|-------------|
| POST | `/api/auth/signup` | все (роль — только админ) | Создать учётную запись, опционально с ролью |
| POST | `/api/auth/login` | все | Вход, возвращает токен и `roles` |
| GET | `/api/auth/me` | аутентифицированные | Email, `userId`, `roles` |
| POST | `/api/auth/change-password` | аутентифицированные | Смена собственного пароля (нужен текущий) |
| POST | `/api/auth/reset-password` | admin, trainer | Сброс пароля другой учётной записи без текущего |

### Примеры запросов

#### Публичная регистрация (всегда роль `user`)

```bash
curl -X POST http://localhost:5000/api/auth/signup \
  -H "Content-Type: application/json" \
  -d '{"email": "ivan@example.com", "password": "secret123"}'
```

#### Администратор регистрирует тренера

```bash
curl -X POST http://localhost:5000/api/auth/signup \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <токен администратора>" \
  -d '{"email": "trainer@example.com", "password": "secret123", "role": "trainer"}'
```

#### Сброс пароля пользователя (тренеру доступны только пользователи)

```bash
curl -X POST http://localhost:5000/api/auth/reset-password \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <токен администратора или тренера>" \
  -d '{"userId": "<guid пользователя>", "newPassword": "newsecret123"}'
```

## Архитектура

Проект построен по принципу **луковой архитектуры (Onion Architecture)**:

- **Domain Layer** - ядро системы, содержит сущности и интерфейсы репозиториев
- **Application Layer** - бизнес-логика, DTO, сервисы
- **Infrastructure Layer** - реализация репозиториев, DbContext, работа с БД
- **API Layer** - контроллеры и маршрутизация

Зависимости направлены внутрь: API → Application → Infrastructure → Domain

## Тестирование

```bash
dotnet test
```

## Отладка (Debug)

### Visual Studio Code

1. Откройте проект в VS Code
2. Установите расширения:
   - C# Dev Kit (ms-dotnettools.csdevkit)
   - C# (ms-dotnettools.csharp)

3. Нажмите `F5` или перейдите в Debug (`Ctrl+Shift+D`)
4. Выберите конфигурацию "Launch ivanovGymBackendNetCore.API"
5. API запустится с отладкой и откроет Swagger в браузере

**Альтернатива - запуск с watch режимом:**
- Выберите конфигурацию "Run with watch"
- Код будет автоматически пересобираться при изменениях

### JetBrains Rider

1. Откройте проект в Rider
2. Rider автоматически обнаружит конфигурацию запуска
3. Нажмите на зелёную стрелку рядом с `ivanovGymBackendNetCore.API`
4. Выберите "Debug"

## Лицензия

MIT
