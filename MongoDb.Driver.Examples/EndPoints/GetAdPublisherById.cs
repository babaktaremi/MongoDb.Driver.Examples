using Carter;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class GetAdPublisherById:ICarterModule
{
    private readonly IMongoDatabase _database;

    public GetAdPublisherById(IMongoDatabase database)
    {
        _database = database;
    }

    public record GetAdPublisherResultModel(string PublisherId, string PublisherName);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/ GetAdPublisher/{publisherId}", async ( string publisherId) =>
        {
            var collection = _database.GetCollection<AdPublisher>(AdPublisher.CollectionName);

            if (!ObjectId.TryParse(publisherId, out var objectId))
                return Results.BadRequest();

            var filter = Builders<AdPublisher>.Filter.Eq(c=>c.Id, objectId);

            var adPublisher = await collection.Find(filter).FirstOrDefaultAsync();

            return adPublisher is null 
                ? Results.NotFound() 
                : Results.Ok(new GetAdPublisherResultModel(adPublisher.PublisherName, adPublisher.Id.ToString()));
        });
    }
}