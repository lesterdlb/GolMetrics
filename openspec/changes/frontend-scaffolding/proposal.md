## Why

TICK-012: The React frontend (`src/GolMetrics.Web/`) is missing several packages and configuration required by the `frontend-core` spec. Zustand, Axios, React Router, and react-markdown are not installed. shadcn/ui is not initialized (only a manually-built button exists). The Vite dev server lacks an API proxy, and the folder structure is incomplete (missing `pages/`, `services/`, `store/`, `types/` directories).

## What Changes

- Install missing npm packages: `react-router-dom`, `axios`, `zustand`, `react-markdown`
- Initialize shadcn/ui via `npx shadcn@latest init` and add required components (button, input, textarea, card, avatar, scroll-area)
- Configure Vite dev proxy for `/api` requests to `http://localhost:7000`
- Create missing folder structure: `pages/`, `services/`, `store/`, `types/`

## Capabilities

### New Capabilities

None. This change implements existing requirements.

### Modified Capabilities

- `frontend-core`: No requirement changes. This change brings the implementation in line with the existing spec (scaffolding, packages, proxy, folder structure).

## Impact

- **Code**: `src/GolMetrics.Web/` only (package.json, vite.config.ts, new directories, shadcn/ui config)
- **Dependencies**: New npm packages: react-router-dom, axios, zustand, react-markdown, shadcn/ui components
- **Breaking**: None. Existing chat UI continues to work; this adds infrastructure for future features.

## Dependencies

- npm packages: `react-router-dom`, `axios`, `zustand`, `react-markdown`
- shadcn/ui CLI: `npx shadcn@latest init` + component additions
