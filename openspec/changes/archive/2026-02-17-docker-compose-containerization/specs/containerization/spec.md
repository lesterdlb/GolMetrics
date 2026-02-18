## MODIFIED Requirements

### Requirement: Docker Compose

The system SHALL provide a Docker Compose configuration for the full stack.

#### Scenario: Service definitions

- **WHEN** Docker Compose is configured
- **THEN** it SHALL define 3 services: `api`, `web`, `db`

#### Scenario: Database service

- **WHEN** the `db` service is configured
- **THEN** it SHALL use `postgres:16-alpine` with configurable `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`
- **AND** it SHALL use a named volume for data persistence
- **AND** it SHALL include a health check using `pg_isready`

#### Scenario: API service

- **WHEN** the `api` service is configured
- **THEN** it SHALL depend on `db` with `condition: service_healthy`, expose port 7000 mapped to container port 8080
- **AND** it SHALL receive database connection string via `ConnectionStrings__DefaultConnection`
- **AND** it SHALL receive JWT configuration via `TokenOptions__SecretKey`, `TokenOptions__Issuer`, `TokenOptions__Audience`, `TokenOptions__ExpirationMinutes`
- **AND** it SHALL receive encryption configuration via `Encryption__Key`
- **AND** it SHALL include a health check using `wget` against a health endpoint

#### Scenario: Web service

- **WHEN** the `web` service is configured
- **THEN** it SHALL depend on `api` with `condition: service_healthy`, expose port 5173
- **AND** it SHALL pass `VITE_API_URL` as a build argument

#### Scenario: Full stack startup

- **WHEN** `docker compose up --build` is run
- **THEN** all 3 services SHALL start in order: `db` -> `api` -> `web`
- **AND** each service SHALL wait for its dependencies to be healthy before starting

## ADDED Requirements

### Requirement: Dockerignore

The project SHALL include `.dockerignore` files to optimize Docker build context.

#### Scenario: Root dockerignore for API builds

- **WHEN** the API Docker image is built from the repository root
- **THEN** a `.dockerignore` file SHALL exclude `node_modules/`, `.git/`, `bin/`, `obj/`, `*.md`, `openspec/`, and test project output directories

#### Scenario: Web dockerignore for frontend builds

- **WHEN** the Web Docker image is built from `src/GolMetrics.Web/`
- **THEN** a `.dockerignore` file SHALL exclude `node_modules/`, `.git/`, `dist/`, and `*.md`

### Requirement: Backend Dockerfile cleanup

The backend Dockerfile SHALL only expose ports that are used.

#### Scenario: Port exposure

- **WHEN** the backend Docker image is built
- **THEN** the Dockerfile SHALL expose only port 8080
- **AND** it SHALL NOT expose port 8081

### Requirement: Service restart policy

All services SHALL define restart policies.

#### Scenario: Restart on failure

- **WHEN** a service container crashes
- **THEN** it SHALL restart automatically using `restart: unless-stopped`
