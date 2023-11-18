using Carter;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class PublishAd : ICarterModule
{
    private readonly IMongoDatabase _mongoDatabase;

    public PublishAd(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    private record PublishAdApiModel(string PublisherAdId, string AdId);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/publishAd", async (PublishAdApiModel model) =>
        {
            if (!ObjectId.TryParse(model.AdId, out var adId) ||
                !ObjectId.TryParse(model.PublisherAdId, out var publisherId))
                return Results.BadRequest("Invalid Parameters");

            var adsCollection = _mongoDatabase.GetCollection<AdModel>(AdModel.CollectionName);
           
            var filter = Builders<AdModel>.Filter.And(
                Builders<AdModel>.Filter.Eq(c => c.AdId,adId)
                ,Builders<AdModel>.Filter.Eq(c=>c.AdPublisher,publisherId));

            var ad = await adsCollection.Find(filter).FirstOrDefaultAsync();

            if (ad is null)
                return Results.NotFound("Specified Ad not found");

            if (ad.CurrentAdState == AdModel.AdState.Published)
                return Results.BadRequest("you cannot publish an already published ad");
            
            ad.PublishAd();

           var replaceResult= await adsCollection.ReplaceOneAsync(filter, ad,
                new ReplaceOptions() { IsUpsert = false, BypassDocumentValidation = false });

           if (replaceResult.ModifiedCount > 0)
               return Results.NoContent();

           return Results.BadRequest("Unable to modify related mongo doc");

        });
    }
}