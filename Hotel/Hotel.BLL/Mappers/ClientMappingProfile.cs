using AutoMapper;
using Hotel.BLL.Dtos.Client;
using Hotel.DAL.Entities;

namespace Hotel.BLL.Mappers
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<ClientLoginDto, Client>();
            CreateMap<ClientRegisterDto, Client>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<Client, ClientDto>();
        }
    }
}
