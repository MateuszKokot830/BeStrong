using System.Security.Cryptography;
using System.Text;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _usersRepository;
        private readonly IMapper _mapper;
        public AppUserService(IAppUserRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUserDto>> GetAllUsers()
        {
            var users = await _usersRepository.GetAll();
            return _mapper.Map<IEnumerable<AppUserDto>>(users);
        }

        public async Task<AppUserDto> GetUserById(int id)
        {
            var user = await _usersRepository.GetById(id);
            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetUserByUsername(string username)
        {
            var user = await _usersRepository.GetByUsername(username);
            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> AddUser(RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Username = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            await _usersRepository.Add(user);
            return _mapper.Map<AppUserDto>(user);
        }

        public bool IsPasswordCorrect(AppUserDto appUserDto, LoginDto loginDto)
        {
            using var hmac = new HMACSHA512(appUserDto.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != appUserDto.PasswordHash[i]) return false;
            }

            return true;
        }

    }
}