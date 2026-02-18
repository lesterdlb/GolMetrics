## Why

TICK-019: The containerization setup exists but is incomplete. The docker-compose.yml is missing required environment variables (JWT, encryption config), there are no health checks, no `.dockerignore` files to optimize build context, and the API Dockerfile exposes an unused port 8081. These gaps prevent reliable full-stack startup via `docker compose up --build`.

## What Changes

- Add missing JWT and AES-256 encryption environment variables to the `api` service in `docker-compose.yml`
- Add health checks for `db` and `api` services with proper `depends_on` conditions
- Add `.dockerignore` files for both API and Web to reduce build context size
- Remove unused port 8081 exposure from the API Dockerfile
- Add `restart` policies for production readiness

## Capabilities

### New Capabilities

(none)

### Modified Capabilities

- `containerization`: Adding health checks, missing environment variables, `.dockerignore` files, and restart policies to match spec requirements

## Impact

- **Files modified**: `docker-compose.yml`, `src/GolMetrics.API/Dockerfile`
- **Files created**: `.dockerignore` (root), `src/GolMetrics.Web/.dockerignore`
- **Dependencies**: No new dependencies required
- **Infrastructure**: Docker and Docker Compose (already required)
