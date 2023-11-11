using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace MongoDb.Driver.Examples.Models.Ads;

public class AdModel
{

    [NotMapped]
    [BsonIgnore]
    public const string CollectionName = "AdsCollection";

    public AdModel()
    {
        CurrentAdState = AdState.Created;
        ModifiedDate=DateTime.Now;
        ChangeLog.Add("Ad Created");
        AdId = AdId == ObjectId.Empty ? ObjectId.GenerateNewId() : AdId;
    }

    [BsonId]
    public ObjectId AdId { get; set; }
    public string Name { get;private set; }
    public string Description { get;private set; }
    public DateTime PublishDate { get;private set; }
    public DateTime ModifiedDate { get; private set; }
    public List<ObjectId> Categories { get; private set; } = new();
    public AdState CurrentAdState { get; private set; }
    public List<string> ChangeLog { get; private set; } = new();
    public required ObjectId AdPublisher { get; set; }
    public enum AdState
    {
        Created,
        Published,
        Disabled
    }

    public void SetAdName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;

        ModifiedDate=DateTime.Now;
        ChangeLog.Add("Ad Name Set Donev");
    }

    public void SetAdDescription(string description)
    {
        ArgumentNullException.ThrowIfNull(description);

        Description = description;
        ModifiedDate=DateTime.Now;
        ChangeLog.Add("Ad Description Set Done");
    }

    public void SetAdCategories(List<AdCategory> categories)
    {
        Categories.AddRange(categories.Select(c=>c.Id));
        ModifiedDate=DateTime.Now;
        ChangeLog.Add("Ad Categories Set");
    }

    public void PublishAd()
    {
        CurrentAdState = AdState.Published;
        PublishDate=DateTime.Now;
        ModifiedDate=DateTime.Now;
        ChangeLog.Add("Ad State Changed To Publish");
    }

    public bool IsStateValid()
        => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
}