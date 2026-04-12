# Instruções de Contexto: Testes NUnit - Omnis Review API

Você é um especialista em testes com NUnit e .NET 10, atuando no projeto Omnis Review API. Siga estas diretrizes estritamente ao gerar testes de unidade e integração.

## Tecnologias e Versões
- Framework de Testes: **NUnit** (versão 4.x)
- Mocking: **Moq** (versão 4.x)
- Projeto: **OmnisReview.Tests**
- .NET: 10 (C# 14)

## Estrutura de Pastas de Testes

```
OmnisReview.Tests/
├── Services/
│   ├── AuthServiceTests.cs
│   ├── EmailSenderTests.cs
│   └── [Nome]ServiceTests.cs
├── Repositorys/
│   ├── AuthRepositoryTests.cs
│   └── [Nome]RepositoryTests.cs
├── Controllers/
│   ├── AuthControllerTests.cs
│   └── [Nome]ControllerTests.cs
├── Fixtures/
│   ├── AuthTestFixture.cs
│   └── [Nome]TestFixture.cs
└── Helpers/
    ├── TestDataBuilder.cs
    └── MockSetupHelper.cs
```

## Padrão de Nomenclatura

**Métodos de Teste:**
```
[NomeDoMetodo]_[Cenario]_[ResultadoEsperado]
```

**Exemplos:**
- `Login_WithValidCredentials_ReturnsToken`
- `Register_WithExistingEmail_ThrowsException`
- `ForgotPassword_WithNullEmail_ThrowsArgumentNullException`
- `ChangePassword_WithIncorrectOldPassword_ReturnsFalse`

## Estrutura de Arquivo de Teste

### Imports e Namespace
```csharp
using NUnit.Framework;
using Moq;
using OmnisReview.Services;
using OmnisReview.Repositorys;
using OmnisReview.Models;

namespace OmnisReview.Tests.Services;
```

### Classe de Teste Básica
```csharp
[TestFixture]
public class AuthServiceTests
{
    private AuthService _authService = null!;
    private Mock<IAuthRepository> _mockAuthRepository = null!;
    private Mock<IEmailSender> _mockEmailSender = null!;

    [SetUp]
    public void SetUp()
    {
        _mockAuthRepository = new Mock<IAuthRepository>();
        _mockEmailSender = new Mock<IEmailSender>();
        _authService = new AuthService(_mockAuthRepository.Object, _mockEmailSender.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockAuthRepository?.Reset();
        _mockEmailSender?.Reset();
    }
}
```

## Regras Obrigatórias

### 1. **Nullability**
- Sempre trate o projeto com `<Nullable>enable</Nullable>`
- Use `!` (null-forgiving operator) quando apropriado em inicializações de SetUp
- Valide argumentos nulos com `Assert.Throws<ArgumentNullException>()`

```csharp
[Test]
public void Login_WithNullEmail_ThrowsArgumentNullException()
{
    Assert.Throws<ArgumentNullException>(() => _authService.Login(null!, "password"));
}
```

### 2. **Injeção de Dependência e Mocking**
- Use **Moq** para mockar interfaces
- Injete todas as dependências pelo construtor
- Sempre use `.Object` ao passar mocks para o construtor

```csharp
_mockAuthRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
    .ReturnsAsync(new ApplicationUser { Email = "test@example.com" });
```

### 3. **Padrão AAA (Arrange-Act-Assert)**
```csharp
[Test]
public async Task Login_WithValidCredentials_ReturnsToken()
{
    // Arrange
    var loginDto = new LoginDto { Email = "user@example.com", Password = "Password123!" };
    var user = new ApplicationUser { Email = "user@example.com", Id = "user-id" };
    
    _mockAuthRepository.Setup(x => x.GetUserByEmailAsync(loginDto.Email))
        .ReturnsAsync(user);

    // Act
    var result = await _authService.LoginAsync(loginDto);

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Token, Is.Not.Empty);
}
```

## Tipos de Testes

### Testes de Serviços (Services)
- Mockar repositórios e dependências externas
- Testar lógica de negócio
- Validar transações e fluxos

```csharp
[TestFixture]
public class AuthServiceTests
{
    private AuthService _authService = null!;
    private Mock<IAuthRepository> _mockAuthRepository = null!;

    [SetUp]
    public void SetUp()
    {
        _mockAuthRepository = new Mock<IAuthRepository>();
        _authService = new AuthService(_mockAuthRepository.Object);
    }

    [Test]
    public async Task Register_WithValidData_CreatesUserSuccessfully()
    {
        // Arrange
        var registerDto = new RegisterDto 
        { 
            Email = "newuser@example.com", 
            Password = "Password123!" 
        };

        _mockAuthRepository.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(true);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.That(result.Success, Is.True);
        _mockAuthRepository.Verify(x => x.CreateUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
    }
}
```

### Testes de Controladores (Controllers)
- Mockar serviços
- Testar respostas HTTP e status codes
- Validar bindagem de parâmetros

```csharp
[TestFixture]
public class AuthControllerTests
{
    private AuthController _authController = null!;
    private Mock<IAuthService> _mockAuthService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockAuthService = new Mock<IAuthService>();
        _authController = new AuthController(_mockAuthService.Object);
    }

    [Test]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "user@example.com", Password = "Password123!" };
        var response = new AuthResponseDto { Success = true, Token = "jwt-token" };

        _mockAuthService.Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(response);

        // Act
        var result = await _authController.Login(loginDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }
}
```

### Testes de Repositórios (Repositories)
- Usar DbContext em memória (InMemory Database)
- Testar queries LINQ
- Validar persistência

```csharp
[TestFixture]
public class AuthRepositoryTests
{
    private AuthRepository _authRepository = null!;
    private ApplicationDbContext _dbContext = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _authRepository = new AuthRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    [Test]
    public async Task GetUserByEmailAsync_WithExistingEmail_ReturnsUser()
    {
        // Arrange
        var user = new ApplicationUser { Id = "1", Email = "test@example.com", UserName = "test" };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _authRepository.GetUserByEmailAsync("test@example.com");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo("test@example.com"));
    }
}
```

## Fixtures e Builders

### Test Fixture (para setup reutilizável)
```csharp
[TestFixture]
public class AuthTestFixture
{
    protected ApplicationUser CreateTestUser(string email = "test@example.com")
    {
        return new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            UserName = email.Split('@')[0],
            EmailConfirmed = true
        };
    }

    protected LoginDto CreateValidLoginDto(string email = "test@example.com")
    {
        return new LoginDto
        {
            Email = email,
            Password = "Password123!"
        };
    }
}
```

### Test Data Builder
```csharp
public class UserBuilder
{
    private ApplicationUser _user;

    public UserBuilder()
    {
        _user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "default@example.com",
            UserName = "default",
            EmailConfirmed = true
        };
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithUserName(string userName)
    {
        _user.UserName = userName;
        return this;
    }

    public ApplicationUser Build() => _user;
}
```

## Asserções Comuns

```csharp
// Valores
Assert.That(result, Is.EqualTo(expected));
Assert.That(result, Is.Not.Null);
Assert.That(result, Is.Null);
Assert.That(result, Is.True);
Assert.That(result, Is.False);

// Coleções
Assert.That(collection, Is.Empty);
Assert.That(collection, Is.Not.Empty);
Assert.That(collection, Has.Count.EqualTo(3));
Assert.That(collection, Contains.Item(item));

// Strings
Assert.That(text, Is.EqualTo("expected"));
Assert.That(text, Does.Contain("substring"));
Assert.That(text, Does.StartWith("prefix"));

// Tipos e Exceções
Assert.That(obj, Is.TypeOf<SomeType>());
Assert.That(obj, Is.InstanceOf<BaseType>());
Assert.Throws<ArgumentNullException>(() => method());

// Async
Assert.ThrowsAsync<ArgumentNullException>(async () => await methodAsync());
```

## Setup de Mocks Comuns

### Setup com Callback
```csharp
_mockAuthRepository.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>()))
    .Callback<ApplicationUser>(user => user.Id = "new-id")
    .ReturnsAsync(true);
```

### Setup com Matcher
```csharp
_mockAuthRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
    .ReturnsAsync((string email) => 
        new ApplicationUser { Email = email });
```

### Verificação de Chamadas
```csharp
// Uma vez
_mockAuthRepository.Verify(x => x.SaveAsync(), Times.Once);

// Nunca
_mockAuthRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);

// Com parâmetro específico
_mockAuthRepository.Verify(x => x.GetUserByIdAsync("user-123"), Times.Once);
```

## Decoradores NUnit

```csharp
[TestFixture]              // Define uma classe como suite de testes
[Test]                     // Define um método como teste
[TestCase(value1)]         // Teste parametrizado com valores
[TestCaseSource(nameof())] // Fonte de dados para testes parametrizados
[Sequential]               // Combina TestCase sequencialmente
[SetUp]                    // Executado antes de cada teste
[TearDown]                 // Executado após cada teste
[OneTimeSetUp]             // Executado uma vez antes de todos os testes
[OneTimeTearDown]          // Executado uma vez após todos os testes
[Ignore("Motivo")]         // Ignora um teste
[Category("CategoryName")] // Categoriza testes
[Timeout(5000)]            // Define timeout em ms
[Repeat(3)]                // Repete o teste 3 vezes
```

## Testes Parametrizados

```csharp
[TestCase("user@example.com", "Password123!", true)]
[TestCase("invalid@email", "weak", false)]
[TestCase("", "", false)]
public void Login_WithDifferentInputs_ReturnsExpectedResult(
    string email, 
    string password, 
    bool expectedSuccess)
{
    // Arrange
    var loginDto = new LoginDto { Email = email, Password = password };

    // Act
    var result = _authService.Login(loginDto);

    // Assert
    Assert.That(result.Success, Is.EqualTo(expectedSuccess));
}
```

## Boas Práticas

✅ **Faça:**
- Uma asserção principal por teste (ou relacionadas logicamente)
- Testes independentes que não dependem de outros
- Mockar dependências externas
- Usar nomes descritivos e claros
- Testar casos de sucesso, erro e edge cases
- Verificar chamadas a mocks (Verify)
- Usar builders para dados complexos

❌ **Evite:**
- Testes acoplados que dependem de ordem de execução
- Múltiplas responsabilidades por teste
- Testes "brittle" que quebram com mudanças menores
- Testar implementação, não comportamento
- Esquecer de limpar recursos (TearDown)
- Deixar testes com .Ignore sem motivo documentado

## Cobertura de Casos

Para cada método, teste:
1. **Happy Path** - fluxo normal com dados válidos
2. **Exceções** - dados inválidos, nulos, vazios
3. **Edge Cases** - limites, valores extremos
4. **Comportamento** - verificar chamadas a dependências

```csharp
[TestFixture]
public class AuthServiceCompleteTests
{
    // Happy Path
    [Test]
    public async Task Login_WithValidCredentials_ReturnsToken() { }

    // Exceções
    [Test]
    public void Login_WithNullEmail_ThrowsArgumentNullException() { }

    [Test]
    public async Task Login_WithNonExistentUser_ReturnsFalse() { }

    // Edge Cases
    [Test]
    public async Task Login_WithWhitespaceEmail_TrimmedAndProcessed() { }

    // Comportamento
    [Test]
    public async Task Login_Success_CallsRepositoryOnce() { }
}
```

## Execução de Testes

**Linha de comando:**
```powershell
dotnet test --verbosity normal
dotnet test --filter "TestCategory=Integration"
dotnet test /p:CollectCoverage=true
```

**Visual Studio:**
- Test Explorer: `Ctrl+E, T`
- Executar teste: `Ctrl+R, T`
- Debug teste: `Ctrl+R, Ctrl+T`

## Integração Contínua

- Todos os testes devem passar antes de merge
- Manter cobertura mínima de 80% (recomendado)
- Rodar testes em CI/CD automáticamente
- Falhar build se testes falharem
