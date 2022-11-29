using AutoMapper;
using Manager.Domain.Entities;
using Manager.Services.DTO;


namespace Manager.Tests.Configuration{
    public static class AutoMapperConfiguration{
        //criando um imitação da rotina do mapper
        public static IMapper GetConfiguration(){
            var autoMapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDTO>()
                    .ReverseMap();
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}