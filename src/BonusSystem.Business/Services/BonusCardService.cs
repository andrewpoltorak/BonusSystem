using BonusSystem.Models.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Linq;
using BonusSystem.Models.RequestModels;
using BonusSystem.Models.DTO;
using Mapster;
using BonusSystem.Models.ResponseModels;
using BonusSystem.Business.Extentions;

namespace BonusSystem.Business.Services
{
    public interface IBonusCardService
    {
        Task<BonusCardResponseModel> GetByValueAsync(string searchValue);
        Task<BonusCardResponseModel> CreateCardAsync(CreateClientAndCardRequestModel requestModel);
    }

    public class BonusCardService : IBonusCardService
    {
        private readonly IMongoCollection<BonusCard> _bonusCards;

        private readonly IClientService _clientService;

        private readonly IDebitCreditService _debitCreditService;

        public BonusCardService(IMongoCollection<BonusCard> bonusCards, IClientService clientService, IDebitCreditService debitCreditService)
        {
            _bonusCards = bonusCards;
            _clientService = clientService;
            _debitCreditService = debitCreditService;
        }

        public async Task<BonusCardResponseModel> CreateCardAsync(CreateClientAndCardRequestModel requestModel)
        {
            var existClient = await _clientService.GetByPhoneNumberAsync(requestModel.Telephone);
            if (existClient is null)
            {
                var cardNumber = GetCardNumber();
                var newBonusCard = new BonusCard()
                {
                    Number = cardNumber
                };
                _bonusCards.InsertOne(newBonusCard);
                var client = await CreateClient(requestModel, newBonusCard);
                DebitCredit bonusSum = await _debitCreditService.CreateDebitCredit(newBonusCard.Id);
                var bonusCardFullInfo = new BonusCardResponseModel();
                bonusCardFullInfo = bonusCardFullInfo.ToResponseModel(newBonusCard, bonusSum.Sum, client);
                return bonusCardFullInfo;
            }
            else
            {
                throw new InvalidOperationException(message: "Client with current phone number is already exist");
            }
        }

        private async Task<Client> CreateClient(CreateClientAndCardRequestModel requestModel, BonusCard newBonusCard)
        {
            var clientDto = requestModel.Adapt<ClientDto>();
            clientDto.CardId = newBonusCard.Id;
            return await _clientService.CreateClientAsync(clientDto);
        }

        private int GetCardNumber()
        {
            var oldNumber = SearchOldCardNumber();
            if (oldNumber == 0)
            {
                var bonusCardsList = _bonusCards.Find(_ => true).ToList();
                return bonusCardsList.Count > 0 ? bonusCardsList.Select(m => m.Number).OrderByDescending(bc => bc).First() + 1 : 1;
            }
            else
            {
                _bonusCards.DeleteOne(d => d.Number == oldNumber);
                return oldNumber;
            }
        }

        private int SearchOldCardNumber()
        {
            var builder = Builders<BonusCard>.Filter;
            var filter = builder.Lt("DateEnd", DateTime.Now.Date);
            var oldBonusCard = _bonusCards.Find(filter).ToList().OrderBy(f => f.DateEnd).FirstOrDefault();
            return oldBonusCard != null ? oldBonusCard.Number : 0;
        }

        public async Task<BonusCardResponseModel> GetByValueAsync(string searchValue)
        {
            var bonusCardFullInfo = new BonusCardResponseModel();

            //if searchValue is a phone number
            if (searchValue.Length > 6)
            {
                var client = await _clientService.GetByPhoneNumberAsync(searchValue);
                if (client is not null)
                {
                    var bonusCard = (await _bonusCards.FindAsync(bc => bc.Id == client.CardId)).FirstOrDefault();
                    var bonusSum = await _debitCreditService.GetSumByCardIdAsync(bonusCard.Id);
                    bonusCardFullInfo = bonusCardFullInfo.ToResponseModel(bonusCard, bonusSum, client);
                }
            }
            //if searchValue is a bonus card number 
            else
            {
                var bonusCard = (await _bonusCards.FindAsync(bc => bc.Number == Convert.ToInt32(searchValue))).FirstOrDefault();
                if (bonusCard is not null)
                {
                    var client = await _clientService.GetByCardIdAsync(bonusCard.Id);
                    var bonusSum = await _debitCreditService.GetSumByCardIdAsync(bonusCard.Id);
                    bonusCardFullInfo = bonusCardFullInfo.ToResponseModel(bonusCard, bonusSum, client);
                }
            }

            return bonusCardFullInfo;
        }
    }
}
