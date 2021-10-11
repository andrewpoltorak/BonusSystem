using BonusSystem.Models.DTO;
using BonusSystem.Models.Entities;
using Mapster;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace BonusSystem.Business.Services
{
    public interface IClientService
    {
        Task<Client> GetByPhoneNumberAsync(string searchValue);
        Task<Client> CreateClientAsync(ClientDto clientDto);
        Task<Client> GetByCardIdAsync(string cardId);
    }

    public class ClientService : IClientService
    {
        private readonly IMongoCollection<Client> _clients;

        public ClientService(IMongoCollection<Client> clients)
        {
            _clients = clients;
        }

        public async Task<Client> CreateClientAsync(ClientDto clientDto)
        {
            var client = clientDto.Adapt<Client>();
            await _clients.InsertOneAsync(client);
            return client;
        }

        public async Task<Client> GetByPhoneNumberAsync(string searchValue)
        {
            return (await _clients.FindAsync(c => c.PhoneNumber == searchValue)).FirstOrDefault();
        }

        public async Task<Client> GetByCardIdAsync(string cardId)
        {
            return (await _clients.FindAsync(c => c.CardId == cardId)).FirstOrDefault();
        }
    }
}
