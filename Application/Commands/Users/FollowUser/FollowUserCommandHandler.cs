using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Commands.Users.FollowUser
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FollowUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {   
            var user = _userRepository.GetByIdAsync(request.UserId).Result;
            var followUser = _userRepository.GetByIdAsync(request.FollowUserId).Result;

            var isFollowed = user.FollowedUsers.FirstOrDefault(x => x.FollowedUserId == followUser.Id);
            if (isFollowed != null)
            {
                await _userRepository.DeleteFollowerAsync(isFollowed);
            }
            else
            {
                var follower = new Follower 
                {
                    UserId = user.Id,
                    User = user,
                    FollowedUserId = followUser.Id,
                    FollowedUser = followUser
                };
                await _userRepository.AddFollowerAsync(follower);
            }
       
            return Unit.Value;
        }
    }
}