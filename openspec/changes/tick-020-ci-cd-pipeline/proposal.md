## Why

TICK-020: The application is fully containerized but has no automated pipeline to build, test, and deploy. Manual deployment is error-prone and blocks continuous delivery. The project needs a GitHub Actions workflow that runs tests on every push to `finalproject-LDLB` and deploys to Render via deploy hooks.

## What Changes

- Add a GitHub Actions workflow (`.github/workflows/ci-cd.yml`) that builds, runs unit and integration tests, and triggers Render deployment on push to `finalproject-LDLB`
- Add a Render Blueprint (`render.yaml`) defining two web services (`golmetrics-api`, `golmetrics-web`) and a managed PostgreSQL database (`golmetrics-db`)
- Update the frontend nginx configuration to remove the Docker-internal API proxy and listen on Render's expected port (10000)
- Update the frontend Dockerfile to expose port 10000 instead of 5173

## Capabilities

### New Capabilities

None (the ci-cd-pipeline spec already exists)

### Modified Capabilities

- `ci-cd-pipeline`: Updated to target `finalproject-LDLB` branch, deploy to Render via deploy hooks, and include Render Blueprint specification
- `containerization`: Frontend Dockerfile and nginx.conf changes for Render compatibility (port 10000, remove API proxy block)

## Impact

- **Code**: `.github/workflows/ci-cd.yml` (new), `render.yaml` (new), `src/GolMetrics.Web/nginx.conf` (modified), `src/GolMetrics.Web/Dockerfile` (modified)
- **Dependencies**: No new packages. Requires Render account and two GitHub repository secrets (`RENDER_DEPLOY_HOOK_API`, `RENDER_DEPLOY_HOOK_WEB`)
- **APIs**: No API changes. CORS already allows all origins.
- **Systems**: Render (hosting platform), GitHub Actions (CI/CD)
