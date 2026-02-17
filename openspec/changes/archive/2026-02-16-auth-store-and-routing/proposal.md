## Why

TICK-013: The frontend currently renders a single `App` component with no routing, authentication state, or API client configuration. Users cannot log in, and there is no mechanism to protect routes or attach JWT tokens to API requests. This change implements the frontend auth infrastructure required before building login/register pages and protected features.

## What Changes

- Create a Zustand auth store (`useAuthStore`) exposing `token`, `user`, `isAuthenticated`, `login()`, `logout()` with localStorage persistence
- Configure React Router with route definitions: `/login`, `/register`, `/chat` (protected), `/settings` (protected), and `/` redirect
- Create `ProtectedRoute` and `PublicRoute` guard components for route access control
- Create a configured Axios instance with request interceptor (JWT injection) and response interceptor (401 logout + redirect)
- Refactor `App.tsx` to use `RouterProvider` instead of rendering chat UI directly
- Define TypeScript types for auth state and API responses

## Capabilities

### New Capabilities

_None_ - all requirements are already defined in the `frontend-core` spec.

### Modified Capabilities

_None_ - this implements existing requirements from `frontend-core` spec (Routing, State Management, HTTP Client sections).

## Impact

- **Frontend**: `src/GolMetrics.Web/src/` - new files in `store/`, `services/`, `pages/`, `components/`; refactored `App.tsx` and `main.tsx`
- **Dependencies**: No new npm packages required (react-router-dom, axios, zustand already installed)
- **Backend**: No backend changes
