# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository shape

Two independent apps side by side, with no root-level solution or workspace file:

- `RestaurantSuggestionApp/` — .NET 10 backend (`RestaurantSuggestionApp.slnx`, the new XML solution format)
- `RestaurantSuggestionAppFrontend/` — Nuxt 3 SPA (Pinia + Leaflet)

Comments, error messages, and all user-facing strings are in **Turkish**. Keep new code consistent with that.

## Current state: the backend does not compile

`RestaurantSuggestionApp.DataAccess/DbContextSeed.cs` lines 13–15 assign `Id`, `Name`, and `Email` on `User` via object initializer, but the `Extract OSRM distance logic into OsrmService and encapsulate User entity` commit made those members read-only / `private set`. This is 9 × `CS0200` and blocks every project downstream, including tests.

`RestaurantSuggestionApp.Tests/Helpers/TestDataHelper.cs:13` uses the same `new User { Name = …, Email = … }` pattern and will hit the same wall (`private set` is inaccessible cross-assembly) once DataAccess compiles.

Fixing this means choosing how `User` gets constructed — a constructor, an `init` accessor, or a factory — and applying it in both places. `Id` also needs a setter or backing-field access for EF `HasData` seeding, which requires explicit keys.

## Commands

All backend commands run from `RestaurantSuggestionApp/`.

```powershell
dotnet build RestaurantSuggestionApp.slnx
dotnet run --project RestaurantSuggestionApp.WebApi                      # http://localhost:5062
dotnet run --project RestaurantSuggestionApp.WebApi --launch-profile https  # https://localhost:7085 — what the frontend expects
```

Swagger UI is at `/swagger` and only registered in the Development environment.

### Tests

The `.slnx` marks the test project `<Build Solution="Debug|*" Project="false" />`, so **a Debug solution build skips it entirely** and solution-level `dotnet test` won't pick it up. Always target the csproj:

```powershell
dotnet test RestaurantSuggestionApp.Tests\RestaurantSuggestionApp.Tests.csproj
dotnet test RestaurantSuggestionApp.Tests\RestaurantSuggestionApp.Tests.csproj --filter "FullyQualifiedName~RestaurantServicesTests"
dotnet test RestaurantSuggestionApp.Tests\RestaurantSuggestionApp.Tests.csproj --filter "Name=GetAll_NoRestaurants_ReturnsSuccessAndEmptyList"
```

### Migrations

Migrations live in `DataAccess` but the DI-configured `DbContext` lives in `WebApi`, so both flags are required:

```powershell
dotnet ef migrations add <Name> -p RestaurantSuggestionApp.DataAccess -s RestaurantSuggestionApp.WebApi
dotnet ef database update      -p RestaurantSuggestionApp.DataAccess -s RestaurantSuggestionApp.WebApi
```

Default connection string targets `(localdb)\mssqllocaldb`, database `MultinetRestaurantDb`.

### Frontend

```powershell
cd RestaurantSuggestionAppFrontend
npm install
npm run dev          # nuxt dev
npm run dev:clean    # wipes .nuxt and node_modules/.cache first — use when HMR gets stuck
npm run build        # wipes .nuxt/.output/cache, then nuxt build
```

There is no lint or test setup on the frontend.

## Backend architecture

Layered, one-way references: `Core` ← `DataAccess` ← `Business` ← `WebApi`. `Core` holds entities and DTOs and depends on nothing but NPOI. Tests reference Business, DataAccess, and Core.

### `BaseResponse<T>` is the service contract

Every service method returns `BaseResponse<T>`, built through the `Success(data)` / `Fail(message)` static factories. Services wrap their bodies in `try/catch` and convert exceptions into `Fail` with a Turkish message — they do not throw to the caller. Controllers then translate:

- `!IsSuccess` on a lookup → `NotFound(result)`
- `!IsSuccess` on a list/create/delete → `BadRequest(result)`

HTTP status codes are therefore decided entirely in the controller; adding a new failure mode means deciding its status there, not throwing from the service. The internal `GetXOrThrow(id, message)` helpers throw `ArgumentException` only to be caught by the same method one frame up.

### The recommendation pipeline

`RestaurantService.GetNearbyAsync` is the core domain logic and runs in a fixed order:

1. **Bounding-box prefilter** (`GetNearbyCandidates`) — converts the km radius to lat/lng degrees (111 km per degree, longitude scaled by `cos(lat)`) so SQL Server can use an indexable range query, then filters the result in memory with an exact Haversine check.
2. **OSRM walking distances** — one `/table/v1/foot/…?sources=0` call for all candidates at once, user coordinate first, so `Distances[0][i+1]` is candidate `i`.
3. **Fallback** — if OSRM returns an empty list, recompute from Haversine and estimate duration as `metres / 1.38` (~5 km/h).
4. **Scoring** (`ScoreRestaurants`) — `0.60 × locationScore + 0.40 × budgetScore`. Location is 100 within 500 m and decays linearly to 0 at 5 km. Budget compares the restaurant's `BudgetHelper.GetBudgetIndex(AveragePrice)` against the user's index, losing 25 points per step of difference. The user's index comes from the average of their last 20 `UserExpenditures`, defaulting to `3` for anonymous or spend-less users.
5. Sort by `RecommendationScore` descending.

