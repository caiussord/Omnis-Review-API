

# Omnis Review API

API robusta desenvolvida em **ASP.NET Core** para gerenciamento centralizado de críticas de filmes, séries, livros e games.

## Sobre

A **Omnis Review** oferece uma plataforma unificada para que usuários gerenciem suas bibliotecas pessoais de entretenimento, atribuindo notas, escrevendo resenhas e filtrando conteúdos por diversas categorias.

## Stack

- **Runtime:** .NET 10 (C# 14)
- **Banco de Dados:** SQL Server (Entity Framework Core)
- **Autenticação:** ASP.NET Core Identity + JWT
- **Testes:** NUnit 4.2.2 + Moq 4.20.70
- **Documentação:** Swagger/OpenAPI

## Funcionalidades

- Autenticação e autorização com JWT
- CRUD unificado (Filmes, Séries, Livros, Games)
- Sistema de pontuação e resenhas
- Filtros e consultas otimizadas
- CORS configurável
- 41 testes unitários (100% passing)

## Instalação

### Pré-requisitos
- .NET 10 SDK
- SQL Server

### Setup

```bash
# 1. Clonar repositório
git clone https://github.com/caiussord/Omnis-Review-API.git
cd Omnis-Review-API

# 2. Restaurar dependências
dotnet restore

# 3. Configurar appsettings.json
# - Atualizar connection string do SQL Server
# - Configurar variáveis de JWT

# 4. Executar migrations
dotnet ef database update

# 5. Iniciar API
dotnet run
```

A API estará disponível em `https://localhost:7001`

## Testes

O projeto inclui **41 testes unitários** cobrindo todas as camadas:

| Camada | Qtd | Descrição |
|--------|-----|-----------|
| Services | 13 | Lógica de negócio |
| Repositories | 14 | Acesso a dados |
| Controllers | 14 | Endpoints HTTP |

### Executar Testes

```bash
# Todos os testes
dotnet test

# Com verbosidade
dotnet test --verbosity normal

# Testes específicos
dotnet test --filter "AuthServiceTests"

# Com cobertura de código
dotnet test /p:CollectCoverage=true
```

**Status:** 41/41 passando (1.2s)

## Documentação

- [**Testes**](./Omnis-Review-API.Tests/README.md) - Padrões e cobertura de testes
- [**Git Conventions**](./GIT_CONVENTIONS.md) - Padrões de branches e commits
- [**Instruções NUnit**](./copilot-instructions-tests.md) - Guia de testes
- [**Configurações**](./CONFIGURACAO.md) - Setup e variáveis de ambiente
- [**Swagger**](https://localhost:7001/swagger) - API interativa (quando rodando)

## Estrutura

```
Omnis-Review-API/
├── Controllers/              # Endpoints HTTP
├── Services/                 # Lógica de negócio
├── Repositorys/              # Acesso a dados
├── Models/                   # DTOs e entidades
├── Data/                     # DbContext
├── Migrations/               # EF Core migrations
├── Omnis-Review-API.Tests/   # Testes unitários
├── Program.cs                # Configuração da API
└── appsettings.json          # Configurações

Omnis-Review-API.Tests/
├── Services/                 # Testes de serviços
├── Repositorys/              # Testes de repositórios
├── Controllers/              # Testes de controllers
└── Helpers/                  # Builders de dados
```

## Padrões

### Testes
- **AAA Pattern:** Arrange-Act-Assert
- **Mocking:** Moq com isolamento de dependências
- **Builders:** Fluent builders para dados de teste
- **Cobertura:** Happy Path + Exceções + Edge Cases

### Git
- **Branches:** `feat/`, `fix/`, `hotfix/`, `refactor/`, `test/`, `docs/`, `chore/`
- **Commits:** `type(scope): subject`
- Exemplo: `feat(auth): implement jwt token generation`

## Como Contribuir

1. Criar branch seguindo padrão: `feat/descricao-funcionalidade`
2. Implementar com testes
3. Commits com padrão convencional
4. Pull request com descrição clara
5. Verificar que todos os 41 testes passam

## Variáveis de Ambiente

```json
{
  "Jwt": {
    "SecretKey": "sua-chave-secreta-aqui",
    "ExpirationMinutes": 60
  },
  "Database": {
    "DefaultConnection": "Server=.;Database=OmnisReviewDb;Integrated Security=true;"
  }
}
```

Veja [appsettings.example.json](./appsettings.example.json) para template completo.

## Licença

Projeto pessoal - Omnis Review API
