## Why

TICK-014: The Login and Register pages are currently placeholder stubs (rendering only headings). Users cannot authenticate through the frontend, blocking all protected functionality. The backend auth endpoints (`POST /api/auth/login`, `POST /api/auth/register`) are fully implemented and waiting for frontend integration.

## What Changes

- Implement Login page with email/password form, API integration, error handling, and navigation to Register
- Implement Register page with email/password form, API integration, validation error display, and navigation to Login
- Both pages integrate with the existing Zustand auth store and Axios instance for JWT management
- Both pages use the existing Background component and shadcn/ui components for consistent styling

## Capabilities

### New Capabilities

_None. This change implements existing requirements defined in `frontend-features`._

### Modified Capabilities

- `frontend-features`: Implements the Login Screen and Register Screen requirements that are currently stubbed out. No spec-level changes needed; requirements are already defined.

## Impact

- **Frontend pages**: `src/GolMetrics.Web/src/pages/LoginPage.tsx` and `RegisterPage.tsx` (replace stubs)
- **Possible new shadcn/ui components**: `label`, `form` components if needed for form layout
- **No backend changes**: All auth endpoints already exist
- **No new dependencies**: Uses existing React Hook Form or native form handling with existing packages

## Dependencies

- No new NuGet packages required
- Possible npm additions: none expected (shadcn/ui components are added via CLI, not npm)
