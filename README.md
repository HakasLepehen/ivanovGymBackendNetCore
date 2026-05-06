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
├── docker-compose.yml
└── ivanovGymBackendNetCore.slnx
```

## Требования

- .NET 8.0 или выше
- Docker (для запуска PostgreSQL)

## Настройка базы данных

Запуск PostgreSQL в Docker контейнере:

```bash
docker-compose up -d
```

Это запустит PostgreSQL на порту 5432.

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
