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
        }
    }
}
