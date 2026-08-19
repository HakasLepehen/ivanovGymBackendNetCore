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
