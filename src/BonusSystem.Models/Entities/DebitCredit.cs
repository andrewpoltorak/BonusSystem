using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BonusSystem.Models.Entities
{
    public class DebitCredit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CardId { get; set; }

        public decimal Sum = 0;

        public DateTime DateTransact = DateTime.Now;
    }
}
