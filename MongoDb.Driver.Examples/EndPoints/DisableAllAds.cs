using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class DisableAllAds:ICarterModule
{
    private readonly IMongoDatabase _mongoDatabase;

    public DisableAllAds(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }
    

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("DisableAllAds", async () =>
        {
            var adsFilter = Builders<AdModel>.Filter.Empty;

            var adCollection = _mongoDatabase.GetCollection<AdModel>(AdModel.CollectionName);

            var adsUpdateDefinition = Builders<AdModel>.Update.Set(c => c.CurrentAdState, AdModel.AdState.Disabled);

            var bsonValue = adsUpdateDefinition.ToBsonDocument();

           var updateResult= await adCollection.UpdateManyAsync(adsFilter, adsUpdateDefinition);

           return Results.NoContent();
        });
    }
}