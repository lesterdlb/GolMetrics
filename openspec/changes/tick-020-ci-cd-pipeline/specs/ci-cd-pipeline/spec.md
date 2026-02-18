## MODIFIED Requirements

### Requirement: Pipeline Triggers

The pipeline SHALL be triggered by specific Git events.

#### Scenario: Push triggers

- **WHEN** code is pushed to the `finalproject-LDLB` branch
- **THEN** the pipeline SHALL trigger

#### Scenario: Pull request triggers

- **WHEN** a pull request targets `finalproject-LDLB`
- **THEN** the pipeline SHALL trigger

### Requirement: Build and Test Job

The pipeline SHALL build and test the application.

#### Scenario: Build steps

- **WHEN** the build-and-test job runs
- **THEN** it SHALL checkout the repository
- **AND** it SHALL set up .NET 10 SDK
- **AND** it SHALL run `dotnet restore`
- **AND** it SHALL run `dotnet build --no-restore`

#### Scenario: Unit test execution

- **WHEN** the build completes
- **THEN** the pipeline SHALL run unit tests via `dotnet test --no-build --filter "Category=Unit"`

#### Scenario: Integration test execution

- **WHEN** unit tests pass
- **THEN** the pipeline SHALL run integration tests via `dotnet test --no-build --filter "Category=Integration"`
- **AND** the runner SHALL have Docker available for Testcontainers (ubuntu-latest provides this)

#### Scenario: Test failure handling

- **WHEN** any test fails (unit or integration)
- **THEN** the pipeline SHALL fail and block deployment

### Requirement: Deploy Job

The pipeline SHALL deploy to Render on successful pushes to the branch.

#### Scenario: Deploy conditions

- **WHEN** code is pushed to `finalproject-LDLB` (not on pull requests)
- **THEN** the deploy job SHALL run
- **AND** it SHALL depend on the build-and-test job succeeding

#### Scenario: Deploy via Render deploy hooks

- **WHEN** the deploy job runs
- **THEN** it SHALL trigger Render deployment by sending HTTP requests to deploy hook URLs
- **AND** it SHALL trigger the API service deploy hook via `curl "$RENDER_DEPLOY_HOOK_API"`
- **AND** it SHALL trigger the Web service deploy hook via `curl "$RENDER_DEPLOY_HOOK_WEB"`

#### Scenario: Deployment credentials

- **WHEN** deployment credentials are required
- **THEN** the deploy hook URLs SHALL be stored as GitHub repository secrets named `RENDER_DEPLOY_HOOK_API` and `RENDER_DEPLOY_HOOK_WEB`

## ADDED Requirements

### Requirement: Render Blueprint

The project SHALL include a `render.yaml` blueprint at the repository root for infrastructure-as-code deployment.

#### Scenario: API web service definition

- **WHEN** `render.yaml` is configured
- **THEN** it SHALL define a `web` service named `golmetrics-api`
- **AND** it SHALL use `runtime: docker`
- **AND** it SHALL use `plan: free`
- **AND** it SHALL set `dockerfilePath` to `src/GolMetrics.API/Dockerfile`
- **AND** it SHALL set `dockerContext` to `.` (repository root, since the Dockerfile copies from root)
- **AND** it SHALL set `branch` to `finalproject-LDLB`

#### Scenario: API environment variables

- **WHEN** the API service is configured
- **THEN** it SHALL set `ASPNETCORE_ENVIRONMENT` to `Production`
- **AND** it SHALL set `ConnectionStrings__DefaultConnection` via `fromDatabase` referencing the database's `connectionString` property
- **AND** it SHALL set `TokenOptions__SecretKey` with `generateValue: true`
- **AND** it SHALL set `TokenOptions__Issuer` to `GolMetrics`
- **AND** it SHALL set `TokenOptions__Audience` to `GolMetrics`
- **AND** it SHALL set `TokenOptions__ExpirationMinutes` to `60`
- **AND** it SHALL set `Encryption__Key` with `generateValue: true`
- **AND** it SHALL set `ApiFootball__BaseUrl` to `https://v3.football.api-sports.io`
- **AND** it SHALL set `ApiFootball__ApiKey` with `sync: false`
- **AND** it SHALL set `Gemini__ApiKey` with `sync: false`
- **AND** it SHALL set `Gemini__ModelId` to `gemini-2.0-flash`

#### Scenario: API health check

- **WHEN** the API service is configured
- **THEN** it SHALL set `healthCheckPath` to `/health`

#### Scenario: Frontend web service definition

- **WHEN** `render.yaml` is configured
- **THEN** it SHALL define a `web` service named `golmetrics-web`
- **AND** it SHALL use `runtime: docker`
- **AND** it SHALL use `plan: free`
- **AND** it SHALL set `dockerfilePath` to `src/GolMetrics.Web/Dockerfile`
- **AND** it SHALL set `dockerContext` to `src/GolMetrics.Web`
- **AND** it SHALL set `branch` to `finalproject-LDLB`

#### Scenario: Frontend environment variables

- **WHEN** the Web service is configured
- **THEN** it SHALL set `VITE_API_URL` via `fromService` referencing the `golmetrics-api` service's `host` property with `https://` prefix

#### Scenario: PostgreSQL database definition

- **WHEN** `render.yaml` is configured
- **THEN** it SHALL define a database named `golmetrics-db`
- **AND** it SHALL use `plan: free`

### Requirement: Frontend API Communication

The frontend SHALL communicate with the API via the API's public Render URL, not via Docker internal networking.

#### Scenario: Vite API URL injection

- **WHEN** the frontend Docker image is built on Render
- **THEN** `VITE_API_URL` SHALL be set to the API service's public HTTPS URL
- **AND** the frontend SHALL make API calls directly to this URL

#### Scenario: CORS configuration

- **WHEN** the API is deployed to Render
- **THEN** the API SHALL allow CORS requests from the frontend's Render URL
- **AND** the CORS policy SHALL allow any origin, any header, and any method

### Requirement: Workflow File Location

The GitHub Actions workflow SHALL be stored at `.github/workflows/ci-cd.yml`.

#### Scenario: File structure

- **WHEN** the pipeline is configured
- **THEN** the workflow file SHALL be located at `.github/workflows/ci-cd.yml`
- **AND** it SHALL define two jobs: `build-and-test` and `deploy`
- **AND** the `deploy` job SHALL have `needs: build-and-test`
- **AND** the `deploy` job SHALL have `if: github.event_name == 'push'` to skip deployment on PRs

## REMOVED Requirements

### Requirement: Code Coverage

**Reason**: Code coverage collection and reporting is out of scope for this change. The pipeline focuses on build, test, and deploy to Render.
**Migration**: None required. Coverage can be added in a future change.
