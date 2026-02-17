## 1. Dependencies and Shared Components

- [x] 1.1 Install `jwt-decode` package: `npm install jwt-decode` in `src/GolMetrics.Web/`
- [x] 1.2 Add shadcn/ui `label` component: `npx shadcn@latest add label` in `src/GolMetrics.Web/`
- [x] 1.3 Create `src/GolMetrics.Web/src/services/auth-service.ts` with `login(email, password)` and `register(email, password)` functions that call the API and return typed responses
- [x] 1.4 Create `src/GolMetrics.Web/src/components/auth/AuthLayout.tsx` shared layout component with Background, centered glass-panel card, and GOL METRICS branding
- [x] 1.5 Verify: `npm run build` succeeds in `src/GolMetrics.Web/`

## 2. Login Page

- [x] 2.1 Implement `src/GolMetrics.Web/src/pages/LoginPage.tsx` with email/password form, client-side validation, loading state, inline error display, and link to `/register`
- [x] 2.2 Verify: Login page renders at `/login`, submits to API, stores JWT, and redirects to `/chat` on success; displays inline error on 401

## 3. Register Page

- [x] 3.1 Implement `src/GolMetrics.Web/src/pages/RegisterPage.tsx` with email/password/confirm-password form, client-side validation (including password match), loading state, inline error display, and link to `/login`
- [x] 3.2 Verify: Register page renders at `/register`, submits to API, stores JWT, and redirects to `/chat` on success; displays inline errors on 400/409

## 4. Final Verification

- [x] 4.1 Verify: `npm run build` succeeds with no TypeScript errors
- [x] 4.2 Verify: Full auth flow works end-to-end (register -> redirect to chat -> logout -> login -> redirect to chat)