`OsrmService` swallows every exception and returns an empty list. **That empty list is the fallback signal** — making it throw or return partial data would silently disable step 3.

### Auth

`AuthService` issues HS256 JWTs with a 1-hour lifetime and `NameIdentifier` / `Name` / `Email` claims; the signing key, issuer, and audience come from the `Jwt` config section. Passwords are BCrypt.

`ClaimsPrincipalExtensions.GetUserId()` (in `Business/Helpers`) is the single way to read the current user — it falls back from `NameIdentifier` to `sub` and returns `int?`. `UserController.Update` and `Delete` use it to enforce self-only access by comparing against the route `id` and returning 403 otherwise. `RestaurantController` is `[Authorize]` wholesale; `AuthController` is anonymous. `nearby` passes the possibly-null user id straight through, so it works authenticated and would degrade to the default budget index if not.

### DTO namespace inconsistency

The folder is `Core/Dtos/`, but the namespaces disagree: `BaseResponse` is in `Core.Dtos.Responses` while `RestaurantDistance` is in `Core.DTOs.Responses` and request DTOs are in `Core.DTOs.Requests`. Files that touch both need both `using` lines — see the imports in `RestaurantServices.cs`. Match whatever the file you're editing already uses rather than "fixing" one side in isolation.

Also note `RestaurantDistanceDto.cs` declares a class named `RestaurantDistance`, not `RestaurantDistanceDto`.

### Seed data

`DbContextSeed.SeedData` is called from `OnModelCreating` and uses `HasData`, so it is baked into migrations — 3 users, 50 restaurants across 5 Istanbul districts, 5 expenditures each. It uses `new Random(42)` and a fixed `2026-01-01` base date to stay deterministic. Changing it requires generating a new migration.

## Test conventions

NUnit 4 with `Assert.That`, Moq, and the EF Core InMemory provider.

- `[SetUp]` builds a fresh `AppDbContext` with `UseInMemoryDatabase(Guid.NewGuid().ToString())`, so each test gets an isolated database; `[TearDown]` disposes it.
- Seed through `TestDataHelper.SeedUser` / `SeedRestaurant` rather than constructing entities inline.
- Method names are `Method_Scenario_ExpectedResult`; tests are grouped with `#region` blocks per service method.
- `IOsrmService` is mocked directly. `OsrmServiceTests` instead mocks `HttpMessageHandler` via `Moq.Protected` and injects a real `HttpClient` — that's the pattern for anything hitting the network.

## Frontend architecture

### Directory casing

`Pages/` and `Stores/` are capitalized, against Nuxt convention. Routing works only because Windows filesystems are case-insensitive — **this will break on a Linux or macOS build**. Because Nuxt's auto-import doesn't recognize `Stores/`, every consumer imports explicitly: `import { useAuthStore } from '~/Stores/authStore'`. Composables in `composables/` are lowercase and auto-imported normally. If you add a store, follow the existing explicit-import pattern.

### API access

`useApi()` is the only fetch wrapper: it reads `apiBase` from runtime config, pulls the `auth_token` cookie, and attaches `Authorization: Bearer …`. Feature services (`useAuthService`, `useRestaurantService`) sit on top of it and are the only place endpoint paths appear. Don't call `$fetch` directly.

`apiBase` is hardcoded to `https://localhost:7085` in `nuxt.config.ts`, matching the backend's `https` launch profile. The backend has `UseHttpsRedirection` plus a permissive `AllowAnyOrigin` CORS policy, so running the API on the http-only profile will break the frontend.

Responses are unwrapped defensively — `response?.data || response` — because the backend always wraps payloads in `BaseResponse`.

### Auth state

`useAuthStore` decodes the JWT client-side in `parseJwt` and initializes `isLoggedIn` / `user` from the cookie at state-construction time, so a page refresh restores the session without an API call. `extractUserFromDecoded` checks short claim names *and* the full `schemas.xmlsoap.org` claim URIs, since which form appears depends on JWT handler claim mapping.

`middleware/auth.global.ts` runs on every route: unauthenticated users are pushed to `/login` (only `/login` and `/register` are exempt), and authenticated users are bounced off those two pages back to `/`.

### Map

`RestaurantMap.client.vue` is client-only by suffix because Leaflet needs `window`. It owns geolocation (`navigator.geolocation.getCurrentPosition`) and calls `store.fetchNearbyRestaurants` on load and on radius change, so `Pages/index.vue` and the map can both trigger fetches against the same Pinia store.

### i18n

Hand-rolled in `composables/useI18n.ts` against `locales/tr.json` and `locales/en.json` — dot-path lookup, `%{param}` interpolation, falls back to returning the key itself on a miss. The `currentLocale` ref lives at module scope, so it is shared across all callers but is not persisted. Default locale is `tr`.

## Notes

- `RestaurantSuggestionApp.WebApi/webapi.http` is untouched scaffolding pointing at a `/weatherforecast` endpoint that doesn't exist.
- `NPOI` in `Core` drags in a vulnerable `System.Security.Cryptography.Xml` 8.0.2, producing 43 `NU1903` warnings on every build. No code currently uses NPOI.
- `postman/` and `.postman/` are untracked and empty apart from workspace metadata.
