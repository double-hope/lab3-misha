using Hotel.BLL.Dtos.Client;

namespace Hotel.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<ClientDto> Login(ClientLoginDto clientDto);
        Task<ClientDto> Register(ClientRegisterDto clientDto);
    }
}
