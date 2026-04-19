---
trigger: always_on
---

## Architecture & C# .NET Rules

### General Principles
- Follow Clean Architecture: separate Domain, Application, Infrastructure, Presentation layers.
- Dependencies always point inward (Domain has zero external dependencies).
- Use SOLID principles; prefer composition over inheritance.
- Prefer vertical slicing (feature folders) over horizontal layering inside Application.

### C# Code Style
- Use modern C# (12+): primary constructors, collection expressions, pattern matching, records.
- Always enable nullable reference types (`<Nullable>enable</Nullable>`).
- Use `async`/`await` consistently; never use `.Result` or `.Wait()`.
- Prefer `IReadOnlyList<T>` / `IEnumerable<T>` for return types unless mutation is needed.
- Use `record` for DTOs and Value Objects; use `sealed` where inheritance is not intended.

### Domain Layer
- Entities must enforce their own invariants via private setters + factory methods.
- Value Objects must be immutable records with structural equality.
- No infrastructure dependencies (no EF, no HTTP, no logging) in Domain.
- Use Domain Events for side effects across aggregates.

### Application Layer
- Use MediatR (CQRS): Commands mutate state, Queries return data.
- Command/Query handlers must be thin orchestrators; business logic lives in Domain.
- Use FluentValidation for input validation on Commands/Queries.
- Return `Result<T>` (e.g., ErrorOr or custom) instead of throwing exceptions for expected failures.

### Infrastructure Layer
- EF Core Code First; configure mappings via `IEntityTypeConfiguration<T>` classes.
- Repository pattern only if it adds value (avoid wrapping DbContext 1:1).
- Use the Outbox Pattern for reliable domain event publishing.

### API / Presentation Layer
- Minimal APIs or Controllers — be consistent within the project.
- Map Domain/Application objects to DTOs; never expose Domain entities directly.
- Use ProblemDetails (RFC 7807) for error responses.
- Version APIs from the start (`/api/v1/`).

### Testing
- Unit test Domain and Application layers in isolation (no DB, no HTTP).
- Integration test Infrastructure with Testcontainers (SQL Server container).
- Name tests: `MethodName_Scenario_ExpectedResult`.