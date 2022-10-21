using Application.Dto;
using Application.Interfaces;
using AutoMapper;
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
    }
}