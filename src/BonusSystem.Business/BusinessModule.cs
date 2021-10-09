using BonusSystem.Business.Services;
using BonusSystem.Models;
using BonusSystem.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BonusSystem.Business
{
    public static class BusinessModule
    {
        public static void RegisterServices(IServiceCollection services, BonusSystemDatabaseSettings settings)
        {
            AddScopedServices(services);
            CreateDBConfiguration(services, settings);
        }

        private static void AddScopedServices(IServiceCollection services)
        {
            services.AddScoped<IBonusCardService, BonusCardService>();
        }

        private static void CreateDBConfiguration(IServiceCollection services, BonusSystemDatabaseSettings settings)
        {
            var db = CreateMongoDatabase(settings);

            AddMongoDbService<BonusCardService, BonusCard>(settings.BonusCardCollection);
            AddMongoDbService<ClientService, Client>(settings.ClientCollection);
            AddMongoDbService<DebitService, Debit>(settings.DebitCollection);
            AddMongoDbService<CreditService, Credit>(settings.CreditCollection);

            void AddMongoDbService<TService, TModel>(string collectionName)
            {
                services.AddSingleton(db.GetCollection<TModel>(collectionName));
                services.AddSingleton(typeof(TService));
            }
        }

        private static IMongoDatabase CreateMongoDatabase(BonusSystemDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            return client.GetDatabase(settings.DatabaseName);
        }
    }
}
