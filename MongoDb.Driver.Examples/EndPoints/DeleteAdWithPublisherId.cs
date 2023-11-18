using Carter;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class DeleteAdWithPublisherId:ICarterModule
{
    private readonly IMongoDatabase _mongoDatabase;

    public DeleteAdWithPublisherId(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/DeleteAd", async (string adPublisherId, string adId) =>
        {
            if (!ObjectId.TryParse(adPublisherId, out var adPublisherObjectId) ||
                !ObjectId.TryParse(adId, out var adObjectId))
                return Results.BadRequest();

            var adsCollection = _mongoDatabase.GetCollection<AdModel>(AdModel.CollectionName);

            var deleteResult=await adsCollection.DeleteOneAsync(c => c.AdPublisher == adPublisherObjectId && c.AdId == adObjectId);

            return Results.NoContent();
        });
    }
}