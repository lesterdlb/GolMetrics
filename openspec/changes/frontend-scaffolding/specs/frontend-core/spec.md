## MODIFIED Requirements

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
