## Why

TICK-017: The test project exists with 9 unit test files but has significant gaps compared to the `testing-standards` spec. There are no validator tests, no Chat feature tests, and no tests for core abstractions (Result, Entity, ErrorCategory). The project also lacks Bogus for test data generation. Filling these gaps brings unit test coverage closer to the 70% target defined in the spec.

## What Changes

- Add unit tests for core abstractions: `Result`, `Result<T>`, `Error`, `ErrorCategory`, `ResultExtensions`, `ValidationBehavior`, `EntityConfiguration`, and `Entity` base class
- Add unit tests for all FluentValidation validators across features (Auth, Chat, UserManagement)
- Add unit tests for Chat feature handlers: `CreateConversation`, `GetConversations`, `GetConversationMessages`, `SendMessage`
- Add Bogus NuGet package for test data generation
- Create shared test utilities: custom `Faker` configurations for domain entities

## Capabilities

### New Capabilities

- `unit-testing`: Unit test coverage for core abstractions, validators, and Chat feature handlers using xUnit, FluentAssertions, Moq, and Bogus

### Modified Capabilities

_None_ - no spec-level requirement changes; this implements existing requirements from `testing-standards`.

## Impact

- **Code**: `tests/GolMetrics.API.Tests/` - new test files under `Core/` and `Features/Chat/`
- **Dependencies**: Add `Bogus` NuGet package to test project
- **No breaking changes** to production code
