## MODIFIED Requirements

### Requirement: Frontend Dockerfile

The frontend SHALL be containerized using a multi-stage Docker build.

#### Scenario: Build stage

- **WHEN** the frontend Docker image is built
- **THEN** it SHALL use a Node.js stage that runs `npm ci` then `npm run build`

#### Scenario: Serving stage

- **WHEN** the frontend container starts
- **THEN** it SHALL serve the `dist/` output via nginx
- **AND** it SHALL use a custom nginx config for SPA routing
- **AND** it SHALL accept a `NGINX_CONF` build arg (default: `nginx.conf`) to select which nginx config to use
- **AND** it SHALL accept a `PORT` build arg (default: `5173`) to set the exposed port

### Requirement: Health Check

The system SHALL be reachable when the stack is running.

#### Scenario: API reachability

- **WHEN** the stack is running
- **THEN** the API SHALL be reachable at `http://localhost:7000`

#### Scenario: Frontend reachability (local)

- **WHEN** the stack is running via docker-compose
- **THEN** the frontend SHALL be reachable at `http://localhost:5173`

#### Scenario: Frontend reachability (Render)

- **WHEN** the frontend is deployed to Render
- **THEN** it SHALL listen on port 10000

## ADDED Requirements

### Requirement: Render nginx configuration

The project SHALL include a separate `nginx.render.conf` for Render deployment.

#### Scenario: Render-specific nginx config

- **WHEN** the frontend is deployed to Render
- **THEN** `src/GolMetrics.Web/nginx.render.conf` SHALL listen on port `10000`
- **AND** it SHALL serve static files from `/usr/share/nginx/html`
- **AND** it SHALL use `try_files $uri $uri/ /index.html` for SPA routing
- **AND** it SHALL NOT include an API proxy block (frontend calls the API directly via public URL)
- **AND** it SHALL enable gzip compression for common static file types
