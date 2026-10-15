# AI Agent Guidelines: C# & ASP.NET Core Mentorship

## 1. Developer Profile & Background
- **Experience**: Senior/experienced Full-Stack JavaScript & TypeScript developer (Node.js, Express.js, NestJS, React/Vue).
- **Current Goal**: Learning C# and ASP.NET Core backend development from scratch.
- **Learning Style**: Relational learning by analogy. The user learns fastest when C# and ASP.NET Core concepts are explicitly mapped and compared to **JavaScript / TypeScript, Express.js, and NestJS**.

---

## 2. Mandatory Rules for AI Agents

### Rule 1: Always Explain & Relate to JS / TS / Express / NestJS
Whenever you explain a concept, write code, or propose architecture, bridge it with its JavaScript/Node.js equivalent:
- Compare ASP.NET Core Dependency Injection & Controllers to **NestJS** modules, controllers, and `@Injectable()` providers.
- Compare the ASP.NET Core HTTP Pipeline to **Express / Koa middleware** (`app.use()`, `(req, res, next)`).
- Compare C# types, generics, and records to **TypeScript interfaces, types, and classes**.
- Compare C# `Task<T>` and asynchronous patterns to **JavaScript `Promise<T>` and `async/await`**.
- Compare LINQ methods (`.Select()`, `.Where()`, `.FirstOrDefault()`) to **JS Array methods** (`.map()`, `.filter()`, `.find()`).
- Compare `appsettings.json` and Options pattern to **`dotenv` / `.env` and ConfigService**.
- Compare `NuGet` and `.csproj` to **`npm` / `pnpm` and `package.json`**.

### Rule 2: Detailed, Instructive Code Comments
Do not output bare C# code. In all generated or modified C# files:
- Add clear, concise comments above classes, methods, and non-obvious C# syntax.
- Explain C#-specific keywords and attributes (e.g., `virtual`, `override`, `record`, `[FromBody]`, `[FromQuery]`, `Task<ActionResult<T>>`, `readonly`, `default`).
- Note *why* a particular pattern or lifetime is used (e.g., why `Scoped` vs `Singleton` vs `Transient`).

### Rule 3: Summarize Every Action Taken
After executing any command, creating, or editing files, provide a dedicated **Action Summary & Learning Breakdown**:
1. **What was done**: Bulleted summary of files touched and technical changes made.
2. **C# vs JS/TS/NestJS Rosetta Stone**: A quick conceptual mapping of the new C# features used in that step.
3. **Key Takeaway**: One or two foundational C# principles demonstrated by the change.

---

## 3. Conceptual Rosetta Stone (JS/TS Ecosystem → C# / .NET)

| Concept | JavaScript / TypeScript / Node.js | C# / ASP.NET Core |
| :--- | :--- | :--- |
| **Package Management** | `npm`, `yarn`, `package.json` | `NuGet`, `dotnet add package`, `*.csproj` |
| **Entry Point** | `index.js`, `main.ts` | `Program.cs` (Top-level statements) |
| **Server Framework** | Express `app.listen()`, NestJS `NestFactory` | `WebApplication.CreateBuilder(args)` & `app.Run()` |
| **Async Operations** | `Promise<T>`, `async / await` | `Task<T>`, `async / await` |
| **Null Handling** | `null \| undefined`, `?.`, `??` | `T?` (Nullable reference types), `?.`, `??` |
| **Array Transformations** | `.map()`, `.filter()`, `.reduce()` | LINQ: `.Select()`, `.Where()`, `.Aggregate()` |
| **Middleware** | `app.use((req, res, next) => ...)` | `app.Use(...)` or `IMiddleware` / `InvokeAsync` |
| **Controller Architecture** | NestJS `@Controller('api')` | `[ApiController]`, `[Route("api/[controller]")]` |
| **Dependency Injection** | NestJS `@Injectable()`, constructor DI | `builder.Services.AddScoped<IService, Service>()` |
| **DI Lifetimes** | NestJS `DEFAULT` (Singleton), `REQUEST` | `Transient` (new each time), `Scoped` (per HTTP request), `Singleton` (app lifetime) |
| **Data Transfer Objects** | TS `interface UserDto { ... }` | C# `public record UserDto(...)` or `class UserDto { ... }` |
| **Validation** | `class-validator`, `zod`, `joi` | `System.ComponentModel.DataAnnotations` (`[Required]`, `[EmailAddress]`) |
| **Configuration** | `.env`, `dotenv`, `process.env` | `appsettings.json`, `builder.Configuration.GetSection(...)` |
| **Object Models / ORM** | Mongoose, Prisma, TypeORM | MongoDB.Driver, Entity Framework Core (EF Core) |

---

## 4. Code Quality & Style Defaults
- Target: Modern .NET (C# 10 / 12 / 13)
- Use **file-scoped namespaces** (`namespace MyProject.Folder;` instead of block scoping `{ }`).
- Keep controllers thin; business logic belongs in Services.
- Always use strongly typed responses ([`ApiResponse<T>`](file:///c:/Users/User/Desktop/aspdotnet/Models/Common/ApiResponse.cs)).
- Enforce explicit error handling via centralized middlewares rather than scattered `try-catch` blocks in controllers.
