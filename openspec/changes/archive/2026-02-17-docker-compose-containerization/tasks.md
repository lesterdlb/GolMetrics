## 1. Dockerignore Files

- [x] 1.1 Create `.dockerignore` at repository root excluding `node_modules/`, `.git/`, `**/bin/`, `**/obj/`, `*.md`, `openspec/`, `tests/`, `project-task-management/`, `.vs/`, `.vscode/`, `.idea/`
- [x] 1.2 Create `src/GolMetrics.Web/.dockerignore` excluding `node_modules/`, `.git/`, `dist/`, `*.md`
- [x] 1.3 Verify: confirm both files exist and contain expected patterns

## 2. API Dockerfile Cleanup

- [x] 2.1 Modify `src/GolMetrics.API/Dockerfile` to remove the `EXPOSE 8081` line (keep only `EXPOSE 8080`)
- [x] 2.2 Verify: `docker build -f src/GolMetrics.API/Dockerfile .` completes without errors

## 3. Docker Compose Configuration

- [x] 3.1 Update `docker-compose.yml` `db` service: add health check using `pg_isready -U admin -d golmetrics_db` with interval 10s, timeout 5s, retries 5; add `restart: unless-stopped`
- [x] 3.2 Update `docker-compose.yml` `api` service: add `condition: service_healthy` to `depends_on.db`; add environment variables for `TokenOptions__SecretKey`, `TokenOptions__Issuer`, `TokenOptions__Audience`, `TokenOptions__ExpirationMinutes`, `Encryption__Key`; add health check using `wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1`; add `restart: unless-stopped`
- [x] 3.3 Update `docker-compose.yml` `web` service: add `condition: service_healthy` to `depends_on.api`; change `VITE_API_URL` to a build arg; add `restart: unless-stopped`
- [x] 3.4 Add a `/health` endpoint to the API (minimal GET endpoint returning 200 OK) at `src/GolMetrics.API/Features/Health/HealthSlice.cs`
- [x] 3.5 Verify: `docker compose config` validates without errors

## 4. End-to-End Verification

- [x] 4.1 Run `docker compose up --build -d` and verify all 3 services start and become healthy
- [x] 4.2 Verify API is reachable at `http://localhost:7000/health`
- [x] 4.3 Verify frontend is reachable at `http://localhost:5173`
- [x] 4.4 Run `docker compose down` to clean up
