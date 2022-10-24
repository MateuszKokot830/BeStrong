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
        private readonly IAppUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AppUserService(IAppUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppUserDto>>(users);
        }

        public async Task<AppUserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetByUsername(username);
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
            await _userRepository.AddAsync(user);
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