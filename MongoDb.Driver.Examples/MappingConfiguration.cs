using MongoDb.Driver.Examples.Models.Ads;
using MongoDB.Bson.Serialization;

namespace MongoDb.Driver.Examples;

public class MappingConfiguration
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<AdModel>(c =>
        {
            c.AutoMap();
            c.MapIdProperty(p => p.AdId);
           
        });
    }
}