## 1. NuGet Packages

- [x] 1.1 Add `Microsoft.SemanticKernel` and `Microsoft.SemanticKernel.Connectors.Google` packages to `src/GolMetrics.API/GolMetrics.API.csproj`
- [x] 1.2 Run `dotnet restore src/GolMetrics.API/` to verify packages resolve

## 2. Service Interfaces and Stubs

- [x] 2.1 Create `src/GolMetrics.API/Core/Abstractions/IEncryptionService.cs` with `Encrypt(string plainText)` and `Decrypt(string cipherText)` methods returning `string`
- [x] 2.2 Create `src/GolMetrics.API/Core/Abstractions/IFootballApiClient.cs` with the methods defined in the football-data spec
- [x] 2.3 Create `src/GolMetrics.API/Core/Abstractions/ISemanticKernelService.cs` with `ProcessMessageAsync` method

## 3. DependencyInjection Extension Methods

- [x] 3.1 Add `#pragma warning disable SKEXP0070` at the top of `src/GolMetrics.API/DependencyInjection.cs`
- [x] 3.2 Add `AddSemanticKernel()` extension method to `src/GolMetrics.API/DependencyInjection.cs` that registers Semantic Kernel with `AddGoogleAIGeminiChatCompletion()` using `Gemini:ModelId` and `Gemini:ApiKey` from configuration
- [x] 3.3 Add `AddEncryptionServices()` extension method to `src/GolMetrics.API/DependencyInjection.cs` that registers `IEncryptionService` as singleton
- [x] 3.4 Add `AddFootballServices()` extension method to `src/GolMetrics.API/DependencyInjection.cs` that registers `IFootballApiClient` as a typed HttpClient with base address from `ApiFootball:BaseUrl`
- [x] 3.5 Verify `dotnet build src/GolMetrics.API/` compiles without errors

## 4. Program.cs Pipeline

- [x] 4.1 Update `src/GolMetrics.API/Program.cs` service registration to call extension methods in order: `AddApiServices()` -> `AddDatabase()` -> `AddAuthenticationServices()` -> `AddSemanticKernel()` -> `AddEncryptionServices()` -> `AddFootballServices()` -> `AddErrorHandling()` -> `AddCors()`
- [x] 4.2 Update `src/GolMetrics.API/Program.cs` middleware pipeline to: `UseExceptionHandler()` -> `UseSerilogRequestLogging()` -> `UseCors("AllowAll")` -> `UseAuthentication()` -> `UseAuthorization()` -> `MapSliceEndpoints()` -> `MapOpenApi()` + `MapScalarApiReference()` (dev only)
- [x] 4.3 Verify `dotnet build src/GolMetrics.API/` compiles without errors

## 5. Verification

- [x] 5.1 Run `dotnet build src/GolMetrics.API/` to confirm full compilation
- [x] 5.2 Run `dotnet test tests/GolMetrics.API.Tests/` to confirm existing tests pass (N/A - test project not yet created)
