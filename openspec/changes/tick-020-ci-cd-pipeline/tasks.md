## 1. Render nginx configuration

- [x] 1.1 Create `src/GolMetrics.Web/nginx.render.conf` — listen on port 10000, serve static files with SPA routing, gzip compression, no API proxy block
- [x] 1.2 Modify `src/GolMetrics.Web/Dockerfile` — add `NGINX_CONF` build arg (default: `nginx.conf`) and `PORT` build arg (default: `5173`); use `COPY ${NGINX_CONF}` and `EXPOSE ${PORT}`
- [x] 1.3 Verify docker-compose still works: `docker compose build web` (should use defaults and build successfully)

## 2. Render Blueprint

- [x] 2.1 Create `render.yaml` at repository root with:
  - `golmetrics-api` web service: `runtime: docker`, `plan: free`, `dockerfilePath: src/GolMetrics.API/Dockerfile`, `dockerContext: .`, `branch: finalproject-LDLB`, `healthCheckPath: /health`, environment variables (`ASPNETCORE_ENVIRONMENT`, `ConnectionStrings__DefaultConnection` via `fromDatabase`, `TokenOptions__*`, `Encryption__Key`, `ApiFootball__*`, `Gemini__*`)
  - `golmetrics-web` web service: `runtime: docker`, `plan: free`, `dockerfilePath: src/GolMetrics.Web/Dockerfile`, `dockerContext: src/GolMetrics.Web`, `branch: finalproject-LDLB`, `VITE_API_URL` via `fromService` referencing `golmetrics-api` host, `NGINX_CONF=nginx.render.conf`, `PORT=10000`
  - `golmetrics-db` database: `plan: free`
- [x] 2.2 Validate render.yaml syntax: `python3 -c "import yaml; yaml.safe_load(open('render.yaml'))"`

## 3. GitHub Actions Workflow

- [x] 3.1 Create `.github/workflows/ci-cd.yml` with:
  - Trigger on push to `finalproject-LDLB` and PRs targeting `finalproject-LDLB`
  - `build-and-test` job on `ubuntu-latest`: checkout, setup .NET 10 SDK, `dotnet restore`, `dotnet build --no-restore`, `dotnet test --no-build --filter "Category=Unit"`, `dotnet test --no-build --filter "Category=Integration"`
  - `deploy` job: `needs: build-and-test`, `if: github.event_name == 'push'`, curl `RENDER_DEPLOY_HOOK_API` and `RENDER_DEPLOY_HOOK_WEB` from GitHub secrets
- [x] 3.2 Verify workflow syntax: `python3 -c "import yaml; yaml.safe_load(open('.github/workflows/ci-cd.yml'))"`

## 4. Final Verification

- [x] 4.1 Run `docker compose build` to confirm local development is unaffected
- [x] 4.2 Run `dotnet build src/GolMetrics.API/ --no-restore` to confirm API builds
- [x] 4.3 Run `dotnet test tests/GolMetrics.API.Tests/ --filter "Category=Unit"` to confirm unit tests pass
