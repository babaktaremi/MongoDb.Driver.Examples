using MongoDB.Driver;

namespace MongoDb.Driver.Examples.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services,string connectionString, string databaseName)
    {
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);

        services.AddSingleton(mongoDatabase);

        return services;
    }
}