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
        Task CreateClientAsync(ClientDto clientDto);
    }

    public class ClientService : IClientService
    {
        private readonly IMongoCollection<Client> _clients;

        public ClientService(IMongoCollection<Client> clients)
        {
            _clients = clients;
        }

        public Task CreateClientAsync(ClientDto clientDto)
        {
            var client = clientDto.Adapt<Client>();
            return _clients.InsertOneAsync(client);
        }

        public async Task<Client> GetByPhoneNumberAsync(string searchValue)
        {
            return (await _clients.FindAsync(c => c.PhoneNumber == searchValue)).FirstOrDefault();
        }
    }
}
