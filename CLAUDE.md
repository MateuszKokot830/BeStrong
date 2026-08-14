# CLAUDE.md - .NET 10 / C# Guidelines

Scope: this file covers the .NET solution (`BeStrong.sln` — `Domain`, `Application`, `Infrastructure`, `WebAPI`, `Tests/*`).
The Angular frontend lives in `UI/` and follows its own conventions, not these.

## Build & Test Commands
- Build (whole solution, run from repo root): `dotnet build`
- Test all: `dotnet test`
- Run a single test: `dotnet test --filter FullyQualifiedName~YourTestName`
- Run the API: `dotnet run --project WebAPI`
- Add a migration: `dotnet ef migrations add YourMigrationName --project Infrastructure --startup-project WebAPI`
  - The `dotnet-ef` CLI tool version must match the `Microsoft.EntityFrameworkCore.Design` package version (currently `10.0.8`) or migration generation fails outright. Check with `dotnet ef --version`; update with `dotnet tool update --global dotnet-ef --version 10.0.8` if it's behind.
  - No manual `dotnet ef database update` needed for local dev — `Program.cs` calls `context.Database.MigrateAsync()` on startup, so the (SQLite) dev database migrates itself the next time the API runs.

## Tech Stack & Versioning
- Target Framework: `net10.0` (all five source/test projects)
- Language Version: `latest` (C# 14), `Nullable` and `ImplicitUsings` both enabled
- Web Framework: ASP.NET Core 10 with **Controllers** (`WebAPI/Controllers/*Controller.cs` : `BaseApiController`) — this project does not use Minimal API endpoints; don't introduce `MapGet`/`MapPost` style routes
- ORM: EF Core 10 (`10.0.8` pinned across `Microsoft.EntityFrameworkCore`, `.Design`, `.Sqlite`, `.Identity.EntityFrameworkCore`) — SQLite in dev
- Auth: ASP.NET Core Identity (`AddIdentityCore<User>`) + JWT bearer
- CQRS/Mediator: MediatR — `IRequest`/`IRequestHandler` for commands and queries, `INotification`/`INotificationHandler` for domain events (see Architecture below)
- Validation: FluentValidation, wired through a MediatR pipeline behavior — do not call validators manually in handlers
- Result handling: `ErrorOr<T>` for expected/domain failures returned from command and query handlers; exceptions are reserved for truly unexpected failures (caught by `ExceptionHandlingBehavior`)
- Testing: xUnit + Moq (`Application.Tests`, `Infrastructure.Tests`), `Microsoft.AspNetCore.Mvc.Testing` (`Integration.Tests`, real `WebApplicationFactory` + SQLite), ArchUnitNET (`Architecture.Tests`, enforces the layering/naming rules below)

## Architecture Rules
This is a four-project Clean Architecture solution, and the boundaries are enforced by `Tests/Architecture.Tests` (ArchUnitNET) — a change that violates one of these fails `dotnet test`, not just review:
- **Domain** must not depend on Application, Infrastructure, or WebAPI.
- **Application** must not depend on Infrastructure or WebAPI (it defines repository/service interfaces; Infrastructure implements them — the dependency points inward, never back out).
- **Infrastructure** must not depend on WebAPI.
- Classes named `*CommandHandler` or `*QueryHandler` must implement `IRequestHandler<,>`, and vice versa — every `IRequestHandler<,>` must be named one of those two ways.
- Every `*Command`/`*Query` type must implement `IRequest<>`.
- Repository interfaces live in `Application.Interfaces.Repositories`; repository implementations live in `Infrastructure.Repositories`.
- Classes named `*Validator` must be a sealed `AbstractValidator<T>`, and vice versa.

MediatR pipeline behaviors run in this order for every command/query: `ExceptionHandlingBehavior` → `LoggingBehavior` → `ValidationBehavior` → `TransactionBehavior`. `TransactionBehavior` opens a DB transaction around every request whose type name ends in `Command`, then commits it after the handler returns — handlers must not call `BeginTransactionAsync` themselves.

This matters for notification handlers specifically: if a command handler publishes an `INotification` (e.g. `WorkoutSavedNotification`) mid-request, the handler for it must not dispatch another `*Command` via `IMediator.Send` — that command would re-enter `TransactionBehavior` and try to open a second transaction on the same `DbContext` while the outer one is still open, which throws. Call the repository (`IPostRepository`, etc.) and `IUnitOfWork.CommitAsync` directly instead; it's still inside the same ambient transaction, so it stays atomic with the rest of the request.

## Code Style Rules
- Prefer explicit, clean, readable code over clever or overly abstracted patterns.
- Always pass `CancellationToken` through every asynchronous method signature (handlers, repositories, searchers, services all currently do this — keep it up).
- Do NOT use sync-over-async (`.Result` or `.Wait()`).
- Do NOT use `Task.Run` inside ASP.NET Core request handlers or controllers.
- Use primary constructors, collection expressions, and modern C# 14 features — this is already the norm throughout the codebase (`public class FooCommandHandler(IBarRepository barRepository, ...) : IRequestHandler<...>`).
- DTOs are positional `record`s (see `Application/Dto/**`); follow that pattern for new ones rather than mutable classes.
- The codebase has no outbound `HttpClient` usage today (the one external integration, Cloudinary, goes through its own SDK, not raw HTTP). If a new outbound HTTP integration is ever added, it must be registered via `IHttpClientFactory` with an explicit timeout — don't `new HttpClient()` directly.
- Do not add unapproved NuGet packages or new architectural layers without asking first.

## Workflow Expectations
1. Propose a short plan and list files to modify before executing code changes.
2. Keep changes scoped strictly to what was requested (smallest viable change).
3. Run `dotnet build` and `dotnet test` after code modification to verify correctness — the ArchUnitNET suite in particular will catch layering violations that build success alone won't.
