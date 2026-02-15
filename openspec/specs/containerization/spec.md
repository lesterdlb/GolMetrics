# Containerization

## Purpose

Specifies the Docker and Docker Compose configuration for the full stack (backend, frontend, database).

## Requirements

### Requirement: Backend Dockerfile

The backend SHALL be containerized using a multi-stage Docker build.

#### Scenario: Build and publish

- **WHEN** the backend Docker image is built
- **THEN** it SHALL use a .NET SDK stage for restore, build, and publish in Release configuration
- **AND** it SHALL use a .NET ASP.NET runtime image for the final stage

#### Scenario: Runtime configuration

- **WHEN** the backend container starts
- **THEN** it SHALL expose port 8080
- **AND** the entrypoint SHALL be `dotnet GolMetrics.API.dll`

### Requirement: Frontend Dockerfile

The frontend SHALL be containerized using a multi-stage Docker build.

#### Scenario: Build stage

- **WHEN** the frontend Docker image is built
- **THEN** it SHALL use a Node.js stage that runs `npm ci` then `npm run build`

#### Scenario: Serving stage

- **WHEN** the frontend container starts
- **THEN** it SHALL serve the `dist/` output via nginx
- **AND** it SHALL use a custom `nginx.conf` for SPA routing and API proxying

### Requirement: Docker Compose

The system SHALL provide a Docker Compose configuration for the full stack.

#### Scenario: Service definitions

- **WHEN** Docker Compose is configured
- **THEN** it SHALL define 3 services: `api`, `web`, `db`

#### Scenario: Database service

- **WHEN** the `db` service is configured
- **THEN** it SHALL use `postgres:16-alpine` with configurable `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`
- **AND** it SHALL use a named volume for data persistence

#### Scenario: API service

- **WHEN** the `api` service is configured
- **THEN** it SHALL depend on `db`, expose port 7000 mapped to container port 8080
- **AND** it SHALL receive database connection string, JWT, and encryption config via environment variables

#### Scenario: Web service

- **WHEN** the `web` service is configured
- **THEN** it SHALL depend on `api`, expose port 5173
- **AND** it SHALL receive `VITE_API_URL` via environment variable

#### Scenario: Full stack startup

- **WHEN** `docker compose up --build` is run
- **THEN** all 3 services SHALL start and be accessible

### Requirement: Health Check

The system SHALL be reachable when the stack is running.

#### Scenario: API reachability

- **WHEN** the stack is running
- **THEN** the API SHALL be reachable at `http://localhost:7000`

#### Scenario: Frontend reachability

- **WHEN** the stack is running
- **THEN** the frontend SHALL be reachable at `http://localhost:5173`
