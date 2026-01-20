# Deployment Guide - GolMetrics

## 1. Requisitos Previos

- Docker 24+ y Docker Compose
- .NET 10 SDK (desarrollo local)
- Node.js 20+ (desarrollo local)
- PostgreSQL 16 (o contenedor)

---

## 2. Variables de Entorno

### Backend (.env)

```env
# Database
POSTGRES_USER=admin
POSTGRES_PASSWORD=secure_password_here
POSTGRES_DB=golmetrics
DATABASE_URL=Host=postgres;Database=golmetrics;Username=admin;Password=secure_password_here

# JWT
JWT__SECRETKEY=your-secret-key-min-32-chars
JWT__ISSUER=https://golmetrics.com
JWT__AUDIENCE=https://golmetrics.com
JWT__EXPIRATIONDAYS=7

# Encryption
ENCRYPTION__MASTERKEY=base64-encoded-32-byte-key

# External APIs
GEMINI_API_KEY=your-gemini-api-key
API_FOOTBALL_KEY=your-default-api-football-key

# Logging
SERILOG__MINIMUMLEVEL__DEFAULT=Information
```

### Frontend (.env)

```env
VITE_API_URL=https://localhost:7000
```

---

## 3. Docker Compose (Local)

### docker-compose.yml

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:16-alpine
    environment:
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
      POSTGRES_DB: ${POSTGRES_DB}
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  backend:
    build:
      context: ./src/GolMetrics.API
      dockerfile: Dockerfile
    environment:
      - DATABASE_URL=${DATABASE_URL}
      - JWT__SECRETKEY=${JWT__SECRETKEY}
      - GEMINI_API_KEY=${GEMINI_API_KEY}
    ports:
      - "7000:8080"
    depends_on:
      - postgres

  frontend:
    build:
      context: ./src/GolMetrics.Web
      dockerfile: Dockerfile
    environment:
      - VITE_API_URL=http://localhost:7000
    ports:
      - "5173:80"
    depends_on:
      - backend

volumes:
  postgres_data:
```

---

## 4. Dockerfile Backend

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "GolMetrics.API.dll"]
```

---

## 5. Dockerfile Frontend

```dockerfile
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . ./
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

---

## 6. CI/CD (GitHub Actions)

### .github/workflows/ci.yml

```yaml
name: CI

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore

      - name: Test
        run: dotnet test --no-build --verbosity normal

  deploy:
    needs: test
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Deploy to Render
        env:
          RENDER_API_KEY: ${{ secrets.RENDER_API_KEY }}
        run: |
          curl -X POST https://api.render.com/deploy/srv-xxx?key=$RENDER_API_KEY
```

---

## 7. Despliegue en Render.com

### render.yaml

```yaml
services:
  - type: web
    name: golmetrics-api
    env: docker
    dockerfilePath: ./src/GolMetrics.API/Dockerfile
    envVars:
      - key: DATABASE_URL
        fromDatabase:
          name: golmetrics-db
          property: connectionString
      - key: JWT__SECRETKEY
        sync: false

  - type: web
    name: golmetrics-frontend
    env: static
    buildCommand: npm run build
    staticPublishPath: ./dist

databases:
  - name: golmetrics-db
    databaseName: golmetrics
    user: admin
```

---

## 8. Comandos Útiles

```bash
# Desarrollo local
docker compose up --build

# Aplicar migraciones
dotnet ef database update --project src/GolMetrics.API

# Logs
docker compose logs -f backend

# Detener todo
docker compose down -v
```

---

**Última actualización:** 2025-10-10
