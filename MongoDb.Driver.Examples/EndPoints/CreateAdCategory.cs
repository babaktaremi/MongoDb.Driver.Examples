using Carter;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class CreateAdCategory:ICarterModule
{
    private readonly IMongoDatabase _mongoDatabase;

    public record CreateAdCategoryModel(string CategoryName);
    public record CreateAdCategoryResultModel(string CategoryName,string CategoryId);

    public CreateAdCategory(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/CreateAdCategory", async (CreateAdCategoryModel model) =>
        {
            var collection = _mongoDatabase.GetCollection<AdCategory>(AdCategory.CollectionName);

            var adCategory = new AdCategory() { CategoryName = model.CategoryName, Id = ObjectId.GenerateNewId() };

            await collection.InsertOneAsync(adCategory);

            return Results.Created($"/GetCategory/{adCategory.Id.ToString()}",
                new CreateAdCategoryResultModel(adCategory.CategoryName, adCategory.Id.ToString()));
        });
    }
}