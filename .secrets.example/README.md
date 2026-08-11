# Docker Secrets

Compose reads the PostgreSQL password from `.secrets/postgres_password`. Do not commit this directory.

For local development, create the file yourself:

The file must be readable by the non-root API container user:

```sh
mkdir -p .secrets
chmod 700 .secrets
printf '%s' 'replace-with-a-long-random-password' > .secrets/postgres_password
chmod 444 .secrets/postgres_password
```

Before the first `docker compose up`, create the external persistent volumes:

```sh
docker volume create postgres_data
docker volume create postgres_backups
```

`postgres-backup` writes one custom-format dump per day to `postgres_backups` and removes dumps older than seven days. Copy those dumps to independent storage; an external Docker volume is protected from `docker compose down -v`, but not from explicit Docker volume removal or host loss.

When an actual deployment workflow is added, it can create this file from a GitHub secret named `POSTGRES_PASSWORD`. The current GitHub Actions workflows only build and publish images; they do not deploy Compose services. See `docs/deployment-secrets.md`.
