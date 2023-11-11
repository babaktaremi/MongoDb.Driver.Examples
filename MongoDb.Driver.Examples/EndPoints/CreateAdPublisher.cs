using Carter;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class CreateAdPublisher:ICarterModule
{

    public record CreateAdPublisherModel(string PublisherName);


    private readonly IMongoDatabase _mongoDatabase;

    public CreateAdPublisher(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }


    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/CreatePublisher", async (CreateAdPublisherModel model) =>
        {
            var collection = _mongoDatabase.GetCollection<AdPublisher>(AdPublisher.CollectionName);

            var adPublisher = new AdPublisher()
                { PublisherName = model.PublisherName, Id = ObjectId.GenerateNewId() };

            await collection.InsertOneAsync(adPublisher);

            return Results.Created($"/GetAdPublisher/{adPublisher.Id.ToString()}", adPublisher);
        });
    }
}