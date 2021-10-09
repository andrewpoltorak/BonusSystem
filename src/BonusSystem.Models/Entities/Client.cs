using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BonusSystem.Models.Entities
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CardId { get; set; }

        public string Name { get; set; }

        [BsonElement("Telephone")]
        public string TelNumber { get; set; }
    }
}
