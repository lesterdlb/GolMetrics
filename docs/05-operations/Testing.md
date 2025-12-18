# Testing Strategy - GolMetrics

## 1. Pirámide de Testing

```
      /\
     /E2E\     5% - Flujo completo del sistema
    /____\
   /      \
  /  INT   \   25% - Integración con BD/APIs
 /__________\
/            \
/    UNIT     \ 70% - Lógica de negocio
/______________\
```

**Objetivo de cobertura:** >70% total

---

## 2. Tests Unitarios (xUnit)

### 2.1. Handlers

```csharp
// tests/Features/Chat/SendMessageTests.cs

public class SendMessageHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_SavesUserMessage()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();
        var aiServiceMock = new Mock<ISemanticKernelService>();
        aiServiceMock
            .Setup(x => x.ProcessQueryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("AI Response");

        var handler = new SendMessage.Handler(dbContext, aiServiceMock.Object);
        var command = new SendMessage.Command(
            UserId: Guid.NewGuid(),
            ConversationId: Guid.NewGuid(),
            Content: "Test message"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        dbContext.Messages.Should().HaveCount(2); // user + assistant
    }
}
```

### 2.2. Validators

```csharp
public class SendMessageValidatorTests
{
    [Fact]
    public void Validate_EmptyContent_ReturnsError()
    {
        // Arrange
        var validator = new SendMessage.Validator();
        var command = new SendMessage.Command(Guid.NewGuid(), Guid.NewGuid(), "");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Content");
    }
}
```

---

## 3. Tests de Integración

### 3.1. Repositorios (Testcontainers)

```csharp
public class CachedQueryRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    private AppDbContext _dbContext;

    public CachedQueryRepositoryTests()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _dbContext = CreateDbContext(_container.GetConnectionString());
        await _dbContext.Database.MigrateAsync();
    }

    [Fact]
    public async Task GetCachedQuery_ExistingHash_ReturnsData()
    {
        // Arrange
        var cachedQuery = new CachedQuery
        {
            QueryHash = "abc123",
            ResponseData = JsonDocument.Parse("{\"data\":\"test\"}"),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
        _dbContext.CachedQueries.Add(cachedQuery);
        await _dbContext.SaveChangesAsync();

        var repository = new CachedQueryRepository(_dbContext);

        // Act
        var result = await repository.GetByHashAsync("abc123");

        // Assert
        result.Should().NotBeNull();
        result!.QueryHash.Should().Be("abc123");
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();
}
```

---

## 4. Tests E2E (Playwright)

### 4.1. Flujo Completo de Chat

```csharp
[Test]
public async Task CompleteUserFlow_LoginAndSendMessage_Success()
{
    await using var playwright = await Playwright.CreateAsync();
    await using var browser = await playwright.Chromium.LaunchAsync();
    var page = await browser.NewPageAsync();

    // 1. Login
    await page.GotoAsync("http://localhost:5173/login");
    await page.FillAsync("input[name='email']", "test@example.com");
    await page.FillAsync("input[name='password']", "SecurePass123");
    await page.ClickAsync("button[type='submit']");

    // 2. Esperar redirección al chat
    await page.WaitForURLAsync("**/chat");

    // 3. Enviar mensaje
    await page.FillAsync("textarea[placeholder*='Pregunta']", "Goleadores Premier 2024");
    await page.ClickAsync("button:has-text('Enviar')");

    // 4. Verificar respuesta
    await page.WaitForSelectorAsync("text=Erling Haaland");

    // Assert
    var messageCount = await page.Locator(".message-bubble").CountAsync();
    Assert.That(messageCount, Is.GreaterThanOrEqualTo(2));
}
```

---

## 5. Mocking de Servicios Externos

### 5.1. API-Football Mock

```csharp
public class FakeFootballApiClient : IFootballApiClient
{
    public Task<FootballApiResponse> GetTopScorersAsync(int leagueId, int season)
    {
        return Task.FromResult(new FootballApiResponse
        {
            Response = new[]
            {
                new PlayerStats
                {
                    Player = new Player { Name = "E. Haaland" },
                    Statistics = new[] { new Stats { Goals = new Goals { Total = 27 } } }
                }
            }
        });
    }
}
```

---

## 6. Test Data Generation (Bogus)

```csharp
public class TestDataGenerator
{
    public static Faker<User> UserFaker => new Faker<User>()
        .RuleFor(u => u.Id, f => Guid.NewGuid())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword("password123"));

    public static List<User> GenerateUsers(int count = 10)
        => UserFaker.Generate(count);
}
```

---

## 7. Comandos

```bash
# Ejecutar todos los tests
dotnet test

# Solo unitarios
dotnet test --filter Category=Unit

# Solo integración
dotnet test --filter Category=Integration

# Con cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

---

## 8. CI/CD Integration

```yaml
# .github/workflows/test.yml
- name: Run Tests
  run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"

- name: Upload Coverage
  uses: codecov/codecov-action@v3
  with:
      files: ./coverage.opencover.xml
```

---

**Última actualización:** 2025-10-10
