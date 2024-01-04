using AutoMapper;
using Hotel.BLL.Dtos.Client;
using Hotel.BLL.Interfaces;
using Hotel.DAL.Entities;
using Hotel.DAL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Hotel.BLL.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public async Task<ClientDto> Login(ClientLoginDto clientDto)
        {
            var client = await _unitOfWork.ClientRepository.FirstOrDefaultAsync(u => u.Email == clientDto.Email);

            if (client == null || !VerifyPassword(clientDto.Password, client.Password))
            {
                throw new Exception("Invalid email or password.");
            }

            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> Register(ClientRegisterDto clientDto)
        {
            if ((await _unitOfWork.ClientRepository.GetAllAsync(u => u.Email == clientDto.Email)).Any())
            {
                throw new Exception("Client with this email already exists.");
            }

            var hashedPassword = HashPassword(clientDto.Password);

            var newClient = _mapper.Map<Client>(clientDto);
            newClient.Password = hashedPassword;

            await _unitOfWork.ClientRepository.AddAsync(newClient);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ClientDto>(newClient);
        }

        private string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);

            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }


        private bool VerifyPassword(string providedPassword, string storedPassword)
        {
            byte[] storedHashedPassword = Convert.FromBase64String(storedPassword);

            using (var sha = SHA256.Create())
            {
                byte[] providedHashedPassword = sha.ComputeHash(Encoding.Default.GetBytes(providedPassword));

                return storedHashedPassword.SequenceEqual(providedHashedPassword);
            }
        }
    }
}
