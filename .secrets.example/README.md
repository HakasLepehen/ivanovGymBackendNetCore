# Секреты

В репозитории два секрета: пароль PostgreSQL и ключ подписи JWT. Оба читаются
из `/run/secrets/` (см. `entrypoint.sh` и `Program.cs`).

## Локальная разработка

Пароль БД не требуется: `docker-compose.yml` берёт его из переменной `POSTGRES_PASSWORD`
(по умолчанию `mypassword`, совпадает с `appsettings.Development.json`).
При необходимости создайте `.env` рядом с `docker-compose.yml`:

```sh
POSTGRES_PASSWORD=another-password
```

Ключ JWT обязателен и локально — он хранится в user-secrets (файл вне репозитория):

```sh
openssl rand -base64 48 | tr -d '/+=' | cut -c1-48
dotnet user-secrets --project src/ivanovGymBackendNetCore.API set "JwtSettings:Key"=<сгенерированный-ключ>
```

## Production (VPS)

Compose-файл для VPS находится вне этого репозитория и управляется вручную. Пароль БД
задаётся там же: либо через переменную окружения `POSTGRES_PASSWORD`, либо через Docker
secret `/run/secrets/postgres_password`, который читают `entrypoint.sh` и `Program.cs`.
Ключ JWT задаётся `/run/secrets/jwt_key` либо переменной `JwtSettings__Key`.

Репозиторий содержит только Dockerfile и GitHub Actions workflow для сборки и
публикации образа в Docker Hub — секреты деплоя в него попадать не должны.
