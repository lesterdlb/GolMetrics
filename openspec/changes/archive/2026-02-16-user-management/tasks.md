## 1. Shared Infrastructure

- [x] 1.1 Add `User` section to `src/GolMetrics.API/EndpointNames.cs` with constants: `GetProfile`, `UpdateApiKey` and routes `/api/user/profile`, `/api/user/api-key`
- [x] 1.2 Create `src/GolMetrics.API/Features/UserManagement/UserErrors.cs` with static properties: `InvalidApiKey` (BadRequest), `UserNotFound` (NotFound), `ApiValidationUnavailable` (BadRequest)
- [x] 1.3 Verify: `dotnet build src/GolMetrics.API/`

## 2. GetProfile Slice

- [x] 2.1 Create `src/GolMetrics.API/Features/UserManagement/GetProfile.cs` as `internal sealed class : ISlice` with nested Query, Response, Handler
- [x] 2.2 Query: `internal sealed record Query(Guid UserId) : IRequest<Result<Response>>`
- [x] 2.3 Response: `public sealed record Response(Guid Id, string Email, bool HasApiKey, DateTime CreatedAt)`
- [x] 2.4 Handler: inject `UserManager<User>`, look up by `query.UserId`, return `UserErrors.UserNotFound` if null, otherwise map to Response with `HasApiKey = !string.IsNullOrEmpty(user.EncryptedApiKey)`
- [x] 2.5 RegisterEndpoints: `GET /api/user/profile` with `RequirePermissions(Permissions.Users.Read)`, extract user ID from `ICurrentUserService`
- [x] 2.6 Verify: `dotnet build src/GolMetrics.API/`

## 3. UpdateApiKey Slice

- [x] 3.1 Create `src/GolMetrics.API/Features/UserManagement/UpdateApiKey.cs` as `internal sealed class : ISlice` with nested Command, Validator, Handler
- [x] 3.2 Command: `public sealed record Command(string ApiKey) : IRequest<Result>` (non-generic Result for 200 with no body)
- [x] 3.3 Validator: `ApiKey` not empty
- [x] 3.4 Handler: inject `UserManager<User>`, `IFootballApiClient`, `IEncryptionService`, `ICurrentUserService`
- [x] 3.5 Handler logic: find user by ID -> validate key via `IFootballApiClient.ValidateApiKeyAsync()` (catch `HttpRequestException`/`TaskCanceledException` -> return `UserErrors.ApiValidationUnavailable`; if returns false -> return `UserErrors.InvalidApiKey`) -> encrypt via `IEncryptionService.Encrypt()` -> set `user.EncryptedApiKey` -> `UserManager.UpdateAsync()`
- [x] 3.6 RegisterEndpoints: `PUT /api/user/api-key` with `RequirePermissions(Permissions.Users.Write)`
- [x] 3.7 Verify: `dotnet build src/GolMetrics.API/`

## 4. Testing

- [x] 4.1 Create unit tests in `tests/GolMetrics.API.Tests/Features/UserManagement/GetProfileTests.cs`: successful retrieval, user not found
- [x] 4.2 Create unit tests in `tests/GolMetrics.API.Tests/Features/UserManagement/UpdateApiKeyTests.cs`: successful update, invalid key, validation unavailable, user not found
- [x] 4.3 Verify: `dotnet test tests/GolMetrics.API.Tests/`
