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

## API Endpoints

### Members (Члены клуба)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | http://localhost:5000/api/members | Получить всех членов |
| GET | http://localhost:5000/api/members/{id} | Получить члена по ID |
| POST | http://localhost:5000/api/members | Создать нового члена |
| PUT | http://localhost:5000/api/members/{id} | Обновить члена |
| DELETE | http://localhost:5000/api/members/{id} | Удалить члена |

### Примеры запросов

#### Создать члена клуба

```bash
curl -X POST http://localhost:5000/api/members \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Иван Иванов",
    "email": "ivan@example.com",
    "phone": "+79001234567",
    "membershipStartDate": "2024-01-01"
  }'
```

#### Получить всех членов

```bash
curl http://localhost:5000/api/members
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
