using aspdotnet.Configurations;
using aspdotnet.Data;
using aspdotnet.Middlewares;
using aspdotnet.Models.Common;
using aspdotnet.Services;
using Microsoft.AspNetCore.Mvc;

// ======================================================================================
// 💡 C# & ASP.NET Core Entry Point: Program.cs
//
// In Node.js / Express: This corresponds to `index.js` or `server.js` (creating app, middleware, listening).
// In NestJS: This corresponds to `main.ts` (`NestFactory.create(AppModule)`).
// Top-Level Statements (C# 9+): No `class Program` or `static void Main()` boilerplate needed.
// ======================================================================================

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------------------------
// 1. Configuration (Options Pattern)
// 💡 Equivalent to NestJS `ConfigModule.forRoot()` reading from `.env` or `config.json`.
// Maps the "MongoDbSettings" section in `appsettings.json` to the strongly-typed class.
// --------------------------------------------------------------------------------------
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

// --------------------------------------------------------------------------------------
// 2. Data Access (MongoDB Context)
// 💡 DI Lifetime: `AddSingleton` creates a single instance for the lifetime of the application.
// MongoClient manages connection pooling internally, so a Singleton is best practice.
// --------------------------------------------------------------------------------------
builder.Services.AddSingleton<MongoDbContext>();

// --------------------------------------------------------------------------------------
// 3. Application Services (Dependency Injection)
// 💡 DI Lifetime: `AddScoped` creates one instance per incoming HTTP request.
// In NestJS, this is like setting scope: Scope.REQUEST (though NestJS defaults to Singleton).
// --------------------------------------------------------------------------------------
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();

// --------------------------------------------------------------------------------------
// 4. API Controllers & Routing with Centralized Validation Response
// 💡 In NestJS, this is like registering a global ValidationPipe: `app.useGlobalPipes(new ValidationPipe())`.
// Automatically catches invalid DTO inputs and returns our uniform `ApiResponse` with HTTP 400.
// --------------------------------------------------------------------------------------
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors.Select(x =>
                    string.IsNullOrWhiteSpace(x.ErrorMessage) ? "Invalid input." : x.ErrorMessage))
                .ToList();

            var response = ApiResponse.Failure(
                message: "Validation failed.",
                statusCode: StatusCodes.Status400BadRequest,
                errors: errors
            );

            return new BadRequestObjectResult(response);
        };
    });

// --------------------------------------------------------------------------------------
// 5. CORS Policy
// 💡 In Express: `app.use(cors())`. In NestJS: `app.enableCors()`.
// --------------------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --------------------------------------------------------------------------------------
// 6. OpenAPI / Swagger Documentation
// 💡 In NestJS: `SwaggerModule.setup('api', app, document)`.
// --------------------------------------------------------------------------------------
builder.Services.AddOpenApi();

// Build the configured application instance
var app = builder.Build();

// ======================================================================================
// HTTP Request Pipeline (Middleware execution order matters!)
// Each middleware calls `next()` to pass the request to the next handler in the chain.
// ======================================================================================

// Global Exception Handler: Placed first to catch any errors occurring in subsequent middlewares or controllers.
app.UseMiddleware<GlobalExceptionMiddleware>();

// Enable OpenAPI endpoint in Development environment
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();

// Root Health & Info endpoint wrapped in ApiResponse
app.MapGet("/", () => Results.Ok(ApiResponse.Success(new
{
    status = "Online",
    app = "E-Commerce Store API",
    version = "1.0.0",
}, "API is running healthy.")));

// Map Controller routes (scans all `[ApiController]` classes and binds their `[Route]` and `[Http...]` attributes)
app.MapControllers();

// Start Kestrel web server (equivalent to `app.listen(port)` in Express)
app.Run();
