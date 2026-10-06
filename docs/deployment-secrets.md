# Deployment Secrets

Репозиторий не содержит production-`docker-compose.yml`: на VPS он управляется
вручную и берёт готовый образ API из Docker Hub.

## Как пароль БД попадает в контейнер API

Контейнер API читает пароль двумя способами (см. `entrypoint.sh` и `Program.cs`):

1. **Docker secret** `/run/secrets/postgres_password` — если файл существует,
   пароль добавляется к строке подключения.
2. Иначе пароль должен быть уже в строке подключения
   `ConnectionStrings:DefaultConnection`.

Для VPS достаточно простого варианта — задать пароль переменной окружения
в его `docker-compose.yml`:

```yaml
services:
  api:
    image: your-dockerhub-user/ivanov-gym-api:latest
    environment:
      ConnectionStrings__DefaultConnection: Host=postgres;Port=5432;Database=myapp;Username=myuser;Password=strong-password
```

Либо смонтировать secret в `/run/secrets/postgres_password`.

## Ключ подписи JWT

Ключ подписи токенов в репозитории отсутствует (`JwtSettings:Key` в `appsettings.json` пуст),
а без него приложение не запускается. `entrypoint.sh` берёт ключ в таком порядке:

1. Docker secret `/run/secrets/jwt_key` (пробельные символы удаляются);
2. переменная окружения `JwtSettings__Key`.

Если ни того, ни другого нет, контейнер завершается с ошибкой. То же требование действует для
`dotnet ef`: конфигурация API-проекта читается и в design-time, поэтому локально нужен
user-secrets (см. README).

```yaml
services:
  api:
    image: your-dockerhub-user/ivanov-gym-api:latest
    secrets:
      - postgres_password
      - jwt_key

secrets:
  postgres_password:
    file: ./secrets/postgres_password
  jwt_key:
    file: ./secrets/jwt_key
```

Длина ключа — не меньше 32 символов. Генерация:
`openssl rand -base64 48 | tr -d '/+=' | cut -c1-48`.

Смена ключа делает все выданные токены недействительными: пользователи будут перелогинены.
Ротация обязательна, если предыдущее значение где-то засвечено.

## GitHub Actions

Workflow `.github/workflows/docker.yml` только собирает и публикует образ в
Docker Hub — он не деплоит на VPS. Необходимые секреты репозитория:

- `DOCKERHUB_USERNAME` — логин Docker Hub (используется в имени образа);
- `DOCKERHUB_TOKEN` — токен доступа Docker Hub.

Если позже добавится шаг деплоя (SSH-доступ к VPS), пароль БД можно хранить в
GitHub Secrets и передавать на сервер через SSH/скрипт деплоя.

## Смена пароля БД

Изменение переменной окружения не меняет пароль уже инициализированной базы
PostgreSQL. Сначала смените пароль роли (`ALTER ROLE ... PASSWORD ...`), затем
обновите значение в compose VPS и перезапустите контейнеры одним деплоем.
