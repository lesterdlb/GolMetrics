## 1. Types and Service Layer

- [x] 1.1 Add `UserProfile` type to `src/GolMetrics.Web/src/types.ts` with fields: `id`, `email`, `hasApiKey` (bool), `createdAt` (string)
- [x] 1.2 Create `src/GolMetrics.Web/src/services/settings-service.ts` with `getProfile()` calling `GET /api/user/profile` and `updateApiKey(key: string)` calling `PUT /api/user/api-key`
- [x] 1.3 Verify: TypeScript compiles without errors (`npm run build` in `src/GolMetrics.Web/`)

## 2. Settings Store

- [x] 2.1 Create `src/GolMetrics.Web/src/store/settings-store.ts` Zustand store with state: `profile` (UserProfile | null), `isLoadingProfile`, `isSubmittingApiKey`; actions: `fetchProfile()`, `submitApiKey(key: string)`
- [x] 2.2 `fetchProfile()` SHALL call settings service, set profile on success, show error toast on failure
- [x] 2.3 `submitApiKey()` SHALL call settings service, show success toast and re-fetch profile on success, show error toast on failure
- [x] 2.4 Verify: TypeScript compiles without errors

## 3. Settings Page UI

- [x] 3.1 Replace stub in `src/GolMetrics.Web/src/pages/SettingsPage.tsx` with full implementation using Background + Header layout and centered glass-panel card
- [x] 3.2 Implement Profile Information section displaying email, creation date (formatted), and API key status badge
- [x] 3.3 Implement loading skeleton for profile section while `isLoadingProfile` is true
- [x] 3.4 Implement API Key Management section with password input, show/hide toggle, and "Save API Key" submit button
- [x] 3.5 Implement client-side validation (empty field check) and form submission state (disabled input + button with loading indicator)
- [x] 3.6 Verify: Page renders correctly at `/settings` with all sections visible

## 4. Navigation

- [x] 4.1 Modify `src/GolMetrics.Web/src/components/layout/Header.tsx` to add a settings icon button (gear icon from lucide-react) that navigates to `/settings`
- [x] 4.2 Verify: Settings icon appears in header and navigates to `/settings`

## 5. Final Verification

- [x] 5.1 Full build passes: `npm run build` in `src/GolMetrics.Web/`
- [x] 5.2 Manual verification: navigate to `/settings`, profile loads, API key form submits with success/error feedback
