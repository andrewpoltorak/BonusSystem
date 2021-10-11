using BonusSystem.Models.DTO;
using BonusSystem.Models.RequestModels;
using Mapster;

namespace BonusSystem.Business.Mapping
{
    public class GlobalMapping
    {
        public static void Configure()
        {
            TypeAdapterConfig.GlobalSettings.Default
                .PreserveReference(true)
                .AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);

            TypeAdapterConfig<CreateClientAndCardRequestModel, ClientDto>.NewConfig()
                .Map(dest => dest.PhoneNumber, src => src.Telephone);
        }
    }
}
