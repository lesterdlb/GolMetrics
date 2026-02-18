## Context

The GolMetrics project has existing containerization files (API Dockerfile, Web Dockerfile, nginx.conf, docker-compose.yml) but they are incomplete. The docker-compose.yml only passes the connection string, missing JWT, encryption, API-Football, and Gemini configuration. There are no health checks, no `.dockerignore` files, and the API Dockerfile exposes an unused port.

## Goals / Non-Goals

**Goals:**
- Complete the docker-compose.yml with all required environment variables
- Add health checks so services start in the correct order with readiness verification
- Add `.dockerignore` files to reduce build context and speed up builds
- Clean up the API Dockerfile (remove unused port exposure)

**Non-Goals:**
- Production-grade deployment (no TLS, no reverse proxy beyond nginx SPA routing)
- Container orchestration (no Kubernetes, no Swarm)
- CI/CD integration (separate concern, handled by ci-cd-pipeline spec)
- Secret management solutions (using plain env vars for local development)

## Decisions

### 1. Environment variable naming convention
Use ASP.NET Core's `__` (double underscore) convention for nested configuration (e.g., `TokenOptions__SecretKey`). This maps directly to the `appsettings.json` structure without extra code.

**Alternative**: Use a single `ASPNETCORE_` prefix or custom env var parsing. Rejected because the `__` convention is built into .NET's configuration system.

### 2. Health checks
Use `pg_isready` for PostgreSQL and `curl` against a lightweight endpoint for the API. The `depends_on` directive will use `condition: service_healthy` to ensure proper startup order.

**Alternative**: TCP port checks. Rejected because `pg_isready` actually verifies PostgreSQL is ready for connections, not just that the port is open.

### 3. .dockerignore placement
Place a root `.dockerignore` for the API build (since the API build context is the repo root) and a separate one in `src/GolMetrics.Web/` for the Web build (since its context is scoped to that directory).

### 4. VITE_API_URL at build time
`VITE_API_URL` is baked into the frontend at build time by Vite. The docker-compose passes it as a build arg, not a runtime env var. The nginx reverse proxy handles `/api` routing at runtime by proxying to the `api` service.

**Alternative**: Runtime env var injection via entrypoint script. More complex for minimal benefit in a local dev setup.

## Risks / Trade-offs

- [Secrets in docker-compose.yml] -> Using placeholder values with comments to change them. Acceptable for local development; production should use Docker secrets or external secret management.
- [VITE_API_URL build-time baking] -> Requires rebuild if API URL changes. Acceptable for local development.
- [Health check curl dependency] -> The API container needs `curl` installed. The `mcr.microsoft.com/dotnet/aspnet` image doesn't include it. Will use `wget` instead (available in the base image) or a .NET health check endpoint hit via the `dotnet` CLI. Alternative: add a `/health` endpoint and use `wget`.
