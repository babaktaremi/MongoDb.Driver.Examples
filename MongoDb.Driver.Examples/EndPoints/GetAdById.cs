using Carter;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.EndPoints;

public class GetAdById:ICarterModule
{
    private readonly IMongoDatabase _mongoDatabase;

    public record GetAdByIdResult(string AdName, string AdDescription, List<string> Categories);

    public GetAdById(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetAdById/{adId}", async (string adId) =>
        {
            if (!ObjectId.TryParse(adId, out var adIdObj))
                return Results.BadRequest();


            var adCollection = _mongoDatabase.GetCollection<AdModel>(AdModel.CollectionName);
            
            var adCategory = _mongoDatabase.GetCollection<AdCategory>(AdCategory.CollectionName);

            var adFilter = Builders<AdModel>.Filter.Eq(c => c.AdId, adIdObj);

            var ad = await adCollection.Find(adFilter).FirstOrDefaultAsync();

            if (ad is null)
                return Results.NotFound();

            var categoryFilter = Builders<AdCategory>.Filter.In(c => c.Id, ad.Categories);
            var categoryProjectionFilter = Builders<AdCategory>.Projection.Expression(f => f.CategoryName);

            var category = await adCategory
                .Find(categoryFilter)
                .Project(categoryProjectionFilter)
                .ToListAsync();

            return Results.Ok(new GetAdByIdResult(ad.Name, ad.Description, category));

        });
    }
}