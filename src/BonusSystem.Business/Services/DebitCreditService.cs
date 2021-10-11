using BonusSystem.Models.Entities;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace BonusSystem.Business.Services
{
    public interface IDebitCreditService
    {
        Task<decimal> CreateTransactAsync(string cardId, decimal sum);
        Task<decimal> GetSumByCardIdAsync(string cardId);
        Task<DebitCredit> CreateDebitCredit(string cardId);
    }

    public class DebitCreditService : IDebitCreditService
    {
        private readonly IMongoCollection<DebitCredit> _debitCreditCollection;

        public DebitCreditService(IMongoCollection<DebitCredit> debitCreditCollection)
        {
            _debitCreditCollection = debitCreditCollection;
        }

        public async Task<DebitCredit> CreateDebitCredit(string cardId)
        {
            var debitCreditDocument = new DebitCredit()
            {
                CardId = cardId
            };
            await _debitCreditCollection.InsertOneAsync(debitCreditDocument);
            return debitCreditDocument;
        }

        public Task<decimal> CreateTransactAsync(string cardId, decimal sum)
        {
            var debitCreditDocument = new DebitCredit()
            {
                CardId = cardId,
                Sum = sum
            };
            _debitCreditCollection.InsertOne(debitCreditDocument);
            return Task.Run(() => _debitCreditCollection.Find(dc => dc.CardId == cardId).ToList().Sum(dc => dc.Sum));
        }

        public Task<decimal> GetSumByCardIdAsync(string cardId)
        {
            return Task.Run(() => _debitCreditCollection.Find(dc => dc.CardId == cardId).ToList().Sum(dc => dc.Sum));
        }
    }
}
