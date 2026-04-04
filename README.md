

# Omnis Review: API de Gerenciamento de Críticas Multiplataforma

A **Omnis Review** é uma API robusta desenvolvida em ASP.NET Core projetada para centralizar o registro e a avaliação de diferentes formas de entretenimento: filmes, séries, livros e games. O projeto foca em fornecer uma estrutura unificada e desacoplada, capaz de servir diferentes interfaces através de uma comunicação baseada em REST.

## Sobre o Projeto
Diferente de plataformas especializadas em apenas um tipo de mídia, a Omnis Review utiliza uma arquitetura escalável para lidar com as particularidades de cada formato. A aplicação permite o gerenciamento completo de bibliotecas pessoais, onde usuários podem atribuir notas, escrever resenhas detalhadas e filtrar conteúdos por diversas categorias.

## Tecnologias Utilizadas
* C# 14 / .NET 10
* Entity Framework Core (Abordagem Code First)
* ASP.NET Core Identity (Gestão de usuários e permissões)
* JWT (JSON Web Tokens) (Autenticação e segurança das rotas)
* SQL Server (Banco de dados relacional)
* Swagger/OpenAPI (Documentação e teste de endpoints)

## Funcionalidades Principais
* **Sistema de Autenticação:** Implementação de Identity com suporte a Tokens JWT, garantindo o acesso seguro aos recursos da API.
* **Catálogo Unificado:** CRUD especializado para Filmes, Séries, Livros e Jogos, respeitando as propriedades específicas de cada mídia através de modelagem avançada.
* **Mecanismo de Review:** Sistema de pontuação e comentários com persistência via Entity Framework e validação de regras de negócio.
* **Arquitetura de Dados:** Uso de relacionamentos complexos para otimizar a organização por gêneros, autores, diretores e plataformas.
* **Filtros e Consultas:** Endpoints otimizados via LINQ para recuperação de rankings, médias de notas e buscas personalizadas.
* **CORS Policy:** Configuração de política de compartilhamento de recursos para permitir o consumo seguro por clientes externos.

## Como Executar
1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/omnis-review-api.git
   ```
2. Configure a Connection String no arquivo appsettings.json.
3. Execute as migrações para preparar o banco de dados:
   ```bash
   dotnet ef database update
   ```
4. Inicie o servidor:
   ```bash
   dotnet run
   ```
