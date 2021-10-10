using BonusSystem.Models.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace BonusSystem.Business.Services
{
    public interface IDebitCreditService
    {
        Task CreateTransactAsync(string cardId, int sum);
    }

    public class DebitCreditService : IDebitCreditService
    {
        private readonly IMongoCollection<DebitCredit> _debitCreditCollection;

        public DebitCreditService(IMongoCollection<DebitCredit> debitCreditCollection)
        {
            _debitCreditCollection = debitCreditCollection;
        }

        public Task CreateTransactAsync(string cardId, int sum)
        {
            throw new System.NotImplementedException();
        }
    }
}
