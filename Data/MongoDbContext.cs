using aspdotnet.Configurations;
using aspdotnet.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace aspdotnet.Data;

/// <summary>
/// MongoDB Context holding collections and database client handles.
/// 
/// 💡 TS / NestJS / Mongoose Comparison:
/// In NestJS / Mongoose, you define a Mongoose module and `@InjectModel(Product.name) private productModel: Model<Product>`.
/// In C# MongoDB Driver:
/// - `IOptions<MongoDbSettings>`: Injected configuration options populated from `appsettings.json`.
/// - `IMongoCollection<T>`: Strongly-typed collection handle (equivalent to Mongoose `Model<T>`).
/// - Expression body `public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");`:
///   Equivalent to a getter in JS: `get products() { return this.database.collection('products'); }`.
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        // MongoClient manages connection pooling internally in .NET
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");
    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    public IMongoDatabase Database => _database;
}
