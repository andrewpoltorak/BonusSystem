using BonusSystem.Models.Entities;
using MongoDB.Driver;

namespace BonusSystem.Business.Services
{
    public interface ICreditService
    {

    }

    public class CreditService : ICreditService
    {
        private readonly IMongoCollection<Credit> _credits;

        public CreditService(IMongoCollection<Credit> credits)
        {
            _credits = credits;
        }
    }
}
