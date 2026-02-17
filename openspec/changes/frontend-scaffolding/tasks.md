## 1. Install Missing Packages

- [x] 1.1 Install runtime dependencies: `npm install react-router-dom axios zustand react-markdown` in `src/GolMetrics.Web/`
- [x] 1.2 Verify `src/GolMetrics.Web/package.json` includes all required packages

## 2. Initialize shadcn/ui

- [x] 2.1 Run `npx shadcn@latest init` in `src/GolMetrics.Web/` (New York style, neutral base color, CSS variables enabled)
- [x] 2.2 Add required components: `npx shadcn@latest add button input textarea card avatar scroll-area` in `src/GolMetrics.Web/`
- [x] 2.3 Remove the old manually-built button at `src/GolMetrics.Web/src/components/ui/button.tsx` if shadcn overwrites it; otherwise verify shadcn button replaced it
- [x] 2.4 Merge shadcn/ui CSS variables with existing custom theme in `src/GolMetrics.Web/src/index.css` (preserve background, primary, secondary, accent colors and custom animations)
- [x] 2.5 Update `src/GolMetrics.Web/src/lib/utils.ts` if shadcn generates a conflicting one (keep the `cn()` utility)

## 3. Configure Vite Proxy

- [x] 3.1 Add `server.proxy` configuration to `src/GolMetrics.Web/vite.config.ts`: proxy `/api` to `http://localhost:7000` with `changeOrigin: true`
- [x] 3.2 Verify proxy works: run `npm run dev` and confirm `/api` requests reach backend

## 4. Create Folder Structure

- [x] 4.1 Create `src/GolMetrics.Web/src/pages/` with `.gitkeep`
- [x] 4.2 Create `src/GolMetrics.Web/src/services/` with `.gitkeep`
- [x] 4.3 Create `src/GolMetrics.Web/src/store/` with `.gitkeep`
- [x] 4.4 Create `src/GolMetrics.Web/src/types/` with `.gitkeep`

## 5. Verification

- [x] 5.1 Run `npm run build` in `src/GolMetrics.Web/` to confirm TypeScript compilation succeeds
- [x] 5.2 Run `npm run dev` in `src/GolMetrics.Web/` to confirm dev server starts without errors
