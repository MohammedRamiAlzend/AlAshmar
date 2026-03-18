# Project Guidelines

## Code Style
- Target framework is .NET 10 (`net10.0`) across backend projects; keep nullable enabled and implicit usings as already configured in each `.csproj`.
- Follow current layering: keep API concerns in `AlAshmar`, business logic in `AlAshmar.Application`, persistence concerns in `AlAshmar.Infrastructure.Persistence`, and domain model in `AlAshmar.Domain`.
- Keep AutoMapper configuration centralized in `AlAshmar.Application/Common/EntityMappingProfile.cs`.
- For Form domain mapping, map create/update request DTOs to entities (not positional response DTO records) to avoid constructor binding issues.

## Architecture
- Solution structure is cleanly split by responsibility:
  - `AlAshmar`: ASP.NET Core API host, auth setup, OpenAPI/Scalar, app bootstrap.
  - `AlAshmar.Application`: use cases, DTOs, validators, interfaces, and MediatR request handling.
  - `AlAshmar.Domain`: entities, value objects, domain events.
  - `AlAshmar.Infrastructure`: non-persistence infrastructure services (for example token/auth related services).
  - `AlAshmar.Infrastructure.Persistence`: EF Core `AppDbContext`, entity configurations, repositories, unit of work.
  - `AlAshmar.Tests`: xUnit tests.
  - `react-frontend`: Vite + React + TypeScript frontend.
- DI registration pattern:
  - API startup wires `AddPersistenceServices(...)`, `AddInfrastructureServices(...)`, authorization policies, AutoMapper profile(s), and MediatR registration from `IQuery<>` assembly.

## Build and Test
- Backend restore/build/test (workspace root):
  - `dotnet restore`
  - `dotnet build AlAshmar.slnx`
  - `dotnet test AlAshmar.Tests/AlAshmar.Tests.csproj`
- Run API (from workspace root or `AlAshmar`):
  - `dotnet run --project AlAshmar/AlAshmar.csproj`
- Frontend (from `react-frontend`):
  - `npm install`
  - `npm run dev`
  - `npm run build`
  - `npm run lint`

## Conventions
- Use MediatR + UseCases pattern for application workflows instead of pushing business logic into controllers.
- Prefer repository abstractions from `AlAshmar.Application/Repos` with implementations in persistence layer.
- Auditing is automatic for `IAuditableEntity` in `AppDbContext.SaveChangesAsync`; do not duplicate timestamp-setting logic in handlers/controllers.
- Keep auth-required API verification scenarios in `http-files/*.http`; use `http-files/README.md` for flow-oriented manual API testing.

## Environment and Pitfalls
- SQL Server is required by default: `AddPersistenceServices` uses `UseSqlServer` with `ConnectionStrings:DefaultConnection`.
- API startup seeds authorization data (`AuthorizationSeeder`) at boot; DB connectivity and schema must be ready.
- JWT settings must be present and valid (`Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`) or token generation/validation fails.
- When testing AutoMapper configuration in unit tests, ensure `MapperConfiguration` gets a non-null `ILoggerFactory` (for example `NullLoggerFactory.Instance`) to avoid runtime config errors with AutoMapper v16.

## References
- Solution composition: `AlAshmar.slnx`
- API bootstrap: `AlAshmar/Program.cs`
- Persistence DI and DbContext: `AlAshmar.Infrastructure.Persistence/PersistenceServiceRegistration.cs`, `AlAshmar.Infrastructure.Persistence/AppDbContext.cs`
- Mapping profile: `AlAshmar.Application/Common/EntityMappingProfile.cs`
- API test flows: `http-files/README.md`
- FileManager library notes: `FileManager/README.md`
