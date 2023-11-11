using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace MongoDb.Driver.Examples.Models.Ads;

public class AdCategory
{

    [NotMapped]
    [BsonIgnore]
    public const string CollectionName = "AdCategories";

    public ObjectId Id { get; set; }
    public string CategoryName { get; set; }
}