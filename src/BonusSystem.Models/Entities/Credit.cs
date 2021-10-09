using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BonusSystem.Models.Entities
{
    public class Credit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Number { get; set; }

        public DateTime DateStart = DateTime.Now;
    }
}
