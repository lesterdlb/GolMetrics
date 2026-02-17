## Context

Login and Register pages are currently placeholder stubs. The backend auth endpoints (`POST /api/auth/login`, `POST /api/auth/register`) are fully implemented and return `{ accessToken, refreshToken, expiresAtUtc }`. The frontend has a Zustand auth store with `login(token, user)` and `logout()`, Axios interceptors for JWT injection, and React Router with `PublicRoute`/`ProtectedRoute` wrappers already configured.

Available UI building blocks: shadcn/ui (`button`, `input`, `card`, `label`), Tailwind CSS 4 with custom theme (dark navy background, blue primary, gold accent), `Background` layout component, and glass-morphism utilities.

## Goals / Non-Goals

**Goals:**
- Implement functional Login and Register forms with full API integration
- Display inline validation and API error messages
- Match the existing dark theme and glass-morphism aesthetic
- Reuse existing auth store, Axios instance, and routing infrastructure

**Non-Goals:**
- Password reset/forgot password flow
- OAuth/social login
- Email verification
- Remember me / session persistence beyond existing localStorage behavior
- Refresh token rotation logic (already handled by Axios interceptor)

## Decisions

### 1. Native form handling over React Hook Form

**Decision**: Use React's native form state with `useState` hooks.

**Rationale**: The forms are simple (2 fields each). Adding React Hook Form would introduce a dependency for minimal benefit. If forms grow more complex later, we can migrate.

**Alternative considered**: React Hook Form - overkill for email + password forms.

### 2. Shared AuthLayout component

**Decision**: Create a shared layout component used by both Login and Register pages.

**Rationale**: Both pages share identical structure - centered card over the Background component with branding. A shared layout avoids duplication.

**Location**: `src/GolMetrics.Web/src/components/auth/AuthLayout.tsx`

### 3. Inline error display

**Decision**: Display API errors as inline text below the form, not as toasts.

**Rationale**: Auth errors are contextual to the form. Inline display provides better UX than transient toasts that can be missed.

### 4. JWT decoding for user info

**Decision**: Decode the JWT access token client-side to extract `sub` (user ID) and `email` claims, rather than making a separate `/me` API call.

**Rationale**: The token already contains these claims (set by `TokenService`). Avoids an extra network round-trip on login/register.

**Library**: Use `jwtDecode` from `jwt-decode` package (lightweight, widely used).

**Alternative considered**: Returning user info in the login/register response body - would require backend changes, out of scope.

### 5. Password confirmation on Register

**Decision**: Include a "Confirm Password" field on the Register page with client-side matching validation.

**Rationale**: Standard UX pattern for registration. Prevents typos in passwords without requiring backend changes.

## Risks / Trade-offs

- **[Client-side JWT decoding]** Token claims could theoretically be tampered with. Mitigation: The token is validated server-side on every API call. Client-side claims are only used for display (email) and store hydration.
- **[No loading skeleton]** Simple spinner/disabled state during submission rather than skeleton UI. Mitigation: Auth calls are fast; a full skeleton would be over-engineered.
- **[jwt-decode dependency]** Adds a small package. Mitigation: It's ~1KB gzipped, well-maintained, and avoids manual base64 parsing.
