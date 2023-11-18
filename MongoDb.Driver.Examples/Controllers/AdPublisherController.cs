using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Driver.Examples.Models.Ads;

namespace MongoDb.Driver.Examples.Controllers;

[ApiController]
[Route("api/AdPublisherController/[action]")]
public class AdPublisherController : ControllerBase
{
    [HttpPatch]
    public async Task<IActionResult> EditPublisher(string adPublisherId, JsonPatchDocument<AdPublisher> patchModel,[FromServices]IMongoDatabase mongoDatabase)
    {
        if (!ObjectId.TryParse(adPublisherId, out var adPublisherObjectId))
            return BadRequest("Invalid Object Id");
        
        var adPublisher =await mongoDatabase.GetCollection<AdPublisher>(AdPublisher.CollectionName)
            .Find(Builders<AdPublisher>.Filter.Eq(c => c.Id, adPublisherObjectId)).FirstOrDefaultAsync();

        if (adPublisher is null)
            return NotFound();

        patchModel.ApplyTo(adPublisher);

        await mongoDatabase.GetCollection<AdPublisher>(AdPublisher.CollectionName)
            .ReplaceOneAsync(Builders<AdPublisher>.Filter.Eq(c => c.Id, adPublisherObjectId),adPublisher);

        return NoContent();
    }
}