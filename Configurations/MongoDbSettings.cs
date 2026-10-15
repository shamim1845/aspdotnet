namespace aspdotnet.Configurations;

/// <summary>
/// Strongly-typed configuration class for MongoDB connection options.
/// 
/// 💡 TS / NestJS Comparison:
/// In NestJS, you would inject `ConfigService` and do `configService.get<string>('MONGO_URI')`.
/// In C# / ASP.NET Core, the **Options Pattern** binds sections of `appsettings.json` directly
/// to a strongly-typed C# class:
/// `builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(MongoDbSettings.SectionName));`
/// </summary>
public class MongoDbSettings
{
    // The key name matching the section inside appsettings.json
    public const string SectionName = "MongoDbSettings";

    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "dotnet_ecommerce";
}
