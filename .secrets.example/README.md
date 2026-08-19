# Секреты PostgreSQL

## Локальная разработка

Секреты не нужны. `docker-compose.yml` берёт пароль из переменной `POSTGRES_PASSWORD`
(по умолчанию `mypassword`, совпадает с `appsettings.Development.json`).
При необходимости создайте `.env` рядом с `docker-compose.yml`:

```sh
POSTGRES_PASSWORD=another-password
```

## Production (VPS)

Compose-файл для VPS находится вне этого репозитория и управляется вручную.
Пароль БД на VPS задаётся там же: либо через переменную окружения
`POSTGRES_PASSWORD`, либо через Docker secret `/run/secrets/postgres_password`,
который читают `entrypoint.sh` и `Program.cs`.

Репозиторий содержит только Dockerfile и GitHub Actions workflow для сборки и
публикации образа в Docker Hub — секреты деплоя в него попадать не должны.
