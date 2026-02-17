# Frontend Core

## Purpose

Defines the frontend application shell: build tooling, styling system, component library, routing, state management, and HTTP client configuration.

## Requirements

### Requirement: Build Tooling

The application SHALL be a React SPA built with Vite and TypeScript.

#### Scenario: Application entry point

- **WHEN** the application starts
- **THEN** `main.tsx` SHALL render `<App />` into the `#root` DOM element

#### Scenario: Backend URL configuration

- **WHEN** the application makes API calls
- **THEN** it SHALL use the `VITE_API_URL` environment variable as the backend base URL

### Requirement: Styling System

The application SHALL use Tailwind CSS with shadcn/ui components.

#### Scenario: Tailwind CSS usage

- **WHEN** components are styled
- **THEN** they SHALL use Tailwind CSS utility classes

#### Scenario: shadcn/ui components

- **WHEN** UI elements are rendered
- **THEN** they SHALL use shadcn/ui components (button, input, textarea, card, avatar, scroll-area)

#### Scenario: Class name utility

- **WHEN** conditional class names are composed
- **THEN** they SHALL use a `cn()` utility function built with `clsx` and `tailwind-merge`

### Requirement: Routing

The application SHALL use React Router for client-side navigation.

#### Scenario: Route definitions

- **WHEN** routes are defined
- **THEN** the application SHALL include `/login`, `/register`, `/chat` (protected), and `/settings` (protected)

#### Scenario: Unauthenticated access to protected route

- **WHEN** an unauthenticated user navigates to a protected route
- **THEN** the application SHALL redirect to `/login`

#### Scenario: Authenticated access to auth routes

- **WHEN** an authenticated user navigates to `/login` or `/register`
- **THEN** the application SHALL redirect to `/chat`

#### Scenario: Root path redirect

- **WHEN** a user navigates to `/`
- **THEN** the application SHALL redirect to `/chat`

### Requirement: State Management

The application SHALL use Zustand for global auth state.

#### Scenario: Auth store shape

- **WHEN** the auth store is initialized
- **THEN** `useAuthStore` SHALL expose `token`, `user`, `isAuthenticated`, `login()`, and `logout()`

#### Scenario: Token persistence

- **WHEN** a user logs in
- **THEN** the JWT token SHALL be persisted in `localStorage`

#### Scenario: Logout behavior

- **WHEN** `logout()` is called
- **THEN** the token SHALL be cleared from `localStorage` and the store state SHALL be reset

### Requirement: HTTP Client

The application SHALL use an Axios instance for API communication.

#### Scenario: Base URL configuration

- **WHEN** the Axios instance is created
- **THEN** it SHALL use `VITE_API_URL` as the `baseURL`

#### Scenario: Authorization header injection

- **WHEN** an API request is made and a token exists in localStorage
- **THEN** a request interceptor SHALL add `Authorization: Bearer {token}` to the request headers

#### Scenario: Unauthorized response handling

- **WHEN** an API call returns HTTP 401
- **THEN** the response interceptor SHALL call `logout()` and redirect to `/login`

### Requirement: Frontend Scaffolding

The application SHALL be scaffolded with specific packages, configuration, and folder structure.

#### Scenario: Core npm packages

- **WHEN** the frontend project is initialized
- **THEN** it SHALL install: `react`, `react-dom`, `react-router-dom`, `axios`, `zustand`, `tailwindcss`, `@tailwindcss/vite`, `clsx`, `tailwind-merge`, `class-variance-authority`, `react-markdown`, `lucide-react`

#### Scenario: shadcn/ui setup

- **WHEN** the component library is initialized
- **THEN** it SHALL run `npx shadcn@latest init` to set up shadcn/ui
- **AND** it SHALL install components via `npx shadcn@latest add button input textarea card avatar scroll-area`

#### Scenario: Vite configuration

- **WHEN** `vite.config.ts` is configured
- **THEN** it SHALL proxy `/api` requests to `http://localhost:7000` in development
- **AND** it SHALL include the `@vitejs/plugin-react` and `@tailwindcss/vite` plugins
- **AND** it SHALL define the `@` path alias resolving to `./src`

#### Scenario: TypeScript configuration

- **WHEN** `tsconfig.json` is configured
- **THEN** it SHALL target `ES2022` with strict mode enabled
- **AND** it SHALL define the `@/` path alias mapping to `./src/`

#### Scenario: Folder structure

- **WHEN** the project is organized
- **THEN** it SHALL follow this structure under `src/GolMetrics.Web/src/`: `pages/` (route-level components), `components/` (reusable UI components), `components/ui/` (shadcn/ui components), `services/` (API service modules), `store/` (Zustand stores), `types/` (TypeScript type definitions), `lib/` (utility functions like `cn()`)

#### Scenario: Existing custom theme preserved

- **WHEN** shadcn/ui is initialized
- **THEN** the existing custom theme variables (background, primary, secondary, accent colors) and custom animations (pulse-slow, float) SHALL be preserved in `index.css`
