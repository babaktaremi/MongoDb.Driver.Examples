using Carter;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class GetUserAds:ICarterModule
{
    private readonly IMongoDatabase _mongoDatabase;

    public GetUserAds(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    private record UserAdsModel(string PublisherName,List<AdsModel> PublisherAds);

    private record AdsModel(string AdName, string AdId);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/PublisherAds", async (string publisherId) =>
        {
            if (!ObjectId.TryParse(publisherId, out var publisherObjectId))
                return Results.BadRequest("Invalid PublisherId");


            var publisherDatabase = _mongoDatabase.GetCollection<AdPublisher>(AdPublisher.CollectionName).AsQueryable();

            var adsDatabase = _mongoDatabase.GetCollection<AdModel>(AdModel.CollectionName).AsQueryable();

            var result = publisherDatabase
                .Where(c=>c.Id==publisherObjectId)
                .GroupJoin(adsDatabase
                , publisher => publisher.Id
                , ad => ad.AdPublisher
                , ((publisher, ads) => 
                    new UserAdsModel(publisher.PublisherName
                        , ads.Select(c=>new AdsModel(c.Name,c.AdId.ToString())).ToList())))
                .ToList();

            return Results.Ok(result);
        });
    }
}