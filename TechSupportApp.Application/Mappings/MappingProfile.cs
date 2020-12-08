using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TechSupportApp.Application.Interfaces;

namespace TechSupportApp.Application.Mappings
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            //get types which implements IMapFrom<T> interface
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) ));

            //for each type create instance and invoke "Mapping" method 
            //or default implementation in interface
            //sending current class as profile parameter
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                const string MappingMethodName = "Mapping";
                const string MappingInterfaceName = "IMapFrom`1";

                var methodInfo = type.GetMethod(MappingMethodName) ??
                    type.GetInterface(MappingInterfaceName).GetMethod(MappingMethodName);

                methodInfo.Invoke(instance, new object[] { this });
            }
        }
    }
}
