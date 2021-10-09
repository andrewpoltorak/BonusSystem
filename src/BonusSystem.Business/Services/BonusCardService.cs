using BonusSystem.Models.Entities;
using MongoDB.Driver;

namespace BonusSystem.Business.Services
{
    public interface IBonusCardService
    {

    }

    public class BonusCardService : IBonusCardService
    {
        private readonly IMongoCollection<BonusCard> _bonusCards;

        public BonusCardService(IMongoCollection<BonusCard> bonusCards)
        {
            _bonusCards = bonusCards;
        }
    }
}
