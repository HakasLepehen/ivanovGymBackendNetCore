# Deployment Secrets

Docker Compose cannot read GitHub Secrets directly. It reads the password from
`.secrets/postgres_password`, which must exist on the Docker host for the
lifetime of the running containers.

For local development, create that file using the instructions in
`.secrets.example/README.md`.

The current GitHub Actions workflows only build and publish images. They do
not run Docker Compose, so adding `POSTGRES_PASSWORD` to GitHub has no effect
until a deployment job is added.

In that future deployment job, add `POSTGRES_PASSWORD` as a repository secret,
or as an environment secret and set the job's `environment` accordingly. Run
the following on the deployment runner before `docker compose up -d`:

```yaml
- name: Provision PostgreSQL Compose secret
  shell: bash
  env:
    POSTGRES_PASSWORD: ${{ secrets.POSTGRES_PASSWORD }}
  run: |
    install -d -m 700 .secrets
    printf '%s' "$POSTGRES_PASSWORD" > .secrets/postgres_password
    chmod 444 .secrets/postgres_password

- name: Start services
  run: docker compose up -d --build
```

Do not print the secret, upload `.secrets` as an artifact, or remove the file
while Compose services are running: local Compose mounts file-based secrets
from the Docker host. Restrict access to the deployment workspace and delete
the file only after the services have stopped or after replacing it during the
next deployment.

Changing this file does not rotate the password of an initialized PostgreSQL
database. First change the PostgreSQL role password, then update
`POSTGRES_PASSWORD` and replace the file as one coordinated deployment.
