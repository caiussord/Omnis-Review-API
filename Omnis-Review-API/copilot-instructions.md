# Instruções de Contexto: Omnis Review API

Você é um especialista em C# e .NET 10 atuando no projeto Omnis Review API. Siga estas diretrizes estritamente ao gerar código ou sugerir refatorações.

## Tecnologias e Versões
- Framework: .NET 10 (C# 14).
- ORM: Entity Framework Core com abordagem Code First.
- Autenticação: ASP.NET Core Identity com JWT (JSON Web Tokens).
- Testes: NUnit (Obrigatório para testes de unidade e integração).

## Estrutura de Pastas e Namespaces
O projeto segue uma estrutura de camadas organizada da seguinte forma:

- `/Controllers`: Endpoints da API (ex: `AuthController.cs`).
- `/Data`: Contexto do banco de dados e configurações do EF (ex: `ApplicationDbContext.cs`).
- `/Migrations`: Histórico de migrações do banco de dados.
- `/Models`: DTOs e Entidades (ex: `ApplicationUser.cs`, `LoginDto.cs`).
- `/Repositorys`: Implementações de acesso a dados (ex: `AuthRepository.cs`).
- `/Repositorys/Interfaces`: Contratos dos repositórios (ex: `IAuthRepository.cs`).
- `/Services`: Lógica de negócio (ex: `AuthService.cs`, `EmailSender.cs`).
- `/Services/Interfaces`: Contratos dos serviços (ex: `IAuthService.cs`).

**Importante:** O namespace base é `OmnisReview`. Utilize sempre "file-scoped namespaces".

## Regras de Codificação
- Nullability: O projeto utiliza `<Nullable>enable</Nullable>`. Sempre trate possíveis valores nulos.
- Idioma: Comentários técnicos em Português; nomes de classes, métodos e variáveis em Inglês.
- Injeção de Dependência: Registre sempre as interfaces no `Program.cs`.

## Diretrizes de Testes (NUnit)
- Framework: Utilize estritamente o NUnit para todas as suítes de teste.
- Localização: Os testes devem residir num projeto separado chamado `OmnisReview.Tests`.
- Padrão de Nomenclatura: `NomeDoMetodo_Cenario_ResultadoEsperado`.
- Mocking: Utilize a biblioteca Moq para isolar dependências de interfaces (Services e Repositories).
- Exemplo de estrutura:
    - `OmnisReview.Tests/Services/AuthServiceTests.cs`
    - `OmnisReview.Tests/Controllers/AuthControllerTests.cs`