using BonusSystem.Models.Entities;
using MongoDB.Driver;

namespace BonusSystem.Business.Services
{
    public interface IClientService
    {

    }

    public class ClientService : IClientService
    {
        private readonly IMongoCollection<Client> _clients;

        public ClientService(IMongoCollection<Client> clients)
        {
            _clients = clients;
        }
    }
}
