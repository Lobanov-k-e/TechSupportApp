using AutoMapper;
using TechSupportApp.Application.Mappings;

namespace TechSupportApp.Tests.Application.Common
{
    static class MapperFactory
    {
        public static IMapper GetMapper()
        {

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return configurationProvider.CreateMapper();
        }
    }
}
