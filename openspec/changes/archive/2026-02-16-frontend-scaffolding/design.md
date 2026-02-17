## Context

The frontend at `src/GolMetrics.Web/` has a working chat UI but is missing several packages and configuration defined in the `frontend-core` spec. Currently installed: React 19, Vite 7, Tailwind CSS 4 (inline theme), lucide-react, clsx, tailwind-merge, CVA. Missing: react-router-dom, axios, zustand, react-markdown. shadcn/ui is not initialized (a manually-built button component exists). No Vite proxy is configured. The folder structure lacks `pages/`, `services/`, `store/`, and `types/` directories.

## Goals / Non-Goals

**Goals:**
- Install all missing npm packages required by `frontend-core` spec
- Initialize shadcn/ui and add required components (button, input, textarea, card, avatar, scroll-area)
- Configure Vite dev proxy for `/api` -> `http://localhost:7000`
- Create the folder structure: `pages/`, `services/`, `store/`, `types/`
- Preserve the existing chat UI and custom theme

**Non-Goals:**
- Implementing routing, auth store, or Axios interceptors (separate changes)
- Refactoring existing components to use new shadcn/ui components
- Adding pages or stores (just creating the directories)

## Decisions

### 1. shadcn/ui initialization approach

**Decision**: Run `npx shadcn@latest init` with New York style, neutral base color, and CSS variables enabled. Then add components individually.

**Rationale**: shadcn/ui copies component source into the project, giving full control. The existing manually-built button component follows shadcn patterns (CVA + forwardRef), so the shadcn button will replace it seamlessly.

**Alternative considered**: Keep the manual button and only add new components. Rejected because maintaining two component patterns creates inconsistency.

### 2. Vite proxy configuration

**Decision**: Add a `server.proxy` entry in `vite.config.ts` that proxies `/api` to `http://localhost:7000` with `changeOrigin: true`.

**Rationale**: The backend runs on port 7000 (per launchSettings). A proxy avoids CORS issues in development and matches the `frontend-core` spec. The existing `VITE_API_URL` env var will remain for non-proxied environments (production).

### 3. Tailwind CSS configuration

**Decision**: Keep the existing inline `@theme` configuration in `index.css`. shadcn/ui init may generate a `tailwind.config` file; if so, ensure it coexists with the inline theme by using CSS variable references.

**Rationale**: Tailwind CSS 4 supports inline theme configuration natively. The existing custom colors and animations are defined there. shadcn/ui's CSS variables approach is compatible.

### 4. Folder structure with .gitkeep

**Decision**: Create empty directories with `.gitkeep` files so they are tracked by git.

**Rationale**: Empty directories are not tracked by git. The `.gitkeep` convention ensures the structure is visible to all developers.

## Risks / Trade-offs

- **[shadcn/ui init may conflict with existing Tailwind config]** -> Review generated files after init; merge any conflicts with existing `index.css` theme manually.
- **[Existing button component replaced]** -> The shadcn button uses the same CVA pattern; existing usage should be compatible. Verify after component installation.
- **[react-markdown adds bundle size]** -> Acceptable trade-off for rendering AI chat responses with formatting.
