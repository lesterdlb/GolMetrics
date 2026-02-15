# CI/CD Pipeline

## Purpose

Specifies the GitHub Actions CI/CD pipeline for automated build, test, and deploy.

## Requirements

### Requirement: Pipeline Triggers

The pipeline SHALL be triggered by specific Git events.

#### Scenario: Push triggers

- **WHEN** code is pushed to `main` or `develop` branches
- **THEN** the pipeline SHALL trigger

#### Scenario: Pull request triggers

- **WHEN** a pull request targets `main`
- **THEN** the pipeline SHALL trigger

### Requirement: Build and Test Job

The pipeline SHALL build and test the application.

#### Scenario: Build steps

- **WHEN** the build-and-test job runs
- **THEN** it SHALL checkout the repository
- **AND** it SHALL set up .NET 10 SDK
- **AND** it SHALL run `dotnet restore`, `dotnet build --no-restore`, `dotnet test --no-build`

#### Scenario: Test failure handling

- **WHEN** any test fails
- **THEN** the pipeline SHALL fail and block merge

### Requirement: Code Coverage

The pipeline SHALL collect and report code coverage.

#### Scenario: Coverage collection

- **WHEN** tests are executed
- **THEN** the pipeline SHALL collect code coverage via `--collect:"XPlat Code Coverage"`

#### Scenario: Coverage reporting

- **WHEN** coverage is collected
- **THEN** the pipeline SHALL upload coverage reports (e.g., Codecov)

### Requirement: Deploy Job

The pipeline SHALL deploy on successful merges to main.

#### Scenario: Deploy conditions

- **WHEN** code is pushed to `main` (not on PRs or `develop`)
- **THEN** the deploy job SHALL run
- **AND** it SHALL depend on the build-and-test job succeeding

#### Scenario: Deployment execution

- **WHEN** the deploy job runs
- **THEN** it SHALL deploy the application to the configured hosting platform

#### Scenario: Deployment credentials

- **WHEN** deployment credentials are required
- **THEN** they SHALL be stored as GitHub repository secrets
