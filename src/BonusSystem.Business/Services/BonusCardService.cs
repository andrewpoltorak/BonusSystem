using BonusSystem.Models.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Linq;
using BonusSystem.Models.RequestModels;
using BonusSystem.Models.DTO;
using Mapster;
using System.Globalization;

namespace BonusSystem.Business.Services
{
    public interface IBonusCardService
    {
        Task<BonusCard> GetByValueAsync(string searchValue);
        Task<BonusCard> CreateCardAsync(CreateClientAndCardRequestModel requestModel);
    }

    public class BonusCardService : IBonusCardService
    {
        private readonly IMongoCollection<BonusCard> _bonusCards;

        private readonly IClientService _clientService;

        public BonusCardService(IMongoCollection<BonusCard> bonusCards, IClientService clientService)
        {
            _bonusCards = bonusCards;
            _clientService = clientService;
        }

        public async Task<BonusCard> CreateCardAsync(CreateClientAndCardRequestModel requestModel)
        {
            var cardNumber = CreateCardNumber();
            var newBonusCard = new BonusCard()
            {
                Number = cardNumber
            };
            _bonusCards.InsertOne(newBonusCard);
            var clientDto = requestModel.Adapt<ClientDto>();
            clientDto.CardId = newBonusCard.Id;
            await _clientService.CreateClientAsync(clientDto);
            //TODO: create DebitCredit
            return newBonusCard;
        }

        private int CreateCardNumber()
        {
            var f = _bonusCards.Find(_ => true).ToList().OrderByDescending(bc => bc.Number).Max(m => m.Number) + 1;
            return 0;
        }

        public async Task<BonusCard> GetByValueAsync(string searchValue)
        {
            //if searchValue is a bonus card number 
            if (searchValue.Length > 6)
            {
                var client = await _clientService.GetByPhoneNumberAsync(searchValue);
                if (client is not null)
                {
                    return (await _bonusCards.FindAsync(bc => bc.Id == client.CardId)).FirstOrDefault();
                }
                return null;
            }
            //if searchValue is a phone number
            else
            {
                return (await _bonusCards.FindAsync(bc => bc.Number == Convert.ToInt32(searchValue))).FirstOrDefault();
            }
        }
    }
}
