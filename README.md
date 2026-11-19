# AIPoweredEcomm Back End

ASP.NET Core back end for the AI-powered e-commerce platform.

Work is tracked in Azure DevOps (`letsgotocloud/AIPoweredEcomm`).

## Prerequisites

- .NET SDK 10.0 (pinned in `global.json`)

## Solution structure

```
AIPoweredEcomm.slnx
src/
  AIPoweredEcomm.Api/             ASP.NET Core Web API: controllers, request/response DTOs, composition root (Program.cs)
  AIPoweredEcomm.Application/     Use cases, DTOs, abstractions (interfaces implemented by Infrastructure)
  AIPoweredEcomm.Domain/          Entities, value objects and domain rules; no dependencies on other layers
  AIPoweredEcomm.Infrastructure/  Persistence and external integrations implementing Application abstractions
tests/
  AIPoweredEcomm.ArchitectureTests/  Enforces the layering rules below
```

## Layering rules

- Dependency direction: `Api -> Application -> Domain`.
- `Infrastructure` references only `Application` and `Domain`.
- `Domain` references no other layer.
- `Api` references `Infrastructure` only from `Program.cs` to register services (`AddInfrastructure()`).
- `Api` exposes DTOs only; it must not use `Domain` types.

These rules are verified by `tests/AIPoweredEcomm.ArchitectureTests`.

## Folder conventions

| Project | Folder | Contents |
| --- | --- | --- |
| Api | `Controllers/` | One controller per resource |
| Application | `<Feature>/` | Use cases and DTOs grouped by feature |
| Application | `Abstractions/` | Interfaces implemented by Infrastructure |
| Domain | `Entities/` | Aggregate roots and entities |
| Domain | `Common/` | Shared base types (e.g. `BaseEntity`) |
| Infrastructure | `Persistence/` | Database context, configurations, repositories |

Each layer exposes a `DependencyInjection` class (`AddApplication()`, `AddInfrastructure()`) that `Program.cs` calls.

## Commands

```bash
dotnet build
dotnet test
dotnet format --verify-no-changes
dotnet run --project src/AIPoweredEcomm.Api   # health check: GET /health
```
