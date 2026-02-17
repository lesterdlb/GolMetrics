## Context

The frontend is a single-page React app that currently renders everything in `App.tsx` with no routing, no auth state, and raw `fetch()` calls for API communication. The backend already has JWT authentication (login, register, refresh-token endpoints) but the frontend has no way to use it. This change implements the frontend auth infrastructure as defined in the `frontend-core` spec.

## Goals / Non-Goals

**Goals:**

- Zustand auth store with token persistence in localStorage
- React Router configuration with protected/public route guards
- Axios instance with JWT request interceptor and 401 response interceptor
- TypeScript types for auth domain
- Refactored `App.tsx` and `main.tsx` to use router

**Non-Goals:**

- Login/register page UI (separate ticket)
- Settings page UI (separate ticket)
- Token refresh logic in the Axios interceptor (will be added when refresh-token flow is implemented on frontend)
- Backend changes

## Decisions

### 1. Auth store shape

The `useAuthStore` will hold `token: string | null`, `user: AuthUser | null`, and computed `isAuthenticated`. Actions: `login(token, user)` persists to localStorage, `logout()` clears both store and localStorage.

**Why**: Matches the `frontend-core` spec exactly. Keeping user info in the store avoids parsing the JWT on every render.

### 2. Store hydration from localStorage

On store creation, Zustand's `persist` middleware will hydrate `token` and `user` from localStorage automatically.

**Alternative considered**: Manual hydration in `main.tsx` — rejected because Zustand's built-in `persist` middleware handles this cleanly with no extra code.

### 3. Route guard components

`ProtectedRoute` checks `isAuthenticated` and redirects to `/login`. `PublicRoute` (for login/register) checks `isAuthenticated` and redirects to `/chat`. Both are simple wrapper components using `<Navigate>`.

**Alternative considered**: A single `AuthGuard` with a mode prop — rejected as more complex with no benefit.

### 4. Router setup with `createBrowserRouter`

Use React Router's data router API (`createBrowserRouter` + `RouterProvider`) rather than `<BrowserRouter>` with `<Routes>`.

**Why**: This is the recommended approach in React Router v7 and enables future use of loaders/actions.

### 5. Axios instance in `services/api.ts`

A single Axios instance with:
- `baseURL` from Vite proxy (empty string, since `/api` is proxied)
- Request interceptor: reads token from localStorage and sets `Authorization` header
- Response interceptor: on 401, calls `useAuthStore.getState().logout()` and redirects to `/login`

**Why reading from localStorage instead of store**: The interceptor runs outside React's render cycle. Accessing `getState()` for the token works but reading localStorage is simpler and always in sync since `persist` middleware writes to both.

### 6. Placeholder pages

Create minimal placeholder components for `/login`, `/register`, `/chat`, and `/settings` pages. The chat page will contain the current `App.tsx` content. Auth pages will be simple placeholders.

## Risks / Trade-offs

- **[No token refresh in interceptor]** -> On 401, the user is logged out. Token refresh will be added in a follow-up ticket. The 7-day token expiry makes this acceptable for now.
- **[localStorage for token storage]** -> XSS could expose the token. Mitigated by: CSP headers, no `dangerouslySetInnerHTML`, and this is the standard SPA approach per the spec.

## File Structure

```
src/GolMetrics.Web/src/
  store/
    auth-store.ts          # Zustand auth store with persist middleware
  services/
    api.ts                 # Axios instance with interceptors
  types/
    auth.ts                # AuthUser, LoginResponse types
  pages/
    ChatPage.tsx           # Current App.tsx chat content
    LoginPage.tsx          # Placeholder
    RegisterPage.tsx       # Placeholder
    SettingsPage.tsx       # Placeholder
  components/
    auth/
      ProtectedRoute.tsx   # Redirects to /login if not authenticated
      PublicRoute.tsx       # Redirects to /chat if authenticated
  App.tsx                  # RouterProvider setup
  main.tsx                 # Unchanged (renders <App />)
```
