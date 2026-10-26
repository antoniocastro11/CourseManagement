# CourseManagement

API REST para gerenciamento de cursos, estudantes e matrículas, com autenticação JWT e controle de acesso por papéis.

## Stack

- .NET 10 / ASP.NET Core (Controllers)
- Entity Framework Core 10 + PostgreSQL (Npgsql)
- ASP.NET Core Identity + JWT Bearer
- AutoMapper
- OpenAPI (disponível apenas em Development)
- Testes: `CourseManagementAPI.UnitTests` e `CourseManagementAPI.IntegrationTests`

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- PostgreSQL em execução

## Configuração

A aplicação lê duas configurações obrigatórias (não estão versionadas em `appsettings.json`). Use *user secrets* (o projeto já tem `UserSecretsId`):

```bash
cd CourseManagementAPI
dotnet user-secrets set "ConnectionStrings:DatabaseConnection" "Host=localhost;Port=5432;Database=coursemanagement;Username=postgres;Password=postgres"
dotnet user-secrets set "Jwt:Key" "uma-chave-secreta-com-pelo-menos-32-caracteres"
```

## Executando

```bash
cd CourseManagementAPI
dotnet ef database update   # aplica migrations e dados iniciais (requer dotnet-ef)
dotnet run --launch-profile https
```

- HTTPS: `https://localhost:7212`
- HTTP: `http://localhost:5241`
- Documento OpenAPI (Development): `/openapi/v1.json`
- Exemplos de requisições: `CourseManagementAPI/CourseManagementAPI.http`

Na inicialização, os papéis `Admin`, `Instructor` e `Student` são criados caso não existam.

## Dados iniciais (seed)

A migration `Seed` cria usuários e cursos de exemplo. Todos usam a senha `Senha@123`:

| Usuário | Papéis |
|---|---|
| `admin.instrutor@course.com` | Admin, Instructor |
| `admin.estudante@course.com` | Admin, Student |
| `ana@course.com`, `bruno@course.com`, `carla@course.com`, `diego@course.com`, `elisa@course.com` | Student |

> Use apenas em desenvolvimento. Troque/remova esses usuários em qualquer ambiente real.

## Autenticação

1. `POST /auth/login` com e-mail e senha retorna um token JWT.
2. Envie o token nas demais requisições: `Authorization: Bearer <token>`.

## Endpoints

| Método | Rota | Papéis |
|---|---|---|
| POST | `/auth/login` | público |
| POST | `/auth/register` | Admin |
| GET | `/course` (`take`, `skip`, `category`) | — |
| GET | `/course/{id}` | — |
| POST | `/course` | Admin, Instructor |
| PATCH | `/course/{id}` | — |
| GET | `/student` (`take`, `skip`) | Admin |
| GET | `/student/{id}` | Admin |
| PATCH | `/student/{id}` | Admin |
| GET | `/student/me` | Student |
| PATCH | `/student/me` | Student |
| POST | `/enrollment` | Student, Admin, Instructor |
| GET | `/enrollment/{id}` | Admin, Student |
| GET | `/enrollment/student/{id}` | Admin |
| GET | `/enrollment/me` | Student |

Erros são retornados no formato `ProblemDetails`: violações de regras de matrícula (`EnrollmentException`) resultam em `400`, e exceções não tratadas em `500`.

## Testes

```bash
dotnet test
```

## Estrutura

```
CourseManagementAPI/                  API (Controllers, Services, Models, Data, Migrations, Profiles, Exceptions)
CourseManagementAPI.UnitTests/        Testes de serviços e mapeamentos
CourseManagementAPI.IntegrationTests/ Testes dos endpoints
```
