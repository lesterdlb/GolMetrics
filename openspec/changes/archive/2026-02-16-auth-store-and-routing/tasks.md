## 1. Types and Auth Store

- [x] 1.1 Create `src/GolMetrics.Web/src/types/auth.ts` with `AuthUser` (id, email) and `LoginResponse` (accessToken, refreshToken, expiresAtUtc) types
- [x] 1.2 Create `src/GolMetrics.Web/src/store/auth-store.ts` with Zustand `useAuthStore` using `persist` middleware: state (`token`, `user`, `isAuthenticated`), actions (`login`, `logout`), localStorage persistence
- [x] 1.3 Verify: import `useAuthStore` in a test file or console to confirm store initializes without errors

## 2. Axios HTTP Client

- [x] 2.1 Create `src/GolMetrics.Web/src/services/api.ts` with Axios instance: empty `baseURL` (Vite proxy handles `/api`), request interceptor injecting `Authorization: Bearer {token}` from localStorage, response interceptor calling `logout()` and redirecting to `/login` on 401
- [x] 2.2 Verify: confirm TypeScript compiles with `npx tsc --noEmit` from `src/GolMetrics.Web/`

## 3. Route Guard Components

- [x] 3.1 Create `src/GolMetrics.Web/src/components/auth/ProtectedRoute.tsx` — reads `isAuthenticated` from `useAuthStore`, renders `<Outlet />` if authenticated, otherwise `<Navigate to="/login" replace />`
- [x] 3.2 Create `src/GolMetrics.Web/src/components/auth/PublicRoute.tsx` — reads `isAuthenticated` from `useAuthStore`, renders `<Outlet />` if not authenticated, otherwise `<Navigate to="/chat" replace />`

## 4. Placeholder Pages

- [x] 4.1 Create `src/GolMetrics.Web/src/pages/LoginPage.tsx` — minimal placeholder with "Login" heading
- [x] 4.2 Create `src/GolMetrics.Web/src/pages/RegisterPage.tsx` — minimal placeholder with "Register" heading
- [x] 4.3 Create `src/GolMetrics.Web/src/pages/SettingsPage.tsx` — minimal placeholder with "Settings" heading
- [x] 4.4 Create `src/GolMetrics.Web/src/pages/ChatPage.tsx` — extract current chat UI from `App.tsx` into this page component

## 5. Router and App Refactor

- [x] 5.1 Refactor `src/GolMetrics.Web/src/App.tsx` — replace inline chat UI with `createBrowserRouter` + `RouterProvider`: `/` redirects to `/chat`, `/login` and `/register` wrapped in `PublicRoute`, `/chat` and `/settings` wrapped in `ProtectedRoute`
- [x] 5.2 Verify: run `npm run build` from `src/GolMetrics.Web/` to confirm the app compiles without errors
