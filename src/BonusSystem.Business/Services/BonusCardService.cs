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
                var cardNumber = await GetCardNumberAsync();
                if (cardNumber < 999999)
                {
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
                    throw new InvalidOperationException(message: "Перевищено діапозон доступних номерів для бонусних карт");
                }                
            }
            else
            {
                throw new InvalidOperationException(message: "Кліент з таким номером телефону вже існує");
            }
        }

        private async Task<Client> CreateClient(CreateClientAndCardRequestModel requestModel, BonusCard newBonusCard)
        {
            var clientDto = requestModel.Adapt<ClientDto>();
            clientDto.CardId = newBonusCard.Id;
            return await _clientService.CreateClientAsync(clientDto);
        }

        private async Task<int> GetCardNumberAsync()
        {
            var oldNumber = await SearchOldCardNumberAsync();
            if (oldNumber == 0)
            {
                var bonusCardsList = _bonusCards.Find(_ => true).ToList();
                return bonusCardsList.Count > 0 ? bonusCardsList.Select(m => m.Number).OrderByDescending(bc => bc).First() + 1 : 1;
            }
            else
            {
                await _bonusCards.DeleteOneAsync(d => d.Number == oldNumber);
                return oldNumber;
            }
        }

        private async Task<int> SearchOldCardNumberAsync()
        {
            var builder = Builders<BonusCard>.Filter;
            var filter = builder.Lt("DateEnd", DateTime.Now.Date);
            var oldBonusCard = (await _bonusCards.Find(filter).ToListAsync()).OrderBy(f => f.DateEnd).FirstOrDefault();
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
