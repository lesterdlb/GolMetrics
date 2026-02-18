## Context

The application is fully containerized with multi-stage Dockerfiles for both the API (.NET 10) and Web (React/nginx) services. Docker Compose works for local development with internal networking (`api:8080`). There is no CI/CD pipeline and no cloud deployment configuration. The project needs to be published to Render from the `finalproject-LDLB` branch.

Current state:
- `src/GolMetrics.API/Dockerfile` — multi-stage .NET build, exposes port 8080
- `src/GolMetrics.Web/Dockerfile` — multi-stage Node build + nginx, exposes port 5173
- `src/GolMetrics.Web/nginx.conf` — listens on 5173, proxies `/api` to `http://api:8080`
- `docker-compose.yml` — 3 services with internal networking
- Tests: unit tests (`Category=Unit`) and integration tests (`Category=Integration`) using Testcontainers
- No `.github/workflows/` directory exists

## Goals / Non-Goals

**Goals:**
- GitHub Actions workflow that builds, tests (unit + integration), and deploys on push to `finalproject-LDLB`
- Render Blueprint (`render.yaml`) for infrastructure-as-code with two web services and a managed PostgreSQL database
- Frontend adapted for Render's networking model (direct API calls via public URL, no internal proxy)

**Non-Goals:**
- Production hardening (rate limiting, WAF, custom domains, SSL certificates)
- Staging/preview environments
- Code coverage reporting
- Monitoring or alerting setup
- Changes to docker-compose.yml (local dev remains unchanged)

## Decisions

### D1: Render Blueprint over manual dashboard setup

Use `render.yaml` checked into the repo. This makes infrastructure reproducible and reviewable in version control. The alternative (manual dashboard setup) is faster initially but not portable or auditable.

### D2: Separate nginx configs for local and Render

Keep the existing `nginx.conf` intact for docker-compose local development. Create a separate `nginx.render.conf` for Render that listens on port 10000 and removes the `/api` proxy block. The Dockerfile will accept a build arg to select which config to use, defaulting to the local one so docker-compose continues to work unchanged.

**Alternative considered**: Modify the single `nginx.conf` with environment variable substitution at runtime using `envsubst`. Rejected because it adds complexity and Vite environment variables are baked in at build time anyway, so the API URL decision is already a build-time concern.

### D3: Dockerfile build arg for port and nginx config selection

Add a `NGINX_CONF` build arg to the Web Dockerfile (default: `nginx.conf`). Render's `render.yaml` passes `NGINX_CONF=nginx.render.conf`. This keeps docker-compose working without changes.

Similarly, add a `PORT` build arg (default: `5173`). Render passes `PORT=10000`. The Dockerfile uses `EXPOSE $PORT` and the nginx config uses the appropriate port.

### D4: Deploy hooks over Render API

Use Render deploy hooks (simple HTTP GET to a URL) rather than the Render API with service IDs and API keys. Deploy hooks are simpler, require only one secret per service, and are sufficient for triggering builds from CI.

### D5: Run all tests in a single job

Run unit and integration tests sequentially in one `build-and-test` job rather than splitting into parallel jobs. Integration tests with Testcontainers need Docker, which `ubuntu-latest` provides. A single job avoids duplicating the build step and keeps the workflow simple.

### D6: VITE_API_URL via fromService

In `render.yaml`, use `fromService` to reference the API service's host and prepend `https://`. This auto-resolves the API's public URL without hardcoding. The value is injected as a build-time Docker arg.

## Risks / Trade-offs

- **Free tier cold starts** — Services spin down after 15 minutes of inactivity; first request takes ~30-60s. Acceptable for a demo/evaluation period.
- **Free PostgreSQL expiration** — Free Render databases expire after 90 days. Sufficient for the intended use case.
- **Build-time VITE_API_URL** — The API URL is baked into the frontend at build time. If the API service URL changes, the Web service must be redeployed. This is inherent to Vite's env var model and not a new constraint.
- **Deploy hooks are fire-and-forget** — The GitHub Action triggers the deploy but doesn't wait for it to complete or check its status. A failed Render build won't fail the GitHub Action. Acceptable for this scope; the Render dashboard shows build status.
- **Two deploy hooks fire independently** — API and Web services deploy in parallel. If the API deploys a breaking change, the Web might briefly call an incompatible API. Acceptable for this scope since both deploy from the same commit.
