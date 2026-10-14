using aspdotnet.Configurations;
using aspdotnet.Data;
using aspdotnet.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuration (strongly-typed settings)
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

// 2. Data Access (MongoDB Context)
builder.Services.AddSingleton<MongoDbContext>();

// 3. Application Services (Dependency Injection)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();

// 4. API Controllers & Routing
builder.Services.AddControllers();

// 5. CORS Policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 6. OpenAPI / Swagger Documentation
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();

// Root Health & Info endpoint
app.MapGet("/", () => Results.Ok(new
{
    status = "Online",
    app = "E-Commerce Store API",
    version = "1.0.0",
 }));

// Map all Controller routes (/api/products, /api/users, etc.)
app.MapControllers();

app.Run();
