using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDb.Driver.Examples.Models.Ads;

public class AdPublisher
{
    [NotMapped]
    [BsonIgnore]
    public const string CollectionName = "AdPublisherCollection";

    public string PublisherName { get; set; }
    public required ObjectId Id { get; set; }
}