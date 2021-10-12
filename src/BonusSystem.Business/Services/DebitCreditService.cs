using BonusSystem.Models.Entities;
using MongoDB.Driver;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BonusSystem.Business.Services
{
    public interface IDebitCreditService
    {
        Task<decimal> CreateTransactAsync(string cardId, string sum);
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

        public async Task<decimal> CreateTransactAsync(string cardId, string sum)
        {
            var isNegative = sum.StartsWith("-");
            var valueValue = CreateValidValue(sum, isNegative);

            var debitCreditDocument = new DebitCredit()
            {
                CardId = cardId,
                Sum = valueValue
            };
            await _debitCreditCollection.InsertOneAsync(debitCreditDocument);
            return _debitCreditCollection.Find(dc => dc.CardId == cardId).ToList().Sum(dc => dc.Sum);
        }

        private decimal CreateValidValue(string sum, bool isNegative)
        {
            return isNegative ?
                Decimal.Negate(decimal.Parse(sum.Replace("-", ""), CultureInfo.InvariantCulture)) :
                decimal.Parse(sum, CultureInfo.InvariantCulture);
        }

        public Task<decimal> GetSumByCardIdAsync(string cardId)
        {
            return Task.Run(() => _debitCreditCollection.Find(dc => dc.CardId == cardId).ToList().Sum(dc => dc.Sum));
        }
    }
}
