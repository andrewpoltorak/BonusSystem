using BonusSystem.Models.Entities;
using MongoDB.Driver;

namespace BonusSystem.Business.Services
{
    public interface IDebitService
    {

    }

    public class DebitService : IDebitService
    {
        private readonly IMongoCollection<Debit> _debits;

        public DebitService(IMongoCollection<Debit> debits)
        {
            _debits = debits;
        }
    }
}
